import { useContext } from 'react';
import { AuthContext } from '../AuthContext'

/* IMPORTANT!
 * To use httpsOnly cookie from server we have to configure several things.
 * First we have to make sure oure connection to the server runs on https only then cookies will be 
 * transmittet. we can achieve this by using the proxy setting: "proxy": "https://localhost:5001/api",
 * see https://create-react-app.dev/docs/proxying-api-requests-in-development/
 * also see: https://dev.to/petrussola/today-s-rabbit-hole-jwts-in-httponly-cookies-csrf-tokens-secrets-more-1jbp
 * 
 * Also add credentials: 'include' to send cookie back: https://solrevdev.com/2019/05/20/fetch-wont-send-or-receive-any-cookies.html
 * */


const useAuth = () => {
    const [state, setState] = useContext(AuthContext);

    async function signInAsync(username, password) {
        var response = await fetch(`/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });

        if (response.status === 200) {
            const data = await response.json();
            setState(state => ({
                ...state,
                isSignedIn: true,
                silentSignedInFailed: false,
                tokenType: data.tokenType,
                accessToken: data.accessToken
            }));
        }
    }

    function signOut() {
        setState(state => ({ ...state, isSignedIn: false }));
    }

    function signInSilent() {
        fetch(`/auth/refreshTokenSilent`, { method: 'POST', credentials: 'include' })
            .then(r => { return r.text()})
            .then(d => {
                const data = JSON.parse(d);
                setState(state => ({
                    ...state,
                    isSignedIn: true,
                    silentSignedInFailed: false,
                    tokenType: data.tokenType,
                    accessToken: data.accessToken
                }));
            })
            .catch(e => {
                setState(state => ({
                    ...state,
                    isSignedIn: false,
                    silentSignedInFailed: true,
                    tokenType: null,
                    accessToken: null
                }));
            })
                
        // const response = await fetch(`/auth/refreshTokenSilent`, {
        //     method: 'POST',
        //     credentials: 'include'
        // });

        // if(response.ok) {
        //     const data = await response.json();
        //     setState(state => ({
        //         ...state,
        //         isSignedIn: true,
        //         tokenType: data.tokenType,
        //         accessToken: data.accessToken
        //     }));
        // }
    }

    return {
        isSignedIn: state.isSignedIn,
        signInAsync,
        signOut,
        accessToken: state.accessToken,
        tokenType: state.tokenType,
        test: state.test,
        signInSilent
    }
}
export default useAuth
