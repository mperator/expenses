import React, { useEffect, useState, useRef } from 'react'
import useClient from '../hooks/useClient'
import useForm from '../hooks/useForm'
import { useHistory, useParams } from 'react-router';
import { Link } from 'react-router-dom';

import FormInput from './layout/FormInput'
import dayjs from 'dayjs'

// TODO: what happens if parms.id is invalid?
const EventEditor = () => {
    const { getEventAsync, putEventAsync, postEventAsync, getAttendeeAsync } = useClient();
    const history = useHistory();
    const params = useParams();

    const { state, error, handleFormChange, setError, setForm } = useForm({
        title: "",
        description: "",
        startDate: dayjs(new Date()).format('YYYY-MM-DD'),
        endDate: dayjs(new Date()).format('YYYY-MM-DD')
    });

    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // load event using the incoming id
        if (params.id) {
            (async () => {
                // try catch ignore or redirect when failing
                const event = await getEventAsync(params.id);
                // TODO: on error navigate back?

                setForm({
                    title: event.title,
                    description: event.description,
                    startDate: dayjs(event.startDate).format('YYYY-MM-DD'),
                    endDate: dayjs(event.endDate).format('YYYY-MM-DD'),
                });

                setAttendees(
                    event.attendees
                );

                setLoading(false);
            })();
        } else {
            setLoading(false);
        }
    }, [])

    const [search, setSearch] = useState({
        query: "",
        attendees: []
    });

    const [attendees, setAttendees] = useState([]);
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
                } catch (error) { }
            }

            setSearch(s => ({
                ...s,
                attendees
            }));
        })()
    }, [search.query])

    const selectAttendee = (a) => {
        setAttendees([...attendees, a]);
        setSearch({
            query: "",
            attendees: []
        })
    }

    const handleSearch = async (e) => {
        const query = e.target.value;
        setSearch(s => ({
            ...s,
            query
        }));
    }

    const createAsync = async () => {
        console.log("hello create")

        try {
            await postEventAsync({
                title: state.title,
                description: state.description,
                startDate: state.startDate,
                endDate: state.endDate,
                attendees: attendees
            });
            history.goBack();
        } catch (error) {
            setError(s => ({
                title: (error.Title && error.Title[0]) || "",
                description: (error.Description && error.Description[0]) || "",
                startDate: (error.StartDate && error.StartDate[0]) || "",
                endDate: (error.EndDate && error.EndDate[0]) || ""
            }))
        }
    }

    const updateAsync = async () => {
        console.log("hello update")

        try {
            await putEventAsync(params.id, {
                title: state.title,
                description: state.description,
                startDate: state.startDate,
                endDate: state.endDate,
                attendees: attendees
            });
            history.goBack();
        } catch (error) {
            setError(s => ({
                title: (error.Title && error.Title[0]) || "",
                description: (error.Description && error.Description[0]) || "",
                startDate: (error.StartDate && error.StartDate[0]) || "",
                endDate: (error.EndDate && error.EndDate[0]) || ""
            }))
        }
    }

    const handleSubmitAsync = async (e) => {
        e.preventDefault();

        if (params.id) {
            await updateAsync();
        } else {
            await createAsync();
        }
    }

    const handleDeleteAttendee = (id) => {
        let updatedAttendees = attendees.filter(a => a.id !== id);
        setAttendees([...updatedAttendees]);
    }

    const handleCancel = () => {
        history.goBack();
    }

    return (
        <div className="container mt-4">
            <h2>{params.id ? "Update Event" : "Create Event"}</h2>
            {loading ? <p>Loading ...</p> :
                <form className="">
                    <FormInput type="text" id="title" label="Title" placeholder="My event title ..." value={state.title} handleChange={handleFormChange} error={error.title}
                    />
                    <FormInput type="textarea" id="description" label="Description" placeholder="My event description ..." value={state.description} handleChange={handleFormChange} error={error.description}
                    />
                    <FormInput type="date" id="startDate" label="Start Date" value={state.startDate} handleChange={handleFormChange} error={error.startDate}
                    />
                    <FormInput type="date" id="endDate" label="End Date" value={state.endDate} handleChange={handleFormChange} error={error.endDate}
                    />

                    <div className="mb-3">
                        <label htmlFor="search" className="form-label">Attendees</label>
                        <input className="form-control" id="search" name="search" type="text" value={search.query} onChange={handleSearch} placeholder="Search for users ..." autoComplete="off"></input>

                        <div className="list-group">
                            {search.attendees.map(a => (
                                <button key={a.id} type="button" className="list-group-item list-group-item-action"
                                    onClick={e => { e.preventDefault(); selectAttendee(a); }}
                                >{a.name}</button>
                            ))}
                        </div>
                    </div>

                    {/* TODO */}
                    {/* https://medium.com/swlh/creating-real-time-autocompletion-with-react-the-complete-guide-39a3bee7e38c */}
                    {/* <div className="mb-3">
                        <label for="exampleDataList" className="form-label">Datalist example</label>
                        <input className="form-control" list="datalistOptions" id="exampleDataList" placeholder="Type to search..." />
                        <datalist id="datalistOptions">
                            {search.attendees.map(a => (
                                <option key={a.id} value={a.name} />
                            ))}
                        </datalist>
                    </div> */}
                    <div className="mb-3">
                        <ul className="list-group">
                            {attendees.map(a => (
                                <li key={a.id} onClick={() => handleDeleteAttendee(a.id)} className="list-group-item d-flex justify-content-between align-items-center">
                                    {a.name}
                                    <button type="button" className="btn" >
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                            <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                                        </svg>
                                    </button>
                                </li>
                            ))}
                        </ul>
                    </div>
                    <div className="d-grid gap-2 d-flex justify-content-end">
                        <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>{params.id ? "Update" : "Create"}</button>
                        <button type="button" className="btn btn-outline-secondary" onClick={handleCancel}>Cancel</button>
                    </div>
                </form>}
        </div>
    )
}

export default EventEditor
