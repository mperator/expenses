import React from 'react'

const FormTextAreaInput = ({ id, label, placeholder, value, handleChange, error }) => {
    const isValid = (e) => (e || "") && " is-invalid";

    return (
        <div className="mb-3">
            <label htmlFor={id} className="form-label">{label}</label>
            <textarea className={"form-control" + isValid(error)} id={id} name={id} placeholder={placeholder} value={value} onChange={handleChange}
            ></textarea>
            {error && <div className="invalid-feedback">{error}</div>}
        </div>
    )
}

export default FormTextAreaInput
