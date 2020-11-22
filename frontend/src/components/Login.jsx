import React, { useState } from 'react'
import useAuth from '../hooks/useAuth';

import { Redirect } from "react-router-dom";

function Login() {
    // check if user is logged in the naviaget to Home (or redirekt)
    const { isLoading, hasToken, loginAsync } = useAuth();

    const [state, setState] = useState({
        username: "",
        password: "",
        error: ""
    });

    function handleChange(e) {
        setState({
            ...state,
            error: "",
            [e.target.name]: e.target.value
        })
    }

    async function handleSubmitAsync(e) {
        e.preventDefault();

        // try logging in 
        // success -> 
        // yes: check if redirect is needed
        // no: go to dashboard

        // fail -> print error
        try {
            await loginAsync(state.username, state.password)
        } catch (error) {
            setState({
                ...state,
                error: error,
            })
            return;
        }
    }

    // TODO: if route contains redirection redirect back
    if (!isLoading && hasToken) {
        return <Redirect to={{ pathname: '/dashboard' }} />
    }

    if (isLoading) {
        return <p>Loading ...</p>
    } else {
        return (
            <div className="container">
                <div className="row mt-5">
                    <div className="col" />
                    <div className="col-lg-6">
                        <form>
                            <div className="row mb-3">
                                <label htmlFor="username" className="col-sm-2 col-form-label">Username</label>
                                <div className="col-sm-10">
                                    <input type="text" className="form-control" id="username" name="username" value={state.username} onChange={handleChange} />
                                </div>
                            </div>
                            <div className="row mb-3">
                                <label htmlFor="password" className="col-sm-2 col-form-label">Password</label>
                                <div className="col-sm-10">
                                    <input type="password" className="form-control" id="password" name="password" value={state.password} onChange={handleChange} />
                                </div>
                            </div>

                            {/* <div className="row mb-3">
                    <div className="col-sm-10 offset-sm-2">
                        <div className="form-check">
                            <input className="form-check-input" type="checkbox" id="gridCheck1" />
                            <label className="form-check-label" for="gridCheck1">Remember</label>
                        </div>
                    </div>
                </div> */}
                            <div className="position-relative">
                                <button type="submit" className="btn btn-primary position-absolute top-0 right-0" onClick={handleSubmitAsync}>Sign in</button>
                            </div>

                            <p>{state.error}</p>
                        </form>
                    </div>
                    <div className="col" />
                </div>
            </div>
        )
    }
}

export default Login
