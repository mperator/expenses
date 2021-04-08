import React from 'react'

function Register() {
    return (
        <div className="container">
            <div className="row mt-5">
                <div className="col" />
                <div className="col-lg-6">
                    <form>
                        <div className="row mb-3">
                            <label htmlFor="username" className="col-sm-2 col-form-label">Username</label>
                            <div className="col-sm-10">
                                <input type="text" className="form-control" id="username" name="username"></input>
                            </div>
                        </div>
                        <div className="row mb-3">
                            <label htmlFor="email" className="col-sm-2 col-form-label">Email</label>
                            <div className="col-sm-10">
                                <input type="email" className="form-control" id="email" name="email"></input>
                            </div>
                        </div>
                        <div className="row mb-3">
                            <label htmlFor="firstName" className="col-sm-2 col-form-label">First Name</label>
                            <div className="col-sm-10">
                                <input type="text" className="form-control" id="firstName" name="firstName"></input>
                            </div>
                        </div>
                        <div className="row mb-3">
                            <label htmlFor="lastName" className="col-sm-2 col-form-label">Last Name</label>
                            <div className="col-sm-10">
                                <input type="text" className="form-control" id="lastName" name="lastName"></input>
                            </div>
                        </div>
                        <div className="row mb-3">
                            <label htmlFor="password" className="col-sm-2 col-form-label">Password</label>
                            <div className="col-sm-10">
                                <input type="password" className="form-control" id="password" name="password"></input>
                            </div>
                        </div>
                        <div className="row mb-3">
                            <label htmlFor="passwordConfirmation" className="col-sm-2 col-form-label">Password Confirmation</label>
                            <div className="col-sm-10">
                                <input type="password" className="form-control" id="passwordConfirmation" name="passwordConfirmation"></input>
                            </div>
                        </div>
                        <div className="position-relative">
                            <button type="submit" className="btn btn-primary float-end">Register</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    )
}

export default Register
