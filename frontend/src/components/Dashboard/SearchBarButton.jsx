import React from 'react'

const SearchBarButton = ({handleClick}) => {
    return (
        <button className="btn btn-outline-secondary" onClick={handleClick}>
            <i className="bi-search"></i>
        </button>
    )
}

export default SearchBarButton
