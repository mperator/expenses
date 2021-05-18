import React from 'react'
import { Link } from 'react-router-dom'
import DateFormat from '../DateFormat'

const DashboardEventListItem = ({ id, title, description, startDate, endDate }) => {

    return (
        <div className="card mb-3">
            <div className="card-body">
                <h5 className="card-title">{title}</h5>
                <h6 className="card-subtitle mb-2 text-muted">
                    <DateFormat date={startDate} />&nbsp;-&nbsp;<DateFormat date={endDate} />
                </h6>
                <p className="card-text">{description}</p>
                <Link to={{ pathname: `/event/${id}`, state: { id: id } }} className="stretched-link" />
            </div>
        </div>
    )
}

export default DashboardEventListItem