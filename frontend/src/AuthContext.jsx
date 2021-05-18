import React, { useState, useEffect } from 'react'

const AuthContext = React.createContext([{}, () => { }]);

async function requestTokenSilentAsync() {
    //console.log("Initial refresh.")
    try {
        const response = await fetch(`/auth/refreshTokenSilent`, { method: 'POST', credentials: 'include' });
        if (response.ok) {
            const data = await response.json();
            return {
                tokenType: data.tokenType,
                accessToken: data.accessToken
            }
        }
    } catch (error) {
        console.log(error);
    }

    return {
        tokenType: null,
        accessToken: null
    }
}

const AuthProvider = (props) => {
    // aquire token if site is loaded initially. If loading is completed and 
    // rrdirect only if loading is completed.
    useEffect(() => {
        (async () => {
            const data = await requestTokenSilentAsync();
            setState(prevState => {
                return {
                    ...prevState,
                    loading: false,
                    ...data,
                    count: Math.random(),
                }
            });
        })();
    }, [])

    const [state, setState] = useState({
        loading: true,
        tokenType: null,
        accessToken: null,
        count: 0
    })

    return (
        <AuthContext.Provider value={[state, setState]}>
            {props.children}
        </AuthContext.Provider>
    )
}

export { AuthContext, AuthProvider };
