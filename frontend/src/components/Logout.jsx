import { Link } from "react-router-dom";
import useAuth from '../hooks/useAuth';

import { useTranslation } from 'react-i18next'
import '../translations/i18n'

function Logout() {
    const { t } = useTranslation();
    const { logoutAsync } = useAuth();

    async function handleLogoutAsync(e) {
        e.preventDefault();

        await logoutAsync();
    }

    return (
        <Link className="nav-link" to='/login' onClick={handleLogoutAsync} data-test="logout-button">{t("logout.signOut")}</Link>
    );
}

export default Logout
