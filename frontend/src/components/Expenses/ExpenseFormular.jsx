import React from 'react'
import FormInput from '../layout/FormInput';

import { useTranslation } from 'react-i18next'
import './../../translations/i18n'

const ExpenseFormular = ({ title, state, error, errorDetail, handleFormChange, handleSplit, handleCreditorChange, handleParticipantAmountChange, children }) => {
    const { t } = useTranslation();
    return (
        <>
            <h2>{title}</h2>
            <form>
                <FormInput type="date" id="date" label={t("expense.date")} value={state.date} handleChange={handleFormChange} error={error.date} />

                <FormInput type="text" id="title" label={t("expense.title")} placeholder={t("expense.titlePlaceholder")} value={state.title} handleChange={handleFormChange} error={error.title} />

                <FormInput type="textarea" id="description" label={t("expense.description")} placeholder={t("expense.descriptionPlaceholder")} value={state.description} handleChange={handleFormChange} error={error.description} />

                <div className="mb-3">
                    <label htmlFor="amount" className="form-label">{t("expense.amount")}</label>
                    <div className="input-group">
                        <input type="text" className="form-control text-right" aria-label="amount" id="amount" name="amount" value={state.amount} onChange={handleFormChange} />
                        <button type="button" className="btn btn-outline-primary" onClick={handleSplit}>{t("expense.split")}</button>
                        <span className="input-group-text">€</span>
                    </div>
                </div>

                <div className="mb-3">
                    <div className="form-floating">
                        <select className="form-select" id="creditorId" aria-label="Id of the creditor" name="creditor" value={state.creditorId} onChange={(e) => handleCreditorChange(e)}>
                            {state.participants.map(participant =>
                                <option key={participant.id} value={participant.id}>{participant.username}</option>
                            )}
                        </select>
                        <label htmlFor="creditorId">{t("expense.creditor")}</label>
                    </div>
                </div>
                
                <div className="mb-3">
                    <h3 className="mb-3" >{t("expense.participants")}</h3>
                    {state.participants.map((p, i) => (
                        <div key={p.id} className="row mb-2">
                            <label className="col-sm-3 col-form-label">{p.username}</label>
                            <div className="col-sm-9">
                                <div className="input-group">
                                    <div className="input-group-text">
                                        <input className="form-check-input" type="checkbox" aria-label="Checkbox for following text input" id="isParticipating" name="isParticipating" checked={p.isParticipating} onChange={e => handleParticipantAmountChange(e, i)} />
                                    </div>
                                    <input type="text" className="form-control text-right" aria-label="Text input with checkbox" id="splitAmount" name="amount" value={p.amount} onChange={e => handleParticipantAmountChange(e, i)} />
                                    <span className="input-group-text">€</span>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>

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

export default ExpenseFormular
