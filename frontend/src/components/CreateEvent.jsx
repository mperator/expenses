import React, { useState } from 'react'
import useClient from '../hooks/useClient'
import { useHistory } from 'react-router';

const CreateEvent = () => {
    const history = useHistory();
    const { postEventAsync } = useClient();

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
                endDate: state.endDate
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
                <div className="col-12">
                    <button className="btn btn-primary" type="submit" onClick={handleCreateAsync}>Create</button>
                </div>
            </form>
        </div>
    )
}

export default CreateEvent
