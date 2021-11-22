$(function () {
    $("body").on("change", "#CameraIn", function () {
        $("#C_In").val($(this).val());
    })

    $("body").on("change", "#CameraOut", function () {
        $("#C_Out").val($(this).val());
    })

    $("body").on("change", "#CameraAll", function () {
        $("#C_All").val($(this).val());
    })
})