import React, { useState } from 'react'

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
    const [state, setState] = useState({
        date: '2020-12-08',
        title: 'title',
        description: 'descr',
        amount: 0,
        participants: [
            { id: 1, isParticipating: true, name: "Peter Parker", amount: 0 },
            { id: 2, isParticipating: true, name: "Undo Umparken", amount: 0 },
            { id: 3, isParticipating: true, name: "Andew Anderson", amount: 0 },
            { id: 4, isParticipating: true, name: "Peter Enis", amount: 0 },
            { id: 5, isParticipating: false, name: "Sam Saupeter", amount: 0 }
        ]
    })

    const handleFormChange = (e) => {
        setState({
            ...state,
            [e.target.name]: e.target.value
        })
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
        if(amount > 0) {
            const split = amount / participants.filter(p => p.isParticipating).length;

            for(const p of participants) {
                if(p.isParticipating) {
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

    const handleSubmit = (e) => {
        const eventId = 0;

        e.preventDefault();
        try {
            // await postExpenseAsync(eventId, {
            //     date: state.date
            //     title: state.title,
            //     description: state.description,
            //     amount: state.amount,
            //     participants: state.participants
            // });
            // history.goBack();
        } catch (error) {
            // setError(s => ({
            //     title: (error.Title && error.Title[0]) || "",
            //     description: (error.Description && error.Description[0]) || "",
            //     startDate: (error.StartDate && error.StartDate[0]) || "",
            //     endDate: (error.EndDate && error.EndDate[0]) || ""
            // }))
        }
    }

    return (
        <div className="container mt-4">
            <h2>Expense</h2>
            <form>
                <div className="mb-3">
                    <label htmlFor="date" className="form-label">Date</label>
                    <input type="date" className="form-control" id="date" name="date" value={state.date} onChange={handleFormChange} />
                </div>

                <div className="mb-3">
                    <label htmlFor="title" className="form-label">Title</label>
                    <input type="text" className="form-control" id="title" name="title" value={state.title} onChange={handleFormChange}/>
                </div>

                <div className="mb-3">
                    <label htmlFor="description" className="form-label">Description</label>
                    <textarea type="form-control" className="form-control" id="description" name="description" value={state.description} onChange={handleFormChange}/>
                </div>

                <div className="mb-3">
                    <label htmlFor="amount" className="form-label">Amount</label>
                    <div className="input-group">
                        <input type="text" className="form-control text-right" aria-label="amount" id="amount" name="amount" value={state.amount} onChange={handleFormChange}/>
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
                                        <input className="form-check-input" type="checkbox" aria-label="Checkbox for following text input" id="isParticipating" name="isParticipating" checked={p.isParticipating} onChange={e => handleParticipantAmountChange(e, i)}/>
                                    </div>
                                    <input type="text" className="form-control text-right" aria-label="Text input with checkbox" id="splitAmount" name="amount" value={p.amount} onChange={e => handleParticipantAmountChange(e, i)}/>
                                    <span className="input-group-text">€</span>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
                 
                 <div className="col-12 text-right">
                    <button className="btn btn-primary mr-1" type="submit" onClick={handleSubmit}>Create</button>
                    <button className="btn btn-outline-secondary" type="submit" onClick={handleSubmit}>Cancel</button>
                </div>
            </form>
        </div>
    )
}

export default ExpenseEdit
