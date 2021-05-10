import React, { useEffect, useState } from 'react';
import useClient from './../../hooks/useClient';
import { useParams } from 'react-router';

import LinkButtonEdit from '../LinkButtonEdit';
import LinkButtonFinancial from '../LinkButtonFinancial'

import DateFormat from '../DateFormat';
import ExpenseList from './ExpenseList';

const Wip = () => {
    const { getEventAsync } = useClient();
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
                        <div className="d-flex flex-row-reverse">
                            <div className="btn-group">
                                <LinkButtonFinancial to={`/wip/${params.id}/financials`} />
                                <LinkButtonEdit to={`/event/editor/${params.id}`} />
                            </div>
                        </div>
                    </div>
                    <div>
                        <p className="mt-3">{event.description}</p>
                    </div>

                    <ExpenseList eventId={event.id} expenses={event.expenses} />
                </>) : null}
        </div>
    )
}

export default Wip
