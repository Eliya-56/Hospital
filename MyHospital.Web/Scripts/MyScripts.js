

$(function () {
    $('#Profile').click(function () {
        location.href = '/Account/UserProfile';
    });
    $('#Doctors').click(function () {
        location.href = '/Account/Doctors';
    });

    $('#Deseases').click(function () {
        location.href = '/Desease/Deseases/';
    });

    $('#Patients').click(function () {
        location.href = '/Account/Patients';
    });

    $('#Logout').click(function () {
        location.href = '/Account/Logout';
    });

    $('#ReverseToDesease').click(function () {
        location.href = '/Desease/Deseases/';
    });

    $('#ReverseToDoctors').click(function () {
        location.href = '/Account/Doctors';
    });
});





