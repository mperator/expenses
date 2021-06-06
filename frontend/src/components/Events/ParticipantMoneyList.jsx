import React from 'react'
import ParticipantMoney from './ParticipantMoney'

import { useTranslation } from 'react-i18next'
import './../../translations/i18n'

const ParticipantMoneyList = ({ financials }) => {
    const { t } = useTranslation();
    return (
        <>
            <h3>{t("event.participants")}</h3>
            <div className="mt-4">
                {financials.map((f, i) => (
                    <ParticipantMoney key={f.userId + `${i}`} username={f.username} balance={f.balance} currency={f.currency} />
                ))}
            </div>
        </>
    )
}

export default ParticipantMoneyList
