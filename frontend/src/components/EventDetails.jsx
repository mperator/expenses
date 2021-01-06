import React, { useEffect, useState, useRef } from 'react'
import useClient from '../hooks/useClient'
import { useHistory, useParams } from 'react-router';
import { Link } from 'react-router-dom';

import dayjs from 'dayjs'

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



    return (
        <div className="container mt-4">
            {loading ? <p>Loading ....</p> :
            <>

                <p>{!loading && calculateExpenseSummary()}</p>

            <Link className="btn btn-outline-primary" to={`/event/editor/${params.id}`}>Edit...</Link>
            <button className="btn btn-outline-danger">Delete</button>

            <h2>{event.title}</h2>
            <p className="lead">{event.description}</p>

            <div>
                <h3>Gesamt Auslagen</h3>
                <p>-200€</p>
            </div>
            {event.attendees.map(a => (
                <div key={a.id}>
                    <h3>{a.name}</h3>
                </div>
            ))}


            <h2>Expenses</h2>
            {event.expenses.map(e => (
                <div key={e.id}>
                    <h3>{e.title}</h3>
                    <p>{e.description}</p>
                    <p>{e.date}</p>
                </div>
            ))}

            </>}
        </div>
    )
}

export default EventDetails
