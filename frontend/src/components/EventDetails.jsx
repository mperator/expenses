import React, { useEffect, useState } from 'react';
import useClient from '../hooks/useClient';
import { useHistory, useParams } from 'react-router';
import { Link } from 'react-router-dom';
import dayjs from 'dayjs';

const EventDetails = () => {
    const { getEventAsync } = useClient();
    const history = useHistory();
    const params = useParams();

    const [loading, setLoading] = useState(true);
    const [event, setEvent] = useState({})

    useEffect(() => {
        // load event using the incoming id
        if (params.id) {
            (async () => {
                // try catch ignore or redirect when failing
                const event = await getEventAsync(params.id);
                setEvent(event)
                setLoading(false);
            })();
        }
    }, [])

    function calculateExpenseSummary() {
        console.log(event.expenses)
        console.log("AAAA Hello")

        return 10;
    }

    //TODO: if total expense is negative show font in red and vice versa
    //TODO: add logo in front of user name
    //TODO: make each expense clickable and open expense editor for corresponding expense after click
    return (
        <div className="container mt-4">
            {loading ? <p>Loading ....</p> :
                <>
                    {/* <p>{!loading && calculateExpenseSummary()}</p> */}
                    <div className="card">
                        <div className="card-header">
                            <div className="row">
                                <div className="col-auto">
                                    <h3>{event.title}</h3>
                                </div>
                                <div className="col-auto">
                                    <Link className="btn btn-outline-primary me-1" to={`/event/editor/${params.id}`}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="0.8em" height="0.8em" fill="currentColor" className="bi bi-pencil-fill" viewBox="0 0 16 16">
                                            <path d="M12.854.146a.5.5 0 0 0-.707 0L10.5 1.793 14.207 5.5l1.647-1.646a.5.5 0 0 0 0-.708l-3-3zm.646 6.061L9.793 2.5 3.293 9H3.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.207l6.5-6.5zm-7.468 7.468A.5.5 0 0 1 6 13.5V13h-.5a.5.5 0 0 1-.5-.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.5-.5V10h-.5a.499.499 0 0 1-.175-.032l-.179.178a.5.5 0 0 0-.11.168l-2 5a.5.5 0 0 0 .65.65l5-2a.5.5 0 0 0 .168-.11l.178-.178z" />
                                        </svg>
                                    </Link>
                                    <button className="btn btn-outline-danger">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="0.8em" height="0.8em" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                            <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                                        </svg>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div className="card-body">
                            <p className="card-text">{event.description}</p>
                            <div className="row">
                                <div className="col">
                                    <div className="form-floating mb-3">
                                        <input type="text" className="form-control" id="floatingStartDate" readOnly value={dayjs(event.startDate).format('DD/MM/YYYY')} />
                                        <label htmlFor="floatingStartDate">Start date</label>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-floating mb-3">
                                        <input type="text" className="form-control" id="floatingEndDate" readOnly value={dayjs(event.endDate).format('DD/MM/YYYY')} />
                                        <label htmlFor="floatingEndDate">End date</label>
                                    </div>
                                </div>
                            </div>
                            <ul className="list-group">
                                <li className="list-group-item">
                                    <div className="row justify-content-between align-items-center">
                                        <div className="col-auto">
                                            <h4>Total expenses:</h4>
                                        </div>
                                        <div className="col-auto">
                                            <h4>-200€</h4>
                                        </div>
                                    </div>
                                </li>
                                {event.attendees.map(a => (
                                    <li key={a.id} className="list-group-item">
                                        <div className="row justify-content-between align-items-center">
                                            <div className="col-auto">
                                                {a.name}
                                            </div>
                                            <div className="col-auto">
                                                <div className="col-auto">
                                                    -50€
                                                </div>
                                                <div className="col-auto">
                                                    75.10€
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                ))}
                            </ul>
                            <div className="accordion my-2" id="accordionFlushExample">
                                <div className="accordion-item">
                                    <h2 className="accordion-header" id="flush-headingOne">
                                        <button className="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#flush-collapseOne" aria-expanded="false" aria-controls="flush-collapseOne">
                                            <h6>Expenses</h6>
                                        </button>
                                    </h2>
                                    <div id="flush-collapseOne" className="accordion-collapse collapse" aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushExample">
                                        <div className="accordion-body">
                                            <ul className="list-group">
                                                {event.expenses.length == 0 ? "no expenses yet ... go and add one"
                                                    : event.expenses.map(e => (
                                                        <li key={e.id} className="list-group-item">
                                                            <p className="mb-0 fs-5">{e.title}</p>
                                                            <p className="fs-3 fw-bold mb-0">{e.amount}€</p>
                                                            <p style={{ fontSize: '0.8rem' }}>{dayjs(e.date).format('DD/MM/YYYY')}</p>
                                                        </li>
                                                    ))
                                                }
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <Link className="text-dark" to={`/expense/editor?eventId=${event.id}`}>
                                <svg width="0.8em" height="0.8em" viewBox="0 0 16 16" className="addButton bi bi-plus-circle-fill text-primary" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                    <path fillRule="evenodd" d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                                </svg>
                            </Link>
                        </div>
                    </div>
                </>}
        </div>
    )
}

export default EventDetails
