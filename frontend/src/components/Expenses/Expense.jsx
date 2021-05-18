import React, { useState, useEffect } from 'react'
import { useParams } from 'react-router';
import DateFormat from '../DateFormat';
import ParticipantMoneyList from '../Events/ParticipantMoneyList';
import LinkButtonEdit from '../LinkButtonEdit';

import useClient from './../../hooks/useClient'

const Expense = () => {
    const { eventId, expenseId } = useParams();
    const { getEventByIdAsync } = useClient();

    const [loading, setLoading] = useState(true)
    const [expense, setExpense] = useState({})
    const [participants, setParticipants] = useState([])

    useEffect(() => {
        (async () => {
            // get event from there get exoebs by filter need event becaus of participants
            const event = await getEventByIdAsync(eventId);
            var expense = event.expenses.filter(e => e.id === parseInt(expenseId))[0];
            if (expense) {
                setParticipants(event.participants);
                setExpense(expense);
                setLoading(false);
            }
        })();
    // eslint-disable-next-line
    }, [])

    const getUsernameById = (id) => {
        if(participants) {
            return participants.filter(p => p.id === id)[0].username;
        }
        return "";
    }

    const calculateFinancials = (expense) => {
        const results = [];

        results.push({ 
            userId: expense.credit.creditorId, 
            username: getUsernameById(expense.credit.creditorId), 
            isCurrentUser: false, 
            loan: 0, 
            debt: 0, 
            balance: expense.credit.amount, 
            currency: expense.currency});

        for(const debit of expense.debits) {
            results.push({ 
                userId: debit.debitorId, 
                username: getUsernameById(debit.debitorId), 
                isCurrentUser: false, 
                loan: 0, 
                debt: 0, 
                balance: debit.amount * -1, 
                currency: expense.currency});
        }

        return results.sort(a => -1 * a.balance);
    }

    return (
        <div className="container">
            {!loading && expense ?
                (<>
                    <h2 className="display-2">{expense.title}</h2>
                    <div className="d-flex w-100 justify-content-between">
                        <span className="small">
                            <DateFormat date={expense.date} />
                        </span>
                        <div className="d-flex flex-row-reverse">
                            <div className="btn-group">
                                <LinkButtonEdit to={`/event/${eventId}/expenses/${expenseId}/edit`} />
                            </div>
                        </div>
                    </div>
                    <div>
                        <p className="mt-3">{expense.description}</p>
                    </div>

                    <ParticipantMoneyList financials={calculateFinancials(expense)} />
                </>) : null}
        </div>
    )
}

export default Expense
