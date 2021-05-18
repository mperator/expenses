import React, { useEffect, useState } from 'react'
import useClient from './../../hooks/useClient'
import useForm from './../../hooks/useForm'
import { useHistory } from 'react-router';
import dayjs from 'dayjs'

import bootstrap from 'bootstrap/dist/js/bootstrap.min.js';
import useAuth from './../../hooks/useAuth';
import Toast from './../layout/Toast';
import EventFormular from './EventFormular'


const EventCreate = () => {
    const history = useHistory();
    const { userId } = useAuth();
    const { postEventAsync, getParticipantByIdAsync } = useClient();

    const [loading, setLoading] = useState(true);
    const { state, error, handleFormChange, setError } = useForm({
        title: "",
        description: "",
        startDate: dayjs(new Date()).format('YYYY-MM-DD'),
        endDate: dayjs(new Date()).format('YYYY-MM-DD')
    });
    const [participants, setParticipants] = useState([]);

    useEffect(() => {
        (async () => {
            const currentUser = await getParticipantByIdAsync(userId);
            const participant = {
                ...currentUser,
                creator: true,
                currentUser: true,
            };

            setParticipants([participant])
            setLoading(false);
        })();
    }, [])

    const createAsync = async () => {
        const filteredParticipantsIds = participants
            .map(p => p.id)
            .filter(p => p !== userId);

        try {
            const response = await postEventAsync({
                title: state.title,
                description: state.description,
                startDate: state.startDate,
                endDate: state.endDate,
                currency: "EUR",
                participantIds: filteredParticipantsIds
            });
            // show the error toast in case something went wrong and stay on current page
            if (response === null) triggerErrorToast();
            else history.goBack();
        } catch (error) {
            setError(s => ({
                title: (error.Title && error.Title[0]) || "",
                description: (error.Description && error.Description[0]) || "",
                startDate: (error.StartDate && error.StartDate[0]) || "",
                endDate: (error.EndDate && error.EndDate[0]) || ""
            }))
        }
    }

    const triggerErrorToast = () => {
        const errorToast = new bootstrap.Toast(document.getElementById('errorToast'));
        errorToast.show();
    }

    const handleSubmitAsync = async (e) => {
        e.preventDefault();

        await createAsync();
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
                        title={"Create Event"}
                        state={state}
                        error={error}
                        handleFormChange={handleFormChange}
                        handleParticipantSearchAdd={handleParticipantSearchAdd}
                        handleParticipantSearchDelete={handleParticipantSearchDelete}
                        participants={participants}>
                        <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>Create</button>
                        <button type="button" className="btn btn-outline-secondary" onClick={handleCancel}>Cancel</button>
                    </EventFormular>
                }
                <Toast idString="errorToast" />
            </div>
        </>
    )
}
export default EventCreate
