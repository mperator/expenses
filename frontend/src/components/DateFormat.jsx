import React from 'react'
import dayjs from 'dayjs';

import { useTranslation } from 'react-i18next'
import './../translations/i18n'

const DateFormat = ({ date }) => {
    const { t } = useTranslation();
    return (
        <>{dayjs(date).format(t("common.dateFormat"))}</>
    )
}

export default DateFormat;