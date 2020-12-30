import React from 'react'
import { Route, Redirect, useLocation } from 'react-router-dom'
import useAuth from '../hooks/useAuth'
import SkeletonLoader from './SkeletonLoader';

const ProtectedRoute = ({ component: Component, ...rest }) => {
    const { isLoading, hasToken } = useAuth();
    const location = useLocation();

    function render(props) {
        if (isLoading) {
            return <SkeletonLoader />;
        }

        if (hasToken) {
            return <Component {...props} {...rest} />
        } else {
            const path = location.pathname.substring(1);
            const search2 = location.search;

            const uri = path + search2;
            console.log(uri)
            const encodedUri = encodeURI(uri);
            const search = `?redirectTo=${encodedUri}`;
            console.log(search)

            return <Redirect to={{ pathname: '/login', search, state: { from: props.location } }} />
        }
    }

    return (
        <Route {...rest} render={props => render(props)} />
    )
}

export default ProtectedRoute
