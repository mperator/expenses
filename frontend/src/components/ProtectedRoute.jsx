import React from 'react'
import { Route, Redirect } from 'react-router-dom'

import useAuth from '../hooks/useAuth'

const ProtectedRoute = ({ component: Component, ...rest }) => {
    const { allowedAsync } = useAuth();

    function render(props) {
        const isAllowed = allowedAsync();

        if(isAllowed) {
            return <Component {...props} {...rest} />
        } else {
            // to appen login with redirect
            return <Redirect to={{ pathname: '/unauthorized', state: { from: props.location } }} />
        }
    }

    return (
        <Route {...rest} render={props => render(props)} />
    )
}

export default ProtectedRoute
