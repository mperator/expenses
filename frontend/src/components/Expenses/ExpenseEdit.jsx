import React, { useState, useEffect } from 'react'
import { useHistory, useParams } from 'react-router-dom'
import useClient from './../../hooks/useClient'

import ExpenseFormular from './ExpenseFormular';
import Toast from './../layout/Toast';
import dayjs from 'dayjs';
import bootstrap from 'bootstrap/dist/js/bootstrap.min.js';

const ExpenseEdit = () => {
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
                    title={"Update Expense"}
                    state={state}
                    error={error}
                    handleFormChange={handleFormChange}
                    handleSplit={handleSplit}
                    handleCreditorChange={handleCreditorChange} handleParticipantAmountChange={handleParticipantAmountChange}
                >
                    <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>Update</button>
                    <button className="btn btn-outline-secondary" type="submit" onClick={handleCancel}>Cancel</button>
                </ExpenseFormular>
            </div>
            <Toast idString="errorToast" />
        </>
    )
}

export default ExpenseEdit
