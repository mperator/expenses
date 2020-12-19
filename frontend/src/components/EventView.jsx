import React, { useState } from 'react'
import { useHistory } from 'react-router';
import { Link } from 'react-router-dom';
import './EventView.css'

export default function EventView(props) {

    const { id, title, description, startDate, endDate, currency, creator, attendees, expenses } = props.location.state.event;

    //TODO: remove editMode property
    //store everything in localstorage

    const history = useHistory();

    // console.log(props.location)
    const [event, setEvent] = useState({
        id: props.match.params.id,
        title: title,
        // needed for not mandatory fields in case they are null
        description: description || '',
        startDate: startDate,
        endDate: endDate,
        currency: currency,
        creator: creator,
        attendees: attendees,
        expenses: expenses,
        editMode: false
    });

    // const [error, setError] = useState({
    //     title: "",
    //     description: "",
    //     startDate: "",
    //     endDate: ""
    // });

    const toggleEdit = (id) => {
        // setEvent({ editMode: true })
        // history.push(`/event/editor/${id}`)
    }

    //TODO: change type of input for start and end date to date instead of text
    // effect das useParams nutzt zum Laden des Events
    if (!event.editMode) {
        return (
            <div className="card mt-3 ms-3 me-3">
                <h5 className="card-header">{event.title}
                    <Link to={{ pathname: `/event/editor/${id}`, state: { event: props.location.state.event } }} className="stretched-link">
                        <button type="button" className="btn btn-dark">Edit</button>
                    </Link>
                </h5>
                <div className="card-body">
                    <form className="form-floating mb-3">
                        <input type="text" className="form-control" id="floatingInputValue" readOnly placeholder="name@example.com" value={event.description} />
                        <label htmlFor="floatingInputValue">Description</label>
                    </form>
                    <div id="datesContainer">
                        <form className="form-floating mb-3 me-3">
                            <input type="text" className="form-control" id="startDate" readOnly placeholder="name@example.com" value={event.startDate} />
                            <label htmlFor="startDate">Start date</label>
                        </form>
                        <form className="form-floating">
                            <input type="text" className="form-control" id="endDate" readOnly placeholder="name@example.com" value={event.endDate} />
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
            <h5 className="card-header">{event.title}
                <button type="button" className="btn btn-dark" onClick={() => toggleEdit(id)}>Edit</button>
            </h5>
            <div className="card-body">
                <form className="form-floating mb-3">
                    <input type="text" className="form-control" id="floatingInputValue" placeholder="name@example.com" value={event.description} />
                    <label htmlFor="floatingInputValue">Description</label>
                </form>
                <div id="datesContainer">
                    <form className="form-floating mb-3 me-5">
                        <input type="text" className="form-control" id="startDate" placeholder="name@example.com" value={event.startDate} />
                        <label htmlFor="startDate">Start date</label>
                    </form>
                    <form className="form-floating">
                        <input type="text" className="form-control" id="endDate" placeholder="name@example.com" value={event.endDate} />
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
