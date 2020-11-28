import React, { useEffect, useState } from 'react'
import useClient from '../hooks/useClient'
import useAuth from '../hooks/useAuth'

const Info = (props) => {
    const { getAuthTestAsync } = useClient();
    const { token} = useAuth();
    
    const [state, setState] = useState({ message: '' });

    useEffect(() => {
        (async () => {
            await loadDataAsync();
        })();
    }, []);

    const loadDataAsync = async () => {
        try {
            const message = await getAuthTestAsync();
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
            <h1>Info</h1>
            <p>Secret Message:</p>
            <p>{state.message}</p>
            <p>{token}</p>
            <button onClick={() => refresh()}>Refresh</button>
        </div>
    )
}

export default Info
