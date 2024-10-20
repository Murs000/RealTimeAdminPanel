let userCtx = document.getElementById('userChart').getContext('2d');

// Doughnut Chart for User Statistics
let userChart = new Chart(userCtx, {
    type: 'doughnut',
    data: {
        labels: ['Total Users', 'Active Users', 'New Users Today'],
        datasets: [{
            data: [0, 0, 0],  // Data will be dynamically updated
            backgroundColor: ['#1e88e5', '#43a047', '#00acc1'],
            borderColor: '#fff',
            borderWidth: 2
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                position: 'bottom',
                labels: {
                    color: '#424242',
                    font: {
                        size: 14
                    }
                }
            }
        }
    }
});

// Function to update the doughnut chart
function updateUserChart(userData) {
    userChart.data.datasets[0].data = [userData.totalUsers, userData.activeUsers, userData.newUsersToday];
    userChart.update();
}