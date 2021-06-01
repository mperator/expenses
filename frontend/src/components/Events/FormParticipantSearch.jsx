import React, { useState, useEffect } from 'react'
import Participant from './Participant';
import useClient from './../../hooks/useClient'

const FormParticipantSearch = ({ participants, handleParticipantSearchAdd }) => {
    const { getParticipantsByNameAsync } = useClient();

    const [search, setSearch] = useState({
        query: "",
        participants: []
    });

    useEffect(() => {
        (async () => {
            let searchParticipants = [];
            if (search.query !== "") {
                try {
                    searchParticipants = await getParticipantsByNameAsync(search.query);

                    // add additional information
                    const ids = participants.map(p => p.id);
                    searchParticipants = searchParticipants.map(p => ({
                        ...p,
                        exists: ids.includes(p.id)
                    }))

                    setSearch(s => ({
                        ...s,
                        participants: searchParticipants
                    }));
                } catch (error) { }
            }

            setSearch(s => ({
                ...s,
                participants: searchParticipants
            }));
        })()
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [search.query])

    const selectParticipant = (a) => {
        handleParticipantSearchAdd(a);
        setSearch({
            query: "",
            participants: []
        })
    }

    const handleSearch = async (e) => {
        const query = e.target.value;
        setSearch(s => ({
            ...s,
            query
        }));
    }

    return (
        <div className="mb-3">
            <label htmlFor="search" className="form-label">Participants</label>
            <input className="form-control" id="search" name="search" type="text" value={search.query} onChange={handleSearch} placeholder="Type name to search ..." autoComplete="off"></input>

            <div className="list-group">
                {search.participants.map(a => (
                    <button key={a.id} type="button" className={`list-group-item list-group-item-action ${a.exists ? 'disabled' : ''}`}
                        onClick={e => { e.preventDefault(); selectParticipant(a); }}
                    >
                        <Participant username={a.username} />
                    </button>
                ))}
            </div>
        </div>
    )
}

export default FormParticipantSearch
