const connection = new signalR.HubConnectionBuilder().withUrl("/hubs/statistics").build();

connection.on("ReceiveStatistics", (data) => {
    // Update the doughnut chart for user statistics
    updateUserChart({
        totalUsers: data.totalUsers,
        activeUsers: data.activeUsers,
        newUsersToday: data.newUsersToday
    });

    // Update Total Sales and Errors Reported
    document.getElementById('totalSales').textContent = `$${data.totalSales.toFixed(2)}`;
    document.getElementById('errorsReported').textContent = data.errorsReported;
});

connection.start().catch(err => console.error(err.toString()));