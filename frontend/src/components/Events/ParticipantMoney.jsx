import React from 'react'
import Money from './Money'
import Participant from './Participant'

const ParticipantMoney = ({ username, balance, currency }) => {
    return (
        <div className="mb-3 d-flex flex-row justify-content-between align-items-center">
            <Participant username={username} />
            <Money balance={balance} currency={currency} />
        </div>
    )
}

export default ParticipantMoney
