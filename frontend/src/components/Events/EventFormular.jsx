import React from 'react'

import FormInput from './../layout/FormInput'
import FormParticipantList from './FormParticipantList'
import FormParticipantSearch from './FormParticipantSearch'

import { useTranslation } from 'react-i18next'
import './../../translations/i18n'

const EventFormular = ({ title, state, error, errorDetail, handleFormChange, children, participants, handleParticipantSearchAdd, handleParticipantSearchDelete }) => {
    const { t } = useTranslation();
    return (
        <>
            <h2>{title}</h2>
            <form className="">
                <FormInput type="text" id="title" label={t("event.title")} placeholder={t("event.titlePlaceholder")} value={state.title} handleChange={handleFormChange} error={error.title} />
                <FormInput type="textarea" id="description" label={t("event.description")} placeholder={t("event.descriptionPlaceholder")} value={state.description} handleChange={handleFormChange} error={error.description} />
                <FormInput type="date" id="startDate" label={t("event.startDate")} value={state.startDate} handleChange={handleFormChange} error={error.startDate} />
                <FormInput type="date" id="endDate" label={t("event.endDate")} value={state.endDate} handleChange={handleFormChange} error={error.endDate} />

                <FormParticipantSearch
                    participants={participants} 
                    handleParticipantSearchAdd={handleParticipantSearchAdd} />

                <FormParticipantList 
                    participants={participants} 
                    handleParticipantSearchDelete={handleParticipantSearchDelete} />

                {errorDetail !== "" ? (<>
                    <div className="is-invalid"></div>
                    <div className="invalid-feedback">{errorDetail}</div>
                </>) : null}

                <div className="d-grid gap-2 d-flex justify-content-end">
                    {children}
                </div>
            </form>
        </>
    )
}

export default EventFormular
