import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import EventFinancialsChart from './EventFinancialsChart';
import useClient from '../../hooks/useClient';
import useAuth from '../../hooks/useAuth';
import dayjs from 'dayjs';

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

    const calculateDelta = (userId) => {
        return calculateUserLoan(userId) - calculateUserDebt(userId);
    }

    /* calcualte the financial table containing
     * userId, name, isCurrentUser, loan, dept, balance
     */
    const calculateFinancials = (event) => {
        const table = [];
        for (const p of event.participants) {
            var dept = calculateUserDebt(p.id)
            var loan = calculateUserLoan(p.id)
            table.push({ userId: p.id, username: p.username, isCurrentUser: p.id == userId, loan, dept, balance: dept - loan })
        }
        table.sort(a => -1 * a.balance);
        return table;
    }

    //TODO: if total expense is negative show font in red and vice versa
    //TODO: add logo in front of user name
    //TODO: make each expense clickable and open expense editor for corresponding expense after click
    //TODO: delete button has no functionality yet
    return (
        <div className="container ">
            {event ?
                (<>
                    <h2 className="display-2">{event.title}</h2>
                    <div className="d-flex w-100 justify-content-between">
                        <span className="small">{dayjs(event.startDate).format("DD.MM.YYYY")} - {dayjs(event.endDate).format("DD.MM.YYYY")}</span>
                    </div>
                    <div>
                        <p className="mt-3 display-5">Total expenses: <span className="text-muted"><strong>{calculateExpenseSummary()}</strong></span> {event.currency}</p>
                    </div>

                    {/* expenses */}
                    <div>
                        <h3>Participants</h3>
                        {event.participants.map(a => (
                            <li key={a.id} className="list-group-item">
                                <div className="row justify-content-between align-items-center">
                                    <div className="col-auto">
                                        {a.username}
                                    </div>
                                    <div className="col-auto">
                                        <div className="col-auto">
                                            {calculateDelta(a.id)} {event.currency}
                                        </div>
                                    </div>
                                </div>
                            </li>
                        ))}
                        <EventFinancialsChart financials={calculateFinancials(event)} />
                    </div>
                </>) : null
            }
        </div >
    )
}

export default EventFinancials
