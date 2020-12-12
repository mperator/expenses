import React, { useState } from 'react'
// import useClient from '../hooks/useClient'
import { useHistory } from 'react-router';

export default function ViewEditEvent(props) {

    const { id, title, description, startDate, endDate, currency, creator, creatorId } = props.location.state.event;


    const history = useHistory();
    // const { postEventAsync, getEventByIdAsync } = useClient();

    //store everything in localstorage

    // console.log(props.location)
    const [event, setEvent] = useState({
        id: props.match.params.id,
        title: title,
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

    // effect das useParams nutzt zum Laden des Events
    return (
        <div className="card">
            <h5 className="card-header">{title}</h5>
            <div className="card-body">
                <div className="card" style={{ width: '18rem' }}>
                    <div className="card-header">
                        Participants
                    </div>
                    <ul className="list-group list-group-flush">
                        <li className="list-group-item">
                            Participant 1
                        </li>
                        <li className="list-group-item">
                            Participant 2
                        </li>
                    </ul>
                </div>
                {/* <h5 className="card-title">Special title treatment</h5>
                <p className="card-text">With supporting text below as a natural lead-in to additional content.</p>
                <a href="" className="btn btn-primary">Go somewhere</a> */}
            </div>
        </div>
    )
}
