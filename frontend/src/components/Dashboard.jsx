import React, { useEffect, useState } from 'react'
import { useHistory, useLocation } from 'react-router';

import useAuth from '../hooks/useAuth'

const Dashboard = (props) => {
    const history = useHistory();
    const location = useLocation();
    const { token, renewAccessTokenAsync } = useAuth();
    
    const [state, setState] = useState({ message: '' });

    useEffect(() => {
        (async () => {
            await loadDataAsync();
        })();
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
                    const path = location.pathname.substring(1);
                    const search = location.search;
                    const uri = path + search;
                    const encodedUri = encodeURIComponent(uri);

                    history.push(`/login?redirectTo=${encodedUri}`)
                    throw "Unauthorized";
                }
                return await getAsync(url, renewedToken);
            default:
                console.log("ERROR:", response)
                return null;
        }
    }

    const loadDataAsync = async () => {
        try {
            const message = await getAsync('/auth/test', token)
            setState({ ...state, message })
        } catch (error) {
            console.log(error)
        }
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
