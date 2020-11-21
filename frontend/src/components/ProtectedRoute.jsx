import React from 'react'
import { Route, Redirect } from 'react-router-dom'

import useAuth from '../hooks/useAuth'

const ProtectedRoute = ({ component: Component, ...rest }) => {
    const { isLoading, hasToken } = useAuth();

    function render(props) {
        if(isLoading) {
            return <p>Loading ...</p>;
        }

        if(hasToken) {
            return <Component {...props} {...rest} />
        } else {
            return <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
        }
    }

    return (
        <Route {...rest} render={props => render(props)} />
    )
}

export default ProtectedRoute
