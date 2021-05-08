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
import EventDetails from './components/EventDetails';
import EventEditor from './components/EventEditor';
import ExpenseEditor from './components/ExpenseEditor'
import ExpenseDetails from './components/ExpenseDetails';
import 'placeholder-loading/dist/css/placeholder-loading.css';

import Wip from './components/Events/Event'

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
                        <ProtectedRoute exact path='/event/editor/:id?' component={EventEditor} />
                        <ProtectedRoute exact path='/event/:id' component={EventDetails} />
                        <ProtectedRoute exact path='/expense/editor:id?' component={ExpenseEditor} />
                        <ProtectedRoute exact path='/expense/:id' component={ExpenseDetails} />
                        <ProtectedRoute exact path='/wip/:id' component={Wip}/>
                    </Switch>
                </Router>
            </AuthProvider>
        </div>
    );
}

export default App;
