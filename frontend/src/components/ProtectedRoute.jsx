import React from 'react'
import { Route, Redirect } from 'react-router-dom'

import useAuth from '../hooks/useAuth'

const ProtectedRoute = ({ component: Component, ...rest }) => {
    const { isSignedIn } = useAuth();

    return (
        <Route {...rest} render={
            props => {
                if (isSignedIn) {
                    return <Component {...props} {...rest} />
                } else {
                    return <Redirect to={{ pathname: '/unauthorized', state: { from: props.location } }} />
                }
            }
        } />
    )
}

export default ProtectedRoute
