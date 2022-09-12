function ValidateInputsEstate() {
    var flag = true;
    var giaBan = $('#SalePrice');
    var giaThue = $('#RentPrice');
    var typeId = $('#Estate_TypeId');
    var khuPhoId = $('#TownId');
    var soDienThoai = $('#Mobile');
    var kyHieu = $('#KyHieu');
    var landArea = $('#LandArea');
    
    var ProvinceId = $('#ProvinceId');
    var DistrictId = $('#DistrictId');
    var GroupId = $('#GroupId');   
    var salePrice = Number($("#SalePrice").val().replace(/[^0-9\.]+/g, ""));
    var rentPrice = Number($("#RentPrice").val().replace(/[^0-9\.]+/g, ""));
    var area = Number($("#LandArea").val().replace(/[^0-9\.]+/g, ""));
    var selectedTown = khuPhoId.val();
    if (landArea.val() != "" && $.trim(landArea.val()) != "0") {
        landArea.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(landArea.val()) == '' || $.trim(landArea.val()) == 0 || $.trim(landArea.val()) == "0") {
        landArea.closest('.form-group').addClass('has-error');
        flag = false;
    }
    if (area.toString().length > 6) {
        landArea.closest('.form-group').addClass('has-error');
        flag = false;
    }
    //var StatusId = $('#StatusId');
    //if (StatusId.val() != "") {
    //    StatusId.closest('.form-group').removeClass('has-error');
    //}
    //if ($.trim(StatusId.val()) == '') {
    //    StatusId.closest('.form-group').addClass('has-error');
    //    flag = false;
    //}
    if (GroupId.val() != "") {
        GroupId.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(GroupId.val()) == '') {
        GroupId.closest('.form-group').addClass('has-error');
        flag = false;
    }

    if (ProvinceId.val() != "") {
        ProvinceId.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(ProvinceId.val()) == '') {
        ProvinceId.closest('.form-group').addClass('has-error');
        flag = false;
    }
    if (DistrictId.val() != "") {
        DistrictId.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(DistrictId.val()) == '') {
        DistrictId.closest('.form-group').addClass('has-error');
        flag = false;
    }

    var selectedGroupId = GroupId.val();
    if (typeof selectedGroupId === "undefined" || selectedGroupId == "") {
        GroupId.closest('.form-group').addClass('has-error');
        flag = false;
    }
    else {
        var saleUnitId = $('#SaleUnitId').val();
        var rentUnitId = $('#RentUnitId').val();
        giaBan.closest('.form-group').removeClass('has-error');
        giaThue.closest('.form-group').removeClass('has-error');

        switch (selectedGroupId) {
            case "1": // ban
                if (salePrice == 0 && saleUnitId != 1) {
                    giaBan.closest('.form-group').addClass('has-error');
                    flag = false;
                }
                else {
                    giaBan.closest('.form-group').removeClass('has-error');
                }
                break;
            case "2": // cho thue
                if (rentPrice == 0 && rentUnitId != 1) {
                    giaThue.closest('.form-group').addClass('has-error');
                    flag = false;
                }
                else {
                    giaThue.closest('.form-group').removeClass('has-error');
                }
                break;
            case "3": // ban - cho thue
                if (salePrice == 0 && saleUnitId != 1) {
                    giaBan.closest('.form-group').addClass('has-error');
                    flag = false;
                }
                else {
                    giaBan.closest('.form-group').removeClass('has-error');
                }

                if (rentPrice == 0 && rentUnitId != 1) {
                    giaThue.closest('.form-group').addClass('has-error');
                    flag = false;
                }
                else {
                    giaThue.closest('.form-group').removeClass('has-error');
                }
                break;
            default:
                break;
        }
    }

    if (typeId.val() != "") {
        typeId.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(typeId.val()) == '') {
        typeId.closest('.form-group').addClass('has-error');
        flag = false;
    }
    if ($("#TownId option").length > 1) {
        var selectProject = $("#ProjectId").val();
        var hasSelectProject = typeof selectProject !== "undefined" && selectProject != "";

        if ((typeof selectedTown === "undefined" || selectedTown == "") && hasSelectProject == true) {
            khuPhoId.closest('.form-group').addClass('has-error');
            flag = false;
        }
        else {
            khuPhoId.closest('.form-group').removeClass('has-error');
        }

        var productCode = $.trim(kyHieu.val());
        if ((typeof productCode === "undefined" || productCode == "") && hasSelectProject == true) {
            kyHieu.closest('.form-group').addClass('has-error');
            flag = false;
        }
        else {
            kyHieu.closest('.form-group').removeClass('has-error');
        }
    }
    else {
        khuPhoId.closest('.form-group').removeClass('has-error');
        kyHieu.closest('.form-group').removeClass('has-error');
    }

    var numberLotElement = $("#Number_Lot_Id");
    var numberPaperElement = $("#Number_Paper_Id");
    var addressElement = $("#Estate_Address");
    numberLotElement.closest('.form-group').removeClass('has-error');
    numberPaperElement.closest('.form-group').removeClass('has-error');
    addressElement.closest('.row').removeClass('has-error');
    var selectedProject = $("#ProjectId").val();
    if ((typeof selectedProject === "undefined" || selectedProject == "")) {
        //Neu khong chon du an thi bat buoc nhap so to, so thua, ten duong
        var numberLotValue = numberLotElement.val();
        var numberPaperValue = numberPaperElement.val();
        var addressValue = addressElement.val();

        if ((typeof numberLotValue === "undefined" || numberLotValue == "" || numberLotValue == 0)) {
            var testa = numberLotElement.closest('.form-group');
            numberLotElement.closest('.form-group').addClass('has-error');
            flag = false;
        }
        if ((typeof numberPaperValue === "undefined" || numberPaperValue == "" || numberPaperValue == 0)) {
            numberPaperElement.closest('.form-group').addClass('has-error');
            flag = false;
        }
        if ((typeof addressValue === "undefined" || addressValue == "")) {
            addressElement.closest('.row').addClass('has-error');
            flag = false;
        }
    }

    if ($.trim(soDienThoai.val()) != '') {
        soDienThoai.closest('.form-group').removeClass('has-error');
        if (soDienThoai.val().lenght < 5 || soDienThoai.val().lenght > 15) {
            soDienThoai.closest('.form-group').addClass('has-error');
            flag = false;
        }
    }
    soDienThoai.closest('.form-group').removeClass('has-error');
    var isOutsideChecked = $('#IsOutSide:checked').val();
    if ($.trim(soDienThoai.val()) === '' && isOutsideChecked == "False") {
        soDienThoai.closest('.form-group').addClass('has-error');
        flag = false;
    }
	
    var isMobileNumberValid = isValidMobileNumber(soDienThoai.val());
    if (!isMobileNumberValid) {
        soDienThoai.closest('.form-group').addClass('has-error');
        flag = false;
    }
    
    //if ($('#SaleUnitId').val() == 3) {
    //    if (salePrice.toString().length > 4) {
    //        giaBan.closest('.form-group').addClass('has-error');
    //        flag = false;
    //    }
    //    else {
    //        giaBan.closest('.form-group').removeClass('has-error');
    //    }
    //}
    if (flag == false) {
        $("#validae-message").show(300);
    }
    return flag;
};


// Change Location Province and District 

function provinceChange(id) {
   // var provinceId = $(this).find(":selected").val();
    var provinceId = id;
    $.ajax({
        type: "POST",
        url: "/Wards/GetAllDistrictByProvinceId",
        data: JSON.stringify({ 'provinceId': provinceId }),
        contentType: "application/json;charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            $("#DistrictId").empty();
            if (data.length > 0) {
                $.each(data, function (index, item) {
                    for (var key in item) { }
                    var name = item.Name;
                    $("<option/>", {
                        value: item.ItemId,
                        text: name
                    }).appendTo("#DistrictId");
                });
            } else {
                $("<option/>", {
                    value: "0",
                    text: "No district/province found"
                }).appendTo("#DistrictId");
            }
        },
       
    });
};

//Get all projects by investor
function investorChange(investorId) {
    if (typeof investorId !== "undefined" && investorId != "") {
        $.ajax({
            type: "POST",
            url: "/Estates/GetAllProjectsByInvestor",
            data: JSON.stringify({ 'investorId': investorId }),
            contentType: "application/json;charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                $("#ProjectId").empty();
                if (data.length > 0) {
                    $("<option/>", {
                        value: "",
                        text: "Select project"
                    }).appendTo("#ProjectId");
                    $.each(data, function (index, item) {
                        $("<option/>", {
                            value: item.ItemId,
                            text: item.Name
                        }).appendTo("#ProjectId");
                    });
                } else {
                    $("<option/>", {
                        value: "",
                        text: "No project found"
                    }).appendTo("#ProjectId");

                    $("#TownId").empty();
                    $("<option/>", {
                        value: "",
                        text: "Select project first"
                    }).appendTo("#TownId");
                    $("#townLabel i").remove();
                    $("#productCodeLabel i").remove();
                }

            }
        });
    }
    else {
        $("#ProjectId").empty();
        $("<option/>", {
            value: "",
            text: "-- Select investor first -- "
        }).appendTo("#ProjectId");

        $("#TownId").empty();
        $("<option/>", {
            value: "",
            text: "-- Select Project first -- "
        }).appendTo("#TownId");
        $("#townLabel i").remove();
        $("#productCodeLabel i").remove();
    } 
};
//Get all towns by project
function projectChange(projectId) {
    if (typeof projectId !== "undefined" && projectId != "") {
        $.ajax({
            type: "POST",
            url: "/Estates/GetAllTownsByProject",
            data: JSON.stringify({ 'projectId': projectId }),
            contentType: "application/json;charset=utf-8",
            cache: false,
            dataType: 'json',
            success: function (data) {
                $("#TownId").empty();
                if (data.length > 0) {
                    $("<option/>", {
                        value: "",
                        text: "Select Town"
                    }).appendTo("#TownId");
                    $.each(data, function (index, item) {
                        $("<option/>", {
                            value: item.ItemId,
                            text: item.Name
                        }).appendTo("#TownId");
                    });
                    $("#townLabel").append(" <i class='fa fa-asterisk text-red'></i>");
                    $("#productCodeLabel").append(" <i class='fa fa-asterisk text-red'></i>");
                } else {
                    $("<option/>", {
                        value: "",
                        text: "No Town found"
                    }).appendTo("#TownId");
                    $("#townLabel i").remove();
                    $("#productCodeLabel i").remove();
                }
            }
        });
    }
    else {
        $("#TownId").empty();
        $("<option/>", {
            value: "",
            text: "-- Select Project first --"
        }).appendTo("#TownId");
        $("#townLabel i").remove();
        $("#productCodeLabel i").remove();
    }
};


function districtChange(id) {
    var districtId = id;
    $.ajax({
        type: "POST",
        url: "/Wards/GetAllWardByDistrictId",      
        data: JSON.stringify({ 'districtId': districtId }),
        contentType: "application/json;charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {           
            $("#WardId").empty();
            if (data.length > 0) {
                $.each(data, function (index, item) {
                    for (var key in item) { }
                    var name = item.Name;
                    $("<option/>", {
                        value: item.ItemId,
                        text: name
                    }).appendTo("#WardId");
                });
            }
            else {
                $("<option/>", {
                    value: "0",
                    text: "No ward found"
                }).appendTo("#WardId");
            }
        },
    });
    $.ajax({
        type: "POST",
        url: "/Estate_Projects/GetAllProjectByDistrictId",
        data: JSON.stringify({ 'districtId': districtId }),
        contentType: "application/json;charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            $("#ProjectId").empty();
            if (data.length > 0) {
                $("<option/>", {
                    value: "",
                    text: "Select project"
                }).appendTo("#ProjectId");
                $.each(data, function (index, item) {
                    $("<option/>", {
                        value: item.ItemId,
                        text: item.Name
                    }).appendTo("#ProjectId");
                });
            } else {
                $("<option/>", {
                    value: "",
                    text: "No project found"
                }).appendTo("#ProjectId");

                $("#TownId").empty();
                $("<option/>", {
                    value: "",
                    text: "Select Project first"
                }).appendTo("#TownId");
                $("#townLabel i").remove();
                $("#productCodeLabel i").remove();
            }
        }
    });
    $.ajax({
        type: "POST",
        url: "/Streets/GetStreetByDistrictId",
        data: JSON.stringify({ 'districtId': districtId }),
        contentType: "application/json;charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            $("#StreetId").empty();
            if (data.length > 0) {
                $("<option/>", {
                    value: "",
                    text: "Select street"
                }).appendTo("#StreetId");
                $.each(data, function (index, item) {
                    $("<option/>", {
                        value: item.StreetId,
                        text: item.Name
                    }).appendTo("#StreetId");
                });
            } else {
                $("<option/>", {
                    value: "",
                    text: "No street found"
                }).appendTo("#StreetId");
            }
        }
    });
};

function districtChangeFirstTime(id) {
    var districtId = id;
    $.ajax({
        type: "POST",
        url: "/Wards/GetAllWardByDistrictId",
        data: JSON.stringify({ 'districtId': districtId }),
        contentType: "application/json;charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            $("#WardId").empty();
            if (data.length > 0) {
                $.each(data, function (index, item) {
                    for (var key in item) { }
                    var name = item.Name;
                    $("<option/>", {
                        value: item.ItemId,
                        text: name
                    }).appendTo("#WardId");
                });
            }
            else {
                $("<option/>", {
                    value: "0",
                    text: "No ward found"
                }).appendTo("#WardId");
            }
        },
    });
    $.ajax({
        type: "POST",
        url: "/Estate_Projects/GetAllProjectByDistrictId",
        data: JSON.stringify({ 'districtId': districtId }),
        contentType: "application/json;charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            $("#ProjectId").empty();
            if (data.length > 0) {
                $("<option/>", {
                    value: "",
                    text: "Select project"
                }).appendTo("#ProjectId");
                $.each(data, function (index, item) {
                    $("<option/>", {
                        value: item.ItemId,
                        text: item.Name
                    }).appendTo("#ProjectId");
                });
            } else {
                $("<option/>", {
                    value: "",
                    text: "No project found"
                }).appendTo("#ProjectId");

                $("#TownId").empty();
                $("<option/>", {
                    value: "",
                    text: "Select Project first"
                }).appendTo("#TownId");
                $("#townLabel i").remove();
                $("#productCodeLabel i").remove();
            }
        }
    });
    $.ajax({
        type: "POST",
        url: "/Streets/GetStreetByDistrictId",
        data: JSON.stringify({ 'districtId': districtId }),
        contentType: "application/json;charset=utf-8",
        cache: false,
        dataType: 'json',
        success: function (data) {
            $("#StreetId").empty();
            if (data.length > 0) {
                $("<option/>", {
                    value: "",
                    text: "Select street"
                }).appendTo("#StreetId");
                $.each(data, function (index, item) {
                    $("<option/>", {
                        value: item.StreetId,
                        text: item.Name
                    }).appendTo("#StreetId");
                });
            } else {
                $("<option/>", {
                    value: "",
                    text: "No street found"
                }).appendTo("#StreetId");
            }
        }
    });
};

function isValidMobileNumber(mobile) {
    if ((typeof mobile === "undefined" || mobile == "")) {
        return true;
    }
    mobile = mobile.replace(/\s+/g, "");
    return mobile.length > 9 && mobile.length < 16
        mobile.match(/^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/);
}

