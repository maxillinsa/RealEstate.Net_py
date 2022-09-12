// JScript File
function FormatNumber(obj) {
    var strvalue;
    if (eval(obj))
        strvalue = eval(obj).value;
    else
        strvalue = obj;
    var num;
    if (strvalue.indexOf(".") >= 0)
        return;
    num = strvalue.toString().replace(/\$|\,/g, '');

    if (isNaN(num))
        num = "";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    num = Math.floor(num / 100).toString();
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
    //return (((sign)?'':'-') + num);
    var finalNumber = (((sign) ? '' : '-') + num);
    eval(obj).value = finalNumber;
}
function NumberOnly(evt) {
    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        alert("Chỉ nhập số! (Number Only!)")
        return false
    }
    return true
}
function NumberAndDash(evt) {
    evt = (evt) ? evt : window.event
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47) {
        alert("Chỉ nhập số hoặc ký tự '/'")
        return false
    }
    return true
}
function FormatNumberString(strvalue) {
    var num;
    num = strvalue.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    num = Math.floor(num / 100).toString();
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
    return (((sign)?'':'-') + num);
}
        $(function () {
           
            $('#stores').dataTable({
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bAutoWidth": true
            });
            $('#products').dataTable({
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bAutoWidth": true
            });
            $('#categories').dataTable({
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bAutoWidth": true
            });
            $('#Categorys').dataTable({
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bAutoWidth": true
            });
            $('#accounts').dataTable({
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bAutoWidth": true 
            });
            $('#orders').dataTable({
                "bPaginate": false,
                "bLengthChange": false,
                "bFilter": false,
                "bAutoWidth": true
            });
            $('#sanphams').dataTable({
                "bPaginate": false,
                "bAutoWidth": false,
                "bFilter": true,

            });
        });