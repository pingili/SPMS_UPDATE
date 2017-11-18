/*
 you can apply transliteration to multiple controls by giving array of control id's

 To apply this feature to controls:

 calling : CustomTransliteration(['txtID1', 'txtID2', 'txtID3']);

*/

var CustomTransliteration = function (arrIds) {
    //
    //alert(arrIds.length);

    //Load the Google Transliterate API
    google.load("elements", "1", {
        packages: "transliteration"
    });
    var arrTEIds = new Array();

    for (i = 0; i < arrIds.length; i++) {
        if ($('#' + arrIds[i]).length > 0)
            arrTEIds.push(arrIds[i]);
    }

    onLoad = function () {
        var options = {
            sourceLanguage: google.elements.transliteration.LanguageCode.ENGLISH,
            destinationLanguage: [google.elements.transliteration.LanguageCode.TELUGU],
            transliterationEnabled: true
        };

        // Create an instance on TransliterationControl with the required
        // options.
        var control = new google.elements.transliteration.TransliterationControl(options);

        // Enable transliteration in the editable DIV with id
        // 'transliterateDiv'.
        //var ids = [id_txtTEBankName, id_txtBankName];
        var ids = arrTEIds;
        control.makeTransliteratable(ids);
        //control.showControl(lblOutput);
    }
    google.setOnLoadCallback(onLoad);
};