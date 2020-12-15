import React, { useEffect, useState } from 'react'
import { useHistory } from 'react-router'
import useClient from '../hooks/useClient'


import Event from './Event'

/* Get a list of events and display them. */
const Dashboard = () => {
    const history = useHistory();
    const { getEventAsync } = useClient();
    const [events, setEvents] = useState([]);

    useEffect(() => {
        (async () => {
            await loadEventsAsync();
        })();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const loadEventsAsync = async () => {
        try {
            const events = await getEventAsync();
            setEvents(e => events);
        } catch (error) {
            console.log(error)
        }
    }

    const create = () => [
        history.push('/event/editor')
    ]

    return (
        <div className="container mt-4">
            <h2>My Events</h2>
            <button className="btn btn-primary" onClick={create}>New</button>

            <div className="mt-3">
                {events && events.map(e => (
                    <Event key={e.id} {...e} />
                ))}
            </div>
        </div>
    )
}

export default Dashboard
