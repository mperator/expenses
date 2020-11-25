import React, { useContext } from 'react'
import { Link } from 'react-router-dom'
import { AuthContext } from '../AuthContext';

import useAuth from '../hooks/useAuth'

const Landing = () => {
    const { hasToken, token } = useAuth();

    const [context] = useContext(AuthContext);

    return (
        <div>
            <h1>Landing {context.count}</h1>
            <p>{token} {hasToken}</p>
            <p><Link to='/dashboard'>View Dashboard</Link></p>
        </div>
    )
}

export default Landing
