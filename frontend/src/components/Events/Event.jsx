import React, { useEffect, useState } from 'react';
import useClient from '../hooks/useClient';
import { useParams } from 'react-router';
import { Link } from 'react-router-dom';
import dayjs from 'dayjs';
import bootstrap from 'bootstrap/dist/js/bootstrap.min.js';
import { HorizontalBar } from 'react-chartjs-2';
import jwt_decode from "jwt-decode";
import useAuth from '../hooks/useAuth';

const EventDetails = () => {
    const { getEventAsync } = useClient();
    const { token } = useAuth();
    const params = useParams();

    const [loading, setLoading] = useState(true);
    const [event, setEvent] = useState('')
    const [showFinancials, setShowFinancials] = useState(false)
    const [chartData, setChartData] = useState({
        labels: '',
        datasets: []
    });
    const receiveColor = 'rgba(0, 255, 0, 0.8)';
    const loanColor = 'rgba(255, 0, 0, 0.8)';

    useEffect(() => {
        // load event using the incoming id
        if (params.id) {
            (async () => {
                // try catch ignore or redirect when failing
                const event = await getEventAsync(params.id);
                
                calculateOwings(event);
                // console.log(event.expenses[1].debits)
                setEvent(event);
                setLoading(false);
            })();
        }
    }, [])

    useEffect(() => {
    }, [chartData])

    const calculateOwings = (event) => {
        // calculate for every person the 
        // gross spendings | net spendings | delta | transaction partner
        // dynamic problem
        const table = [];
        for(const participant of event.participants) {
            var dept = calculateUserDebt(participant.id)
            var loan = calculateUserLoan(participant.id)
            table.push({ userId: participant.id, balance: dept-loan})
        }

        table.sort(a => -1 * a.balance);

        



        console.table(table)
        
        
        
    }

    const calculateExpenseSummary = () => {
        const expenses = event.expenses;
        if (expenses.length > 0)
            return expenses.map(a => a.credit.amount).reduce((a, c) => a + c);
        else
            return 0;
    }

    const calculateUserDebt = (userId) => {
        const dept = event.expenses
            .flatMap(e => e.debits)
            .filter(d => d.debitorId === userId)
            .map(d => d.amount)
            .reduce((a, c) => a + c, 0);
        return dept;
    }

    const calculateUserLoan = (userId) => {
        const loan = event.expenses
            .flatMap(e => e.credit)
            .filter(c => c.creditorId === userId)
            .map(c => c.amount)
            .reduce((a, c) => a + c, 0);
        return loan;
    }

    const handleDeleteButton = () => {
        const confirmDeletionModal = new bootstrap.Modal(document.getElementById('deleteConfirmationModal'));
        confirmDeletionModal.toggle();
    }

    const deleteEvent = () => {
        console.log("implement deleting event!!!!");
    }

    const loadChart = () => {
        // extract own user id from jwt token
        const decodedToken = jwt_decode(token);
        const idSelf = decodedToken.sub;
        console.log("THATS ME!!!", idSelf)
        // console.log(event.expenses)
        // console.log(event.expenses[1].debits)
        // FIXME: loans and debts seem to be saved somehow --> weird behaviour
        if (event !== '') {
            //     // FIXME: navigating to expense details view and back to event details view throws error --> why?
            const labels = event.participants.filter(participant => participant.id !== idSelf).flatMap(p =>
                p.username);
            // console.log("Lables for chart", labels)
            const labelsWithIds = event.participants.filter(participant => participant.id !== idSelf).map(p => {
                return { username: p.username, id: p.id }
            });
            console.log("Labels with IDs", labelsWithIds)
            let loans = [];
            let debts = [];
            console.log("BEGINNING loans", loans)
            console.log("BEGINNING debts", debts)
            const expenses = event.expenses;
            console.log(expenses)
            expenses.forEach(expense => {
                if (expense.credit.creditorId === idSelf) {
                    // expense.debits.forEach(debit => {
                    // loans.push(debit)
                    // });
                    console.log("original", expense.debits)
                    const filteredDebits = expense.debits.filter(debt => debt.debitorId !== idSelf);
                    console.log("filtered", filteredDebits)
                    console.log(loans)
                    loans = [...loans, ...filteredDebits]
                    console.log(loans)
                }
                if (expense.credit.creditorId !== idSelf) {
                    const debtsIdSelf = expense.debits.filter(debit => debit.debitorId === idSelf);
                    debtsIdSelf.forEach(d => {
                        debts.push({
                            amount: d.amount,
                            creditorId: expense.credit.creditorId
                        })
                    });
                }
            });
            console.log("BEFORE loans", loans)
            console.log("BEFORE debts", debts)
            loans.forEach((loan, indexLoan) => {
                debts.forEach(debt => {
                    if (loan.debitorId === debt.creditorId) {
                        // subtract loan from debt
                        const newLoan = loan.amount - debt.amount;
                        // is loan still bigger than 0 --> update loan
                        if (newLoan > 0) {
                            console.log("in here")
                            loan.amount = newLoan;
                        }
                        else {
                            console.log("doing the else")
                            // loan is smaller than 0 --> delete entry in loan array and update calculated amount to correct entry of debts array
                            debt.amount = newLoan;
                            // remove one element at indexLoan
                            loans.splice(indexLoan, 1);
                            console.log(loans)
                        }
                    }
                });
            });
            console.log("AFTER loans", loans)
            console.log("AFTER debts", debts)
            let combinedDebts = [];
            // combine all debts of the same id to one debt
            labelsWithIds.forEach(user => {
                const debtsSameId = debts.filter(debt => debt.creditorId === user.id)
                console.log(debtsSameId)
                let summedUpDebt = 0;
                debtsSameId.forEach(debtSameId => {
                    summedUpDebt = summedUpDebt + debtSameId.amount
                });
                combinedDebts.push({
                    amount: summedUpDebt,
                    creditorId: user.id
                })
            });
            console.log("Combined debts: ", combinedDebts)
            // combine all loans of the same id to one loan??
            // TODO:

            let backgroundColorsReceive = [];
            let backgroundColorsLoan = [];
            let newLabels = [];
            loans.forEach(loan => {
                backgroundColorsReceive.push(receiveColor);
                // add user only as label as long as he's not already in the label array
                if (!newLabels.includes(labelsWithIds.find(member => member.id === loan.debitorId).username)) {
                    newLabels.push(labelsWithIds.find(member => member.id === loan.debitorId).username)
                }
            });
            combinedDebts.forEach(debt => {
                if (debt.amount !== 0) {
                    loans.forEach(loan => {
                        backgroundColorsLoan.push("");
                    });
                    backgroundColorsLoan.push(loanColor);
                    // add user only as label as long as he's not already in the label array
                    if (!newLabels.includes(labelsWithIds.find(member => member.id === debt.creditorId).username)) {
                        newLabels.push(labelsWithIds.find(member => member.id === debt.creditorId).username)
                    }
                }
            });
            console.log("new labels", newLabels)
            let loanData = [...loans.flatMap(loan => loan.amount)];
            combinedDebts.forEach(debt => {
                // only push empty strings in case there are loans
                if (loans.length > 0) {
                    loanData.push("");
                }
            });
            let debtData = [];
            const filteredFlattedDebts = combinedDebts.filter(debt => debt.amount !== 0).flatMap(debt => debt.amount);
            loans.forEach(loan => {
                if (filteredFlattedDebts.length > 0) {
                    debtData.push("");
                }
            });
            debtData = [...debtData, ...filteredFlattedDebts];
            // console.log(test)
            let datasets = []
            if (loanData.length > 0 && debtData.length > 0) {
                datasets = [
                    {
                        label: 'you receive',
                        data: loanData,
                        backgroundColor: backgroundColorsReceive,
                    },
                    {
                        label: 'you owe',
                        data: debtData,
                        backgroundColor: backgroundColorsLoan,
                    },
                ]
            } else if (loanData.length > 0 && debtData.length === 0) {
                datasets = [
                    {
                        label: 'you receive',
                        data: loanData,
                        backgroundColor: backgroundColorsReceive,
                    }
                ]
            } else if (loanData.length === 0 && debtData.length > 0) {
                datasets = [
                    {
                        label: 'you owe',
                        data: debtData,
                        backgroundColor: backgroundColorsLoan,
                    },
                ]
            }
            console.log(datasets)
            setChartData({
                labels: newLabels,
                datasets: datasets
            })
        }
    }

    const data = {
        labels: ['Seppl Sappl', 'Testuser', 'Hans', 'Appleboy'],
        datasets: [
            {
                label: 'you receive',
                data: [4, 4, 4, ""],
                backgroundColor: [
                    'rgba(0, 255, 0, 0.8)',
                    'rgba(0, 255, 0, 0.8)',
                    'rgba(0, 255, 0, 0.8)',
                    "",
                ],
                // borderColor: [
                //     'rgba(255, 99, 132, 1)',
                //     'rgba(54, 162, 235, 1)',
                //     'rgba(255, 206, 86, 1)',
                // ],
                // borderWidth: 1,
            },
            {
                label: 'you owe',
                data: ["", "", "", -2],
                backgroundColor: [
                    "",
                    "",
                    "",
                    'rgba(255, 0, 0, 0.8)',
                ],
                // borderColor: [
                //     'rgba(255, 99, 132, 1)',
                //     'rgba(54, 162, 235, 1)',
                //     'rgba(255, 206, 86, 1)',
                // ],
                // borderWidth: 1,
            },
        ],
    }

    const options = {
        indexAxis: 'y',
        // Elements options apply to all of the options unless overridden in a dataset
        // In this case, we are setting the border of each horizontal bar to be 2px wide
        // elements: {
        //     bar: {
        //         borderWidth: 2,
        //     }
        // },
        responsive: true,
        plugins: {
            legend: {
                position: 'right',
            },
            // title: {
            //     display: true,
            //     text: 'Chart.js Horizontal Bar Chart'
            // }
        }
    }

    const handleTab = () => {
        setShowFinancials(!showFinancials, loadChart())
    }

    //TODO: if total expense is negative show font in red and vice versa
    //TODO: add logo in front of user name
    //TODO: make each expense clickable and open expense editor for corresponding expense after click
    //TODO: delete button has no functionality yet
    return (
        <div className="container mt-4">
            {loading ? <p>Loading ....</p> :
                <>
                    {/* <p>{!loading && calculateExpenseSummary()}</p> */}
                    <div className="card">
                        <div className="card-header">
                            <div className="row">
                                <div className="col-auto">
                                    <h3>{event.title}</h3>
                                </div>
                                <div className="col-auto">
                                    <Link className="btn btn-outline-primary me-1" to={`/event/editor/${params.id}`}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="0.8em" height="0.8em" fill="currentColor" className="bi bi-pencil-fill" viewBox="0 0 16 16">
                                            <path d="M12.854.146a.5.5 0 0 0-.707 0L10.5 1.793 14.207 5.5l1.647-1.646a.5.5 0 0 0 0-.708l-3-3zm.646 6.061L9.793 2.5 3.293 9H3.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.207l6.5-6.5zm-7.468 7.468A.5.5 0 0 1 6 13.5V13h-.5a.5.5 0 0 1-.5-.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.5-.5V10h-.5a.499.499 0 0 1-.175-.032l-.179.178a.5.5 0 0 0-.11.168l-2 5a.5.5 0 0 0 .65.65l5-2a.5.5 0 0 0 .168-.11l.178-.178z" />
                                        </svg>
                                    </Link>
                                    <button className="btn btn-outline-danger" onClick={handleDeleteButton}>
                                        <svg xmlns="http://www.w3.org/2000/svg" width="0.8em" height="0.8em" fill="currentColor" className="bi bi-trash-fill" viewBox="0 0 16 16">
                                            <path d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 1 0z" />
                                        </svg>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div className="card-body">
                            <p className="card-text">{event.description}</p>
                            <div className="row">
                                <div className="col">
                                    <div className="form-floating mb-3">
                                        <input type="text" className="form-control" id="floatingStartDate" readOnly value={dayjs(event.startDate).format('DD/MM/YYYY')} />
                                        <label htmlFor="floatingStartDate">Start date</label>
                                    </div>
                                </div>
                                <div className="col">
                                    <div className="form-floating mb-3">
                                        <input type="text" className="form-control" id="floatingEndDate" readOnly value={dayjs(event.endDate).format('DD/MM/YYYY')} />
                                        <label htmlFor="floatingEndDate">End date</label>
                                    </div>
                                </div>
                            </div>
                            <ul className="list-group">
                                <li className="list-group-item">
                                    <div className="row justify-content-between align-items-center">
                                        <div className="col-auto">
                                            <h4>Total expenses:</h4>
                                        </div>
                                        <div className="col-auto">
                                            <h4>{calculateExpenseSummary()}€</h4>
                                        </div>
                                    </div>
                                </li>
                                {event.participants.map(a => (
                                    <li key={a.id} className="list-group-item">
                                        <div className="row justify-content-between align-items-center">
                                            <div className="col-auto">
                                                {a.username}
                                            </div>
                                            <div className="col-auto">
                                                <div className="col-auto">
                                                    Dept: {calculateUserDebt(a.id)}€
                                                </div>
                                                <div className="col-auto">
                                                    Loan: {calculateUserLoan(a.id)}€
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                ))}
                            </ul>
                            {/* <div className="accordion my-2" id="accordionFlushExample">
                                <div className="accordion-item">
                                    <h2 className="accordion-header" id="flush-headingOne">
                                        <button className="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#flush-collapseOne" aria-expanded="false" aria-controls="flush-collapseOne">
                                            <h6>Expenses</h6>
                                        </button>
                                    </h2>
                                     <div id="flush-collapseOne" className="accordion-collapse" aria-labelledby="flush-headingOne" data-bs-parent="#accordionFlushExample">
                                        <div className="accordion-body">
                                            <ul className="list-group">
                                                {event.expenses.length === 0 ? "no expenses yet ... go and add one"
                                                    : event.expenses.map(e => (
                                                        <li key={e.id} className="list-group-item">
                                                            <p className="mb-0 fs-6">{e.title}</p>
                                                            <p className="fs-4 fw-bold mb-0">{e.credit.amount}€</p>
                                                            <p style={{ fontSize: '0.7rem' }}>{dayjs(e.date).format('DD/MM/YYYY')}</p>
                                                            <Link to={`/expense/editor?expenseId=${e.id}&eventId=${event.id}`} className="stretched-link" />
                                                        </li>
                                                    ))
                                                }
                                            </ul>
                                        </div>
                                    </div> 
                                </div>
                            </div>*/}
                            <div className="mt-3">
                                <ul className="nav nav-tabs" role="tablist">
                                    <li className="nav-item" role="presentation">
                                        <button className="nav-link active" id="expenses-tab" data-bs-toggle="tab" data-bs-target="#expenses" type="button" role="tab" aria-controls="expenses" aria-selected="true">Expenses</button>
                                    </li>
                                    <li className="nav-item" role="presentation">
                                        <button className="nav-link" id="expenses-tab" onClick={loadChart} data-bs-toggle="tab" data-bs-target="#financials" type="button" role="tab" aria-controls="financials" aria-selected="false">Financials</button>
                                    </li>
                                </ul>
                                <div className="tab-content" id="myTabContent">
                                    <div className="tab-pane fade show active" id="expenses" role="tabpanel" aria-labelledby="expenses-tab">
                                        <ul className="list-group">
                                            {event.expenses.length === 0 ? "no expenses yet ... go and add one"
                                                : event.expenses.map(e => (
                                                    <li key={e.id} className="list-group-item">
                                                        <p className="mb-0 fs-6">{e.title}</p>
                                                        <p className="fs-4 fw-bold mb-0">{e.credit.amount}€</p>
                                                        <p style={{ fontSize: '0.7rem' }}>{dayjs(e.date).format('DD/MM/YYYY')}</p>
                                                        <Link to={`/expense/editor?expenseId=${e.id}&eventId=${event.id}`} className="stretched-link" />
                                                    </li>
                                                ))
                                            }
                                        </ul>
                                    </div>
                                    <div className="tab-pane fade" id="financials" role="tabpanel" aria-labelledby="financials-tab">
                                        {/* <canvas id="loanChart"></canvas> */}
                                        <HorizontalBar data={chartData} options={options} />
                                    </div>
                                </div>
                            </div>
                            <Link className="text-dark" to={`/expense/editor?eventId=${event.id}`}>
                                <svg width="0.8em" height="0.8em" viewBox="0 0 16 16" className="mt-3 addButton bi bi-plus-circle-fill text-primary" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                    <path fillRule="evenodd" d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
                                </svg>
                            </Link>
                        </div>
                    </div>
                    {/* <!-- Modal --> */}
                    <div className="modal fade" id="deleteConfirmationModal" tabIndex="-1" aria-labelledby="deleteConfirmationModalLabel" aria-hidden="true">
                        <div className="modal-dialog">
                            <div className="modal-content">
                                <div className="modal-header">
                                    <h5 className="modal-title" id="deleteConfirmationModalLabel">Confirm deletion</h5>
                                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div className="modal-body">
                                    Do you want to delete the selected event?
                                </div>
                                <div className="modal-footer">
                                    <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">No</button>
                                    <button type="button" className="btn btn-primary" onClick={deleteEvent}>Yes</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </>}
        </div>
    )
}

export default EventDetails
