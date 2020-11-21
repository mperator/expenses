import React, { useContext } from 'react'
import { Link } from 'react-router-dom'
import { AuthContext } from '../AuthContext';

import useAuth from '../hooks/useAuth'

const Landing = props => {
    const { allowedAsync, signInAsync, hasToken, token } = useAuth();

    const [context, setContext] = useContext(AuthContext);

    // if(context.count < 20)
    //     setContext({ ...context, count: context.count + 1})

    return (
        <div>
            <h1>Landing {context.count}</h1>
            <p>{token} {hasToken}</p>
            <p><Link to='/dashboard'>View Dashboard</Link></p>
            {/* <p>Logged in status: {allowedAsync() ? "logged in" : "logged out"}</p> */}
            <button onClick={async () => await signInAsync("spidey", "Abc!23")}>Login</button>
        </div>
    )
}

export default Landing
