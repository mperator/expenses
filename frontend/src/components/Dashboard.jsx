import React, { useEffect, useState } from 'react'
import { useHistory } from 'react-router';

import useAuth from '../hooks/useAuth'

const Dashboard = (props) => {
    const history = useHistory();
    const { token, renewAccessTokenAsync } = useAuth();

    const [state, setState] = useState({message: ''});

    useEffect(() => {
        loadDataAsync();
    }, []);

    async function getAsync(url, token) {
        console.log("TOKEN:", token)

        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `${token}`
            }
        })

        switch (response.status) {
            case 200:
                console.log("valid")
                return await response.text();
                
            case 401:
                console.log("access token invalid, refresh token.")
                const renewedToken = await renewAccessTokenAsync();
                console.log("Renewd TRoken", renewedToken)
                if (!renewedToken) {
                    history.push('/login?redirect=dashboard') // TODO append page info for redirect

                    // this cause Can't perform a React state update on an unmounted component. This is a no-op, but it indicates a memory leak in your application. To fix, cancel all subscriptions and asynchronous tasks in a useEffect cleanup function.
                    // TODO: look for other solution
                    return null; // to login
                }
                return await getAsync(url, renewedToken);
            default: 
                console.log("ERROR:",response)
                return null;
        }
    }

    const loadDataAsync = async () => {
        const message = await getAsync('/auth/test', token)

        setState({ ...state, message})
    }

async function refresh() {
    await loadDataAsync();
}

return (
    <div>
        <h1>Dashboard</h1>
        <p>Secret Message:</p>
        <p>{state.message}</p>
        <p>{token}</p>
        <button onClick={() => refresh()}>Refresh</button>
    </div>
)
}

export default Dashboard
