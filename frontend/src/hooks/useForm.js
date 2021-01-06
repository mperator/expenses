import { useState } from 'react'

/* hook to update and validat forms */

// state, validation(takes state), error
// overload validations for each value, validation can depend on multiple  vother values
// eg if this then that

const useForm = (form) => {
    const [state, setState] = useState(form)
    const errorInit = {
        message: "",
    }
    for(const f in form) {
        errorInit[f] = ''
    }
    const [error, setError] = useState(errorInit)


    // error onSubmit, onChange, onBlur
    // onSubmit -> handleSubmit
    // onValidate -> handleValidation
    // handleChange
    // setError
    // setState

    const handleFormChange = (e) => {
        setState(s => ({
            ...s,
            [e.target.name]: e.target.value
        }))

        setError(s => ({
            ...s,
            [e.target.name]: ""
        }))
    }



    return {
        state,
        error,
        handleFormChange,
        setError
    };
}

export default useForm;