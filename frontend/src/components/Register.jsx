import React from 'react'
import useForm from '../hooks/useForm'
import FormInput from './layout/FormInput'
import { useHistory } from 'react-router';
import useClient from '../hooks/useClient'

import { useTranslation } from 'react-i18next'
import '../translations/i18n'

function Register() {
    const {t} = useTranslation();
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
            setError({...error, passwordConfirmation: t("register.errorPasswordNoMatch") });
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
                    <h2 className="mb-5">{t("register.title")}</h2>
                    <form>
                        <FormInput type="text" id="username" label={t("register.username")} placeholder="JohnDoe21" value={state.username} handleChange={handleFormChange} error={error.username} />
                        <FormInput type="email" id="email" label={t("register.email")} placeholder="johndoe@example.com" value={state.email} handleChange={handleFormChange} error={error.email} />
                        <FormInput type="text" id="firstName" label={t("register.firstName")} placeholder="John" value={state.firstName} handleChange={handleFormChange} error={error.firstName} />
                        <FormInput type="text" id="lastName" label={t("register.lastName")} placeholder="Doe" value={state.lastName} handleChange={handleFormChange} error={error.lastName} />
                        <FormInput type="password" id="password" label={t("register.password")} placeholder="" value={state.password} handleChange={handleFormChange} error={error.password} />
                        <FormInput type="password" id="passwordConfirmation" label={t("register.passwordConfirmation")} placeholder="" value={state.passwordConfirmation} handleChange={handleFormChange} error={error.passwordConfirmation} />
                        <div className="position-relative">
                            <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>{t("register.register")}</button>
                        </div>
                    </form>
                </div>
                <div className="col" />
            </div>
        </div>
    )
}

export default Register
