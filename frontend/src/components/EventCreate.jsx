import React, { useEffect, useState } from 'react'
import useClient from '../hooks/useClient'
import { useHistory } from 'react-router';

const EventCreate = () => {
    const history = useHistory();
    const { postEventAsync, getAttendeeAsync } = useClient();

    const [state, setState] = useState({
        title: "",
        description: "",
        startDate: "",
        endDate: ""
    });

    const [error, setError] = useState({
        title: "",
        description: "",
        startDate: "",
        endDate: ""
    });

    const [search, setSearch] = useState({
        query: "",
        attendees: []
    });

    const [attendees, setAttendees] = useState([]);

    const handleChange = (e) => {
        setState(s => ({
            ...s,
            [e.target.name]: e.target.value
        }))

        setError(s => ({
            ...s,
            [e.target.name]: ""
        }))
    }

    const handleCreateAsync = async (e) => {
        e.preventDefault();
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

    const isValid = (e) => {
        return e && " is-invalid";
    }

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

    return (
        <div className="container mt-4">
            <h2>Create Event</h2>
            <form className="">
                <div className="mb-3">
                    <label htmlFor="title" className="form-label">Title</label>
                    <input className={"form-control" + isValid(error.title)} id="title" name="title"
                        type="text" placeholder="My event title ..." value={state.title} onChange={handleChange}
                    />
                    {error.title && <div className="invalid-feedback">{error.title}</div>}
                </div>
                <div className="mb-3">
                    <label htmlFor="description" className="form-label">Description</label>
                    <textarea className={"form-control" + isValid(error.description)} id="description" name="description"
                        placeholder="My event description ..." value={state.description} onChange={handleChange}
                    ></textarea>
                    {error.description && <div className="invalid-feedback">{error.description}</div>}
                </div>
                <div className="mb-3">
                    <label htmlFor="startDate" className="form-label">Start Date</label>
                    <input className={"form-control" + isValid(error.startDate)} id="startDate" name="startDate"
                        type="date" value={state.startDate} onChange={handleChange}
                    ></input>
                    {error.startDate && <div className="invalid-feedback">{error.startDate}</div>}
                </div>
                <div className="mb-3">
                    <label htmlFor="endDate" className="form-label">End Date</label>
                    <input className={"form-control" + isValid(error.endDate)} id="endDate" name="endDate"
                        type="date" value={state.endDate} onChange={handleChange}
                    ></input>
                    {error.endDate && <div className="invalid-feedback">{error.endDate}</div>}
                </div>

                <div className="mb-3">
                    <label htmlFor="search" className="form-label">Search</label>
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
                    <input className="form-control" list="datalistOptions" id="exampleDataList" placeholder="Type to search..."/>
                    <datalist id="datalistOptions">
                        {search.attendees.map(a => (
                            <option key={a.id} value={a.name}/>
                        ))}
                    </datalist>
                </div> */}

                <div className="mb-3">
                    {attendees.map(a => (
                        <div>{a.name}</div>
                    ))}
                </div>

                <div className="col-12 text-right">
                    <button className="btn btn-primary" type="submit" onClick={handleCreateAsync}>Create</button>
                </div>
            </form>
        </div>
    )
}

export default EventCreate
