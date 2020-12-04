import React from 'react'

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

const data = [
    { id: 1, name: "Peter Parker" },
    { id: 2, name: "Undo Umparken" },
    { id: 3, name: "Andew Anderson" },
    { id: 4, name: "Peter Enis" },
    { id: 5, name: "Sam Saupeter" }
]

const ExpenseEdit = () => {
    return (
        <div className="container mt-4">
            <h2>Expense</h2>
            <form>
                <div className="mb-3">
                    <label htmlFor="date" className="form-label">Date</label>
                    <input type="date" className="form-control" />
                </div>

                <div className="mb-3">
                    <label htmlFor="title" className="form-label">Title</label>
                    <input type="text" className="form-control" />
                </div>

                <div className="mb-3">
                    <label htmlFor="description" className="form-label">Description</label>
                    <textarea type="form-control" className="form-control" />
                </div>

                <div className="mb-3">
                    <label htmlFor="amount" className="form-label">Amount</label>
                    <div className="input-group">
                        <input type="text" id="amount" className="form-control text-right" aria-label="amount" />
                        <button type="button" className="btn btn-outline-primary">Split</button>
                        <span class="input-group-text">€</span>
                    </div>
                </div>

                <div className="mb-3">
                    <h3 className="mb-3" >Teilnehmer</h3>
                    {data.map(d => (
                        <div className="row mb-2">
                            <label className="col-sm-3 col-form-label">Password</label>
                            <div className="col-sm-9">
                                <div className="input-group">
                                    <div className="input-group-text">
                                        <input className="form-check-input" type="checkbox" value="" aria-label="Checkbox for following text input" />
                                    </div>
                                    <input type="text" className="form-control text-right" aria-label="Text input with checkbox" />
                                    <span className="input-group-text">€</span>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </form>
        </div>
    )
}

export default ExpenseEdit
