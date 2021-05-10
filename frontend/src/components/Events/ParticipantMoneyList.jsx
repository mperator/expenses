import React from 'react'
import ParticipantMoney from './ParticipantMoney'

const ParticipantMoneyList = ({ financials }) => {
    return (
        <>
            <h3>Participants</h3>
            <div className="mt-4">
                {financials.map(f => (
                    <ParticipantMoney key={f.userId} username={f.username} balance={f.balance} currency={f.currency} />
                ))}
            </div>
        </>
    )
}

export default ParticipantMoneyList
