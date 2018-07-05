/// <reference path="../index.aspx" />
/// <reference path="../index.aspx" />
/*Variable Globales*/
var titulo = "";
var table;
var nomConsulta;
var parametroParaExcel;


function AlertaInvocacion(url) {
    window.console && console.log('Invocando servicio... - ' + url);
}
function webservices() {
    var url="WsPrueba.asmx/HelloWorld";
    var parametros = "";
    consumeWs(
       url,
       parametros,
       function () { AlertaInvocacion(url); }, //Antes
       function (data) {console.log(data); }, //Despues Exitoso       
       function (error) { ErrorGenerico(error); } //En caso de Error
       );
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

function alertaMensajeConDatos(cabecera, sabana) {

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
    window.console && console.log('Entranado al menu');
    var obj = $.parseJSON(data.d);
    window.console && console.log(obj);
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
            if (parametros === "") {
                window.console && console.log(parametroParaExcel);
               valorParam = $('#' + item.Id).val();
                window.console && console.log(valorParam);
                parametros = item.Id + "#" + valorParam;
            }
            else {
                parametros = parametros + "|" + item.Id + "#" + $("#" + item.Id).val();
            }
            parametroParaExcel = parametroParaExcel + ' - ' + $('#' + item.Id).val();
        }
   );

    return parametros;
}

//despliega los parámetros
function DesplegarParamatro(idConsulta) {
    $('#tablaConsulta').hide();
    window.servicio.ConsultaParametros(idConsulta);

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
                        "<select class=\"form-control\" id=\"" + item.Id + "\"></select></div>"

                    );
                    item.Lista.forEach(
                        function (elemento) {
                            $("#" + item.Id).append("<option value='" + elemento.Id + "'>" + elemento.Texto + "</option>");
                        }
                    );
                } else {
                    $("#formParametro").append(
                        "<div class='form-group'><label for=\"" + item.Id + "\">" + item.Nombre + "</label>" +
                        "<input type=\"text\" class=\"form-control\" id=\"" + item.Id + "\"></div>");
                }
            }
        );
        $("#btnEjecutar").html("");
        $("#btnExcel").html("");

        $("#btnEjecutar").append(
            "<button type='button' class='btn btn-default' onclick='EjecutaConsultaBD(" + idConsulta + ");'>" +
            "<span class='glyphicon glyphicon-circle-arrow-right' aria-hidden='true'></span> Ejecutar" +
            "</button>"
        );
        $("#btnExcel").append(
            "<button type='button' class='btn btn-default'>" +
            "<span class='glyphicon glyphicon-circle-arrow-right' aria-hidden='true'></span> Excel" +
            "</button>"
        );
    } else {
        window.console && console.log('respuesta NOK del servicio - ' + obj.mensaje);
    }
}

//envía a ejecutar query de la consulta || opc = 0 (ver tabla), opc=1 (ver excel)
function EjecutaConsultaBD(idConsulta) {
    $('body').loading({ message: 'Ejecutando Consulta' });
    window.servicio.EjecutarConsulta(idConsulta)
}

//obtiene los datos de las consultas realizadas
function DespliegaConsulta(data) {
 
    var obj = $.parseJSON(data.d);
    var listadoConsulta = obj.cabecera;
    var listadoSabana = obj.sabana;

    if (obj.sabana.length <= 0)
    {
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
            "responsive": true,
            "pageLength": 10,
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
                                title: nomConsulta + parametroParaExcel
                            }
                    ]
        });

   if (opc == 1)
   {
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

    EjecutarConsulta: function (idConsulta,opc) {
        var listaParametros = ObtenerParametros();
        var url = '../index.aspx/EjecutarConsulta';

        var parametros = '{ idConsulta:' + idConsulta + ', parametros:"' + listaParametros + '" }';

        //  alert(parametros);
        // window.console && console.log(parametros);

        consumeWs(
        url,
        parametros,
        function () { AlertaInvocacion(url); }, //Antes
        function (data) {DespliegaConsulta(data, opc);}, //Despues Exitoso       
        function (error) { ErrorGenerico(error); } //En caso de Error
        );

    }
}
