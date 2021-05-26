import React from 'react'

const Money = ({balance, currency }) => {
    return (
        <span className={balance >= 0 ? 'text-success' : 'text-danger'}>
            {balance} {currency}
        </span>
    )
}

export default Money