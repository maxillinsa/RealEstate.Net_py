$(function () {

    $('.print').click(function () {
        var container = $(this).attr('rel');
        $('#' + container).printArea();
        return false;
    });
    $('.print2').click(function () {
        var container = $(this).attr('rel');
        $('#' + container).printArea();
        return false;
    });
    $('.print3').click(function () {
        $("#print33").hide();
//        $("#back").show();
        var container = $(this).attr('rel');
        $('#' + container).printArea();
        //    location.reload();
       
        return false;
    });
    $('.printPhoiVe').click(function () {
        $("#print33").hide();
        var container = $(this).attr('rel');
        $('#' + container).printArea();
        return false;
    });
    $('.print4').click(function () {
        $("#print44").hide();
        var container = $(this).attr('rel');
        $('#' + container).printArea();
        return false;
    });
});