﻿$(function () {
    

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

   
})

var ServiceController = {
    PartialService: function (page) {
        var obj = {
            key: $("input[name=key]").val(),
            StatusID: $("#StatusID").val(),
            ServiceId: $("#ServiceId").val(),
            GroupId: $("#GroupId").val(),
            page: page
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Partial_Service', obj)
            .done(function (data) {
                $('#boxTable').html('');
                $('#boxTable').html(data);

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
           
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Partial_Vehicle', obj)
            .done(function (data) {
                $('#tblVehicle tbody').html('');
                $('#tblVehicle tbody').html(data);
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
   
}