import React from 'react'
import './App.css';

import { BrowserRouter as Router, Route } from 'react-router-dom'
import Landing from './components/Landing'
import Dashboard from './components/Dashboard'
import ProtectedRoute from './components/ProtectedRoute';

import { AuthProvider } from './AuthContext';
import Navigation from './components/Navigation';
import Login from './components/Login';
import Info from './components/Info';
import EventCreate from './components/EventCreate';
import ExpenseEdit from './components/ExpenseEdit'
import EventView from './components/EventView';
import EventEdit from './components/EventEdit';

function App() {
    return (
        <div className="App">
            <AuthProvider>
                <Router>
                    <Navigation />
                    <Route exact path='/' render={props => <Landing {...props} />} />
                    <Route exact path='/login' component={Login} />

                    <ProtectedRoute exact path='/dashboard' component={Dashboard} />
                    <ProtectedRoute exact path='/info' component={Info} />
                    <ProtectedRoute exact path='/event/editor' component={EventCreate} />
                    <ProtectedRoute path='/event/view/:id' component={EventView} />
                    <ProtectedRoute exact path='/expense/editor' component={ExpenseEdit} />
                    <ProtectedRoute path='/event/editor/:id' component={EventEdit} />
                </Router>
            </AuthProvider>
        </div>
    );
}

export default App;
