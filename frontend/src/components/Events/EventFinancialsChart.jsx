import { HorizontalBar } from 'react-chartjs-2';

const EventFinancialsChart = ({ financials }) => {
    const options = {
        indexAxis: 'y',
        responsive: true,
        plugins: { legend: { position: 'right', }, }
    }
    const colorCredit = 'rgba(0, 255, 0, 0.8)';
    const colorDebit = 'rgba(255, 0, 0, 0.8)';

    const calcualateCartData = (data) => {
        const labels = [];
        const credit = { label: 'credit', data: [], backgroundColor: [] };
        const debit = { label: 'debit', data: [], backgroundColor: [] };

        for (const d of data) {
            labels.push(d.username);
            if (d.balance >= 0) {
                credit.data.push(d.balance);
                credit.backgroundColor.push(colorCredit);
                debit.data.push(0);
                debit.backgroundColor.push(colorDebit);
            } else {
                debit.data.push(d.balance);
                debit.backgroundColor.push(colorDebit);
                credit.data.push(0);
                credit.backgroundColor.push(colorCredit);
            }
        }
        return {
            labels,
            datasets: [credit, debit]
        };
    };

    return (
        <>
            {financials ?
                <HorizontalBar data={calcualateCartData(financials)} options={options} /> :
                null}
        </>
    )
}

export default EventFinancialsChart
