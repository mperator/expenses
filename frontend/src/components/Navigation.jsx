import React from 'react';
import { Link, useHistory, useLocation } from 'react-router-dom';
import Logout from './Logout';
import useAuth from '../hooks/useAuth';

const Navigation = () => {
    const { hasToken } = useAuth();
    const location = useLocation();
    const history = useHistory();

    const handleBackArrow = () => {
        history.goBack();
    }

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <div className="float-start d-flex">
                    {
                        (location.pathname.includes('event') || location.pathname.includes('expense')) ? <button type="button" className="btn pb-3" onClick={handleBackArrow} >
                            <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" fill="currentColor" className="bi bi-arrow-left" viewBox="0 0 16 16">
                                <path fillRule="evenodd" d="M15 8a.5.5 0 0 0-.5-.5H2.707l3.147-3.146a.5.5 0 1 0-.708-.708l-4 4a.5.5 0 0 0 0 .708l4 4a.5.5 0 0 0 .708-.708L2.707 8.5H14.5A.5.5 0 0 0 15 8z" />
                            </svg>
                        </button> : null
                    }
                    <Link className="navbar-brand" to='/dashboard'>Expenses</Link>
                </div>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul className="navbar-nav me-auto mb-2 mb-lg-0">
                        {
                            hasToken ?
                                <>
                                    <li className="nav-item">
                                        <Link className="nav-link" to='/dashboard'>Dashboard</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link" to='/info'>Info</Link>
                                    </li>
                                </> : null
                        }
                    </ul>
                    <ul className="navbar-nav me-0 mb-2 mb-lg-0">
                        {
                            hasToken ?
                                <li className="nav-item">
                                    <Logout />
                                </li> :
                                <>
                                <li className="nav-item">
                                    <Link className="nav-link" to='/register'>Register</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className="nav-link" to='/login'>Login</Link>
                                </li>
                                </>
                        }
                    </ul>
                </div>
            </div>
        </nav>
    )
}

export default Navigation
