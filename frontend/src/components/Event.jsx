import React from 'react'
import dayjs from 'dayjs'
import { Link } from 'react-router-dom'


const Event = (props) => {

    const { id, title, description, startDate, endDate } = props;

    return (
        <div className="card mb-3">
            <div className="card-body">
                <h5 className="card-title">{title}</h5>
                <h6 className="card-subtitle mb-2 text-muted">{dayjs(startDate).format('DD/MM/YYYY')} - {dayjs(endDate).format('DD/MM/YYYY')}</h6>
                <p className="card-text">{description}</p>
                <Link to={{ pathname: `/event/editor/${id}`, state: { event: props } }} className="stretched-link" />
                {/* <a href={`/event-view/${id}`} className="stretched-link"></a> */}
            </div>
        </div>
    )
}

export default Event