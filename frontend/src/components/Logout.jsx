import { Link } from "react-router-dom";
import useAuth from '../hooks/useAuth';

function Logout() {
    const { logoutAsync } = useAuth();

    async function handleLogoutAsync(e) {
        e.preventDefault();

        await logoutAsync();
    }

    return (
        <Link className="nav-link" to='/login' onClick={handleLogoutAsync} data-test="logout-button">Logout</Link>
    );
}

export default Logout
