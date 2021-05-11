import React from 'react'

const ButtonRemove = ({handleClick}) => {
    return (
        <button className="btn btn-outline-danger" onClick={handleClick}>
            <i className="bi-trash"></i>
        </button>
    )
}

export default ButtonRemove
