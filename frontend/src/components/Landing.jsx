import React from 'react'
import { Link } from 'react-router-dom'

import useAuth from '../hooks/useAuth'

const Landing = props => {
    const { allowedAsync, signInAsync } = useAuth();

    return (
        <div>
            <h1>Landing</h1>
            <p><Link to='/dashboard'>View Dashboard</Link></p>
            <p>Logged in status: {allowedAsync() ? "logged in" : "logged out"}</p>
            <button onClick={async () => await signInAsync("spidey", "Abc!23")}>Login</button>
        </div>
    )
}

export default Landing
