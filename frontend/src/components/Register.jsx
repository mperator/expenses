import React from 'react'

function Register() {
    return (
        <div>
            <form action="">
                <div>
                    <label htmlFor="email">Email</label>
                    <input id="email" name="email" type="text" className="text" />
                </div>
                <div>
                    <label htmlFor="username">Username</label>
                    <input id="username" name="username" type="text" className="text" />
                </div>
                <div>
                    <label htmlFor="firstName">First name</label>
                    <input id="firstName" name="firstName" type="text" className="text" />
                </div>
                <div>
                    <label htmlFor="lastName">Last name</label>
                    <input id="lastName" name="lastName" type="text" className="text" />
                </div>
                <div>
                    <label htmlFor="password">Password</label>
                    <input id="password" name="password" type="password" className="text" />
                </div>
                <div>
                    <label htmlFor="passwordConfirm">Confirm password</label>
                    <input id="passwordConfirm" name="passwordConfirm" type="password" className="text" />
                </div>
                <button>Login</button>
            </form>
        </div>
    )
}

export default Register
