import React, { useEffect, useState } from 'react'
import useClient from './../../hooks/useClient'
import useForm from './../../hooks/useForm'
import { useHistory, useParams } from 'react-router';
import dayjs from 'dayjs'

import useAuth from './../../hooks/useAuth';
import EventFormular from './EventFormular'

import { useTranslation } from 'react-i18next'
import './../../translations/i18n'

const EventEdit = () => {
    const { t } = useTranslation();
    const history = useHistory();
    const params = useParams();
    const { userId } = useAuth();
    const { getEventAsync, putEventAsync } = useClient();

    const [loading, setLoading] = useState(true);
    const { state, error, errorDetail, handleFormChange, setError, setErrorDetail, setForm } = useForm({
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
                        creator: p.id === event.creatorId,
                        currentUser: p.id === userId
                    }))
                );
                setLoading(false);
            })();
        } else setLoading(false);
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    const updateAsync = async () => {
        const filteredParticipantsIds = participants.map(participant => {
            return participant.id;
        });
        try {
            await putEventAsync(params.id, {
                title: state.title,
                description: state.description,
                startDate: state.startDate,
                endDate: state.endDate,
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
                    title={t("event.editTitle")}
                    state={state}
                    error={error}
                    errorDetail={errorDetail}
                    handleFormChange={handleFormChange}
                    handleParticipantSearchAdd={handleParticipantSearchAdd}
                    handleParticipantSearchDelete={handleParticipantSearchDelete}
                    participants={participants}>
                    <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>{t("event.editAction")}</button>
                    <button type="button" className="btn btn-outline-secondary" onClick={handleCancel}>{t("event.cancel")}</button>
                </EventFormular>
                }
            </div>
        </>
    )
}

export default EventEdit
