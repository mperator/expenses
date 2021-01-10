import React from 'react'
import './App.css';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import Landing from './components/Landing'
import Dashboard from './components/Dashboard'
import ProtectedRoute from './components/ProtectedRoute';
import { AuthProvider } from './AuthContext';
import Navigation from './components/Navigation';
import Login from './components/Login';
import Info from './components/Info';
import EventDetails from './components/EventDetails';
import EventEditor from './components/EventEditor';
import ExpenseEditor from './components/ExpenseEditor'
import 'placeholder-loading/dist/css/placeholder-loading.css';

function App() {
    return (
        <div className="App">
            <AuthProvider>
                <Router>
                    <Navigation />
                    <Switch>
                        <Route exact path='/' render={props => <Landing {...props} />} />
                        <Route exact path='/login' component={Login} />

                        <ProtectedRoute exact path='/dashboard' component={Dashboard} />
                        <ProtectedRoute exact path='/info' component={Info} />
                        <ProtectedRoute exact path='/event/editor/:id?' component={EventEditor} />
                        <ProtectedRoute exact path='/event/:id' component={EventDetails} />
                        <ProtectedRoute exact path='/expense/editor' component={ExpenseEditor} />
                    </Switch>
                </Router>
            </AuthProvider>
        </div>
    );
}

export default App;
