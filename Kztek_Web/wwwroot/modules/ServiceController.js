$(function () {
    

    $('body').on('click', '#pagService li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        ServiceController.PartialService(_page);

        return false;
    })

    $('body').on('click', '.btnSearch', function () {
        ServiceController.PartialService(1);
    })

    $('body').on('click', '.btnSearchAssignment', function () {
        ServiceController.PartialVehicle();
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

    $('body').on('click', '.btnAssign', function () {
        var id = $(this).attr("idata");

        ServiceController.Modal_Assign(id);
    })

    $('body').on('click', '#ModalAssign #btnCompleted', function () {
        ServiceController.SaveAssign();
    })

    $('body').on('click', '#tblGroup tr.trItem', function () {
        var row = $(this);
        var id = row.attr('idata');
        //alert(id);
        $('tr.trHide:not(.showDetailBox-child-' + id + ')').hide();
        // nếu đã load rồi
        if (row.parent().find('.showDetailBox-child-' + id).length > 0) {

            var chk = row.parent().find('.showDetailBox-child-' + id).is(":hidden");
            if (chk) {
                row.parent().find('.showDetailBox-child-' + id).stop().fadeIn();
            } else {
                row.parent().find('.showDetailBox-child-' + id).stop().fadeOut();
            }
        } else { // nếu chưa load
            ServiceController.PartialGroupDetail(id, row);
        }
    });

    $('body').on('click', '.btnUpdateGroup', function () {
        var id = $(this).attr("idata");

        ServiceController.Modal_UpdateGroup(id);
    })

    $('body').on('click', '#ModalUpdateGroup #btnCompleted', function () {
        ServiceController.UpdateGroup();
    })
})

var ServiceController = {
    PartialService: function (page) {
        var obj = {
            key: $("input[name=key]").val(),
            StatusID: $("#StatusID").val(),
            ServiceId: $("#ServiceId").val(),
            GroupId: $("#GroupId").val(),
            fromdate: $("#fromdate").val(),
            todate: $("#todate").val(),
            page: page
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Partial_Service', obj)
            .done(function (data) {
              /*  JSLoader.load_ViewImage();*/
                $('#boxTable1').html('');
                $('#boxTable1').html(data);
                JSLoader.load_ViewImage();
                $("#spCount").text($("#totalCount").val());
            });
    },
    AddValueSelect: function (str, controller, action) {
        var obj = {
            str: str,
            controller: controller,
            action: action
        
        }
        JSHelper.AJAX_LoadDataPOST('/Admin/Service/AddChooseSelect', obj).done(function (result) {
            if (result.isSuccess) {
                  ServiceController.PartialService(1);
                //$('table tr').find('th').each(function (i) {
                //    var cmd1 = $(this);
                //    var column = cmd1.attr('idata');
                //    var shows = repose.split(',');
                //    for (var i = 0; i < shows.length; i++) {
                //        if (column == shows[i]) {
                //            $("thead tr").find("th[idata=" + shows[i] + "]").css("display", "");
                //        }
                //    }
                //});
                //$('table tr').find('td').each(function (i) {
                //    var cmd1 = $(this);
                //    var column = cmd1.attr('idata');
                //    var shows = repose.split(',');
                //    for (var i = 0; i < shows.length; i++) {
                //        if (column == shows[i]) {
                //            $("tbody tr").find("td[idata=" + shows[i] + "]").css("display", "");
                //        }
                //    }
                //});
              
            }
        });
               
    },

    PartialVehicle: function () {
        var obj = {
            key: $("input[name=key]").val(),
            ServiceId: $("#ServiceId").val(),
            fromdate: $("#fromdate").val(),
            ParkingPosittion: $("#ParkingPosittion").val()
          
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Partial_Vehicle', obj)
            .done(function (data) {
                $('#tblVehicle tbody').html('');
                $('#tblVehicle tbody').html(data);
                $("#countPt").text($("#totalCount").val());
                
            });
    },
    PartialGroup: function () {
        var obj = {

        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Partial_Group', obj)
            .done(function (data) {
                $('#tblGroup tbody').html('');
                $('#tblGroup tbody').html(data);
            });
    },
    PartialGroupDetail: function (id,row) {
        var obj = {
            id: id
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Partial_GroupDetail', obj)
            .done(function (data) {
                if (data !== '') {
                    if (row.parent().find('.showDetailBox-child-' + id).length <= 0) {
                        row.after(data);
                        row.parent().find('.showDetailBox-child-' + id).stop().fadeIn();
                    }
                }
            });       
    },
    Modal_Assign: function (id) {
        var obj = {
            id: id
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Modal_Assign', obj)
            .done(function (data) {
                $("#boxModal").html(data);
                $("#ModalAssign").modal("show");
            });
    },
    SaveAssign: function () {
        var gid = $("#GroupId").val();

        if (gid === '' || gid === null || typeof gid === 'undefined') {
            toastr.error("Vui lòng chọn tổ thực hiện!");
            return false;
        }

        var obj = {
            id: $("#serId").val(),
            groupid: gid
        };

        JSHelper.AJAX_HttpPost('/Admin/Service/SaveAssign', obj)
            .done(function (data) {
                if (data.isSuccess) {
                    $('#ModalAssign').modal("hide");

                    toastr.success("Thành công!");

                    ServiceController.PartialVehicle();

                    ServiceController.PartialGroup();

                } else {
                    toastr.error(data.Message);
                }
            });
    },
    Modal_UpdateGroup: function (id) {
        var obj = {
            id: id
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Modal_UpdateGroup', obj)
            .done(function (data) {
                $("#boxModal").html(data);
                $("#ModalUpdateGroup").modal("show");
            });
    },
    UpdateGroup: function () {
        var gid = $("#GroupId").val();

        var obj = {
            id: $("#serId").val(),
            groupid: gid
        };

        JSHelper.AJAX_HttpPost('/Admin/Service/UpdateGroup', obj)
            .done(function (data) {
                if (data.isSuccess) {
                    $('#ModalUpdateGroup').modal("hide");

                    ServiceController.PartialVehicle();

                    ServiceController.PartialGroup();

                    toastr.success("Cập nhật thành công!");

                } else {
                    toastr.error(data.Message);
                }
            });
    },
}