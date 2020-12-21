import React from 'react'
import dayjs from 'dayjs'
import { Link } from 'react-router-dom'

const Event = ({ id, title, description, startDate, endDate }) => {

    return (
        <div className="card mb-3">
            <div className="card-body">
                <h5 className="card-title">{title}</h5>
                <h6 className="card-subtitle mb-2 text-muted">{dayjs(startDate).format('DD/MM/YYYY')} - {dayjs(endDate).format('DD/MM/YYYY')}</h6>
                <p className="card-text">{description}</p>
                <Link to={{ pathname: `/event/view/${id}`, state: { id: id } }} className="stretched-link" />
            </div>
        </div>
    )
}

export default Event