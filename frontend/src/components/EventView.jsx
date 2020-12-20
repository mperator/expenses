import React, { useEffect, useState } from 'react'
import { useHistory } from 'react-router';
import { Link } from 'react-router-dom';
import useClient from '../hooks/useClient';
import './EventView.css';

export default function EventView(props) {

    console.log("porps eventView", props.location.state)
    const { id, title, description, startDate, endDate, currency, creator, expenses } = props.location.state.event;
    // const { onAttendeeUpdate } = props.location.state.onAttendeeUpdate;

    //TODO: remove editMode property
    //store everything in localstorage

    const history = useHistory();

    const { getAttendeeAsync, putEventAsync, getEventAsync } = useClient();

    // console.log(props.location)
    const [event, setEvent] = useState({
        id: props.match.params.id,
        title: title,
        // needed for not mandatory fields in case they are null
        description: description || '',
        startDate: startDate.substring(0, 10),
        endDate: endDate.substring(0, 10),
        currency: currency,
        creator: creator,
        expenses: expenses
    });

    const [attendeeSearch, setAttendeeSearch] = useState(false);

    const [attendees, setAttendees] = useState(props.location.state.event.attendees);

    const [search, setSearch] = useState({
        query: "",
        attendees: []
    });

    useEffect(() => {
        (async () => {
            let attendees = [];
            if (search.query !== "") {
                try {
                    // console.log("hier")
                    attendees = await getAttendeeAsync(search.query);
                    // console.log("search", attendees)
                    setSearch(s => ({
                        ...s,
                        attendees
                    }));
                } catch (error) {
                    console.log("FEHLER", error)
                }
            }
            console.log("attendees useEffect", attendees)
            setSearch(s => ({
                ...s,
                attendees
            }));
        })()
    }, [search.query]);

    const handleSearch = async (e) => {
        const query = e.target.value;
        setSearch(s => ({
            ...s,
            query
        }));
    }
    //TODO: somehow user feedback in case something went wrong
    const selectAttendee = async (a) => {
        // console.log("intermediate attendees", [...attendees, a])
        const attendeesData = [...attendees, a];
        setAttendees([...attendees, a]);

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
        console.log("DATA", data)
        const response = await putEventAsync(event.id, data);
        // in case of successful putting fetch again
        if (response != null && response.status === 204) {
            console.log("RESPONSE", response);

            // props.location.state.event.attendees.push(a);
        }
        setAttendeeSearch(!attendeeSearch);
    }

    // const [error, setError] = useState({
    //     title: "",
    //     description: "",
    //     startDate: "",
    //     endDate: ""
    // });

    const addAttendee = () => {
        setAttendeeSearch(!attendeeSearch);
    }

    //TODO: change type of input for start and end date to date instead of text
    // effect das useParams nutzt zum Laden des Events
    if (attendeeSearch) {
        return (
            <div className="card mt-3 ms-3 me-3">
                <h5 className="card-header">
                    {event.title}
                    <Link to={{ pathname: `/event/editor/${id}`, state: { event: props.location.state.event } }}>
                        <button type="button" className="btn btn-primary ms-3">Edit</button>
                    </Link>
                    <Link to="/dashboard">
                        <button type="button" className="btn btn-primary ms-3">Back</button>
                    </Link>
                </h5>
                <div className="card-body">
                    <form className="form-floating mb-3">
                        <input type="text" className="form-control" id="floatingInputValue" readOnly placeholder="name@example.com" value={event.description} />
                        <label htmlFor="floatingInputValue">Description</label>
                    </form>
                    <div id="datesContainer">
                        <form className="form-floating mb-3 me-3">
                            <input type="date" className="form-control" id="startDate" readOnly placeholder="name@example.com" value={event.startDate} />
                            <label htmlFor="startDate">Start date</label>
                        </form>
                        <form className="form-floating">
                            <input type="date" className="form-control" id="endDate" readOnly placeholder="name@example.com" value={event.endDate} />
                            <label htmlFor="endDate">End date</label>
                        </form>
                    </div>
                    <div id="participantExpenseContainer">
                        <div className="card" style={{ width: '20rem' }}>
                            <div className="card-header">
                                Participants
                            </div>
                            <ul className="list-group list-group-flush">
                                {attendees && attendees.map(a => (
                                    <li className="list-group-item" key={a.id}>
                                        {a.name}
                                    </li>
                                ))}
                                <input className="form-control" id="search" name="search" type="text" value={search.query} onChange={handleSearch} placeholder="Search for users ..." autoComplete="off" />
                                <div className="list-group">
                                    {search.attendees.map(a => (
                                        <button key={a.id} type="button" className="list-group-item list-group-item-action"
                                            onClick={e => { e.preventDefault(); selectAttendee(a); }}
                                        >{a.name}</button>
                                    ))}
                                </div>
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
    return (
        <div className="card mt-3 ms-3 me-3">
            <h5 className="card-header">
                {event.title}
                <Link to={{ pathname: `/event/editor/${id}`, state: { event: props.location.state.event } }}>
                    <button type="button" className="btn btn-primary ms-3">Edit</button>
                </Link>
                <Link to="/dashboard">
                    <button type="button" className="btn btn-primary ms-3">Back</button>
                </Link>
            </h5>
            <div className="card-body">
                <form className="form-floating mb-3">
                    <input type="text" className="form-control" id="floatingInputValue" readOnly placeholder="name@example.com" value={event.description} />
                    <label htmlFor="floatingInputValue">Description</label>
                </form>
                <div id="datesContainer">
                    <form className="form-floating mb-3 me-3">
                        <input type="date" className="form-control" id="startDate" readOnly placeholder="name@example.com" value={event.startDate} />
                        <label htmlFor="startDate">Start date</label>
                    </form>
                    <form className="form-floating">
                        <input type="date" className="form-control" id="endDate" readOnly placeholder="name@example.com" value={event.endDate} />
                        <label htmlFor="endDate">End date</label>
                    </form>
                </div>
                <div id="participantExpenseContainer">
                    <div className="card" style={{ width: '20rem' }}>
                        <div className="card-header">
                            Participants
                        </div>
                        <ul className="list-group list-group-flush">
                            {attendees && attendees.map(a => (
                                <li className="list-group-item" key={a.id}>
                                    {a.name}
                                </li>
                            ))}
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
