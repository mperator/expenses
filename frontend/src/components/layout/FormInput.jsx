import React from 'react'

const FormInput = ({ type, id, label, placeholder, value, handleChange, error }) => {
    const isValid = (e) => (e || "") && " is-invalid";

    return (
        <div className="mb-3">
            <label htmlFor={id} className="form-label">{label}</label>
            {type == "textarea" ?
                <textarea className={"form-control" + isValid(error)} id={id} name={id} placeholder={placeholder} value={value} onChange={handleChange}
                ></textarea>
                : <input type={type} className={"form-control" + isValid(error)} id={id} name={id} placeholder={placeholder} value={value} onChange={handleChange} />
            }
            {error && <div className="invalid-feedback">{error}</div>}
        </div>
    )
}

export default FormInput
