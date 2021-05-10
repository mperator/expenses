import React from 'react'
import dayjs from 'dayjs';

const DateFormat = ({ date }) => {
    return (
        <>{dayjs(date).format("DD.MM.YYYY")}</>
    )
}

export default DateFormat;