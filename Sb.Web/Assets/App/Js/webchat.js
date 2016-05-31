function startChatBGA() {
    window.gChatWindowHeight = 850;
    window.gChatWindowWidth = 700;

    var chatParams = [];
    chatParams["ORG_NAME"] = "DIIS";
    chatParams["ICHANNEL_NAME"] = "DOI_BUS_SPECIALIST";
    chatParams["SITE_CSS"] = "https://extranets.innovation.gov.au/Style%20Library/BGA-webchat/BGA-webchat.css";
    chatParams["SITE_LOGO"] = window.location.origin + "/Assets/App/img/header_logo.png";
    chatParams["SOURCE_URL"] = window.location.href;

    var internalParams = [];
    internalParams["BUSINESS_UNIT_ID"] = "1002000";
    internalParams["INTERACTION_DRIVER_ID"] = "1005806";

    window.startUpChat(chatParams, internalParams, "chat1.business.gov.au");
    return false;
}

$(document).ready(function () {
    $('img[src *= "nonexistent.gif"]').remove();

    $(".chat-now").click(function(e) {
        e.preventDefault();
        startChatBGA();
    });
});