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

    // if(!isSignedIn) {

    //     console.log("silent 1")

    //     // silent sign in
    //     //signInSilent();

    //     console.log("silent 2")
    // }

    return (
        <Route {...rest} render={props => dodo(props)} />
    )
}

export default ProtectedRoute
