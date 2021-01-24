import React from 'react'

export default function Toast({ idString }) {
    return (
        <>
            {/* <!-- Toast --> */}
            <div className="toast position-absolute top-0 end-0 p-3 hide" id={idString} role="alert" aria-live="assertive" aria-atomic="true">
                <div className="toast-header">
                    <svg className="bd-placeholder-img rounded me-2" width="20" height="20" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" preserveAspectRatio="xMidYMid slice" focusable="false"><rect width="100%" height="100%" fill="#ff0011"></rect></svg>
                    <strong className="me-auto">Error</strong>
                    <button type="button" className="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
                <div className="toast-body">
                    An error occured. Please try again!
                </div>
            </div>
        </>
    )
}
