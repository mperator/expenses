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

            setState(state => ({ 
                ...state, 
                isSignedIn: true,
                token_type: data.token_type,
                access_token: data.access_token
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
        access_token: state.access_token,
        token_type: state.token_type,
        test: state.test
    }
}
export default useAuth
