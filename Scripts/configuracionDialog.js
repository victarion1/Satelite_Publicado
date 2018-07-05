var dialogoVS = {
    resizable: false,
    height: "auto",
    width: 400,
    modal: true,
    autoOpen: false,
    show: {
        effect: "blind",
        duration: 1000
    },
    hide: {
        effect: "slide",
        duration: 1000
    },
    buttons: {
        VerTabla: function () {
            $(this).dialog("close");
        },
        Excel: function () {
            
            $(this).dialog("close");
        }
    },
    open: function (event, ui) {
        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
    }
}
var repetirSubida = {
    resizable: false,
    height: "auto",
    width: 400,
    modal: true,
    autoOpen: false,
    show: {
        effect: "blind",
        duration: 1000
    },
    hide: {
        effect: "slide",
        duration: 1000
    },
    buttons: {
        Reintentar: function () {
            $('body').loading({ message: 'Cargando...' });
            enviarArchivoValuetech(sessionStorage.IntentosArchivo);
            
        }
    },
    open: function (event, ui) {
        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
    }
}
var totalNoCoincide = {
    resizable: false,
    height: "auto",
    width: 400,
    modal: true,
    autoOpen: false,
    show: {
        effect: "blind",
        duration: 1000
    },
    hide: {
        effect: "slide",
        duration: 1000
    },
    buttons: {
        OK: function () {
            return true;
        },
        Cancelar: function () {
            return false;
            $(this).dialog("close");
        }
    },
    open: function (event, ui) {
        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
    }
}

var audioDialog = {
    resizable: false,
    height: "auto",
    width: 400,
    modal: true,
    autoOpen: false,
    show: {
        effect: "blind",
        duration: 1000
    },
    hide: {
        effect: "slide",
        duration: 1000
    },
    buttons: {
        Ok: function () {
            $(this).dialog("close");
        }
    },
    open: function (event, ui) {
        $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
    }
}