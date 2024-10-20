let userCtx = document.getElementById('userChart').getContext('2d');

// Doughnut Chart for User Statistics
let userChart = new Chart(userCtx, {
    type: 'doughnut',
    data: {
        labels: ['Total Users', 'Active Users', 'New Users Today'],
        datasets: [{
            data: [0, 0, 0],  // Data will be dynamically updated
            backgroundColor: ['#007bff', '#28a745', '#17a2b8'],
            borderColor: '#fff',
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            legend: {
                position: 'bottom'
            }
        }
    }
});

// Function to update the doughnut chart
function updateUserChart(userData) {
    userChart.data.datasets[0].data = [userData.totalUsers, userData.activeUsers, userData.newUsersToday];
    userChart.update();
}