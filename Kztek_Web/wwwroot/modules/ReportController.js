$(function () {

    //gọi dịch vụ
    $("body").on("click", ".btnCall", function () {
        var id = $(this).attr("idata");
        ReportController.Call(id);
    });

    //gọi vào kho
    $("body").on("click", ".btnCallWH", function () {
        var id = $(this).attr("idata");
        ReportController.CallWH(id);
    });

    $('body').on('click', '#pageSearch li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        ReportController.PartialSearchProduct(_page);

        return false;
    })


})

var ReportController = {
    Call: function (id) {
        var model = {
            id: id
        };
        JSHelper.AJAX_HttpPost('/Admin/Report/UpdateEvent', model)
            .success(function (result) {
                if (result.isSuccess) {
                    
                }
            });
     
    },
    CallWH: function (id) {
        var model = {
            id: id
        };
        JSHelper.AJAX_HttpPost('/Admin/Report/UpdateEventService', model)
            .success(function (result) {
                if (result.isSuccess) {

                }
            });

    },
    PartialEvent: function (page) {
        var obj = {
            page: page
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Report/Partial_EventIn', obj)
            .done(function (data) {
                $('#tblEvent tbody').html('');
                $('#tblEvent tbody').html(data);
            });
    },
}