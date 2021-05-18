import { useContext } from 'react';
import { AuthContext } from '../AuthContext'
import { useHistory } from 'react-router';

import jwt_decode from "jwt-decode";

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
    const history = useHistory();
    const [state, setState] = useContext(AuthContext);

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
                tokenType: data.tokenType,
                accessToken: data.accessToken
            }));
        } else {
            throw "Username or password invalid.";
        }
    }

    async function logoutAsync() {
        let response = await fetch('/auth/logout', {
            method: 'POST',
            headers: {
                'Authorization': `${state.token}`,
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }
        });

        switch (response.status) {
            case 204:
                setState(state => ({
                    ...state,
                    tokenType: null,
                    accessToken: null
                }));

                return history.push(`/login`);

            case 401:
                //console.log("access token invalid, refresh token.")
                const renewedToken = await renewAccessTokenAsync();
                //console.log("Renewed Token", renewedToken)
                if (!renewedToken) {
                    // history.push(`/`)
                    throw "Unauthorized";
                }
                response = await fetch('/auth/logout', {
                    method: 'POST',
                    headers: {
                        'Authorization': `${renewedToken}`,
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    }
                });

                setState(state => ({
                    ...state,
                    tokenType: null,
                    accessToken: null
                }));

                return history.push(`/login`);
            default:
                console.log("ERROR:")
            // return null;
        }
    }

    // renew access token independen if existing
    async function renewAccessTokenAsync() {
        const response = await fetch(`/auth/refreshTokenSilent`, { method: 'POST', credentials: 'include' });
        if (response.status === 200) {
            const data = await response.json();

            setState(state => ({
                ...state,
                tokenType: data.tokenType,
                accessToken: data.accessToken
            }));
            return `${data.tokenType} ${data.accessToken}`
        } else {
            setState(state => ({
                ...state,
                tokenType: null,
                accessToken: null
            }));
            return null;
        }
    }


    /* DEPRECATED */

    function signOut() {
        // todo send semd tp server that user logs out
        setState(state => ({ ...state, isSignedIn: false }));
    }

    return {
        isLoading: state.loading,
        hasToken: state.accessToken !== null,
        token: `${state.tokenType} ${state.accessToken}`,
        userId: state.accessToken ? jwt_decode(state.accessToken).sub : null,

        loginAsync,
        logoutAsync,
        renewAccessTokenAsync,

        signOut,
    }
}

export default useAuth