import React from 'react'
import DashboardEventListItem from './DashboardEventListItem'

import { useTranslation } from 'react-i18next'
import './../../translations/i18n'

const DashboardEventList = ({ events }) => {
    const { t } = useTranslation();

    return (
        <div>
            {
                events.length !== 0 ?
                    events && events.map(e => (
                        <DashboardEventListItem key={e.id} {...e} />
                    )) : 
                    <div>{t("dashboard.noEvents")}</div>
            }
        </div>
    )
}

export default DashboardEventList
