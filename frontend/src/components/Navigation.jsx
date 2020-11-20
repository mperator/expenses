import React from 'react'
import { Link } from 'react-router-dom'

const Navigation = () => {
    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <Link className="navbar-brand" to='/'>Expenses</Link>
                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul className="navbar-nav mr-auto mb-2 mb-lg-0">
                        <li className="nav-item">
                            <Link className="nav-link" to='/dashboard'>Secret 1</Link>
                        </li>
                        {/* <li className="nav-item">
                            <a className="nav-link active" aria-current="page" href="#">Secret 1</a>
                        </li> */}
                        <li className="nav-item">
                            <Link className="nav-link" to='/dashboard'>Secret 2</Link>
                        </li>
                    </ul>
                    <ul className="navbar-nav mr-0 mb-2 mb-lg-0">
                        <li className="nav-item">
                            <Link className="nav-link" to='/login'>Login</Link>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    )
}

export default Navigation
