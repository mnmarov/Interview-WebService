
// Supplied by the server
if (typeof(WebServiceBaseAddress) == "undefined")
{
    WebServiceBaseAddress = 'https://local_oms.zeuspackaging.com/WebService/NoConf/';
}

var Token = '';
var UserID = '';

function DoOtherError(xhr, textStatus) {
    //console.log(textStatus);
    //console.log(xhr);
    //if (xhr.status == 401) {


    //window.location.href = "Login.html";


    //}
}

function login_request(data, callback) {
    var url = ComposeURL('Authenticate');
    var dataToSend = null;
    if (data)
        dataToSend = JSON.stringify(data);

    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        contentType: 'application/json',
        data: dataToSend,
        success: function (result, textStatus, xhr) {
            console.log(result);
            if (callback)
                callback(result);
        },
        error: function (xhr, textStatus) {
            if (xhr.status === 400) {
                alert('Bad request');
            }
            else {
                DoOtherError(xhr, textStatus);
            }
        }
    });
}

function ajax_request(url, method, data, callback) {
    $.ajax({
        type: method,
        url: url,
        dataType: 'json',
        contentType: 'application/json',
        data: data,
        beforeSend: function (xhr, settings) {
            xhr.setRequestHeader('Session-ID', Token);
            xhr.setRequestHeader('User-ID', UserID);
        },
        success: function (result, textStatus, xhr) {
            console.log(result);
            if (callback)
                callback(result);
        },
        error: function (xhr, textStatus) {
            if (xhr.status === 400) {
                alert('Bad request');
            }
            else {
                DoOtherError(xhr, textStatus);
            }
        }
    });
}

function getCleanUrl() {
    var sURL = window.location.href.split('?');
    if (sURL.length)
        return sURL[0];
    return sURL;
}

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

function ComposeURL(Method) {
    return WebServiceBaseAddress + Method;
}