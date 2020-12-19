import React, { useState, useEffect } from 'react'
import { useLocation, useHistory } from 'react-router-dom'
import useClient from '../hooks/useClient'

/*
- Datum
- Title
- Description
- Betrag

- Personen
- Personen
- Personen
--------------------------
Personen from expense holen

Es fehlt eine tabelle issuer/expense/amount

*/

const ExpenseEdit = () => {
    const history = useHistory();
    const eventId = useQuery().get('eventId');
    const { getEventAsync, postExpenseAsync } = useClient();

    const [state, setState] = useState({
        date: '2020-12-08',
        title: '',
        description: '',
        amount: 0,
        participants: [
        ]
    });

    const [error, setError] = useState({
        date: "",
        title: "",
        description: "",
        participants: "",
        others: ""
    });

    useEffect(() => {
        if (!eventId) console.log("error");

        // load event with id
        (async () => {
            const event = await getEventAsync(eventId);
            const participants = event.attendees.map(a => ({ id: a.id, isParticipating: true, name: a.name, amount: 0 }));
            setState({
                ...state,
                participants
            })
        })();

        // load data
    }, [eventId])

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

    const handleParticipantAmountChange = (e, i) => {
        const value = e.target.type === 'checkbox' ? e.target.checked : e.target.value;
        const participants = state.participants;
        participants[i][e.target.name] = value;
        setState({
            ...state,
            participants
        })
    }

    const handleSplit = (e) => {
        e.preventDefault();

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
        try {
            await postExpenseAsync(eventId, {
                date: state.date,
                title: state.title,
                description: state.description,
                participants: state.participants
            });
            history.goBack();
        } catch (error) {
            setError(s => ({
                date: (error.Date && error.Date[0]) || "",
                title: (error.Title && error.Title[0]) || "",
                description: (error.Description && error.Description[0]) || "",
                participants: (error.Participants && error.Participants[0]) || "",
                others: (error.Others && error.Others[0]) || ""
            }))
        }
    }

    function useQuery() {
        return new URLSearchParams(useLocation().search);
    }

    const isValid = (e) => {
        return e && " is-invalid";
    }

    return (
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
                    <h3 className="mb-3" >Teilnehmer</h3>
                    {state.participants.map((p, i) => (
                        <div className="row mb-2">
                            <label className="col-sm-3 col-form-label">{p.name}</label>
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

                <div className="col-12 text-right">
                    <button className="btn btn-primary me-1" type="submit" onClick={handleSubmitAsync}>Create</button>
                    <button className="btn btn-outline-secondary" type="submit" onClick={e => { e.preventDefault(); history.goBack() }}
                    >Cancel</button>
                </div>
            </form>
        </div>
    )
}

export default ExpenseEdit
