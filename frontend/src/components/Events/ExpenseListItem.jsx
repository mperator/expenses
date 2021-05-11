import React from 'react'

import { Link } from 'react-router-dom';
import DateFormat from '../DateFormat';

const ExpenseListItem = ({eventId, expense}) => {
    return (
        <Link key={expense.id} to={`/event/${eventId}/expenses/${expense.id}`} className="list-group-item list-group-item-action" aria-current="true">
            <div className="container">
                <div className="row">
                    <div className="col-8 ">
                        <h5 className="mb-1">{expense.title}</h5>
                        <p className="mb-1">{expense.description}</p>
                    </div>
                    <div className="col-4 d-flex flex-column ">
                        <small className="align-self-end"><DateFormat date={expense.date} /></small>
                        <div className="d-flex flex-fill justify-content-end h-100">
                            <p className="h3 align-self-center" ><span>{expense.credit.amount}</span> {expense.currency}</p>
                        </div>
                    </div>
                </div>
            </div>
        </Link>
    )
}

export default ExpenseListItem
