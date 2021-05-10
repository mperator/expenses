import React from 'react'

import './Participant.css'

const Participant = ({ username }) => {
    return (
        <div className="d-flex flex-row align-items-center">
            <div className="participant">
                <div className="icon">
                    <strong>{username[0]?.toUpperCase()}</strong>
                </div>
            </div>
            <div className="mx-2">
                {username}
            </div>
        </div>
    )
}

export default Participant
