import React from 'react'
import { Link } from 'react-router-dom';

const LinkButtonEdit = ({to}) => {
    return (
        <Link className="btn btn-outline-secondary" to={to}>
            <i className="bi-pencil"></i>
        </Link>
    )
}

export default LinkButtonEdit
