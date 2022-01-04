$(function () {
    CoordinatorController.PartialCoordinator(1);

    $('body').on('click', '.btnSearch', function () {
        CoordinatorController.PartialCoordinator(1);
    })

    $('body').on('click', '#pagCoordinator li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        CoordinatorController.PartialCoordinator(_page);

        return false;
    })

    $('body').on('click', '.btnOK', function () {
        var id = $(this).attr("idata");
        CoordinatorController.ModalInforCoordior(id);

    });
    $('body').on('click', '#ModalInfor #btnCompleted', function () {
        CoordinatorController.SaveService();
    });
    $('#columnId').change(function () {
        var str = "";
        var cmd = $(this);
        cmd.parent().find('ul.multiselect-container li.active').each(function () {
            var _cmd = $(this);
            str += _cmd.find('input[type=checkbox]').val() + ",";
        });
        CoordinatorController.AddValueSelects(str, "Coordinator", "Index");

    });
})

var CoordinatorController = {

    AddValueSelects: function (str, controller, action) {
        var obj = {
            str: str,
            controller: controller,
            action: action

        };
        JSHelper.AJAX_LoadDataPOST('/Admin/Coordinator/AddChooseSelect', obj).done(function (result) {
            if (result.isSuccess) {
                CoordinatorController.PartialCoordinator(1);
            }
        });

    },
    PartialCoordinator: function (page) {
        var obj = {
            key: $("input[name=key]").val(),
            StatusID: $("#StatusID").val(),
            fromdate: $("#fromdate").val(),
            todate: $("#todate").val(),
            ServiceId: $("#ServiceId").val(),
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
    ModalInforCoordior: function (id) {
        var obj = {
            id: id
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Coordinator/Modal_Info', obj)
            .done(function (data) {
                $("#boxModal").html(data);
                $("#ModalInfor").modal("show");
                JSLoader.load_MaskInput();
            });
    },
    SaveService: function () {

        var frm = $("#frmInfo");

        var obj = {
            Id: frm.find("#serId").val(),
            ServiceCode: frm.find("input[name=txtServiceCode]").val(),
            VehicleType: frm.find("input[name=txtVehicleType]").val(),
            PackageNumber: frm.find("input[name=txtPackageNumber]").val(),
            Weight: frm.find("input[name=txtWeight]").val(),           
            Description: frm.find("#Description").val()
        };

        JSHelper.AJAX_HttpPost('/Admin/Coordinator/SaveService', obj)
            .done(function (data) {

                if (data.isSuccess) {
                    $("#ModalInfor").modal("hide");

                    var page = $("#pagCoordinator li.active a").attr("idata");

                    CoordinatorController.PartialCoordinator(page);

                 

                    toastr.success("Cập nhật thành công");
                } else {
                    toastr.error(data.Message);
                }

            });
    },
}