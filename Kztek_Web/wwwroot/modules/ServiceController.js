$(function () {
    ServiceController.PartialService(1);

    $('body').on('click', '#pagService li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        ServiceController.PartialService(_page);

        return false;
    })

    $('body').on('click', '.btnSearch', function () {
        ServiceController.PartialService(1);
    })

    $('body').on('click', '.btnDelete', function () {
        var cmd = $(this);
        var id = cmd.attr('idata');

        bootbox.confirm("Bạn chắc chắn muốn xóa bản ghi này?", function (result) {
            if (result) {
                JSHelper.AJAX_Delete('/Admin/Service/Delete', id)
                    .success(function (response) {
                        if (response.isSuccess) {
                            cmd.parent().parent().parent().fadeOut();
                            toastr.success("Thành công")
                        } else {
                            toastr.error(response.Message)
                        }
                    });
            }
        })
    });
})

var ServiceController = {
    PartialService: function (page) {
        var obj = {
            key: $("input[name=key]").val(),
            StatusID: $("#StatusID").val(),
            page: page
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Partial_Service', obj)
            .done(function (data) {
                $('#boxTable').html('');
                $('#boxTable').html(data);

                $("#spCount").text($("#totalCount").val());
            });
    },
}