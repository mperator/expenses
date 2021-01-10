import dayjs from 'dayjs';
import React, { useEffect, useState } from 'react';
import { useHistory, useLocation, useParams } from 'react-router';
import { Link } from 'react-router-dom';
import useClient from '../hooks/useClient';
import bootstrap from 'bootstrap/dist/js/bootstrap.min.js';

export default function ExpenseDetails() {
    const { getExpenseAsync, deleteExpenseAsync } = useClient();
    const params = useParams();
    const location = useLocation();
    const history = useHistory();
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

    const handleDeleteButton = async () => {
        console.log("delete")
        const confirmDeletionModal = new bootstrap.Modal(document.getElementById('deleteConfirmationModal'));
        confirmDeletionModal.toggle();
    }

    const deleteExpense = async () => {
        const modal = bootstrap.Modal.getInstance(document.getElementById('deleteConfirmationModal'));
        const response = await deleteExpenseAsync(eventId, expense.id);
        console.log(response);
        // only call setJobs if response has been a success
        if (response === null) {
            // route back in history
            history.goBack();
        }
        else {
            // show error to user
            triggerErrorToast();
        }
        modal.toggle();
    }

    const triggerErrorToast = () => {
        const errorToast = new bootstrap.Toast(document.getElementById('errorToast'));
        errorToast.show();
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
                            <button className="btn btn-outline-danger" onClick={handleDeleteButton}>
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
            {/* <!-- Modal --> */}
            <div className="modal fade" id="deleteConfirmationModal" tabIndex="-1" aria-labelledby="deleteConfirmationModalLabel" aria-hidden="true">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="deleteConfirmationModalLabel">Confirm deletion</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            Do you want to delete the selected expense?
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">No</button>
                            <button type="button" className="btn btn-primary" onClick={deleteExpense}>Yes</button>
                        </div>
                    </div>
                </div>
            </div>
            {/* <!-- Toast --> */}
            <div className="toast position-absolute top-0 end-0 p-3 hide" id="errorToast" role="alert" aria-live="assertive" aria-atomic="true">
                <div className="toast-header">
                    <svg className="bd-placeholder-img rounded me-2" width="20" height="20" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" preserveAspectRatio="xMidYMid slice" focusable="false"><rect width="100%" height="100%" fill="#ff0011"></rect></svg>
                    <strong className="me-auto">Error</strong>
                    <button type="button" className="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
                <div className="toast-body">
                    An error occured. Please try again!
                </div>
            </div>
        </div>
    )
}
