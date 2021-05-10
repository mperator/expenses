import React from 'react'
import './App.css';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import Landing from './components/Landing'
import Dashboard from './components/Dashboard'
import ProtectedRoute from './components/ProtectedRoute';
import { AuthProvider } from './AuthContext';
import Navigation from './components/Navigation';
import Register from './components/Register'
import Login from './components/Login';
import Info from './components/Info';
import EventEditor from './components/EventEditor';
import ExpenseEditor from './components/ExpenseEditor'
import ExpenseDetails from './components/ExpenseDetails';
import 'placeholder-loading/dist/css/placeholder-loading.css';

import Event from './components/Events/Event'
import EventFinancials from './components/Events/EventFinancials';

function App() {
    return (
        <div className="App">
            <AuthProvider>
                <Router>
                    <Navigation />
                    <Switch>
                        <Route exact path='/' render={props => <Landing {...props} />} />
                        <Route exact path='/login' component={Login} />
                        <Route exact path='/register' component={Register} />

                        <ProtectedRoute exact path='/dashboard' component={Dashboard} />
                        <ProtectedRoute exact path='/info' component={Info} />
                        <ProtectedRoute exact path='/event/:id' component={Event} />
                        <ProtectedRoute exact path='/event/:id/financials' component={EventFinancials} />
                        <ProtectedRoute exact path='/event/editor/:id?' component={EventEditor} />

                        <ProtectedRoute exact path='/expense/editor:id?' component={ExpenseEditor} />
                        <ProtectedRoute exact path='/expense/:id' component={ExpenseDetails} />
                    </Switch>
                </Router>
            </AuthProvider>
        </div>
    );
}

export default App;
