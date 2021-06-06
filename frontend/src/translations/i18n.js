import i18n from 'i18next'
import { initReactI18next } from 'react-i18next'
import LanguageDetector from 'i18next-browser-languagedetector'

import { TRANSLATIONS_DEDE } from './de-DE/translations'
import { TRANSLATIONS_ENUS } from './en-US/translations'

i18n
    .use(LanguageDetector)
    .use(initReactI18next)
    .init({
        fallbackLng: "en-US",
        load: 'currentOnly',
        resources: {
            "en-US": {
                translation: TRANSLATIONS_ENUS
            },
            "de-DE" : {
                translation: TRANSLATIONS_DEDE
            }
        }
    });

// use browser language as default (auto)
const lang = localStorage.getItem("lang") || "auto";
if(lang === "auto") {
    i18n.changeLanguage(navigator.language || navigator.userLanguage)
} else {
    i18n.changeLanguage(lang);
}