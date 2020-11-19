import React, { useEffect, useState } from 'react'

import useAuth from '../hooks/useAuth'

const Dashboard = (props) => {
    const { signOut, getAccessTokenAsync, renewAccessTokenAsync } = useAuth();

    const [state, setState] = useState({accessToken: '', message: ''});

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
                if (!renewedToken) return null; // to login
                return await getAsync(url, renewedToken);
            default: return null;
        }
    }

    const loadDataAsync = async () => {
        const accessToken = await getAccessTokenAsync()
        const message = await getAsync('/auth/test', `${accessToken}`)

        setState({ ...state, accessToken, message})
    }

async function refresh() {
    await loadDataAsync();
}

return (
    <div>
        <h1>Dashboard</h1>
        <p>Secret Message:</p>
        <p>{state.message}</p>
        <p>{state.accessToken}</p>
        <button onClick={() => refresh()}>Refresh</button>
        <button onClick={() => signOut()}>Logout</button>
    </div>
)
}

export default Dashboard
