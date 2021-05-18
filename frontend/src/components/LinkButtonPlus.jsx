import React from 'react'
import { Link } from 'react-router-dom';

const LinkButtonPlus = ({to}) => {
    return (
        <Link className="btn btn-outline-primary" to={to}>
            <i className="bi-plus"></i>
        </Link>
    )
}

export default LinkButtonPlus
