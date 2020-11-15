import React from 'react'
import { Link } from 'react-router-dom'

import useAuth from '../hooks/useAuth'

const Landing = props => {
    const { isSignedIn, signIn } = useAuth();

    return (
        <div>
            <h1>Landing</h1>
            <p><Link to='/dashboard'>View Dashboard</Link></p>
            <p>Logged in status: {isSignedIn ? "logged in" : "logged out"}</p>
            <button onClick={() => signIn()}>Login</button>
        </div>
    )
}

export default Landing
