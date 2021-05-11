import React from 'react'
import SearchBarButton from './SearchBarButton'

const SearchBar = ({query, handleChange, placeholder, handleClick}) => {
    return (
        <form className="d-flex flex-fill">
            <input className="form-control" type="search" 
                onChange={handleChange} placeholder={placeholder} aria-label={placeholder} name="query" value={query} />

            <SearchBarButton handleClick={handleClick} />
        </form>
    )
}

export default SearchBar
