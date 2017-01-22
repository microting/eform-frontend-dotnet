function applyTooltipsToTrumbowygToolbars() {
    $('.trumbowyg-bold-button').attr('data-toggle', 'tooltip');
    $('.trumbowyg-bold-button').attr('data-placement', 'top');
    $('.trumbowyg-bold-button').data('toggle', 'tooltip');
    $('.trumbowyg-bold-button').data('placement', 'top');
    $('.trumbowyg-italic-button').attr('data-toggle', 'tooltip');
    $('.trumbowyg-italic-button').attr('data-placement', 'top');
    $('.trumbowyg-italic-button').data('toggle', 'tooltip');
    $('.trumbowyg-italic-button').data('placement', 'top');
    $('.trumbowyg-underline-button').attr('data-toggle', 'tooltip');
    $('.trumbowyg-underline-button').attr('data-placement', 'top');
    $('.trumbowyg-underline-button').data('toggle', 'tooltip');
    $('.trumbowyg-underline-button').data('placement', 'top');

    //$('.trumbowyg-bold-button').tooltip();
    //$('.trumbowyg-italic-button').tooltip();
    //$('.trumbowyg-underline-button').tooltip();
}


function t(constant_name, default_constant) {
    var result = default_constant;

    if (typeof mt_i18n_hash !== 'undefined') {
        if (typeof mt_i18n_hash[constant_name] !== 'undefined') {
            result = mt_i18n_hash[constant_name];
        }
    }

    return result;
}

//function fnDataTablesPipeline ( sSource, aoData, fnCallback ) {
//    var iPipe = 5; /* Ajust the pipe size */
//
//    var bNeedServer = oCache.bNeedServer; //false;
//    oCache.bNeedServer = false;
//    var sEcho = fnGetKey(aoData, "sEcho");
//    var iRequestStart = fnGetKey(aoData, "iDisplayStart");
//    var iRequestLength = fnGetKey(aoData, "iDisplayLength");
//    var iRequestEnd = iRequestStart + iRequestLength;
//    oCache.iDisplayStart = iRequestStart;
//
//    /* outside pipeline? */
//    if ( oCache.iCacheLower < 0 || iRequestStart < oCache.iCacheLower || iRequestEnd > oCache.iCacheUpper )
//    {
//        bNeedServer = true;
//    }
//
//    /* sorting etc changed? */
//    if ( oCache.lastRequest && !bNeedServer )
//    {
//        for( var i=0, iLen=aoData.length ; i<iLen ; i++ )
//        {
//            if ( aoData[i].name != "iDisplayStart" && aoData[i].name != "iDisplayLength" && aoData[i].name != "sEcho" )
//            {
//                if ( aoData[i].value != oCache.lastRequest[i].value )
//                {
//                    bNeedServer = true;
//                    break;
//                }
//            }
//        }
//    }
//
//    /* Store the request for checking next time around */
//    oCache.lastRequest = aoData.slice();
//
//    if ( bNeedServer )
//    {
//        if ( iRequestStart < oCache.iCacheLower )
//        {
//            iRequestStart = iRequestStart - (iRequestLength*(iPipe-1));
//            if ( iRequestStart < 0 )
//            {
//                iRequestStart = 0;
//            }
//        }
//
//        oCache.iCacheLower = iRequestStart;
//        oCache.iCacheUpper = iRequestStart + (iRequestLength * iPipe);
//        oCache.iDisplayLength = fnGetKey( aoData, "iDisplayLength" );
//        fnSetKey( aoData, "iDisplayStart", iRequestStart );
//        fnSetKey( aoData, "iDisplayLength", iRequestLength*iPipe );
//
//        $.getJSON( sSource, aoData, function (json) {
//            /* Callback processing */
//            oCache.lastJson = jQuery.extend(true, {}, json);
//
//            if ( oCache.iCacheLower != oCache.iDisplayStart )
//            {
//                json.data.splice( 0, oCache.iDisplayStart-oCache.iCacheLower );
//            }
//            json.data.splice( oCache.iDisplayLength, json.data.length );
//
//            fnCallback(json)
//        } );
//    }
//    else
//    {
//        json = jQuery.extend(true, {}, oCache.lastJson);
//        json.sEcho = sEcho; /* Update the echo for each response */
//        json.data.splice( 0, iRequestStart-oCache.iCacheLower );
//        json.data.splice( iRequestLength, json.data.length );
//        fnCallback(json);
//        return;
//    }
//}
//
//function fnGetKey( aoData, sKey )
//{
//    for ( var i=0, iLen=aoData.length ; i<iLen ; i++ )
//    {
//        if ( aoData[i].name == sKey )
//        {
//            return aoData[i].value;
//        }
//    }
//    return null;
//}
//
//var oCache = {
//    iCacheLower: -1,
//    bNeedServer: false
//};
//
//function fnSetKey( aoData, sKey, mValue )
//{
//    for ( var i=0, iLen=aoData.length ; i<iLen ; i++ )
//    {
//        if ( aoData[i].name == sKey )
//        {
//            aoData[i].value = mValue;
//        }
//    }
//}

var iModalLevel = 0;

$(document).ready(function () {
    moment.locale(moment.locale(), {
        week: { dow: 1 } // Monday is the first day of the week
    });

    //var iModalLevel = 0;

    //$(document).on('show.lightbox', function(event){ console.log("show lightbox"); });
    $(document).on('shown.lightbox', function (event) {
        iModalLevel = ($('.modal').size() + $('.lightbox:visible').size());
        $("#lightboxOverlay").css('z-index', iModalLevel * 1000);
        $("#lightbox").css('z-index', iModalLevel * 1000);

        mtDebug('Number of remaining after show ' + iModalLevel);

    });
    //$(document).on('hide.lightbox', function(event){ console.log("hide lightbox"); });
    $(document).on('hidden.lightbox', function (event) {
        iModalLevel = ($('.modal').size() + $('.lightbox:visible').size());

        $('.popover').each(function (index) {
            $('[aria-describedby=' + $(this).attr('id') + ']').popover('destroy');
        });

        $('.tooltip').each(function (index) {
            $('[aria-describedby=' + $(this).attr('id') + ']').tooltip('destroy');
        });

        mtDebug('Number of remaining after hide ' + iModalLevel);

    });

    $(document).on('show.bs.modal', '.modal', function (event) {
        //iModalLevel++;
        //console.log("incrementing iModalLevel from "+iModalLevel);
        //mt-right-padding-fix
    });
    $(document).on('shown.bs.modal', '.modal', function (event) {
        $('.bootbox-close-button').removeClass('bootbox-close-button');
        iModalLevel = ($('.modal').size() + $('.lightbox:visible').size());
        $(event.target).css('z-index', iModalLevel * 1000);

        setTimeout(function () {
            if (typeof $('.mt-quick-submit.in > .modal-dialog > .modal-content > .modal-footer > .mt-submit-button') !== 'undefined') {
                $('.mt-quick-submit.in').removeAttr('tabindex');

                $('.mt-quick-submit.in > .modal-dialog > .modal-content > .modal-footer > .mt-submit-button')
                    .not('.btn-danger')
                    .attr('tabindex', 1);

                $('.mt-quick-submit.in > .modal-dialog > .modal-content > .modal-footer > .mt-submit-button')
                    .not('.btn-danger')
                    .focus();

            }
        });

        //$('.mt-quick-submit > .modal-dialog > .modal-content > .modal-footer > .btn-success')
        //    .not('.btn-danger')
        //    .focus();

        mtDebug('Number of remaining after show ' + iModalLevel);
        //$(event.target).focus();
        applyTooltipsToTrumbowygToolbars();
    });

    function applyTooltipsToTrumbowygToolbars() {
        $('.trumbowyg-bold-button').attr('data-toggle', 'tooltip');
        $('.trumbowyg-bold-button').attr('data-placement', 'top');
        $('.trumbowyg-bold-button').data('toggle', 'tooltip');
        $('.trumbowyg-bold-button').data('placement', 'top');
        $('.trumbowyg-bold-button').tooltip();
        $('.trumbowyg-italic-button').attr('data-toggle', 'tooltip');
        $('.trumbowyg-italic-button').attr('data-placement', 'top');
        $('.trumbowyg-italic-button').data('toggle', 'tooltip');
        $('.trumbowyg-italic-button').data('placement', 'top');
        $('.trumbowyg-italic-button').tooltip();
        $('.trumbowyg-underline-button').attr('data-toggle', 'tooltip');
        $('.trumbowyg-underline-button').attr('data-placement', 'top');
        $('.trumbowyg-underline-button').data('toggle', 'tooltip');
        $('.trumbowyg-underline-button').data('placement', 'top');
        $('.trumbowyg-underline-button').tooltip();
    }

    //$(document).on('click', '.bootbox-close-button', function(event){
    //    console.log("123123123");
    //    $(event.target).closest('bootbox.modal').modal('hide');
    //});
    $(document).on('hide.bs.modal', '.modal', function (event) {
        //iModalLevel--;

        $('.popover').each(function (index) {
            $('[aria-describedby=' + $(this).attr('id') + ']').popover('destroy');
        });

        $('.tooltip').each(function (index) {
            $('[aria-describedby=' + $(this).attr('id') + ']').tooltip('destroy');
        });

        //console.log("decrementing iModalLevel from "+iModalLevel);
    });
    $(document).on('hidden.bs.modal', '.modal', function (event) {
        iModalLevel = ($('.modal').size() + $('.lightbox:visible').size());
        if (iModalLevel > 0) {
            $('body').css('padding-right', '15px');
            $('body').addClass('modal-open');
        }

        mtDebug('Number of remaining after hide ' + iModalLevel);
    });

    $('body').on('click', ':button[data-bootbox-confirm]', function (event) {

        var e = event || window.event;
        var target = e.target || e.srcElement;

        //var iThisLevel = (iModalLevel+1);

        var target_button = $(target);
        if (!target_button.is("button")) {
            target_button = target_button.parent(":button[data-bootbox-confirm]");
        }

        //var successClasses = "btn-ar";
        //if(target_button.hasClass("dangerous-action") || target_button.hasClass("btn-danger")) {
        //    successClasses += " btn-danger";
        //} else {
        //    successClasses += " btn-success mt-submit-button";
        //}
        //
        //var cancelLabel = t("cancel", "Cancel");
        //if (typeof target_button.data("bootbox-cancel-label") !== 'undefined') {
        //    cancelLabel = target_button.data("bootbox-cancel-label");
        //}
        //var cancel = {
        //    label: cancelLabel,
        //    className: "btn-ar btn-default",
        //    callback: function(event) {
        //        mtDebug("CANCEL PRESSED!");
        //
        //        if (typeof target_button.data("oncancel") !== 'undefined') {
        //            eval(target_button.data("oncancel"));
        //        }
        //    }
        //};
        //
        //var successLabel = t("ok", "OK");
        //if (typeof target_button.data("bootbox-confirm-label") !== 'undefined') {
        //    successLabel = target_button.data("bootbox-confirm-label");
        //}
        //var success = {
        //    label: successLabel,
        //    className: successClasses,
        //    callback: function() {
        //        if (typeof target_button.data("onsuccess") !== 'undefined') {
        //            eval(target_button.data("onsuccess"));
        //        }
        //    }
        //};
        //
        //var buttons = {};
        //
        //if (target_button.hasClass("dangerous-action") || target_button.hasClass("btn-danger")) {
        //    buttons = {success: success, cancel: cancel};
        //} else {
        //    buttons = {cancel: cancel, success: success};
        //}
        //
        //var bootboxOptions = { closeButton: false, animate: false };
        //
        //if (typeof target_button.data("bootbox-title") !== 'undefined'){
        //    mtDebug(target_button.data("bootbox-title"));
        //    var title = target_button.data("bootbox-title").match(/^eval\((.*)\);*$/);
        //    if (title === null) {
        //        bootboxOptions['title'] = target_button.data("bootbox-title");
        //    } else {
        //        bootboxOptions['title'] = eval(title[1]);
        //    }
        //}
        //
        //bootboxOptions['onEscape'] = function() {
        //    mtDebug("ESCAPE PRESSED!");
        //
        //    if (typeof target_button.data("oncancel") !== 'undefined') {
        //        eval(target_button.data("oncancel"));
        //    }
        //};
        //
        //bootboxOptions['className'] = 'mt-modal-level-'+iThisLevel+' ';
        //if (typeof target_button.data("bootbox-additional-classes") !== 'undefined') {
        //    bootboxOptions['className'] += target_button.data("bootbox-additional-classes");
        //}
        //
        //var message = target_button.data("bootbox-confirm").match(/^load\((.*)\);*$/);
        //if (message === null) {
        //    bootboxOptions['buttons'] = buttons;
        //
        //    message = target_button.data("bootbox-confirm").match(/^eval\((.*)\);*$/);
        //    if (message === null) {
        //        bootboxOptions['message'] = target_button.data("bootbox-confirm");
        //        bootbox.dialog(bootboxOptions);
        //    } else {
        //        bootboxOptions['message'] = eval(message[1]);
        //        bootbox.dialog(bootboxOptions);
        //    }
        //} else {
        //    bootboxOptions['buttons'] = buttons;
        //    $.get( message[1], function( html_content ) {
        //        bootboxOptions['className'] += ' mt-has-html-content';
        //        bootboxOptions['message'] = html_content;
        //        bootbox.dialog(bootboxOptions);
        //    });
        //}






        /*
         * options {
         *   bootbox-additional-classes: string
         *   bootbox-cancel-label: string
         *   bootbox-confirm:
         *   bootbox-confirm-label: string
         *   bootbox-title: string
         *   oncancel:
         *   onsuccess:
         *   dangerous-action: boolean
         * }
         */

        mtShowBootBox(
          {
              'bootbox-additional-classes': target_button.data("bootbox-additional-classes"),
              'bootbox-cancel-label': target_button.data("bootbox-cancel-label"),
              'bootbox-confirm': target_button.data("bootbox-confirm"),
              'bootbox-confirm-label': target_button.data("bootbox-confirm-label"),
              'bootbox-title': target_button.data("bootbox-title"),
              'escapable': true,
              'oncancel': target_button.data("oncancel"),
              'onsuccess': target_button.data("onsuccess"),
              'dangerous-action': (target_button.hasClass("dangerous-action") || target_button.hasClass("btn-danger"))
          }
        )






    });

    //$('body').on('submit', 'form[data-remote="true"]', function() {
    //    console.log("REMOTE JSON FOMR");
    //
    //
    //
    //    $.ajax({
    //        url: $(this).attr('action'),
    //        type: 'post',
    //        dataType: 'json',
    //        headers: { 'X-CSRF-Token': "#{ form_authenticity_token }" }
    //    }).done(function(data, status, xhr){
    //        show_notification(data.data);
    //        //oCache.bNeedServer = true;
    //        //reports.fnDraw();
    //    }).fail(function() {
    //        //alert( "error" );
    //    }).always(function() {
    //        //alert( "complete" );
    //    });
    //
    //
    //
    //    //var valuesToSubmit = $(this).serialize();
    //    //$.ajax({
    //    //    type: "POST",
    //    //    url: $(this).attr('action'),
    //    //    data: valuesToSubmit,
    //    //    dataType: "JSON",
    //    //    headers: { 'X-CSRF-Token': "#{ form_authenticity_token }" }
    //    //}).success(function(json){
    //    //    //console.log("success", json);
    //    //});
    //    event.preventDefault();
    //    //return false; // prevents normal behaviour
    //});

    $('#submit_form').click(function (event) {
        var e = event || window.event;
        var target = e.target || e.srcElement;

        ////console.log("we were clicked, should submit '"+form_id+"' "+$('#'+form_id).attr('id'));
        //$('#'+form_id).submit();
        $('#' + $(target).data('form-id')).submit();

        //event.preventDefault();
        //return false; // prevents normal behaviour
    });

    $('#submit_form_edit').click(function () {
        //console.log("we were clicked");
        $('form').submit();
    });

    //$('.trumbowyg-element').each(function (index) {
    //    console.log($(this));
    //    applyTrumbowygToElement($(this).attr('id'));
    //});

    //$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    //    //e.target // newly activated tab
    //    //e.relatedTarget // previous active tab
    //    $('.trumbowyg-element').each( function( index ){
    //        applyTrumbowygToElement( $(this).attr('id') );
    //    });
    //})

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    });

    //setTimeout(set_footer_height, 0);
    //setTimeout(set_footer_height, 10);

    $(document).ajaxError(function (event, jqxhr, settings, exception) {
        //console.log("jqxhr.status: " + jqxhr.status);
        if (jqxhr.status == 401) {
            window.location.reload();
        }
    });

    //var iVerticalScrollPos = 0;
    //
    //$(window).scroll(function() {
    //    console.log('vertical scroll');
    //    var iCurrentVerticalScrollPos = $(document).scrollTop();
    //    var iVerticalScrollPosDelta = iCurrentVerticalScrollPos - iVerticalScrollPos;
    //
    //
    //    //$('.ui-selectmenu-menu.ui-selectmenu-open').top( $('.ui-selectmenu-menu.ui-selectmenu-open').position().top() - iVerticalScrollPosDelta);
    //
    //
    //    iVerticalScrollPos = iCurrentVerticalScrollPos;
    //});
});

/*
 * options {
 *   bootbox-additional-classes: string
 *   bootbox-cancel-label: string
 *   bootbox-confirm:
 *   bootbox-confirm-label: string
 *   bootbox-title: string
 *   oncancel:
 *   onsuccess:
 *   dangerous-action: boolean
 * }
 */

function mtShowBootBox(options) {
    //var e = event || window.event;
    //var target = e.target || e.srcElement;

    var iThisLevel = (iModalLevel + 1);
    var generatedDialog = null;

    //var target_button = $(target);
    //if (!target_button.is("button")) {
    //    target_button = target_button.parent(":button[data-bootbox-confirm]");
    //}

    var successClasses = "btn-ar";
    if (options["dangerous-action"]) {
        successClasses += " btn-danger";
    } else {
        successClasses += " btn-success mt-submit-button";
    }

    var cancelLabel = t("cancel", "Cancel");
    if (typeof options["bootbox-cancel-label"] !== 'undefined') {
        cancelLabel = options["bootbox-cancel-label"];
    }
    var cancel = {
        label: cancelLabel,
        className: "btn-ar btn-default",
        callback: function (event) {
            mtDebug("CANCEL PRESSED!");

            if (typeof options["oncancel"] !== 'undefined') {
                eval(options["oncancel"]);
            }
        }
    };

    var successLabel = t("ok", "OK");
    if (typeof options["bootbox-confirm-label"] !== 'undefined') {
        successLabel = options["bootbox-confirm-label"];
    }
    var success = {
        label: successLabel,
        className: successClasses,
        callback: function () {
            if (typeof options["onsuccess"] !== 'undefined') {
                eval(options["onsuccess"]);
            }
        }
    };

    var buttons = {};

    if (options["dangerous-action"]) {
        buttons = { success: success, cancel: cancel };
    } else {
        buttons = { cancel: cancel, success: success };
    }

    var bootboxOptions = { closeButton: false, animate: false };

    if (typeof options["bootbox-title"] !== 'undefined') {
        mtDebug(options["bootbox-title"]);
        var title = options["bootbox-title"].match(/^eval\((.*)\);*$/);
        if (title === null) {
            bootboxOptions['title'] = options["bootbox-title"];
        } else {
            bootboxOptions['title'] = eval(title[1]);
        }
    }

    if (typeof options["escapable"] !== 'undefined' && options["escapable"] === true) {
        bootboxOptions['onEscape'] = function () {
            mtDebug("ESCAPE PRESSED!");

            if (typeof options["oncancel"] !== 'undefined') {
                eval(options["oncancel"]);
            }
        };
    }

    bootboxOptions['className'] = 'mt-modal-level-' + iThisLevel + ' ';
    if (typeof options["bootbox-additional-classes"] !== 'undefined') {
        bootboxOptions['className'] += options["bootbox-additional-classes"];
    }

    var message = options["bootbox-confirm"].match(/^load\((.*)\);*$/);
    if (message === null) {
        bootboxOptions['buttons'] = buttons;

        message = options["bootbox-confirm"].match(/^eval\((.*)\);*$/);
        if (message === null) {
            bootboxOptions['message'] = options["bootbox-confirm"];
            generatedDialog = bootbox.dialog(bootboxOptions);
        } else {
            bootboxOptions['message'] = eval(message[1]);
            generatedDialog = bootbox.dialog(bootboxOptions);
        }
    } else {
        bootboxOptions['buttons'] = buttons;
        $.get(message[1], function (html_content) {
            bootboxOptions['className'] += ' mt-has-html-content';
            bootboxOptions['message'] = html_content;
            generatedDialog = bootbox.dialog(bootboxOptions);
        });
    }

    return generatedDialog;
}

function applyTrumbowygToElement(jqeTargetElement) {
    //var jqeTargetElement = $(jqElement);
    jqeTargetElement.val(jqeTargetElement.val()
        .replace(/\r\n|\n|\r/g, "<br>"));

    jQuery.trumbowyg.langs.en = { close: "" };

    jqeTargetElement.trumbowyg({
        //prefix: element_id+' trumbowyg-',
        prefix: 'trumbowyg-',
        fullscreenable: false,
        semantic: false,
        btns: ['bold', 'italic', 'underline'],
        //removeformatPasted: true,
        closable: jqeTargetElement.hasClass('trumbowyg-closable'),
        tagsToRemove: ['a', 'h1', 'h2', 'h3', 'h4'],
        inlineElementsSelector: 'b, i, u',
        resetCss: true,
        removeformatPasted: true,
    });

    //console.log( $("."+element_id+".trumbowyg-editor") );

    if (jqeTargetElement[0].disabled == true) {
        jqeTargetElement[0].disabled = false;
    }

    jqeTargetElement.closest("form").submit(function () {
        // if (/<div>|<\/div>|<p>|<\/p>/g.test( jqeTargetElement.val() )) {
        //$("."+element_id+".trumbowyg-editor").html( $("."+element_id+".trumbowyg-editor").html().replace(/<div>/g, "<br>").replace(/<\/div>/g, "") );

        jqeTargetElement.val(jqeTargetElement.val()
            .replace(/<div>|<p>/g, "<br>")
            .replace(/<\/div>|<\/p>/g, "")
            .replace(/(<(!DOCTYPE|abbr|acronym|address|applet|area|article|aside|audio|base|basefont|bdi|bdo|big|blockquote|body|button|canvas|caption|center|cite|code|col|colgroup|command|datalist|dd|del|details|dfn|dir|dl|dt|em|embed|fieldset|figcaption|figure|font|footer|form|frame|frameset|h1|h2|h3|h4|h5|h6|head|header|hgroup|hr|html|iframe|img|input|ins|kbd|keygen|label|legend|li|link|map|mark|menu|meta|meter|nav|noframes|noscript|object|ol|optgroup|option|output|p|param|pre|progress|q|rp|rt|ruby|s|samp|script|section|select|small|source|span|strike|strong|style|sub|summary|sup|table|tbody|td|textarea|tfoot|th|thead|time|title|tr|track|tt|ul|var|video|wbr)[0-9a-zA-Z\=\'\"\-\:\;\.\,\s\(\)\#\>]*>)|(<\/(!DOCTYPE|abbr|acronym|address|applet|area|article|aside|audio|base|basefont|bdi|bdo|big|blockquote|body|button|canvas|caption|center|cite|code|col|colgroup|command|datalist|dd|del|details|dfn|dir|dl|dt|em|embed|fieldset|figcaption|figure|font|footer|form|frame|frameset|h1|h2|h3|h4|h5|h6|head|header|hgroup|hr|html|iframe|img|input|ins|kbd|keygen|label|legend|li|link|map|mark|menu|meta|meter|nav|noframes|noscript|object|ol|optgroup|option|output|p|param|pre|progress|q|rp|rt|ruby|s|samp|script|section|select|small|source|span|strike|strong|style|sub|summary|sup|table|tbody|td|textarea|tfoot|th|thead|time|title|tr|track|tt|ul|var|video|wbr)>)/g, ""));
        // }
        //return false;
    });
}

//$('form').submit(function (event) {
//
//    $(event.target).find('.trumbowyg-element').each( function( index, element ){
//        if (/<div>|<\/div>|<p>|<\/p>/g.test( $(element).val() )) {
//            //$("."+element_id+".trumbowyg-editor").html( $("."+element_id+".trumbowyg-editor").html().replace(/<div>/g, "<br>").replace(/<\/div>/g, "") );
//
//            $(element).val( $(element).val()
//                .replace(/<div>|<p>/g, "<br>")
//                .replace(/<\/div>|<\/p>/g, "")
//                .replace(/<(!DOCTYPE|abbr|acronym|address|applet|area|article|aside|audio|base|basefont|bdi|bdo|big|blockquote|body|button|canvas|caption|center|cite|code|col|colgroup|command|datalist|dd|del|details|dfn|dir|dl|dt|em|embed|fieldset|figcaption|figure|font|footer|form|frame|frameset|h1|h2|h3|h4|h5|h6|head|header|hgroup|hr|html|iframe|img|input|ins|kbd|keygen|label|legend|li|link|map|mark|menu|meta|meter|nav|noframes|noscript|object|ol|optgroup|option|output|p|param|pre|progress|q|rp|rt|ruby|s|samp|script|section|select|small|source|span|strike|strong|style|sub|summary|sup|table|tbody|td|textarea|tfoot|th|thead|time|title|tr|track|tt|ul|var|video|wbr)[0-9a-z\=\"\-\:\;\.\s]*>|<\/span>/g, "") );
//        };
//
//        //$(element).trumbowyg('destroy');
//        //return false;
//    });
//});

function success_message(message) {
    $(function () {
        new PNotify({
            title: 'Success',
            text: message,
            type: 'success',
            nonblock: {
                nonblock: true,
                nonblock_opacity: .2
            },
            delay: 1000,
            hide: true,
            history: false
        });
    });
}

function error_message(message) {
    $(function () {
        new PNotify({
            title: 'Error',
            text: message,
            type: 'error',
            nonblock: {
                nonblock: true,
                nonblock_opacity: .2
            },
            delay: 1000,
            hide: true,
            history: false
        });
    });
}

function show_notification(data) {
    if (data.error) {
        error_message(data.error);
    } else {
        success_message(data.message);
    }
}

function html_safe(str) {
    if (str === null) return str;

    if (typeof str === 'undefined') {
        return str;
    } else {
        return str.toString().replace(/'/g, "\\&apos;").replace(/"/g, "\\&quot;");
    }
}

function mtDebug(message) {
    if (sMtLogLevel == 'debug') {
        console.log(message + ' ');
        console.trace();
    }
}