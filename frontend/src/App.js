import React from 'react'
import './App.css';

import { BrowserRouter as Router, Route } from 'react-router-dom'
import Landing from './components/Landing'
import Dashboard from './components/Dashboard'
import ProtectedRoute from './components/ProtectedRoute';

import { AuthProvider } from './AuthContext';
import Navigation from './components/Navigation';
import Login from './components/Login';

function App() {
    return (
        <div className="App">
            <AuthProvider>
                <Router>
                    <Navigation />
                    <Route exact path='/' render={props => <Landing {...props} />} />
                    <Route exact path='/login' component={Login} />

                    <ProtectedRoute exact path='/dashboard' component={Dashboard} />
                    <ProtectedRoute exact path='/dashboard2' component={Dashboard} />
                </Router>
            </AuthProvider>
        </div>
    );
}

export default App;
