import React, { useState, useEffect } from 'react'
import { useHistory, useParams } from 'react-router-dom'

import useClient from './../../hooks/useClient'
import ExpenseFormular from './ExpenseFormular';

import Toast from './../layout/Toast';
import dayjs from 'dayjs';
import bootstrap from 'bootstrap/dist/js/bootstrap.min.js';

const ExpenseNew = () => {
    const history = useHistory();
    const { eventId, expenseId } = useParams()

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
        // load expense in case expenseId is given
        if (eventId) {
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
            const count = participants.filter(p => p.isParticipating).length;
            const split = (amount / count).toFixed(2);

            const delta = (amount - (count * split));
            console.log(delta)

            let applyDelta = false;
            if(delta > 0) {
               applyDelta = true;
            }

            for (const p of participants) {
                if (p.isParticipating) {
                    p.amount = split;

                    if(applyDelta) {
                        p.amount = (parseFloat(p.amount) + parseFloat(delta));
                        applyDelta = false;
                    }

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

    const handleCreditorChange = async (e) => {
        const creditorId = state.participants[state.participants.findIndex(p => p.id === e.target.value)].id;
        setState({
            ...state,
            creditorId
        });
    }

    const handleCancel = (e) => {
        e.preventDefault();
        history.goBack()
    }

    const triggerErrorToast = () => {
        const errorToast = new bootstrap.Toast(document.getElementById('errorToast'));
        errorToast.show();
    }

    return (
        <>
            <div className="container mt-4">
                <ExpenseFormular
                    title={"Create Expense"}
                    state={state}
                    error={error}
                    handleFormChange={handleFormChange}
                    handleSplit={handleSplit}
                    handleCreditorChange={handleCreditorChange} handleParticipantAmountChange={handleParticipantAmountChange}
                >
                    <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>Create</button>
                    <button className="btn btn-outline-secondary" type="submit" onClick={handleCancel}>Cancel</button>
                </ExpenseFormular>
            </div>
            <Toast idString="errorToast" />
        </>
    )
}

export default ExpenseNew
