import React from 'react'

const Event = ({title, description}) => {
    return (
        <div>
            <h3>{title}</h3>
            <p>{description}</p>
        </div>
    )
}

export default Event