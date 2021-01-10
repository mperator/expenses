import dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { useLocation, useParams } from 'react-router';
import { Link } from 'react-router-dom';
import useClient from '../hooks/useClient';

export default function ExpenseDetails() {
    const { getExpenseAsync } = useClient();
    const params = useParams();
    const location = useLocation();
    // read the eventId from the location state to fetch the correct expense from api
    const eventId = location.state.eventId;

    const [expense, setExpense] = useState({});

    useEffect(() => {
        // load expense using the incoming id
        if (params.id) {
            (async () => {
                //FIXME: what will the user see in case an error occurs during the fetch of expense
                const expense = await getExpenseAsync(eventId, params.id);
                setExpense(expense);
            })();
        }
    }, []);

    const handleDelete = (id) => {
        console.log("delete", id)
    }

    return (
        <div className="container mt-4">
            <div className="card">
                <div className="card-header">
                    <div className="row">
                        <div className="col-auto">
                            <h3>{expense.title}</h3>
                        </div>
                        <div className="col-auto">
                            {/* TODO: fix the route to expense editor and introduce editing expense before that */}
                            <Link className="btn btn-outline-primary me-1" to={`/expense/editor?${params.id}`}>
                                <svg xmlns="http://www.w3.org/2000/svg" width="0.8em" height="0.8em" fill="currentColor" className="bi bi-pencil-fill" viewBox="0 0 16 16">
                                    <path d="M12.854.146a.5.5 0 0 0-.707 0L10.5 1.793 14.207 5.5l1.647-1.646a.5.5 0 0 0 0-.708l-3-3zm.646 6.061L9.793 2.5 3.293 9H3.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.207l6.5-6.5zm-7.468 7.468A.5.5 0 0 1 6 13.5V13h-.5a.5.5 0 0 1-.5-.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.5-.5V10h-.5a.499.499 0 0 1-.175-.032l-.179.178a.5.5 0 0 0-.11.168l-2 5a.5.5 0 0 0 .65.65l5-2a.5.5 0 0 0 .168-.11l.178-.178z" />
                                </svg>
                            </Link>
                            <button className="btn btn-outline-danger" onClick={() => handleDelete(expense.id)}>
                                <svg xmlns="http://www.w3.org/2000/svg" width="0.8em" height="0.8em" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                    <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                                </svg>
                            </button>
                        </div>
                    </div>
                </div>
                <div className="card-body">
                    {/* FIXME: add dynamic username using useAuth hook */}
                    <p className="card-text mb-0">Paid by Testimann: {expense.amount}€</p>
                    <p className="card-text"><small className="text-muted">added on {dayjs(expense.date).format('DD/MM/YYYY')}</small></p>
                </div>
            </div>
        </div>
    )
}
