$(function() {
    $('body').on('change', '#AreaCode_HeaderSelected', function() {
        var area = $(this).val();

        window.location.href = "/" + area + "/Home";
    });
});

//Prefix url
var _prefixAdminDomain = '/Admin';
var _prefixAccessDomain = '/Access';//Vào ra
var _prefixParkingDomain = '/Parking';//Bãi xe
var _prefixLockerDomain = '/Locker';//Tủ đồ
var _prefixResidentDomain = '/Resident';
//model for modal
class AJAXModel_Modal {
    constructor(url, idrecord, idmodal, idboxrender, isupdate, title, idupdate) {
        this.url = url;
        this.idrecord = idrecord;
        this.idmodal = idmodal;
        this.idboxrender = idboxrender;
        this.isupdate = isupdate;
        this.title = title;
        this.idupdate = idupdate;
    }
}