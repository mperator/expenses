import React, { useEffect, useState } from 'react'

import useAuth from '../hooks/useAuth'

const Dashboard = (props) => {
    const { signOut, accessToken, tokenType } = useAuth();

    const [message, setMessage] = useState("");

    useEffect(() => {
        loadDataAsync();
    }, [accessToken]);

    const loadDataAsync = async () => {
        const response = await fetch('/auth/test', {
            method: 'GET',
            headers: {
                'Authorization': `${tokenType} ${accessToken}`
            }
        })

        setMessage(await response.json());
    }

    return (
        <div>
            <h1>Dashboard</h1>
            <p>Secret Message:</p>
            <p>{message}</p>
            <p>{accessToken}</p>
            <button onClick={() => signOut()}>Logout</button>
        </div>
    )
}

export default Dashboard
