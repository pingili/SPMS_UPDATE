
// 8-BackSpce , 9-Tab , 13-Enter , 16-Shift ,17-Alt , 18-Ctrl , 32-space , 96-105-NumPad 0-9 , 48-57- 0-9 Numbers
//   189-Period(- or _ ) , 190- . , 191- / ,37-40- l,u,r,d Arrows , 65-90 a-z , 187- =


$(function () {

    $('.number-only').on('keydown', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if (!((key == 8) || (key == 46) || (key == 9) || (key == 16) || (key >= 47 && key <= 57) || (key >= 35 && key <= 40) || (key >= 96 && key <= 105))) {
            e.preventDefault();
        }
        if (e.shiftKey && (key >= 47 && key <= 57)) {
            e.preventDefault();
        }

    });

    $('.alphabet-only').on('keydown', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if (!((key == 8) || (key == 9) || (key == 16) || (key == 189) || (key == 190) || (key == 46) || (key == 32) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
            e.preventDefault();
        }
        if ($(this).val().length == 0) {
            if (key == 32 || key == 189 || key == 190 || key == 191 || key == 187)
                e.preventDefault();
        }
        if (($(this).val().indexOf('.') > -1)) {
            if (key == 190)
                return false;
        }
        if (e.shiftKey && (key == 190)) {
            e.preventDefault();
        }

    });

    $('.alphanumeric-only').on('keydown', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if (!((key == 8) || (key == 16) || (key == 46) || (key == 9) || (key == 189) || (key == 190) || (key == 32) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 47 && key <= 57) || (key >= 96 && key <= 105)) || (key == 187)) {
            e.preventDefault();
        }
        if ($(this).val().length == 0) {
            if (key == 32 || key == 189 || key == 190 || key == 191 || key == 187)
                e.preventDefault();
        }
        if (($(this).val().indexOf('.') > -1)) {
            if (key == 190)
                return false;
        }
        if (e.shiftKey && (key >= 47 && key <= 57)) {
            e.preventDefault();
        }
        if (e.shiftKey && (key == 190)) {
            e.preventDefault();
        }

    });

    $('.alphanumericVTT-only').on('keydown', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if (!((key == 8) || (key == 16) || (key == 46) || (key == 9) || (key == 189) || (key == 190) || (key == 32) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 47 && key <= 57) || (key >= 96 && key <= 105)) || (key == 187)) {
            e.preventDefault();
        }
        if ($(this).val().length == 0) {
            if (key == 32 || key == 189 || key == 190 || key == 191 || key == 187)
                e.preventDefault();
        }
        if (($(this).val().indexOf('.') > -1)) {
            if (key == 190)
                return false;
        }
        if (e.shiftKey && (key >= 47 && key <= 57)) {
            e.preventDefault();
        }
        if (e.shiftKey && (key == 190)) {
            e.preventDefault();
        }
        if (key == 45 && (key == 47)) {
            e.preventDefault();
        }


    });

    $('.email-only').on('change', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if ($(this).val().length == 0)
            if (key == 32 || key == 189 || key == 190 || key == 190)
                e.preventDefault();

        var emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;
        if (emailRegex.test($(this).val())) {
            return true;
        }
        else {
            alert("Please Enter Valid Email..!" + "\n" + "Ex: SampleEmail@gmail.com");
            $(this).focus();
            $(this).val('');
            return false;
        }

    });

    $('.ifsc-only').on('change', function () {

        var key = e.keyCode ? e.keyCode : e.which;

        if ($(this).val().length == 0)
            if (key == 32 || key == 189 || key == 190 || key == 190)
                e.preventDefault();

        var ifscRegex = /[A-Z|a-z]{4}[0][\d]{6}$/;   //^[^\s]{4}\d{7}$;     //[A-Z|a-z]{4}[0][\d]{6}$
        if (ifscRegex.test($(this).val())) {
            return true;
        }
        else {
            alert("Please Enter Valid Input..!" + "\n" + "Ex: SBHI0123456");
            $(this).focus();
            $(this).val('');
            return false;
        }

    });

    $('.phonenumber-only').on('keydown', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if (!((key == 8) || (key == 9) || (key == 16) || (key == 46) || (key == 189) || (key >= 35 && key <= 40) || (key >= 47 && key <= 57) || (key >= 96 && key <= 105))) {
            e.preventDefault();
        }
        if ($(this).val().length == 15 && key != 8 && key != 9) {
            return false;
        }
        if (($(this).val().indexOf('-') > -1) || ($(this).val().length == 0)) {
            if (key == 189)
                return false;
        }
        if (e.shiftKey && (key >= 47 && key <= 57)) {
            e.preventDefault();
        }

    });

    $('.phonenumber-only').on('change', function () {

        var mobileRegex = /^[6-9]{1}[0-9]{9}$/;                //      /^\d{10}$/;    // /^\d{10}$/  /^[1-9]{1}[0-9]{9}$/
        var phoneNumberRegex = /^[0-9]{3,5}-[0-9]{6}$/;

        if ($(this).val().indexOf('-') > -1) {
            if (phoneNumberRegex.test($.trim($(this).val())) == false) {
                alert("Please Enter Valid Phone Number..!" + "\n" + "Ex: 040-123456 / 09718-123456");
                $(this).focus();
                $(this).val('');
                return false;
            }
            else
                return true;
        }
        if (mobileRegex.test($.trim($(this).val())) == false) {
            alert("Please Enter Valid Phone Number..!" + "\n" + "Ex: 9123456789 / 7123456789");
            $(this).focus();
            $(this).val('');
            return false;
        }
        //else
        //    return true;

    });

    $('.num-two-digit-only').on('keydown', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if ($(this).val().length == 0) {
            if (key == 32 || key == 189 || key == 190 || key == 190)
                e.preventDefault();
        } else {
            if (!((key == 8) || (key == 46) || (key == 9) || (key == 16) || (key >= 35 && key <= 40) || (key >= 47 && key <= 57) || (key >= 96 && key <= 105)) || (key >= 49 && key <= 57)) {
                e.preventDefault();
            }
        }
        if (e.shiftKey && (key >= 47 && key <= 57)) {
            e.preventDefault();
        }

    });

    $('.decimal-only').on('keypress', function (event) {

        if ((event.which != 46 || $(this).val().indexOf('.') != -1) &&
          ((event.which < 48 || event.which > 57) &&
            (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }
        var text = $(this).val();

        if ((text.indexOf('.') != -1) &&
          (text.substring(text.indexOf('.')).length > 2) &&
          (event.which != 0 && event.which != 8) &&
          ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }

    });

    $('.decimal-only').on('keydown', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if ($(this).val().length == 0) {
            if (key == 32 || key == 189 || key == 190 || key == 191 || key == 187)
                event.preventDefault();
        }

    });

    $('.date-only').on('keydown', function (e) {

        var key = e.keyCode ? e.keyCode : e.which;

        if (!((key == 8) || (key == 9) || (key == 191) || (key >= 35 && key <= 40) || (key >= 47 && key <= 57) || (key >= 96 && key <= 105))) {
            e.preventDefault();
        }
        if (e.shiftKey && (key >= 47 && key <= 57)) {
            e.preventDefault();
        }

    });

    $('.address-only').on('change', function () {

        var addresslRegex = /^[a-zA-Z0-9-\/] ?([a-zA-Z0-9-\/,]|[a-zA-Z0-9-\/,] )*[a-zA-Z0-9-\/,]$/;
        if (addresslRegex.test($(this).val())) {
            return true;
        }
        else {
            // ($(this).css('border-color', 'red')).focus();
            $(this).focus();
            $(this).val('');
            alert("Please Enter Valid Address..!");
            return false;
        }

    });

    $('.decimal-two-digit-only').on('keypress', 'input[type="text"]', function (e) {
        var el = $(this).get(0);
        var isValid = validateFloatKeyPress(el, e);
        return isValid;
    });


    //$('input').each(function () {
    //    if ($(this).val('')) {
    //        $(this).find('','')
    //        alert("Please Enter Value");
    //        return false;
    //    }
    //});

});

function RequiredFieldValidation(requiredFieldArray) {
    if (!requiredFieldArray)
        return !requiredFieldArray;

    try {
        var validateResult = true;
        for (i = 0; i < requiredFieldArray.length; i++) {
            if ($('#' + requiredFieldArray[i]).val() == '') {
                //$('#' + req[i]).css({ "background-color": "yellow", "font-size": "200%" })
                $('#' + requiredFieldArray[i]).css({ "border": "1px solid red" })
                validateResult = false;
            }
        }
        return validateResult;
    } catch (e) {
        return false;
    }
}

function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (dotPos > -1 || charCode == 46) {
        var number = el.value.split('.');
        //just one dot (thanks ddlab)
        if (number.length > 1 && charCode == 46) {
            return false;
        }
        if (caratPos > dotPos && dotPos > -1 && number[1].length > 1) {
            return false;
        }
    }
    else {
        if (el.value.length > 1)
            return false;
    }
    return true;
}
function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

function validateInterestRate(objtxt) {
    if ($(objtxt).val() == '')
        return;

    var rate = parseFloat($(objtxt).val());
    if (isNaN(rate)) {
        $(objtxt).val('');
        alert('Interest rate should be between 1% to 36%');
        return;
    }

    if (rate < 0 || rate > 36) {
        $(objtxt).val('');
        alert('Interest rate should be between 1% to 36%');
        return;
    }
}