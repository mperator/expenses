import React from 'react'
import './App.css';

import { BrowserRouter as Router, Route } from 'react-router-dom'
import Landing from './components/Landing'
import Dashboard from './components/Dashboard'
import ProtectedRoute from './components/ProtectedRoute';
import Unauthorized from './components/Unauthorized';
// import Login from './components/Login'
// import Register from './components/Register'

import { AuthProvider } from './AuthContext';

function App() {
    return (
        <div className="App">
            <AuthProvider>
                <Router>
                    <Route exact path='/' render={props => <Landing {...props} />} />
                    <ProtectedRoute exact path='/dashboard' component={Dashboard} />
                    <Route exact path='/unauthorized' component={Unauthorized} />
                </Router>
            </AuthProvider>
        </div>
    );
}

export default App;
