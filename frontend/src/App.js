import React from 'react'
import './App.css';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import Landing from './components/Landing'
import Dashboard from './components/Dashboard/Dashboard'
import ProtectedRoute from './components/ProtectedRoute';
import { AuthProvider } from './AuthContext';
import Navigation from './components/Navigation';
import Register from './components/Register'
import Login from './components/Login';
import Info from './components/Info';
import Settings from './components/Settings/Settings';

import Event from './components/Events/Event'
import EventFinancials from './components/Events/EventFinancials';
import EventEdit from './components/Events/EventEdit';
import EventCreate from './components/Events/EventCreate';

import Expense from './components/Expenses/Expense'
import ExpenseNew from './components/Expenses/ExpenseNew';
import ExpenseEdit from './components/Expenses/ExpenseEdit';

import 'placeholder-loading/dist/css/placeholder-loading.css';

function App() {
    return (
        <div className="App">
            <AuthProvider>
                <Router>
                    <Navigation />
                    <Switch>
                        <Route exact path='/landing' render={props => <Landing {...props} />} />
                        <Route exact path='/login' component={Login} />
                        <Route exact path='/register' component={Register} />

                        <ProtectedRoute exact path='/' component={Dashboard} />
                        <ProtectedRoute exact path='/dashboard' component={Dashboard} />
                        <ProtectedRoute exact path='/event/new' component={EventCreate} />
                        <ProtectedRoute exact path='/event/:id' component={Event} />
                        <ProtectedRoute exact path='/event/:id/edit' component={EventEdit} />
                        <ProtectedRoute exact path='/event/:id/financials' component={EventFinancials} />

                        <ProtectedRoute exact path='/event/:eventId/expenses/new' component={ExpenseNew} />
                        <ProtectedRoute exact path='/event/:eventId/expenses/:expenseId' component={Expense} />
                        <ProtectedRoute exact path='/event/:eventId/expenses/:expenseId/edit' component={ExpenseEdit} />

                        <ProtectedRoute exact path='/info' component={Info} />
                        <ProtectedRoute exact path='/settings' component={Settings} />
                    </Switch>
                </Router>
            </AuthProvider>
        </div>
    );
}

export default App;
