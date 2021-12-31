$(function () {
    $('body').on('click', '.btnLic', function () {
        bootbox.confirm("Cập nhật license?", function (result) {
            if (result) {
                HomeController.LicenseAction();
            }
        });
    });
 
    $('body').on('click', '#pagService li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        HomeController.PartialService(_page);

        return false;
    })
    $('body').on('click', '.btnSearch', function () {
        HomeController.PartialService(1);
    })

    $("#langCode_Parking").on("change", function () {
        var langCode = $("#langCode_Parking").val();
        //alert(langCode);
        $.ajax({
            type: "POST",
            url: "/Login/ChangeLanguage?lang=" + langCode,
            success: function () {
                location.reload();
            },
            failure: function () {
                // alert("not ok");
            }
        });
    });

    HomeController.PartialNotifi();
})

var HomeController = {
    init() {
        HomeController.LicenseAction();
    },

    LicenseAction() {
        JSHelper.AJAX_SendRequest('/Home/GetLicense', {})
            .success(function (response) {
                console.log(response);
            if (response.isSuccess) {
                toastr.success(response.message);
            } else {
                toastr.error(response.message);
            }
        });
    },
    PartialService: function (page) {
        var obj = {
            key: $("input[name=key]").val(),
            GroupId: $("#GroupId").val(),
            page: page
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Home/Partial_Service', obj)
            .done(function (data) {
                $('#boxTable').html('');
                $('#boxTable').html(data);
                $("#spCount").text($("#totalCount").val());
            });
    },
    PartialNotifi: function () {
        var obj = {
            
        };

        JSHelper.AJAX_LoadDataPOST('/Admin/Service/Partial_Notifi', obj)
            .done(function (data) {
                $('#liNotifi').html('');
                $('#liNotifi').html(data);
                
            });
    },
}