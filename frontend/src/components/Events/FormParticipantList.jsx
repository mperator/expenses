import React from 'react'
import FormParticipantListItem from './FormParticipantListItem';

const FormParticipantList = ({ participants, handleParticipantSearchDelete }) => {
    const sortedParticipants = participants.sort(a => a.creator ? -1 : 1)
    return (
        <div className="mb-3">
            <ul className="list-group">
                {sortedParticipants.map(a => (
                    <FormParticipantListItem key={a.id} participant={a} handleParticipantSearchDelete={handleParticipantSearchDelete} />
                ))}
            </ul>
        </div>
    )
}

export default FormParticipantList
