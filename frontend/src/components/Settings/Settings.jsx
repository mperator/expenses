import React, { useState, useEffect } from 'react'
import SkeletonLoader from './../SkeletonLoader'

import { useTranslation } from 'react-i18next'
import './../../translations/i18n'
import i18n from 'i18next'

const Settings = () => {
    const { t } = useTranslation();
    const [loading, setLoading] = useState(true);
    const languages = ["auto", "de-DE", "en-US"];
    const [language, setLanguage] = useState("auto");

    useEffect(() => {
        const lang = localStorage.getItem("lang") || "auto";
        setLanguage(lang);
        setLoading(false);
    }, [])

    const handleLanguageChange = (e) => {
        const lang = e.target.value;
        setLanguage(e.target.value)
        if (lang === "auto") {
            i18n.changeLanguage(navigator.language || navigator.userLanguage);
        } else {
            i18n.changeLanguage(lang);
        }

        localStorage.setItem("lang", lang);
    }

    return (
        <>
            { loading ? <SkeletonLoader />
                : <div className="container mt-3">
                    <h2 className="display-3">{t("settings.title")}</h2>
                    <form>
                        <div className="mb-3">
                            <div className="form-floating">
                                <select className="form-select" id="language" name="language" value={language} onChange={(e) => handleLanguageChange(e)}>
                                    {languages.map(l =>
                                        <option key={l} value={l}>{t(`languages.${l}`)}</option>
                                    )}
                                </select>
                                <label htmlFor="lang">{t("settings.lang")}</label>
                            </div>
                        </div>
                    </form>
                </div>
            }
        </>
    )
}

export default Settings
