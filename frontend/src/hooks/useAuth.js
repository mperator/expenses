import { useContext } from 'react';
import { AuthContext } from '../AuthContext'

const useAuth = () => {
    const url = 'https://localhost:5001/api'

    const [state, setState] = useContext(AuthContext);

    async function signInAsync(username, password) {

        var response = await fetch(`${url}/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if(response.status === 200) {
            const data = await response.json();

            console.log(data)

            setState(state => ({ 
                ...state, 
                isSignedIn: true,
                tokenType: data.tokenType,
                accessToken: data.accessToken
            }));
        }
    }

    function signOut() {
        setState(state => ({ ...state, isSignedIn: false }));
    }

    return {
        isSignedIn: state.isSignedIn,
        signInAsync,
        signOut,
        accessToken: state.accessToken,
        tokenType: state.tokenType,
        test: state.test
    }
}
export default useAuth
