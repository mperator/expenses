import React, { useState } from 'react'

export default function EventEdit(props) {

    console.log(props)

    const { id, title, description, startDate, endDate, currency, creator, attendees, expenses } = props.location.state.event;

    //store everything in localstorage

    const [event, setEvent] = useState({
        id: props.match.params.id,
        title: title,
        description: description,
        startDate: startDate.substring(0, 10),
        endDate: endDate.substring(0, 10),
        attendees: attendees,
        expenses: expenses,
        editMode: false
    });
    console.log(event.endDate)
    console.log(event.endDate.substring(0, 10));
    // const [error, setError] = useState({
    //     title: "",
    //     description: "",
    //     startDate: "",
    //     endDate: ""
    // });

    //TODO: check date formatting on dashboard --> we should check timezone of user an adjust the displayed dates accordingly
    //TODO: adding an attendee to create event throws warning

    const saveChanges = (id) => {
        // setEvent({ editMode: true })
        //TODO: save process
        console.log('save everything')
    }

    const handleChange = () => {

    }

    const formatDate = (date) => {

    }
    //TODO: keep going here positioning Title and save button in edit mode on
    // effect das useParams nutzt zum Laden des Events
    return (
        <div className="card mt-3 ml-3 mr-3">
            <div className="card-header">
                <div className="row row-cols-lg-auto g-3 align-items-center">
                    {/* <div className="col-12">

                        <div className="form-floating mb-3 col-sm-7">
                            <input type="text" className="form-control" id="floatingTitle" placeholder="name@example.com" onChange={handleChange} value={event.title} />
                            <label htmlFor="floatingTitle">Title</label>
                        </div>
                    </div>
                    <div className="col-6">

                        <button type="button" className="btn btn-dark" onClick={() => saveChanges()}>Save</button>
                    </div> */}
                    <div class="row">
                        <div class="col">
                            <input type="text" className="form-control" id="floatingTitle" placeholder="name@example.com" />
                            <label htmlFor="floatingTitle">Title</label>
                        </div>
                        <div class="col">
                            <input type="text" class="form-control" placeholder="Last name" aria-label="Last name" />
                        </div>
                    </div>
                </div>
            </div>
            <div className="card-body">
                <form className="form-floating mb-3">
                    <input type="text" className="form-control" id="floatingDescription" placeholder="name@example.com" onChange={handleChange} value={event.description} />
                    <label htmlFor="floatingDescription">Description</label>
                </form>
                <div id="datesContainer">
                    <div className="mb-3">
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

                {/* <div id="participantExpenseContainer">

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

                    <div className="card ml-3" style={{ width: '30rem' }}>
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
                </div> */}
            </div>
        </div >
    )
}
