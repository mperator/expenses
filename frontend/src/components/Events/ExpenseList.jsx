import React from 'react'

import LinkButtonPlus from '../LinkButtonPlus'
import ExpenseListItem from './ExpenseListItem';

const ExpenseList = ({eventId, expenses}) => {
    return (
        <div>
            <div className="d-flex flex-row-reverse">
                <div className="btn-group">
                    <LinkButtonPlus to={`/expense/editor?eventId=${eventId}`} />
                </div>
            </div>

            <div className="mt-3 list-group">
                {expenses.map(e => (
                    <ExpenseListItem key={e.id} eventId={eventId} expense={e} />
                ))}
            </div>
        </div>
    )
}

export default ExpenseList
