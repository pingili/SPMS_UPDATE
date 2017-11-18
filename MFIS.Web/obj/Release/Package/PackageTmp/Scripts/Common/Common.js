var menuLink = '';
var subMenuLink = '';
//debugger;
var __currentOpenMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
function LoadMenu() {
    $('#side-menu > li').removeClass('active');
    $('#side-menu > li > ul > li').removeClass('selected');
    $('#side-menu > li > ul').removeClass('in');


    $('#side-menu > #' + menuLink + ' > ul').addClass('in');
    $('#' + menuLink).addClass("active");
    $('#' + subMenuLink).addClass("selected");
}

Array.prototype.max = function () {
    return Math.max.apply(null, this);
};

Array.prototype.min = function () {
    return Math.min.apply(null, this);
};

function eliminateDuplicates(arr) {
    var i,
        len = arr.length,
        out = [],
        obj = {};

    for (i = 0; i < len; i++) {
        obj[arr[i]] = 0;
    }
    for (i in obj) {
        out.push(i);
    }
    return out;
}



$(document)
         .ajaxStart(function () {
             showProgress();
         })
         .ajaxStop(function () {
             hideProgress();
             __ClearTransactionDependentActions();
         })
        .ajaxError(function () {
            hideProgress();
        });

function showProgress() {
    var loading = $('.modal');
    //var modal = $('<div/>');
    //modal.addClass('modal');
    //$('body').append(modal);
    loading.show();
}
function hideProgress() {
    var loading = $('.modal');
    // var loading = $('#progressDiv');
    loading.hide();
}

$(function () {
    $('ul#side-menu li[id^="lnkSide"], ul#side-menu li[id^="lnkSide"] a').unbind().on('click', function (e) {
        showProgress();
    });

    $('input[type="submit"].loader, button[type="submit"].loader, a.loader').on('click', function () {
        showProgress();
    });

    //For Clearing the all controls
    $('#btnClearAll').click(function () {
        $('.row').find('input:text').each(function () {
            $(this).val('');

        });
        $(".row").find('input:text, select, textarea').val('');
        $(".row").find('input:radio, mm:checkbox').prop('checked', false).prop('selected', false);
    });
    //debugger;
    if ($('input[type="text"].one-month-enable-date-picker').datepicker && __currentOpenMonth.getFullYear() > 2000) {
        $('input[type="text"].one-month-enable-date-picker').datepicker({
            dateFormat: "dd/M/yy",
            defaultDate: __currentOpenMonth,
            minDate: __currentOpenMonth,
            hideIfNoPrevNext: true,
            stepMonths: 0
        });
    }

    if ($('input[type="text"].financial-year-begin-date-picker').datepicker) {
        $('input[type="text"].financial-year-begin-date-picker').datepicker({
            dateFormat: "dd/M/yy",
            minDate: __FYB,
            maxDate: __FYE,
            changeMonth: true,
            changeYear: true,
            defaultDate: __currentOpenMonth,
            yearRange: "-1:+1"
        });
    }
    if ($('input[type="text"].financial-year-end-date-picker').datepicker) {
        $('input[type="text"].financial-year-end-date-picker').datepicker({
            dateFormat: "dd/M/yy",
            minDate: __FYB,
            maxDate: __FYE,
            defaultDate: __currentOpenMonth,
            changeMonth: true,
            changeYear: true,
            yearRange: "-1:+1"
        });
    }
    __ClearTransactionDependentActions();
});

function __ClearTransactionDependentActions() {
    if (window.__lockStatusCode != undefined) {
        if (window.__lockStatusCode.toUpperCase() == 'LOCKED' || window.__lockStatusCode == '') {
            $('.lock-dependent').remove();
        } else if (window.__lockStatusCode.toUpperCase() == 'OPEN') {
            //$('.lock-dependent-inverse').remove();
        }
    }
}