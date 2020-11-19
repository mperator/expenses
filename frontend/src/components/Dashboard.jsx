import React, { useEffect, useState } from 'react'

import useAuth from '../hooks/useAuth'

const Dashboard = (props) => {
    const { signOut, accessToken, tokenType } = useAuth();

    const [message, setMessage] = useState("");

    useEffect(() => {
        loadDataAsync();
    }, [accessToken]);

    async function refreshTokenAsync() {
        const response = await fetch('/auth/refreshTokenSilent', { method: 'POST', credentials: 'include' });
        console.log(response)
        if (response.status === 200) {
            return await response.json();
        } else {
            // user needs to login again
            console.log("User needs to login");
            return null;
        }
    }

    async function getAsync(url, token) {
        console.log(token)

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

                const refreshToken = await refreshTokenAsync();
                if (!refreshToken) return null; // to login
                return await getAsync(url, `${refreshToken.tokenType} ${refreshToken.accessToken}`);
            default: return null;
        }
    }



    const loadDataAsync = async () => {
        const text = await getAsync('/auth/test', `${tokenType} ${accessToken}`)

        setMessage(text);
    }

async function refresh() {
    await loadDataAsync();
}

return (
    <div>
        <h1>Dashboard</h1>
        <p>Secret Message:</p>
        <p>{message}</p>
        <p>{accessToken}</p>
        <button onClick={() => refresh()}>Refresh</button>
        <button onClick={() => signOut()}>Logout</button>
    </div>
)
}

export default Dashboard
