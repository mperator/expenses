import React from 'react'

import FormInput from './../layout/FormInput'
import FormParticipantList from './FormParticipantList'
import FormParticipantSearch from './FormParticipantSearch'

const EventFormular = ({ title, state, error, handleFormChange, children, participants, handleParticipantSearchAdd, handleParticipantSearchDelete }) => {
    return (
        <>
            <h2>{title}</h2>
            <form className="">
                <FormInput type="text" id="title" label="Title" placeholder="My event title ..." value={state.title} handleChange={handleFormChange} error={error.title} />
                <FormInput type="textarea" id="description" label="Description" placeholder="My event description ..." value={state.description} handleChange={handleFormChange} error={error.description} />
                <FormInput type="date" id="startDate" label="Start Date" value={state.startDate} handleChange={handleFormChange} error={error.startDate} />
                <FormInput type="date" id="endDate" label="End Date" value={state.endDate} handleChange={handleFormChange} error={error.endDate} />

                <FormParticipantSearch
                    participants={participants} 
                    handleParticipantSearchAdd={handleParticipantSearchAdd} />

                <FormParticipantList 
                    participants={participants} 
                    handleParticipantSearchDelete={handleParticipantSearchDelete} />

                <div className="d-grid gap-2 d-flex justify-content-end">
                    {children}
                </div>
            </form>
        </>
    )
}

export default EventFormular
