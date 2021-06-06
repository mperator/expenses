import React, { useEffect, useState } from 'react'
import useClient from './../../hooks/useClient'
import SkeletonLoader from './../SkeletonLoader'
import SearchBar from './SearchBar'
import LinkButtonPlus from '../LinkButtonPlus'
import DashboardEventList from './DashboardEventList'

import './Dashboard.css'

import { useTranslation } from 'react-i18next'
import './../../translations/i18n'

/* Get a list of events and display them. */
const Dashboard = () => {
    const { t } = useTranslation();
    const { getEventsAsync, getFilteredEventsAsync } = useClient();
    const [loading, setLoading] = useState(true);
    const [events, setEvents] = useState([]);
    const [search, setSearch] = useState({ query: "" });

    useEffect(() => {
        (async () => {
            await loadEventsAsync();
        })();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    const loadEventsAsync = async () => {
        try {
            const events = await getEventsAsync();
            setEvents(e => events);
            setLoading(false);
        } catch (error) {
            console.log(error)
        }
    }

    const handleQueryChange = (e) => {
        setSearch({
            ...search,
            [e.target.name]: e.target.value
        })
    }

    const handleSearch = (e) => {
        e.preventDefault();
        (async () => {
            if (search.query !== "") {
                try {
                    const events = await getFilteredEventsAsync(search.query)
                    setEvents(events);
                } catch (error) { }
            } else {
                await loadEventsAsync();
            }
        })()
    }

    return (
        <>
            { loading ? <SkeletonLoader />
                : <div className="container mt-3">
                    <h2 className="display-3">{t("dashboard.title")}</h2>

                    {/* search bar  */}
                    <div className="my-4 d-flex align-items-center gap-2">
                        <SearchBar
                            query={search.query}
                            handleChange={handleQueryChange}
                            placeholder={t("dashboard.searchPlaceHolder")}
                            handleClick={handleSearch} />
                        <LinkButtonPlus to={'/event/new'} />
                    </div>

                    {/* search result + pagination */}
                    <div className="mt-3">
                        <DashboardEventList events={events} />
                    </div>
                </div>
            }
        </>
    )
}

export default Dashboard
