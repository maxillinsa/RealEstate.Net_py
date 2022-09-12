
function RenderActions(renderActionstring) {
    $("#OpenDialog").load(renderActionstring);
};
function showLoader(txt) {
    $("#Loader").fadeIn(400);
    var loaderTxt = txt;
    $("#LoaderTxt").fadeIn(400).html(loaderTxt);
};
function hideLoader() {
    $("#Loader").fadeOut(400);
};
function showError(txt) {
    $("#showError").show();
    var errorTxt = '<strong>Error! </strong>' + txt;
    $("#ErrorTxt").fadeIn(400).html(errorTxt);
};
// Class add select2 
$('.select2').select2(
    {
        dropdownAutoWidth: true,
        width: '269'
    });

function Clean() {
    $('#modalCrud').find('textarea,input').val('');
};


function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function notifyer() {
    var notify = $.notify('<strong>Saving</strong> Do not close this page...', {
        allow_dismiss: false,
        showProgressbar: true
    });

    setTimeout(function () {
        notify.update({ 'type': 'success', 'message': '<strong>Success</strong> Save success!', 'progress': 10 });
    }, 4500);
};

/// ----------Start Country Script ajax ----------------------------

// Validate from Input in Form
function ValidateCountryInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');      
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};
function ValidateCustomerCategoryInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};
/// ----------End Country Script ajax ----------------------------

/// ----------Start Country Script ajax ----------------------------

// Validate from Input in Form
function ValidateProvinceInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};

/// ----------End Country Script ajax ----------------------------

/// ----------Start Country Script ajax ----------------------------

// Validate from Input in Form
function ValidateDistrictInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};

/// ----------End Country Script ajax ----------------------------

/// ----------Start Country Script ajax ----------------------------

// Validate from Input in Form
function ValidateWardInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};
function ValidateStreetInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    var districtId = $('#DistrictId');
    if ($.trim(districtId.val()) != '') {
        districtId.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(districtId.val()) === '') {
        districtId.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};
/// ----------End Country Script ajax ----------------------------
/// ----------Start Estate_Investor Script ajax ----------------------------

// Validate from Input in Form
function ValidateEstate_InvestorInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};

/// ----------End Estate_Investor Script ajax ----------------------------
/// ----------Start Validator Script ajax ----------------------------

// Validate from Input in Form
function ValidateEstate_ProjectInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    var district = $('#DistrictId');
    if ($.trim(district.val()) != '') {
        district.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(district.val()) === '') {
        district.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};

function ValidateEstate_StatusInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};

function ValidateEstate_GroupInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};

function ValidateCertificate_TypeInputs() {
    var flag = true;
    var name = $('#Name');
    if ($.trim(name.val()) != '') {
        name.closest('.form-group').removeClass('has-error');
    }
    if ($.trim(name.val()) === '') {
        name.closest('.form-group').addClass('has-error');
        flag = false;
    }
    return flag;
};

/// ----------End Validator Script ajax ----------------------------


function PriceDropDownList(priceType = 1) {
    var prices;
    if (priceType == 1) {
        prices = [
            { PriceId: 0, Name: "Select Sale Price" },
            { PriceId: 1, Name: "< 500 M" },
            { PriceId: 2, Name: "500 - 800 M" },
            { PriceId: 3, Name: "800 M - 1 B" },
            { PriceId: 4, Name: "1 - 2 B" },
            { PriceId: 5, Name: "2 - 3 B" },
            { PriceId: 6, Name: "3 - 5 B" },
            { PriceId: 7, Name: "5 - 7 B" },
            { PriceId: 8, Name: "7 - 10 B" },
            { PriceId: 9, Name: "10 - 20 B" },
            { PriceId: 10, Name: "20 - 30 B" },
            { PriceId: 11, Name: " > 30 B" },

        ];
    }
    else {
        prices = [
            { PriceId: 0, Name: "Select Rent Price" },
            { PriceId: 20, Name: "< 1 M" },
            { PriceId: 21, Name: "1 - 3 M" },
            { PriceId: 22, Name: "3 - 5 M" },
            { PriceId: 23, Name: "5 - 10 M" },
            { PriceId: 24, Name: "10 - 40 M" },
            { PriceId: 25, Name: "40 - 70 M" },
            { PriceId: 26, Name: "70 - 100 M" },
            { PriceId: 27, Name: " > 100 M" },

        ];
    }
    var ddlPrices = $("#typePriceList");
    $(prices).each(function () {
        var option = $("<option />");

        //Set Customer Name in Text part.
        option.html(this.Name);

        //Set Customer CustomerId in Value part.
        option.val(this.PriceId);

        //Add the Option element to DropDownList.
        ddlPrices.append(option);
    });
}