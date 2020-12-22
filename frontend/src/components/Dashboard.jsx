import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import useClient from '../hooks/useClient'
import './Dashboard.css'

import Event from './Event'

/* Get a list of events and display them. */
const Dashboard = () => {
    const { getEventsAsync } = useClient();
    const [events, setEvents] = useState([]);

    useEffect(() => {
        (async () => {
            await loadEventsAsync();
        })();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const loadEventsAsync = async () => {
        try {
            const events = await getEventsAsync();
            setEvents(e => events);
        } catch (error) {
            console.log(error)
        }
    }

    //TODO: loading circle during fetch process
    return (
        <div className="container mt-4">
            <h2>My Events</h2>
            <div className="mt-3">
                {events && events.map(e => (
                    <Event key={e.id} {...e} />
                ))}
            </div>
            <Link className="text-dark" to="/event/editor">
                <svg width="1em" height="1em" viewBox="0 0 16 16" className="addButton bi bi-plus-circle-fill text-primary" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                    <path fillRule="evenodd" d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                </svg>
            </Link>
        </div>
    )
}

export default Dashboard
