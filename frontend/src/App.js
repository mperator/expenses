import React, { useState } from 'react'
import './App.css';

import { BrowserRouter as Router, Route } from 'react-router-dom'
import Landing from './components/Landing'
import Dashboard from './components/Dashboard'
import ProtectedRoute from './components/ProtectedRoute';
import Unauthorized from './components/Unauthorized';
// import Login from './components/Login'
// import Register from './components/Register'

function App() {
    const [user, setUser] = useState(false);

    const handleLogin = e => {
        e.preventDefault();
        setUser(true);
    }

    const handleLogout = e => {
        e.preventDefault();
        setUser(false);
    }

    return (
        <div className="App">
            <Router>
                <Route exact path='/' render={props => <Landing {...props} user={user.toString()} handleLogin={handleLogin} />} />
                <ProtectedRoute exact path='/dashboard' user={user} handleLogout={handleLogout} component={Dashboard} />
                <Route exact path='/unauthorized' component={Unauthorized} />
            </Router>
        </div>
    );
}

export default App;
