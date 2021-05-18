import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import useClient from '../../hooks/useClient';
import useAuth from '../../hooks/useAuth';

import EventFinancialsChart from './EventFinancialsChart';
import ParticipantMoneyList from './ParticipantMoneyList';
import DateFormat from '../DateFormat';

const EventFinancials = () => {
    const { getEventAsync } = useClient();
    const { userId } = useAuth();
    const params = useParams();

    const [loading, setLoading] = useState(true);
    const [event, setEvent] = useState('')

    useEffect(() => {
        // load event using the incoming id
        if (params.id) {
            (async () => {
                // try catch ignore or redirect when failing
                const event = await getEventAsync(params.id);
                setEvent(event);
                setLoading(false);
            })();
        }
    }, [])

    const calculateExpenseSummary = () => {
        const expenses = event.expenses;
        if (expenses.length > 0)
            return expenses.map(a => a.credit.amount).reduce((a, c) => a + c);
        else
            return 0;
    }

    const calculateUserDebt = (userId) => {
        const dept = event.expenses
            .flatMap(e => e.debits)
            .filter(d => d.debitorId === userId)
            .map(d => d.amount)
            .reduce((a, c) => a + c, 0);
        return dept;
    }

    const calculateUserLoan = (userId) => {
        const loan = event.expenses
            .flatMap(e => e.credit)
            .filter(c => c.creditorId === userId)
            .map(c => c.amount)
            .reduce((a, c) => a + c, 0);
        return loan;
    }

    /* calcualte the financial table containing
     * userId, name, isCurrentUser, loan, dept, balance
     */
    const calculateFinancials = (event) => {
        const results = [];
        for (const p of event.participants) {
            var dept = calculateUserDebt(p.id)
            var loan = calculateUserLoan(p.id)
            results.push({ userId: p.id, username: p.username, isCurrentUser: p.id === userId, loan, dept, balance: loan - dept, currency: event.currency })
        }
        return results.sort(a => -1 * a.balance);
    }

    return (
        <div className="container ">
            {!loading && event ?
                (<>
                    <h2 className="display-2">{event.title}</h2>
                    <div className="d-flex w-100 justify-content-between">
                        <span className="small">
                            <DateFormat date={event.startDate} />&nbsp;-&nbsp;
                            <DateFormat date={event.endDate} />
                        </span>
                    </div>
                    <div>
                        <p className="mt-3 display-5">Total expenses: <span className="text-muted"><strong>{calculateExpenseSummary()}</strong></span> {event.currency}</p>
                    </div>

                    {/* expenses */}
                    <div>
                        <ParticipantMoneyList financials={calculateFinancials(event)} />
                        <div className="mt-5">
                            <EventFinancialsChart financials={calculateFinancials(event)} />
                        </div>
                    </div>
                </>) : null
            }
        </div >
    )
}

export default EventFinancials
