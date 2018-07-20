/// <reference path="../index.aspx" />
/// <reference path="../index.aspx" />
/*Variable Globales*/
var titulo = "";
var table;
var nomConsulta;
var parametroParaExcel;
var NombreParamExcel;
var cantRegistros;
//Formatea fecha

function AlertaInvocacion(url) {
    window.console && console.log('Invocando servicio... - ' + url);
}

//función que genera error genérico
function ErrorGenerico(error) {
    $('body').loading('stop');
    window.console && console.log('Error en el servicio - ');
    window.console && console.log(error);
}

//alerta mensaje de error si data no trae datos
function AlertaMensajeError() {
    $("#modalError").dialog({
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
                $(this).dialog("close");
                $('#tablaConsulta').hide();
                $('body').loading('stop');
            }
        },
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });
    $('#mensajeError').text(' No hay datos.');
    $("#modalError").dialog('open');
}

function AlertaMensajeConexion() {
    $("#modalError").dialog({
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
                $(this).dialog("close");
                $('#tablaConsulta').hide();
                $('body').loading('stop');
            }
        },
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });
    $('#mensajeError').text('Problemas al conectarse a la base de datos.');
    $("#modalError").dialog('open');
}

function alertaMensajeConDatos(cabecera, sabana) {
    $('#TablaPanel').hide();
    $("#modalOpciones").dialog({
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
                ConfigurarColumnas(cabecera, sabana, 0)
                $(this).dialog("close");
                
            },
            Excel: function () {
                ConfigurarColumnas(cabecera, sabana, 1);
                $(this).dialog("close");
                $('#TablaPanel').hide();
            }
        },
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });
    $('#mensaje').text(' Consulta en Ejecución.');
    $("#modalOpciones").dialog('open');
}

//Carga menú en datatree
function CargaMenu(data) {
    var obj = $.parseJSON(data.d);
    if (obj.resultado) {
        window.console && console.log('respuesta OK del servicio');
        var listadoOpciones = new Array();
        listadoOpciones = obj.lista;

        $('#menu').treeview({
            levels: 1,
            data: listadoOpciones
        });
        $('#menu').on('nodeSelected', function (event, data) {
            if (!data.nodes) {
                AnalisaConsulta(data.text, data.tags[0], data.tags[1]);
            }
        });
    } else {
        window.console && console.log('respuesta NOK del servicio - ' + obj.mensaje);
    }
}

//escribe consulta en html
function EscribirNombreConsulta(nombreConsulta) {
    nomConsulta = nombreConsulta;
    $("#tituloCard").html(nombreConsulta);
}

//analisa consulta si es con parámetros o no
function AnalisaConsulta(nombre, idConsulta, cantParametro) {
    EscribirNombreConsulta(nombre);

    $("#cuerpoCard").html("");
    if (cantParametro <= 0) {
        EjecutarConsulta(idConsulta, '{}');
    }
    else {
        $("#cuerpoCard").append(
            "<form>" +
            "<div class='row'>" +
            "<div class='col-sm-1'></div><div class='col-sm-10' id='formParametro'></div><div class='col-sm-1'></div>" +
            "</div>" +
            "</form>");
        DesplegarParamatro(idConsulta)
    }
}

//le asigna valor a los parametros
function ObtenerParametros() {
    var valorParam;
    var parametros = "";
    parametroParaExcel = "";
    listadoParametro.forEach(
        function (item) {
            window.console && console.log(item);
            NombreParamExcel = item.Nombre;
            valorParam = $('#' + item.Id).val();
            if (parametros === "") {
                parametros = item.Id + "#" + valorParam;
            }
            else {
                parametros = parametros + "|" + item.Id + "#" + $("#" + item.Id).val();
            }
            parametroParaExcel = parametroParaExcel + ' | ' + NombreParamExcel + ': ' + valorParam;
        }
   );
    return parametros;
}

//despliega los parámetros
function DesplegarParamatro(idConsulta) {
    $('#tablaConsulta').hide();
    $('#TablaPanel').hide();
    $('#colParametros').show();
    window.servicio.ConsultaParametros(idConsulta);
}

function EjecutarParametrosBD(idConsulta) {
    window.servicio.EjecutarConsultaParametros(idConsulta)
}

//obtiene los valores de los parámetros con opciones
function CargaParametros(data, idConsulta) {
    var obj = $.parseJSON(data.d);
    if (obj.resultado) {
        listadoParametro = obj.param;
        listadoParametro.forEach(
            function (item) {
                if (item.EsLista) {
                    $("#formParametro").append(
                        "<div class='form-group'><label for=\"" + item.Id + "\">" + item.Nombre + "</label>" +
                        "<select class=\"form-control\" id=\"" + item.Id + "\"></select></div>");

                    item.Lista.forEach(
                        function (elemento) {
                            $("#" + item.Id).append("<option value='" + elemento.Id + "'>" + elemento.Texto + "</option>");
                        }
                    );
                } else {
                    $("#formParametro").append(
                        "<div class='form-group'><label for=\"" + item.Id + "\">" + item.Nombre + "</label>" +
                        "<input type=\"text\" class=\"form-control\" id=\"" + item.Id + "\" onkeypress=\"pulsar(event)\">  </div>");
                }
            }
        );
        $("#btnEjecutar").html("");
        $("#btnExcel").html("");

        $("#btnEjecutar").append(
            "<button type='button' class='btn btn-default' onclick='EjecutaConsultaBD(" + idConsulta + ");  '>" +
            "<span class='glyphicon glyphicon-circle-arrow-right' aria-hidden='true'></span> Ejecutar" +
            "</button>"
        );
        $("#btnExcel").append(
            "<button type='button' class='btn btn-default'>" +
            "<span class='glyphicon glyphicon-circlonle-arrow-right' aria-hidden='true'></span> Excel" +
            "</button>"
        );
    } else {
        window.console && console.log('respuesta NOK del servicio - ' + obj.mensaje);
    }
}

//envía a ejecutar query de la consulta || opc = 0 (ver tabla), opc=1 (ver excel)
function EjecutaConsultaBD(idConsulta) {
    $('#TablaPanel').hide();
    $('body').loading({ message: 'Ejecutando Consulta' });
    window.servicio.EjecutarConsulta(idConsulta)
    
}

//obtiene los datos de las consultas realizadas
function DespliegaConsulta(data) {
    var obj = $.parseJSON(data.d);
    if (obj.resultado == false) {
        AlertaMensajeConexion();
        return;
    }
    var listadoConsulta = obj.cabecera;
    var listadoSabana = obj.sabana;
    cantRegistros = listadoSabana.length;
    window.console && console.log(cantRegistros);
    listadoSabana.forEach(function (item) {
        var tamanoConsulta = listadoConsulta.length
        for (var i = 0; i < tamanoConsulta; i++) {
            var fecha = String(item["" + listadoConsulta[i] + ""]);
            if (fecha.toLowerCase().indexOf("date") >= 0) {
                item["" + listadoConsulta[i] + ""] = moment(fecha).format('DD-MM-YYYY');
            } else {
                ;
            }
        }
    });

    if (obj.sabana.length <= 0 || listadoConsulta.length <= 0) {
        AlertaMensajeError();
    } else {
        if (table != null) {
            table.destroy();
            $("#id_tablaConsulta").empty();
        }
        alertaMensajeConDatos(listadoConsulta, listadoSabana);
    }
}

//Esto carga  las columnas en el datatable
function ConfigurarColumnas(cabecera, sabana, opc) {
    var id = "#id_tablaConsulta";
    window.console && console.log("INICIO - CARGA GRILLA");
    var orden = [[0, 'asc']];
    var columnas = [];
    try {
        $.each(cabecera, function (index, value) {
            columnas[index] = {
                "title": '' + value + '', "mDataProp": "" + value + "", render: function (sabana, type, row) {
                    return row[value];
                }
            };
        });
    } catch (e) {
        window.console && console.log(e);
    }
    CargaSabanaEnGrilla(id, cabecera, orden, columnas, sabana, opc);
}

//Carga sabada de datos en grilla de datatable
function CargaSabanaEnGrilla(id, cabecera, orden, columnas, sabana, opc) {
    $('#TablaPanel').show();
    $('#tablaConsulta').show();
    var cantCabecera = cabecera.length;
    for (var i = 0; i < cantCabecera; i++) {
        if (i != 0) {
            titulo = titulo + ',' + i;
        } else {
            titulo = i;
        }
    }
    table = $(id).DataTable({
        "aaData": sabana,
        "paging": true,
        "bFilter": true,
        "bInfo": true,
        "bLengthChange": true,
        "columnDefs": [columnas],
        "paging": true,
        "order": orden,
        "pageLength": 10,
        "responsive": true,
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningun dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "Info": "Mostrando pagina _PAGE_ de _PAGES_",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar: ",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Ultimo",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        },

        dom: 'Bfrtip',
        "aoColumns": columnas,

        buttons: [
                        {
                            extend: 'excel',
                            text: '<span class="glyphicon glyphicon-list-alt" aria-hidden="false"></span>&nbsp;&nbsp;Excel',
                            exportOptions: {
                                columns: [titulo]
                            },
                            title: nomConsulta,
                            message: parametroParaExcel
                        }
        ]
    })

    if (opc == 1) {
        $('#tablaConsulta').hide();
        $('.buttons-excel').click();
    }

    $('body').loading('stop');
}

var servicio = {
    ConsultaOpciones: function () {
        var url = '../index.aspx/ConsultaMenu';
        window.console && console.log(url);
        var parametros = '{ }';

        consumeWs(
            url,
            parametros,
            function () { AlertaInvocacion(url); }, //Antes
            function (data) { CargaMenu(data); }, //Despues Exitoso
            function (error) { ErrorGenerico(error); } //En caso de Error
          );
    },

    ConsultaParametros: function (idConsulta) {
        var url = '../index.aspx/ConsultaParametro';
        window.console && console.log("aqui debe pasar" + url);
        var parametros = '{ idConsulta:' + idConsulta + ' }';

        consumeWs(
            url,
            parametros,
            function () { AlertaInvocacion(url); }, //Antes
            function (data) { CargaParametros(data, idConsulta); }, //Despues Exitoso
            function (error) { ErrorGenerico(error); } //En caso de Error
        );
    },

    EjecutarConsulta: function (idConsulta, opc) {
        $('#TablaPanel').hide();
        var listaParametros = ObtenerParametros();
        var url = '../index.aspx/EjecutarConsulta';

        var parametros = '{ idConsulta:' + idConsulta + ', parametros:"' + listaParametros + '" }';

        consumeWs(
        url,
        parametros,
        function () { AlertaInvocacion(url); }, //Antes
        function (data) { DespliegaConsulta(data, opc); }, //Despues Exitoso
        function (error) { ErrorGenerico(error); } //En caso de Error
        );
    }
}