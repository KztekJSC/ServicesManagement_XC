$(function () {
    ConfirmedGroupController.PartialConfirmedGroup(1);

    $('body').on('click', '#pagConfGroup li a', function () {
        var cmd = $(this);
        var _page = cmd.attr('idata');

        ConfirmedGroupController.PartialConfirmedGroup(_page);

        return false;
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
   
}