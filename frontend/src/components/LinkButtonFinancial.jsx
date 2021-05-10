import React from 'react'
import { Link } from 'react-router-dom';

const LinkButtonFinancial = ({to}) => {
    return (
        <Link className="btn btn-outline-secondary" to={to}>
            <i className="bi-pie-chart"></i>
        </Link>
    )
}

export default LinkButtonFinancial
