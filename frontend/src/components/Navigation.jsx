import React from 'react'
import { Link } from 'react-router-dom'
import Logout from './Logout'

import useAuth from '../hooks/useAuth'

const Navigation = () => {
    const { hasToken } = useAuth();



    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <Link className="navbar-brand" to='/'>Expenses</Link>
                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
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
                                <li className="nav-item">
                                    <Link className="nav-link" to='/login'>Login</Link>
                                </li>
                        }
                    </ul>
                </div>

            </div>
        </nav>
    )
}

export default Navigation
