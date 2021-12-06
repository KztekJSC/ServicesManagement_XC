

$(() => {
    let connection = new signalR.HubConnectionBuilder().withUrl("/sqlHub").build()

    connection.start()

    connection.on("Service", function () {
        ServiceController.PartialService(1);
    })

})

var SignalrController = {
   
}