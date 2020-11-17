import React, { useEffect } from 'react'

import useAuth from '../hooks/useAuth'

const Dashboard = (props) => {
    const { signOut, access_token, token_type } = useAuth();

    useEffect(() => {
        loadDataAsync();
    }, []);

    const loadDataAsync = async () => {
        const response = await fetch('https://localhost:5001/api/auth/test', {
            method: 'GET',
            headers: {
                'Authentication': `${token_type} ${access_token}`
            }
        })

        console.log(response)
    }

    return (
        <div>
            <h1>Dashboard</h1>
            <p>Secret Page</p>
            <button onClick={() => signOut()}>Logout</button>
        </div>
    )
}

export default Dashboard
