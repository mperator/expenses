import React, { useEffect, useState, useRef } from 'react'
import useClient from './../../hooks/useClient'
import useForm from './../../hooks/useForm'
import { useHistory, useParams } from 'react-router';
import dayjs from 'dayjs'

import bootstrap from 'bootstrap/dist/js/bootstrap.min.js';
import useAuth from './../../hooks/useAuth';
import Toast from './../layout/Toast';
import EventFormular from './EventFormular'

const EventEdit = () => {
    const history = useHistory();
    const params = useParams();
    const { userId } = useAuth();
    const { getEventAsync, putEventAsync, postEventAsync } = useClient();

    const [loading, setLoading] = useState(true);
    const { state, error, handleFormChange, setError, setForm } = useForm({
        title: "",
        description: "",
        startDate: dayjs(new Date()).format('YYYY-MM-DD'),
        endDate: dayjs(new Date()).format('YYYY-MM-DD')
    });
    const [participants, setParticipants] = useState([]);

    useEffect(() => {
        // load event using the incoming id
        if (params.id) {
            (async () => {
                const event = await getEventAsync(params.id);
                setForm({
                    title: event.title,
                    description: event.description,
                    startDate: dayjs(event.startDate).format('YYYY-MM-DD'),
                    endDate: dayjs(event.endDate).format('YYYY-MM-DD'),
                });
                // map participants
                setParticipants(
                    event.participants.map(p => ({
                        ...p,
                        creator: p.id == event.creatorId,
                        currentUser: p.id == userId
                    }))
                );
                setLoading(false);
            })();
        } else setLoading(false);
    }, [])

    const updateAsync = async () => {
        const filteredParticipantsIds = participants.map(participant => {
            return participant.id;
        });
        try {
            const response = await putEventAsync(params.id, {
                title: state.title,
                description: state.description,
                startDate: state.startDate,
                endDate: state.endDate,
                participantIds: filteredParticipantsIds
            });
            history.goBack();
        } catch (error) {
            // show the error toast in case something went wrong and stay on current page
            console.log(error);
            triggerErrorToast();
            //TODO: implement error messages of fluent validation
            // setError(s => ({
            //     title: (error.Title && error.Title[0]) || "",
            //     description: (error.Description && error.Description[0]) || "",
            //     startDate: (error.StartDate && error.StartDate[0]) || "",
            //     endDate: (error.EndDate && error.EndDate[0]) || ""
            // }))
        }
    }

    const triggerErrorToast = () => {
        const errorToast = new bootstrap.Toast(document.getElementById('errorToast'));
        errorToast.show();
    }

    const handleSubmitAsync = async (e) => {
        e.preventDefault();
        if (params.id) {
            await updateAsync();
        }
    }

    const handleCancel = () => {
        history.goBack();
    }

    // function for search
    const handleParticipantSearchDelete = (id) => {
        let updatedParticipants = participants.filter(a => a.id !== id);
        setParticipants([...updatedParticipants]);
    }

    const handleParticipantSearchAdd = (a) => {
        setParticipants([...participants, a]);
    }

    return (
        <>
            <div className="container mt-4">
                {loading ? null : 
                <EventFormular
                    title={"Update Event"}
                    state={state}
                    error={error}
                    handleFormChange={handleFormChange}
                    handleParticipantSearchAdd={handleParticipantSearchAdd}
                    handleParticipantSearchDelete={handleParticipantSearchDelete}
                    participants={participants}>
                    <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>{params.id ? "Update" : "Create"}</button>
                    <button type="button" className="btn btn-outline-secondary" onClick={handleCancel}>Cancel</button>
                </EventFormular>
                }
                <Toast idString="errorToast" />
            </div>
        </>
    )
}

export default EventEdit
