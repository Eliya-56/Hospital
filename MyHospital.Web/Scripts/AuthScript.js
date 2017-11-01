
$(function () {

    var loadingFlag = 0;
    $(".menu-item").bind('click', function () {
        if (loadingFlag == 0) {
            $(".menu-item").removeClass("active");
            $(this).addClass("active");
        }
    });
    $('#Auth').click(function () {
        if (loadingFlag == 0) {
            Loading();
            $.get('http://' + location.host + '/Account/Login', function (data) {
                EndLoading(data);
            });
        }
    });
    $('#Reg').click(function () {
        if (loadingFlag == 0) {
            Loading();
            $.get('http://' + location.host + '/Account/Register', function (data) {
                EndLoading(data);
            });
        }
    });

    function Loading() {
        loadingFlag = 1;
        $("button").attr("disabled", null);
        $('#main-inner').html("<h3>Загрузка...</h3>");
    }

    function EndLoading(data) {
        loadingFlag = 0;
        $('#main-inner').html(data);
        $("button").removeAttr("disabled");
    }
});
