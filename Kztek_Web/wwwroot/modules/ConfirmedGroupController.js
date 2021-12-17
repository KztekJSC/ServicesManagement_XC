$(function () {
    ConfirmedGroupController.PartialConfirmedGroup(1);

    ConfirmedGroupController.PartialCountEvent();

    $('body').on('click', '.btnSearch', function () {
        ConfirmedGroupController.PartialConfirmedGroup(1);
    })

    $('body').on('click', '#pagConfGroup li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        ConfirmedGroupController.PartialConfirmedGroup(_page);

        return false;
    })

    $('body').on('click', '.btnStart', function () {
        var id = $(this).attr("idata");

        bootbox.confirm({
            message: "<h3 style='font-weight:bold'>Bạn chắc chắn muốn bắt đầu công việc này?</h3>",
            buttons: {
                confirm: {
                    label: 'Đồng ý',
                    className: 'btn-primary'
                },
                cancel: {
                    label: 'Hủy',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    ConfirmedGroupController.UpdateService(id, 3);
                }
            }
        });

        
    })

    $('body').on('click', '.btnEnd', function () {
        var id = $(this).attr("idata");

        bootbox.confirm({
            message: "<h3 style='font-weight:bold'>Bạn đã hoàn thành công việc chưa?</h3>",
            buttons: {
                confirm: {
                    label: 'Hoàn thành',
                    className: 'btn-primary'
                },
                cancel: {
                    label: 'Hủy',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result) {
                    ConfirmedGroupController.UpdateService(id, 4);
                }
            }
        }); 
    })
})

var ConfirmedGroupController = {
    PartialConfirmedGroup: function (page) {
        var obj = {
            key: $("input[name=key]").val(),
            StatusID: $("#StatusID").val(),
            page: page
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/ConfirmedGroup/Partial_ConfirmedGroup', obj)
            .done(function (data) {
                $('#boxTable').html('');
                $('#boxTable').html(data);

                $("#spCount").text($("#totalCount").val());
            });
    },
    UpdateService: function (id,type) {
        var obj = {
            type: type,
            id: id
        };

        JSHelper.AJAX_HttpPost('/Admin/ConfirmedGroup/UpdateService', obj)
            .done(function (data) {

                if (data.isSuccess) {
                    var page = $("#pagConfGroup li.active a").attr("idata");

                    ConfirmedGroupController.PartialConfirmedGroup(page);

                    ConfirmedGroupController.PartialCountEvent();

                    toastr.success("Cập nhật thành công");
                } else {
                    toastr.error(data.Message);
                }
               
            });
    },
    PartialCountEvent: function () {
        var obj = {
            
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/ConfirmedGroup/Partial_CountEvent', obj)
            .done(function (data) {
                $('#boxCountEvent').html('');
                $('#boxCountEvent').html(data);
            });
    },
   
}