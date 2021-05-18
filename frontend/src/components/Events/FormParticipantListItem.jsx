import React from 'react'
import ButtonRemove from '../ButtonRemove'

import Participant from './Participant'

const FormParticipantListItem = ({ participant, handleParticipantSearchDelete }) => {
    var disabledCss = participant.creator ? 'disabled' : '';
    var css = `list-group-item d-flex justify-content-between align-items-center ${disabledCss}`;

    return (
        <li key={participant.id} className={css}>
            <Participant username={participant.username} />
            {participant.creator ? null : 
                <ButtonRemove handleClick={() => handleParticipantSearchDelete(participant.id)} />}
        </li>
    )
}

export default FormParticipantListItem
