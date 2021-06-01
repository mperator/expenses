import React, { useState, useEffect } from 'react'
import { useHistory, useParams } from 'react-router-dom'
import useClient from './../../hooks/useClient'
import useForm from './../../hooks/useForm'

import ExpenseFormular from './ExpenseFormular';
import dayjs from 'dayjs';

const ExpenseEdit = () => {
    const history = useHistory();
    const { eventId, expenseId } = useParams()
    const { getEventAsync, getExpenseAsync, putExpenseAsync } = useClient();

    const [loading, setLoading] = useState(true);
    const { state, error, errorDetail, handleFormChange, setError, setErrorDetail, setForm } = useForm({
        date: dayjs(new Date()).format('YYYY-MM-DD'),
        title: '',
        description: '',
        amount: 0,
        participants: [],
        creditorId: ''
    });

    useEffect(() => {
        // load expense in case expenseId is given
        if (expenseId && eventId) {
            (async () => {
                try {
                    const expense = await getExpenseAsync(eventId, expenseId);
                    const event = await getEventAsync(eventId);
                    const participants = event.participants.map(a => ({
                        id: a.id, isParticipating: true, username: a.username, amount: expense.debits[expense.debits.findIndex(debitor => debitor.debitorId === a.id)].amount
                    }));
                    setForm({
                        ...state,
                        date: dayjs(expense.date).format('YYYY-MM-DD'),
                        title: expense.title,
                        description: expense.description,
                        currency: "EUR",
                        participants,
                        creditorId: expense.credit.creditorId,
                        amount: expense.credit.amount
                    })
                    setLoading(false);
                } catch (e) { console.error(e) }
            })();
        }
        // eslint-disable-next-line
    }, [expenseId])

    const handleParticipantAmountChange = async (e, i) => {
        const value = e.target.type === 'checkbox' ? e.target.checked : !e.target.checked;
        const participants = state.participants;
        participants[i][e.target.name] = value;

        setForm({
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
            console.log(split.toFixed(2))
            for (const p of participants) {
                if (p.isParticipating) {
                    p.amount = split;
                } else {
                    p.amount = 0;
                }
            }

            setForm({
                ...state,
                participants
            })
        }
    }

    const handleSubmitAsync = async (e) => {
        e.preventDefault();
        if (expenseId) {
            try {
                const debitors = state.participants.map(participant => {
                    var debitor = {};
                    debitor["debitorId"] = participant.id;
                    debitor["amount"] = participant.amount;
                    return debitor;
                })
                await putExpenseAsync(eventId, expenseId, {
                    title: state.title,
                    description: state.description,
                    credit: {
                        creditorId: state.creditorId,
                        amount: state.amount
                    },
                    debits: debitors
                });
                history.goBack();
            } catch (ex) {
                if (ex.errors) {
                    setError(s => ({
                        title: (ex.errors.Title && ex.errors.Title[0]) || "",
                        description: (ex.errors.Description && ex.errors.Description[0]) || "",
                        startDate: (ex.errors.StartDate && ex.errors.StartDate[0]) || "",
                        endDate: (ex.errors.EndDate && ex.errors.EndDate[0]) || ""
                    }))
                } else {
                    setErrorDetail(ex.detail);
                }
            }
        }
    }

    const handleCreditorChange = async (e) => {
        const creditorId = state.participants[state.participants.findIndex(p => p.id === e.target.value)].id;
        setForm({
            ...state,
            creditorId
        });
    }

    const handleCancel = (e) => {
        e.preventDefault();
        history.goBack()
    }

    return (
        <>
            <div className="container mt-4">
                {loading ? null :
                    <ExpenseFormular
                        title={"Update Expense"}
                        state={state}
                        error={error}
                        errorDetail={errorDetail}
                        handleFormChange={handleFormChange}
                        handleSplit={handleSplit}
                        handleCreditorChange={handleCreditorChange} handleParticipantAmountChange={handleParticipantAmountChange}>
                        <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>Update</button>
                        <button className="btn btn-outline-secondary" type="submit" onClick={handleCancel}>Cancel</button>
                    </ExpenseFormular>
                }
            </div>
        </>
    )
}

export default ExpenseEdit
