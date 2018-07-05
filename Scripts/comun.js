function consumeWs(url, parametros, before, callback, fnerror) {
    try {

        window.console && console.log($(location).attr('protocol') + '//' + $(location).attr('host') + '/' + url);

        $.ajax({
            async: true,
            cache: false,
            type: "POST",
            beforeSend: function () {
                before();
            },
            //url: $(location).attr('protocol') + '//' + $(location).attr('host') + '/' + url,
            url:'Satelite/'+url,
            data: parametros,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            timeout: 3600000,
            success: function (data) {
                callback(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                window.console && console.log("Error Ws - [" + url + "] - jqXHR: [" + jqXHR + "] - textStatus: [" + textStatus + "] - errorThrown: [" + errorThrown + "]");
                fnerror("Error Ws - [" + url + "] - jqXHR: [" + jqXHR + "] - textStatus: [" + textStatus + "] - errorThrown: [" + errorThrown + "]");
                window.console && console.log(jqXHR);
            }
        });
    } catch (e) {
        window.console && console.log(e);
        window.console && console.log("Error al consumir [" + url + "] - " + e.message);
        fnerror("Error al consumir [" + url + "] - " + e.message);
    }
};