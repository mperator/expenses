import React from 'react'
import useAuth from '../hooks/useAuth'
import useForm from '../hooks/useForm'

import FormInput from './layout/FormInput';



import { Redirect, useLocation } from "react-router-dom";
import SkeletonLoader from './SkeletonLoader';

function Login() {
    // check if user is logged in the naviaget to Home (or redirekt)
    const { isLoading, hasToken, loginAsync } = useAuth();
    const location = useLocation();

    const { state, error, errorDetail, handleFormChange, setError, setErrorDetail } = useForm({
        username: "",
        password: "",
    });

    async function handleSubmitAsync(e) {
        e.preventDefault();

        try {
            await loginAsync(state.username, state.password)
        } catch (ex) {
            if (ex.errors) {
                setError(s => ({
                    username: (ex.errors.Username && ex.errors.Username[0]) || "",
                    password: (ex.errors.Password && ex.errors.Password[0]) || ""
                }))
            } else {
                setErrorDetail(ex.detail);
            }
        }
    }

    if (!isLoading && hasToken) {
        const searchParams = new URLSearchParams(location.search);
        let path = 'dashboard', search = null;
        const url = searchParams.get("redirectTo")
        if (url) {
            const split = url.split('?');
            if (split.length > 0) path = split[0];
            if (split.length > 1) search = split[1];
        }
        //console.log(path, search)
        return <Redirect to={{ pathname: `/${path}`, search }} />
    }

    if (isLoading) {
        return <SkeletonLoader />;
    } else {
        return (
            <div className="container">
                <div className="row mt-5">
                    <div className="col" />
                    <div className="col-lg-6">
                        <form>
                            <FormInput type="text" id="username" label="Username" value={state.username} handleChange={handleFormChange} error={error.username} />

                            <FormInput type="password" id="password" label="Password" value={state.password} handleChange={handleFormChange} error={error.password} />

                            {errorDetail !== "" ? (<>
                                <div className="is-invalid"></div>
                                <div className="invalid-feedback">{errorDetail}</div>
                            </>) : null}

                            <div className="d-grid gap-2 d-flex justify-content-end">
                                <button className="btn btn-primary" type="submit" onClick={handleSubmitAsync}>Sign In</button>
                            </div>
                        </form>
                    </div>
                    <div className="col" />
                </div>
            </div>
        )
    }
}

export default Login
