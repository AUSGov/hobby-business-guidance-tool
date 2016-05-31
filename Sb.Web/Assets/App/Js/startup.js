// show a warning if cookies are disabled (see _DisabledCookies.cshtml)
if (!Modernizr.cookies) {
    document.getElementById("cookies").className = "row";
}

$(document).foundation({
    accordion: {
        // allow multiple accordion panels to be active at the same time
        multi_expand: true,
        // allow accordion panels to be closed by clicking on their headers
        // setting to false only closes accordion panels when another is opened
        toggleable: true
    }
});



$(document).ready(function () {
    /* Expand collapse headings config */
    /*var help = new jQueryCollapse($("#help").find(".showhide"), {
        open: function () {
            this.slideDown(150);
            $("#helpTopic").focus();
        },
        close: function () {
            this.slideUp(150);
        }
    });*/
    $("#help").find('.cd-panel').on('click', function (event) {
        if ($(event.target).is('.cd-panel') || $(event.target).is('.cd-panel-close')) {
            //help.close();
            $("#help").find('.cd-panel').removeClass('is-visible');
            event.preventDefault();
            // $('#settings').toggle();
        }
    });
    $('.cd-btn').on('click', function (event) {
        var index = $("a.cd-btn").index(this);
        //help.open(index);
        event.preventDefault();
        $("#help").find('.cd-panel').addClass('is-visible');
        // $('#settings').toggle();
    });



    // unselect the none checkbox if any other option is selected
    $(".answers :checkbox:not(.NoneOption)").click(function () {
        $noneBox = $(this).closest(".answers").find(':checkbox.NoneOption');
        if ($noneBox.prop("checked")) {
            $noneBox.prop('checked', false);
        }
    });

    // unselect all other checkboxs if the none option selected
    $(".answers :checkbox.NoneOption").click(function () {
        if ($(this).prop("checked")) {
            $(this).closest(".answers").find(':checkbox:not(.NoneOption)').each(function (i, element) {
                if ($(element).prop("checked")) {
                    $(element).prop('checked', false);
                }
            });
        }
    });


    // show / hide information tips when radio button answers are selected
    $(".answers input:radio").click(function () {
        $(this).closest(".answers").find('div.registration-tip').each(function (i, element) {
            $(element).hide();
            $(element).attr("aria-expanded", "false");
        });
        if ($(this).prop("checked")) {
            $(this).siblings('div.registration-tip').show();
            $(this).siblings('div.registration-tip').attr("aria-expanded", "true");
        }
    });

    // show / hide information tips when multi select answers are selected
    $(".answers input:checkbox").click(function () {
        $(this).closest(".answers").find('input:checkbox').each(function (i, element) {
            if ($(element).prop("checked")) {
                $(element).siblings('div.registration-tip').show();
                $(element).siblings('div.registration-tip').attr("aria-expanded", "true");
            } else {
                $(element).siblings('div.registration-tip').hide();
                $(element).siblings('div.registration-tip').attr("aria-expanded", "false");
            }
        });
    });

    // restore hidden infomation tips when page is reloaded with existing cookie
    $(".answers input").each(function (i, element) {
        if ($(element).prop("checked")) {
            $(element).siblings('div.registration-tip').show();
            $(element).siblings('div.registration-tip').attr("aria-expanded", "true");
        } else {
            $(element).siblings('div.registration-tip').hide();
            $(element).siblings('div.registration-tip').attr("aria-expanded", "false");
        }
    });

    // validate radio / checkboxes to ensure at least one option is selected
    $("button.validateAnswers").click(function (event) {
        var valid = false;
        $(".answers input").each(function (i, element) {
            if ($(element).prop("checked")) {
                valid = true;
                return;
            }
        })
        if (!valid) {
            event.preventDefault();
            $(".panel.validation").show().focus();
        }
    });

    // Expose share links from Share link in sub footer
    $('.js-share').on("click", function(e) {
        $(".share").toggleClass('is-showing');
        $('.share a').toggleAttr('tabindex', '0', '-1');
        $('.share').toggleAttr('aria-expanded', 'true', 'false');
    });



});
// toggle attribute polyfill for jquery
$.fn.toggleAttr = function (attr, attr1, attr2) {
    return this.each(function () {
        var self = $(this);
        if (self.attr(attr) == attr1)
            self.attr(attr, attr2);
        else
            self.attr(attr, attr1);
    });
};

// social share popup window
function popup(url) {
    newwindow = window.open(url, "_blank", "toolbar=no,scrollbars=no,resizable=yes,top=500,right=500,width=600,height=700");
    if (window.focus) { newwindow.focus() }
    return false;
}

