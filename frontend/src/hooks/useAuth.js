import { useContext } from 'react';
import { AuthContext } from '../AuthContext'

const useAuth = () => {
    const [state, setState] = useContext(AuthContext);

    function signIn() {
        setState(state => ({ ...state, isSignedIn: true }));
    }

    function signOut() {
        setState(state => ({ ...state, isSignedIn: false }));
    }

    return {
        isSignedIn: state.isSignedIn,
        signIn,
        signOut,
        test: state.test
    }
}
export default useAuth
