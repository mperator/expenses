import React from 'react'

function Login() {
    return (
        <div>
            <form action="">
                <div>
                    <label htmlFor="email">Email or Username</label>
                    <input id="email" name="email" type="text" className="text" />
                </div>
                <div>
                    <label htmlFor="password">Password</label>
                    <input id="password" name="password" type="password" className="text" />
                </div>
                <button>Login</button>
            </form>
        </div>
    )
}

export default Login
