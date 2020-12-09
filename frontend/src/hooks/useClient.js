/* api client */
import { useHistory, useLocation } from 'react-router';
import useAuth from './useAuth'

const useClient = () => {
    const { token, renewAccessTokenAsync } = useAuth();
    const history = useHistory();
    const location = useLocation();

    async function getWithAuthenticationAsync(url, token) {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `${token}`
            }
        })

        switch (response.status) {
            case 200:
                console.log("valid")
                return await response.json();
            case 401:
                console.log("access token invalid, refresh token.")
                const renewedToken = await renewAccessTokenAsync();
                console.log("Renewd TRoken", renewedToken)
                if (!renewedToken) {
                    const path = location.pathname.substring(1);
                    const search = location.search;
                    const uri = path + search;
                    const encodedUri = encodeURIComponent(uri);

                    history.push(`/login?redirectTo=${encodedUri}`)
                    throw "Unauthorized";
                }
                return await getWithAuthenticationAsync(url, renewedToken);
            default:
                console.log("ERROR:", response)
                return null;
        }
    }

    async function postWithAuthenticationAsync(url, token, data) {
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Authorization': `${token}`,
                'Content-type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(data)
        })

        switch (response.status) {
            case 200:
                console.log("valid")
                return await response.json();
            case 400:
                const error = await response.json();
                throw (error).errors;

            case 401:
                console.log("access token invalid, refresh token.")
                const renewedToken = await renewAccessTokenAsync();
                console.log("Renewd TRoken", renewedToken)
                if (!renewedToken) {
                    const path = location.pathname.substring(1);
                    const search = location.search;
                    const uri = path + search;
                    const encodedUri = encodeURIComponent(uri);

                    history.push(`/login?redirectTo=${encodedUri}`)
                    throw "Unauthorized";
                }
                return await postWithAuthenticationAsync(url, renewedToken, data);
            default:
                console.log("ERROR:", response)
                return null;
        }
    }

    /* protected routes */
    /* use own hook for auth api */
    const getAuthTestAsync = async () => {
        return await getWithAuthenticationAsync('/auth/test', token);
    }

    /* events */
    const getEventAsync = async() => {
        return await getWithAuthenticationAsync('/events', token);
    }

    const postEventAsync = async(data) => {
        return await postWithAuthenticationAsync('/events', token, data);
    }

    const getAttendeeAsync = async(name) => {
        return await getWithAuthenticationAsync(`/attendees?name=${name}`, token);
    }

    return {
        getAuthTestAsync,
        getEventAsync,
        postEventAsync,
        getAttendeeAsync,
    }
}

export default useClient