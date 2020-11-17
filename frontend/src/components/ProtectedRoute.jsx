import React from 'react'
import { Route, Redirect } from 'react-router-dom'

import useAuth from '../hooks/useAuth'

const ProtectedRoute = ({ component: Component, ...rest }) => {
    const { isSignedIn, silentSignedInFailed, signInSilent } = useAuth();

    function dodo(props) {
        if (isSignedIn) {
            return <Component {...props} {...rest} />
        } 
        else if(silentSignedInFailed) {
            return <Redirect to={{ pathname: '/unauthorized', state: { from: props.location } }} />
        } 
        else {
            signInSilent()
            return <Component {...props} {...rest} />
        }
    }

    return (
        <Route {...rest} render={props => dodo(props)} />
    )
}

export default ProtectedRoute
