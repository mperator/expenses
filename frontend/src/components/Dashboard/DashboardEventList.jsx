import React from 'react'
import DashboardEventListItem from './DashboardEventListItem'

const DashboardEventList = ({ events }) => {
    return (
        <div>
            {
                events.length !== 0 ?
                    events && events.map(e => (
                        <DashboardEventListItem key={e.id} {...e} />
                    ))
                    : <div>
                        No events found. Sorry about that. Go and create a new one</div>
            }
        </div>
    )
}

export default DashboardEventList
