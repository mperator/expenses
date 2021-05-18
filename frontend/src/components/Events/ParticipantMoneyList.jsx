import React from 'react'
import ParticipantMoney from './ParticipantMoney'

const ParticipantMoneyList = ({ financials }) => {
    return (
        <>
            <h3>Participants</h3>
            <div className="mt-4">
                {financials.map((f, i) => (
                    <ParticipantMoney key={f.userId + `${i}`} username={f.username} balance={f.balance} currency={f.currency} />
                ))}
            </div>
        </>
    )
}

export default ParticipantMoneyList
