import React from 'react'
import useForm from '../hooks/useForm'
import FormInput from './layout/FormInput'
import { useHistory } from 'react-router';
import useClient from '../hooks/useClient'

function Register() {
    const { registerUserAsync } = useClient();
    const history = useHistory();

    const { state, error, handleFormChange, setError } = useForm({
        username: "",
        email: "",
        firstName: "",
        lastName: "",
        password: "",
        passwordConfirmation: "",
    });

    const handleSubmitAsync = async (e) => {
        e.preventDefault();

        // soft validation to prevent sending wrong data.
        if(state.password !== state.passwordConfirmation) {
            setError({...error, passwordConfirmation: "Password does not match." });
        } else {
            setError({...error, passwordConfirmation: "" });
        }

        try {
            await registerUserAsync({
                username: state.username,
                email: state.email,
                firstName: state.firstName,
                lastName: state.lastName,
                password: state.password
            })

            history.push("/login");
        }
        catch(error) {

        }
    }

    return (
        <div className="container">
            <div className="row mt-5">
                <div className="col" />
                <div className="col-lg-6">
                    <h2 className="mb-5">Register</h2>
                    <form>
                        <FormInput type="text" id="username" label="Username" placeholder="JohnDoe21" value={state.username} handleChange={handleFormChange} error={error.username} />
                        <FormInput type="email" id="email" label="Email" placeholder="johndoe@example.com" value={state.email} handleChange={handleFormChange} error={error.email} />
                        <FormInput type="text" id="firstName" label="First Name" placeholder="John" value={state.firstName} handleChange={handleFormChange} error={error.firstName} />
                        <FormInput type="text" id="lastName" label="Last Name" placeholder="Doe" value={state.lastName} handleChange={handleFormChange} error={error.lastName} />
                        <FormInput type="password" id="password" label="Password" placeholder="" value={state.password} handleChange={handleFormChange} error={error.password} />
                        <FormInput type="password" id="passwordConfirmation" label="Password Confirmation" placeholder="" value={state.passwordConfirmation} handleChange={handleFormChange} error={error.passwordConfirmation} />
                        <div className="position-relative">
                            <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>Register</button>
                        </div>
                    </form>
                </div>
                <div className="col" />
            </div>
        </div>
    )
}

export default Register
