import React, { useEffect, useState } from 'react'
import useClient from './../../hooks/useClient'
import useForm from './../../hooks/useForm'
import { useHistory } from 'react-router';
import dayjs from 'dayjs'

import useAuth from './../../hooks/useAuth';
import EventFormular from './EventFormular'

import { useTranslation } from 'react-i18next'
import './../../translations/i18n'

const EventCreate = () => {
    const { t } = useTranslation();
    const history = useHistory();
    const { userId } = useAuth();
    const { postEventAsync, getParticipantByIdAsync } = useClient();

    const [loading, setLoading] = useState(true);
    const { state, error, errorDetail, handleFormChange, setError, setErrorDetail } = useForm({
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
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    const createAsync = async () => {
        const filteredParticipantsIds = participants
            .map(p => p.id)
            .filter(p => p !== userId);

        try {
            await postEventAsync({
                title: state.title,
                description: state.description,
                startDate: state.startDate,
                endDate: state.endDate,
                currency: "EUR",
                participantIds: filteredParticipantsIds
            });
            history.goBack();
        } catch (ex) {
            if(ex.errors) {
                setError(s => ({
                    title: (ex.errors.Title && ex.errors.Title[0]) || "",
                    description: (ex.errors.Description && ex.errors.Description[0]) || "",
                    startDate: (ex.errors.StartDate && ex.errors.StartDate[0]) || "",
                    endDate: (ex.errors.EndDate && ex.errors.EndDate[0]) || ""
                }))
            } else {
                setErrorDetail(ex.detail);
            }
        }
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
                        title={t("event.newTitle")}
                        state={state}
                        error={error}
                        errorDetail={errorDetail}
                        handleFormChange={handleFormChange}
                        handleParticipantSearchAdd={handleParticipantSearchAdd}
                        handleParticipantSearchDelete={handleParticipantSearchDelete}
                        participants={participants}>
                        <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>{t("event.newAction")}</button>
                        <button type="button" className="btn btn-outline-secondary" onClick={handleCancel}>{t("event.cancel")}</button>
                    </EventFormular>
                }
            </div>
        </>
    )
}
export default EventCreate
