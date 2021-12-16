$(function () {
    CoordinatorController.PartialCoordinator(1);

    $('body').on('click', '#pagCoordinator li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        CoordinatorController.PartialCoordinator(_page);

        return false;
    })

    $('body').on('click', '.btnOK', function () {
        var id = $(this).attr("idata");

        bootbox.confirm({
            message: "<h3 style='font-weight:bold'>Bạn đồng ý xác nhận công việc này?</h3>",
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
                    CoordinatorController.UpdateService(id);
                }
            }
        });


    })

})

var CoordinatorController = {
    PartialCoordinator: function (page) {
        var obj = {
            key: $("input[name=key]").val(),
            StatusID: $("#StatusID").val(),
            page: page
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Coordinator/Partial_Coordinator', obj)
            .done(function (data) {
                $('#boxTable').html('');
                $('#boxTable').html(data);

                $("#spCount").text($("#totalCount").val());
            });
    },
    UpdateService: function (id) {
        var obj = {
            id: id
        };

        JSHelper.AJAX_HttpPost('/Admin/Coordinator/UpdateService', obj)
            .done(function (data) {

                if (data.isSuccess) {
                    var page = $("#pagConfGroup li.active a").attr("idata");

                    CoordinatorController.PartialCoordinator(page);

                    toastr.success("Cập nhật thành công");
                } else {
                    toastr.error(data.Message);
                }

            });
    },
}