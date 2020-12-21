import React, { useEffect, useState } from 'react';
import { useLocation } from 'react-router';
import { Link } from 'react-router-dom';
import useClient from '../hooks/useClient';
import './EventViewEdit.css';

export default function EventViewEdit() {

    const location = useLocation();

    const { id } = location.state.id;

    //TODO: store everything in localstorage

    const { getAttendeeAsync, putEventAsync, getEventAsync } = useClient();

    const [event, setEvent] = useState({
        id: id,
        title: '',
        description: '',
        startDate: '',
        endDate: '',
        currency: '',
        creator: '',
        expenses: [],
        attendees: []
    });

    const [attendeeSearch, setAttendeeSearch] = useState(false);

    const [editMode, setEditMode] = useState(false);

    const [search, setSearch] = useState({
        query: "",
        attendees: []
    });


    useEffect(() => {
        (async () => {
            let attendees = [];
            if (search.query !== "") {
                try {
                    attendees = await getAttendeeAsync(search.query);
                    setSearch(s => ({
                        ...s,
                        attendees
                    }));
                } catch (error) {
                    console.log("FEHLER", error)
                }
            }
            setSearch(s => ({
                ...s,
                attendees
            }));
        })()
    }, [search.query]);

    useEffect(() => {
        // load event using the incoming id
        (async () => {
            const event = await getEventAsync(location.state.id);

            setEvent({
                ...event,
                startDate: event.startDate.substring(0, 10),
                endDate: event.endDate.substring(0, 10),
                // needed for not mandatory fields in case they are null
                description: event.description || ''
            });
        })();
    }, [])

    useEffect(() => {

    }, [event])

    const handleSearch = async (e) => {
        const query = e.target.value;
        setSearch(s => ({
            ...s,
            query
        }));
    }

    //TODO: somehow user feedback in case something went wrong
    const selectAttendee = async (a) => {
        const attendeesData = [...event.attendees, a];
        setEvent({
            ...event,
            attendees: attendeesData
        });

        setSearch({
            query: "",
            attendees: []
        });

        const data = {
            title: event.title,
            description: event.description,
            startDate: event.startDate,
            endDate: event.endDate,
            attendees: attendeesData
        }

        const response = await putEventAsync(event.id, data);
        // in case of successful putting fetch again
        if (response != null && response.status === 204) {
            console.log("RESPONSE", response);
        }
        setAttendeeSearch(!attendeeSearch);
    }

    const addAttendee = () => {
        setAttendeeSearch(!attendeeSearch);
    }

    const toggleEditMode = async () => {
        if (editMode) {
            // save event and send it to backend
            const response = await putEventAsync(event.id, event);
            if (response != null && response.status === 204) {
                console.log("saved event successfully");
                setEditMode(!editMode);
            }
        } else {
            // just enable edit mode
            setEditMode(!editMode);
        }
    }

    return (
        <div className="card mt-3 ms-3 me-3">
            <h5 className="card-header">
                {event.title}
                <button type="button" className="btn btn-primary ms-3" onClick={toggleEditMode}>{!editMode ? "Edit" : "Save"}</button>
                <Link to="/dashboard">
                    <button type="button" className="btn btn-primary ms-3">Back</button>
                </Link>
            </h5>
            <div className="card-body">
                <form className="form-floating mb-3">
                    <input type="text" className="form-control" id="floatingInputValue" readOnly={!editMode} placeholder="name@example.com" value={event.description} />
                    <label htmlFor="floatingInputValue">Description</label>
                </form>
                <div id="datesContainer">
                    <form className="form-floating mb-3 me-3">
                        <input type="date" className="form-control" id="startDate" readOnly={!editMode} placeholder="name@example.com" value={event.startDate} />
                        <label htmlFor="startDate">Start date</label>
                    </form>
                    <form className="form-floating">
                        <input type="date" className="form-control" id="endDate" readOnly={!editMode} placeholder="name@example.com" value={event.endDate} />
                        <label htmlFor="endDate">End date</label>
                    </form>
                </div>
                <div id="participantExpenseContainer">
                    <div className="card" style={{ width: '20rem' }}>
                        <div className="card-header">
                            Participants
                        </div>
                        <ul className="list-group list-group-flush">
                            {event.attendees && event.attendees.map(a => (
                                <li className="list-group-item" key={a.id}>
                                    {a.name}
                                </li>
                            ))}
                            {attendeeSearch ?
                                <><input className="form-control" id="search" name="search" type="text" value={search.query} onChange={handleSearch} placeholder="Search for users ..." autoComplete="off" />
                                    <div className="list-group">
                                        {search.attendees.map(a => (
                                            <button key={a.id} type="button" className="list-group-item list-group-item-action"
                                                onClick={e => { e.preventDefault(); selectAttendee(a); }}
                                            >{a.name}</button>
                                        ))}
                                    </div></> : null
                            }
                            <li className="list-group-item" style={{ cursor: 'pointer' }} onClick={addAttendee}>
                                <svg width="0.8em" height="0.8em" viewBox="0 0 16 16" className="addButton bi bi-plus-circle-fill text-primary" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                    <path fillRule="evenodd" d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                                </svg>
                            </li>
                        </ul>
                    </div>
                    <div className="card ms-3" style={{ width: '30rem' }}>
                        <div className="card-header">
                            Expenses
                        </div>
                        <div className="card-body">
                            {event.expenses && event.expenses.map(e => (
                                <div key={e.id}>
                                    <h5 className="card-title">{e.title}</h5>
                                    <h6 className="card-subtitle mb-2 text-muted">{e.amount} {e.currency}</h6>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
