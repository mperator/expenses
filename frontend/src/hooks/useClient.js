/* api client */
import { useHistory, useLocation } from 'react-router';
import useAuth from './useAuth'

const useClient = () => {
    const { token, renewAccessTokenAsync } = useAuth();
    const history = useHistory();
    const location = useLocation();

    async function handleResponseAsync(response, callback, url, data) {
        switch (response.status) {
            case 200:   // OK
                return await response.json();
            case 201:   // Created
                return await response.json();
            case 204:   // No Content
                return null;
            case 400:   // Bad Request
                // TODO: Create error object that contains errors.
                const error = await response.json();
                throw (error).errors;
            case 401:   // Unauthorized
                const renewedToken = await renewAccessTokenAsync();
                //console.log("renewed token:", renewedToken)
                if (!renewedToken) {
                    const path = location.pathname.substring(1);
                    const search = location.search;
                    const uri = path + search;
                    const encodedUri = encodeURIComponent(uri);
                    history.push(`/login?redirectTo=${encodedUri}`)
                    // TODO: Create error object to throw.
                    throw "Unauthorized";
                }
                // retry call with new token.
                return await callback(url, renewedToken, data);
            default:
                console.log(`TODO: Response status code ${response.status} not implemented!!!`)
                return null;
        }
    }

    async function getWithAuthenticationAsync(url, token) {
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Authorization': `${token}`
            }
        });
        return await handleResponseAsync(response, getWithAuthenticationAsync, url);
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
        return await handleResponseAsync(response, postWithAuthenticationAsync, url, data);
    }

    async function putWithAuthenticationAsync(url, token, data) {
        const response = await fetch(url, {
            method: 'PUT',
            headers: {
                'Authorization': `${token}`,
                'Content-type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify(data)
        })
        return await handleResponseAsync(response, putWithAuthenticationAsync, url, data);
    }

    /* test hook */
    const getAuthTestAsync = async () => {
        return await getWithAuthenticationAsync('/auth/test', token);
    }

    /* events */
    const getEventsAsync = async () => {
        return await getWithAuthenticationAsync('/events', token);
    }

    const getEventAsync = async (id) => {
        return await getWithAuthenticationAsync(`/events/${id}`, token);
    }

    const getEventByIdAsync = async (id) => {
        return await getWithAuthenticationAsync(`/events/${id}`, token)
    }

    const postEventAsync = async (data) => {
        return await postWithAuthenticationAsync('/events', token, data);
    }

    const putEventAsync = async (id, data) => {
        return await putWithAuthenticationAsync(`/events/${id}`, token, data);
    }

    /* expenses */
    const postExpenseAsync = async (eventid, data) => {
        return await postWithAuthenticationAsync(`/events/${eventid}/expenses`, token, data);
    }

    /* attendees */
    const getAttendeeAsync = async (name) => {
        return await getWithAuthenticationAsync(`/attendees?name=${name}`, token);
    }

    return {
        getAuthTestAsync,
        getEventsAsync,
        getEventAsync,
        getEventByIdAsync,
        postEventAsync,
        putEventAsync,
        postExpenseAsync,
        getAttendeeAsync
    }
}

export default useClient