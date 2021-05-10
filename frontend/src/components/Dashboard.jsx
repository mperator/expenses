import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import useClient from '../hooks/useClient'
import DashboardEvent from './DashboardEvent'
import SkeletonLoader from './SkeletonLoader'
import './Dashboard.css'

/* Get a list of events and display them. */
const Dashboard = () => {
    const { getEventsAsync, getFilteredEventsAsync } = useClient();
    const [events, setEvents] = useState([]);
    const [fetchInProgress, setFetchInProgress] = useState(true);
    const [searchBarEnabled, setSearchBarEnabled] = useState(false);
    const [eventSearch, setEventSearch] = useState("");

    useEffect(() => {
        (async () => {
            await loadEventsAsync();
        })();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    useEffect(() => {
        (async () => {
            let events = [];
            if (eventSearch !== "") {
                try {
                    events = await getFilteredEventsAsync(eventSearch)
                    setEvents(events);
                } catch (error) { }
            }
            // load all events in case query is empty but searchbar is still visible
            if (eventSearch === "" && searchBarEnabled) {
                await loadEventsAsync();
            }
        })()
    }, [eventSearch])

    const loadEventsAsync = async () => {
        try {
            const events = await getEventsAsync();
            setEvents(e => events);
            setFetchInProgress(false);
        } catch (error) {
            console.log(error)
        }
    }
    const handleSearchButton = () => {
        setSearchBarEnabled(!searchBarEnabled);
    }

    const handleEventSearch = (e) => {
        const eventQuery = e.target.value;
        setEventSearch(eventQuery);
    }

    return (
        <>
            { fetchInProgress ? <SkeletonLoader />
                : <div className="container mt-3">
                    <h2>My Events</h2>
                    <div className="row justify-content-between">
                        <div className="col-6">
                            {/* Placeholder for coming pagination */}
                        </div>
                        <div className="col-auto">
                            <Link className="text-dark" to="/event/editor">
                                <svg width="0.8em" height="0.8em" viewBox="0 0 16 16" className="addButton bi bi-plus-circle-fill text-primary" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                    <path fillRule="evenodd" d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                                </svg>
                            </Link>
                            <button type="button" className="btn btn-primary rounded-circle me-1" onClick={handleSearchButton}>
                                <svg xmlns="http://www.w3.org/2000/svg" width="0.8em" height="0.8em" fill="currentColor" className="bi bi-search" viewBox="0 0 16 16">
                                    <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
                                </svg>
                            </button>
                        </div>
                    </div>
                    {searchBarEnabled ? <form className="d-flex mt-3">
                        <input className="form-control" type="search" onChange={(e) => handleEventSearch(e)} placeholder="Search for an event title ..." aria-label="Search for an event title ..." />
                    </form>
                        : null}
                    <div className="mt-3">
                        {
                            events.length !== 0 ?
                                events && events.map(e => (
                                    <DashboardEvent key={e.id} {...e} />
                                ))
                                : <div>
                                    No events found. Sorry about that. Go and create a new one</div>
                        }
                    </div>
                </div>
            }
        </>
    )
}

export default Dashboard
