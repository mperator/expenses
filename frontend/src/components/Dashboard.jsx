import React from 'react'

import useAuth from '../hooks/useAuth'

const Dashboard = (props) => {
    const { signOut } = useAuth();

    return (
        <div>
            <h1>Dashboard</h1>
            <p>Secret Page</p>
            <button onClick={() => signOut()}>Logout</button>
        </div>
    )
}

export default Dashboard
