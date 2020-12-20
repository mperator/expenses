import React, { useState } from 'react';
import { useHistory } from 'react-router';
import { Link } from 'react-router-dom';
import useClient from '../hooks/useClient';

export default function EventEdit(props) {

    const { id, title, description, startDate, endDate, currency, creator, attendees, expenses } = props.location.state.event;

    const [event, setEvent] = useState({
        id: props.match.params.id,
        title: title,
        description: description,
        // format the date for a better view experience
        startDate: startDate.substring(0, 10),
        endDate: endDate.substring(0, 10),
        attendees: attendees,
        expenses: expenses
    });

    const { putEventAsync } = useClient();
    const history = useHistory();

    // const [error, setError] = useState({
    //     title: "",
    //     description: "",
    //     startDate: "",
    //     endDate: ""
    // });

    //TODO: check date formatting on dashboard --> we should check timezone of user an adjust the displayed dates accordingly
    //TODO: adding an attendee to create event throws warning
    //TODO: save changes in local storage in case of site reload
    //TODO: validation of input

    const saveChanges = async (id) => {
        // setEvent({ editMode: true })
        //TODO: save process
        console.log('save everything')
        const data = {
            title: event.title,
            description: event.description,
            startDate: event.startDate,
            endDate: event.endDate
        };

        const response = await putEventAsync(event.id, data);
        console.log(response);
    }

    const handleChange = (e) => {
        setEvent(event => ({
            ...event,
            [e.target.name]: e.target.value
        }))
    }

    return (
        <div className="card mt-3 ms-3 me-3">
            <div className="card-header">
                <div className="row g-3">
                    <div className="col-sm-8 form-floating">
                        <input type="text" className="form-control" id="floatingTitle" onChange={handleChange} name="title" value={event.title} placeholder="name@example.com" />
                        <label htmlFor="floatingTitle">Title</label>
                    </div>
                    <div className="col-1">
                        <button type="button" className="btn btn-primary mt-2" onClick={() => saveChanges()}>Save</button>
                    </div>
                    <div className="col-1">
                        <Link to={{ pathname: `/event/view/${id}`, state: { event: props.location.state.event } }}>
                            <button type="button" className="btn btn-primary mt-2">Back</button>
                        </Link>
                    </div>
                </div>
            </div>
            <div className="card-body">
                <form className="form-floating mb-3">
                    <input type="text" className="form-control" id="floatingDescription" placeholder="name@example.com" name="description" onChange={handleChange} value={event.description} />
                    <label htmlFor="floatingDescription">Description</label>
                </form>
                <div id="datesContainer">
                    <div className="mb-3 me-3">
                        <label htmlFor="startDate" className="form-label">Start Date</label>
                        <input className="form-control" id="startDate" name="startDate"
                            type="date" value={event.startDate} onChange={handleChange}
                        ></input>
                    </div>
                    <div className="mb-3">
                        <label htmlFor="endDate" className="form-label">End Date</label>
                        <input className="form-control" id="endDate" name="endDate"
                            type="date" value={event.endDate} onChange={handleChange}
                        ></input>
                    </div>
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
        </div >
    )
}
