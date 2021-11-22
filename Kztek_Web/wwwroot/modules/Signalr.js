

$(() => {
    let connection = new signalR.HubConnectionBuilder().withUrl("/sqlHub").build()

    connection.start()

    connection.on("tbl_Event", function (type, obj) {
        if (type === "service") {
            $("#spPlate").text(obj.plate_Number1);
            $("#" + obj.id).remove();

            var countText = $("#spCount").text();

            if (countText === '' || countText === null) {
                countText = 0;
            }

            var count = parseInt(countText);

            if (count > 0) {
                $("#spCount").text(count - 1);
            }

            //bàn làm việc
            if ($("#hRow_" + obj.id).length > 0) {
                $("#hRow_" + obj.id).remove();

                var countHomeText = $("#spCountHome").text();

                if (countHomeText === '' || countHomeText === null) {
                    countHomeText = 0;
                }

                var counthome = parseInt(countHomeText);

                if (counthome > 0) {
                    $("#spCountHome").text(counthome - 1);
                }             
            }
            SignalrController.PartialTopService();

        } else {
            $("#spPlateWH").text(obj.plate_Number1);
            $("#WH_" + obj.id).remove();

            var countText = $("#spCountWH").text();

            if (countText === '' || countText === null) {
                countText = 0;
            }

            var count = parseInt(countText);

            if (count > 0) {
                $("#spCountWH").text(count - 1);
            }
        }

        
    })

    SignalrController.PartialTopService();
})

var SignalrController = {
    PartialTopService: function () {
        var obj = {
            
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Home/Partial_TopService', obj)
            .done(function (data) {
                $('#boxService ').html('');
                $('#boxService ').html(data);
            });
    },
}