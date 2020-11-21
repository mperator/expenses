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
                token: data.tokenType,
                accessToken: data.accessToken
            }));
        }
    }

    async function loginAsync(username, password) {
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
                // tokenType: data.tokenType,
                // accessToken: data.accessToken
                token: `${data.tokenType} ${data.accessToken}`
            }));
        } else {
            throw "Username or password invalid.";
        }
    }


    function signOut() {
        // todo send semd tp server that user logs out
        setState(state => ({ ...state, isSignedIn: false }));
    }

    async function getAccessTokenAsync() {
        if (state.token) {
            return state.token;
        }
        else {
            return await renewAccessTokenAsync();
        }
    }

    // renew access token independen if existing
    async function renewAccessTokenAsync() {
        const response = await fetch(`/auth/refreshTokenSilent`, { method: 'POST', credentials: 'include' });
        if (response.status === 200) {
            const data = await response.json();
            const token = `${data.tokenType} ${data.accessToken}`;

            setState(state => ({
                ...state,
                token
            }));
            return token;
        } else {
            setState(state => ({
                ...state,
                token: null
            }));
            return null;
        }
    }

    // returns if user is sined in refresh automaticall if user was not signed in
    async function allowedAsync() {
        return await getAccessTokenAsync() !== null;
    }

    return {
        signInAsync,
        signOut,
        getAccessTokenAsync,
        renewAccessTokenAsync,
        allowedAsync,

        loginAsync
    }
}

export default useAuth