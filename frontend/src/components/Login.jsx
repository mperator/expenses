import React from 'react'

function Login() {
    return (
        <div className="container">
            <div className="row mt-5">
                <div className="col" />
                <div className="col-lg-6">
                    <form>
                        <div className="row mb-3">
                            <label for="inputEmail3" className="col-sm-2 col-form-label">Email</label>
                            <div className="col-sm-10">
                                <input type="email" className="form-control" id="inputEmail3" />
                            </div>
                        </div>
                        <div className="row mb-3">
                            <label for="inputPassword3" className="col-sm-2 col-form-label">Password</label>
                            <div className="col-sm-10">
                                <input type="password" className="form-control" id="inputPassword3" />
                            </div>
                        </div>

                        {/* <div className="row mb-3">
                            <div className="col-sm-10 offset-sm-2">
                                <div className="form-check">
                                    <input className="form-check-input" type="checkbox" id="gridCheck1" />
                                    <label className="form-check-label" for="gridCheck1">Remember</label>
                                </div>
                            </div>
                        </div> */}
                        <div className="position-relative">
                            <button type="submit" className="btn btn-primary position-absolute top-0 right-0">Sign in</button>
                        </div>
                    </form>
                </div>
                <div className="col" />
            </div>
        </div>
    )
}

export default Login
