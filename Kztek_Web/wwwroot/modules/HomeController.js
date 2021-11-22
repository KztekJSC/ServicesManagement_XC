$(function () {
    $('body').on('click', '.btnLic', function () {
        bootbox.confirm("Cập nhật license?", function (result) {
            if (result) {
                HomeController.LicenseAction();
            }
        });
    });

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
    }
}