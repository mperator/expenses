import dayjs from 'dayjs';
import React, { useState, useEffect } from 'react'
import { useLocation, useHistory } from 'react-router-dom'
import useClient from '../hooks/useClient'
import Toast from './layout/Toast';
import bootstrap from 'bootstrap/dist/js/bootstrap.min.js';

const ExpenseEditor = () => {
    const history = useHistory();
    const eventId = useQuery().get('eventId');
    const expenseId = useQuery().get('expenseId');
    const { getEventAsync, postExpenseAsync, getExpenseAsync, putExpenseAsync } = useClient();

    const [state, setState] = useState({
        date: dayjs(new Date()).format('YYYY-MM-DD'),
        title: '',
        description: '',
        //TODO: change to empty string! and check evaluation
        amount: 0,
        participants: [],
        creditorId: ''
    });

    const [error, setError] = useState({
        date: "",
        title: "",
        description: "",
        amount: "",
        participants: "",
        others: ""
    });

    useEffect(() => {
        // load event with id and set participants in case no expenseId is given
        if (eventId && !expenseId) {
            (async () => {
                const event = await getEventAsync(eventId);
                const participants = event.participants.map(a => ({ id: a.id, isParticipating: true, username: a.username, amount: 0 }));
                const creditorId = participants[0].id;
                setState({
                    ...state,
                    participants,
                    creditorId
                })
            })();
        }
    }, [eventId])

    useEffect(() => {
        // load expense in case expenseId is given
        if (expenseId && eventId) {
            (async () => {
                const expense = await getExpenseAsync(eventId, expenseId);
                //TODO: load all participants in expense --> implement in CQRS command
                const event = await getEventAsync(eventId);
                const participants = event.participants.map(a => ({
                    id: a.id, isParticipating: true, username: a.username, amount: expense.debits[expense.debits.findIndex(debitor => debitor.debitorId == a.id)].amount
                }));
                setState({
                    ...state,
                    date: dayjs(expense.date).format('YYYY-MM-DD'),
                    title: expense.title,
                    description: expense.description,
                    currency: "EUR",
                    participants,
                    creditorId: expense.credit.creditorId,
                    amount: expense.credit.amount
                })
            })();
        }
    }, [expenseId])

    const handleFormChange = (e) => {
        setState(s => ({
            ...state,
            [e.target.name]: e.target.value
        }))

        setError(s => ({
            ...s,
            [e.target.name]: ""
        }))
    }

    const handleParticipantAmountChange = async (e, i) => {
        const value = e.target.type === 'checkbox' ? e.target.checked : !e.target.checked;
        const participants = state.participants;
        participants[i][e.target.name] = value;

        setState({
            ...state,
            participants
        }, handleSplit(e))
    }

    const handleSplit = (e) => {
        if (e.target.type !== 'checkbox') e.preventDefault();

        const participants = state.participants;
        const amount = state.amount;
        if (amount > 0) {
            const split = amount / participants.filter(p => p.isParticipating).length;

            for (const p of participants) {
                if (p.isParticipating) {
                    p.amount = split;
                } else {
                    p.amount = 0;
                }
            }

            setState({
                ...state,
                participants
            })
        }
    }

    const handleSubmitAsync = async (e) => {
        e.preventDefault();
        if (expenseId) {
            try {
                console.log(state)
                const debitors = state.participants.map(participant => {
                    var debitor = {};
                    debitor["debitorId"] = participant.id;
                    debitor["amount"] = participant.amount;
                    return debitor;
                })
                const response = await putExpenseAsync(eventId, expenseId, {
                    title: state.title,
                    description: state.description,
                    credit: {
                        creditorId: state.creditorId,
                        amount: state.amount
                    },
                    debits: debitors
                });
                if (response !== null) triggerErrorToast();
                else history.goBack();
            } catch (error) {
                //TODO: implement showing the error to the user
                console.log(error)
            }
        } else {
            try {
                const debitors = state.participants.map(participant => {
                    var debitor = {};
                    debitor["debitorId"] = participant.id;
                    debitor["amount"] = participant.amount;
                    return debitor;
                });
                const response = await postExpenseAsync(eventId, {
                    date: state.date,
                    title: state.title,
                    description: state.description,
                    currency: "EUR",
                    participants: state.participants,
                    credit: {
                        creditorId: state.creditorId,
                        amount: state.amount,
                    },
                    debits: debitors
                });
                if (response === null) triggerErrorToast();
                else history.goBack();
            } catch (error) {
                setError(s => ({
                    date: (error.Date && error.Date[0]) || "",
                    title: (error.Title && error.Title[0]) || "",
                    description: (error.Description && error.Description[0]) || "",
                    amount: (error.Amount && error.Amount[0]) || "",
                    participants: (error.Participants && error.Participants[0]) || "",
                    others: (error.Others && error.Others[0]) || ""
                }))
            }
        }
    }

    function useQuery() {
        return new URLSearchParams(useLocation().search);
    }

    const isValid = (e) => {
        return e && " is-invalid";
    }

    const triggerErrorToast = () => {
        const errorToast = new bootstrap.Toast(document.getElementById('errorToast'));
        errorToast.show();
    }

    const handleCreditorChange = async (e) => {
        const creditorId = state.participants[state.participants.findIndex(p => p.id === e.target.value)].id;
        setState({
            ...state,
            creditorId
        });
    }

    return (
        <>
            <div className="container mt-4">
                <h2>Expense</h2>
                <form>
                    <div className="mb-3">
                        <label htmlFor="date" className="form-label">Date</label>
                        <input type="date" className={"form-control" + isValid(error.date)} id="date" name="date" value={state.date} onChange={handleFormChange} />
                        {error.date && <div className="invalid-feedback">{error.date}</div>}
                    </div>
                    <div className="mb-3">
                        <label htmlFor="title" className="form-label">Title</label>
                        <input type="text" className={"form-control" + isValid(error.title)} id="title" name="title" value={state.title} onChange={handleFormChange} />
                        {error.title && <div className="invalid-feedback">{error.title}</div>}
                    </div>
                    <div className="mb-3">
                        <label htmlFor="description" className="form-label">Description</label>
                        <textarea type="form-control" className={"form-control" + isValid(error.description)} id="description" name="description" value={state.description} onChange={handleFormChange} />
                        {error.description && <div className="invalid-feedback">{error.description}</div>}
                    </div>
                    <div className="mb-3">
                        <label htmlFor="amount" className="form-label">Amount</label>
                        <div className="input-group">
                            <input type="text" className="form-control text-right" aria-label="amount" id="amount" name="amount" value={state.amount} onChange={handleFormChange} />
                            <button type="button" className="btn btn-outline-primary" onClick={handleSplit}>Split</button>
                            <span className="input-group-text">€</span>
                        </div>
                    </div>
                    <div className="mb-3">
                        <div className="form-floating">
                            <select className="form-select" id="creditorId" aria-label="Id of the creditor" name="creditor" value={state.creditorId} onChange={(e) => handleCreditorChange(e)}>
                                {state.participants.map(participant =>
                                    <option key={participant.id} value={participant.id}>{participant.username}</option>
                                )}
                            </select>
                            <label htmlFor="creditorId">Creditor</label>
                        </div>
                    </div>
                    <div className="mb-3">
                        <h3 className="mb-3" >Participants</h3>
                        {state.participants.map((p, i) => (
                            <div key={p.id} className="row mb-2">
                                <label className="col-sm-3 col-form-label">{p.username}</label>
                                <div className="col-sm-9">
                                    <div className="input-group">
                                        <div className="input-group-text">
                                            <input className="form-check-input" type="checkbox" aria-label="Checkbox for following text input" id="isParticipating" name="isParticipating" checked={p.isParticipating} onChange={e => handleParticipantAmountChange(e, i)} />
                                        </div>
                                        <input type="text" className="form-control text-right" aria-label="Text input with checkbox" id="splitAmount" name="amount" value={p.amount} onChange={e => handleParticipantAmountChange(e, i)} />
                                        <span className="input-group-text">€</span>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                    <div className="row justify-content-end mb-3">
                        <div className="col-auto">
                            <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>{expenseId ? "Save" : "Create"}</button>
                        </div>
                        <div className="col-auto">
                            <button className="btn btn-outline-secondary" type="submit" onClick={e => { e.preventDefault(); history.goBack() }}
                            >Cancel</button>
                        </div>
                    </div>
                </form>
            </div>
            <Toast idString="errorToast" />
        </>
    )
}

export default ExpenseEditor
