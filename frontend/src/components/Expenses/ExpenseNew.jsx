import React, { useState, useEffect } from 'react'
import { useHistory, useParams } from 'react-router-dom'

import useClient from './../../hooks/useClient'
import useForm from './../../hooks/useForm'
import ExpenseFormular from './ExpenseFormular';
import dayjs from 'dayjs';

const ExpenseNew = () => {
    const history = useHistory();
    const { eventId, expenseId } = useParams()
    const { getEventAsync, postExpenseAsync } = useClient();

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
        if (eventId) {
            (async () => {
                try {
                    const event = await getEventAsync(eventId);
                    const participants = event.participants.map(a => ({ id: a.id, isParticipating: true, username: a.username, amount: 0 }));
                    const creditorId = participants[0].id;
                    setForm({
                        ...state,
                        participants,
                        creditorId
                    })
                    setLoading(false);
                } catch (e) { console.error(e) }
            })();
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
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
            const count = participants.filter(p => p.isParticipating).length;
            const split = (amount / count).toFixed(2);

            const delta = (amount - (count * split));

            let applyDelta = false;
            if (delta !== 0) {
                applyDelta = true;
            }

            for (const p of participants) {
                if (p.isParticipating) {
                    p.amount = split;

                    if (applyDelta) {
                        p.amount = (parseFloat(p.amount) + parseFloat(delta)).toFixed(2);
                        applyDelta = false;
                    }

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
        try {
            const debitors = state.participants.map(participant => {
                var debitor = {};
                debitor["debitorId"] = participant.id;
                debitor["amount"] = participant.amount;
                return debitor;
            });
            await postExpenseAsync(eventId, {
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
            history.goBack();
        } catch (ex) {
            if (ex.errors) {
                setError(s => ({
                    date: (ex.errors.Date && ex.errors.Date[0]) || "",
                    title: (ex.errors.Title && ex.errors.Title[0]) || "",
                    description: (ex.errors.Description && ex.errors.Description[0]) || "",
                    amount: (ex.errors.Amount && ex.errors.Amount[0]) || "",
                    participants: (ex.errors.Participants && ex.errors.Participants[0]) || "",
                    others: (ex.errors.Others && ex.errors.Others[0]) || ""
                }))
            } else {
                setErrorDetail(ex.detail);
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
                        title={"Create Expense"}
                        state={state}
                        error={error}
                        errorDetail={errorDetail}
                        handleFormChange={handleFormChange}
                        handleSplit={handleSplit}
                        handleCreditorChange={handleCreditorChange} handleParticipantAmountChange={handleParticipantAmountChange}
                    >
                        <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>Create</button>
                        <button className="btn btn-outline-secondary" type="submit" onClick={handleCancel}>Cancel</button>
                    </ExpenseFormular>
                }
            </div>
        </>
    )
}

export default ExpenseNew
