/* api client */
import { useHistory, useLocation } from 'react-router';
import useAuth from './useAuth'

const useClient = () => {
    const { token, renewAccessTokenAsync } = useAuth();
    const history = useHistory();
    const location = useLocation();

    async function getAsync(url, token) {
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
                return await getAsync(url, renewedToken);
            default:
                console.log("ERROR:", response)
                return null;
        }
    }



    /* protected routes */

    const getAuthTestAsync = async () => {
        return await getAsync('/auth/test', token);
    }




    return {
        getAuthTestAsync
    }
}

export default useClient