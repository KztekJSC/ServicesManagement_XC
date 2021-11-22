$(function () {

})

var tblLaneController = {
    GetListCamera: function (pcID, number, select) {
        var model = {
            pcID: pcID,
            cameraNumber: number,
            selected: select
        }
        JSHelper.AJAX_LoadDataPOST(_prefixParkingDomain + '/tblLane/ListCameraByPC', model)
            .success(function (data) {
                $("#boxC" + number).html('');
                $("#boxC" + number).html(data);
            });       
    },
    ShowCam: function (c1,c2,c3,c4,c5,c6) {
        var pcid = $("#PCID").val();
        var lanetype = $("#LaneType").val();

        if (lanetype === "" || lanetype === "0" || lanetype === "1") {
            $(".boxHiddenCamera").fadeOut();

            tblLaneController.GetListCamera(pcid, "1", c1);
            tblLaneController.GetListCamera(pcid, "2", c2);
            tblLaneController.GetListCamera(pcid, "3", c3);
        } else {
            $(".boxHiddenCamera").fadeIn();

            tblLaneController.GetListCamera(pcid, "1", c1);
            tblLaneController.GetListCamera(pcid, "2", c2);
            tblLaneController.GetListCamera(pcid, "3", c3);
            tblLaneController.GetListCamera(pcid, "4", c4);
            tblLaneController.GetListCamera(pcid, "5", c5);
            tblLaneController.GetListCamera(pcid, "6", c6);
        }
    }
}