<%@ Page Title="" Language="C#" MasterPageFile="~/BaseSatelite.Master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Sitio.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cabecera" runat="Server">
    <script type="text/javascript" src="Scripts/servicio.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Cuerpo" runat="Server">
    <asp:HiddenField ID="hf_username" runat="server" />
    <div class="row">
        <div class="col-md-4">
            <div class="list-group">
                <div class="alert alert-info text-center" role="alert" style="font-size: 14px;"><b>Menu</b></div>
            </div>
        </div>
        <div class="col-md-8">
            <div class="alert alert-info text-center" role="alert" style="font-size: 14px;"><b>Satelite</b></div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4">
            <div id="divMenu" style="height: 300px; overflow-y: auto;">
                <div id="menu" class="treeview" style="font-size: 10px; padding: 0px 15px 0px 0px;"></div>
            </div>
        </div>
        <div class="col-md-8" id="colParametros" onkeypress="if(event.keyCode == 13) event.returnValue = false";>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title text-center" id="tituloCard"></h3>
                </div>
                <div class="panel-body">
                    <div id="cuerpoCard"></div>
                    <div id="btnEjecutar" ></div>
                    <br />
                    <div id="modalOpciones" title="Alerta">
                        <br />
                        <p id="mensaje"></p>
                    </div>
                    <div id="modalError" title="Alerta">
                        <br />
                        <p id="mensajeError"></p>
                    </div>
                </div>
                <div class="panel-footer">
                </div>
            </div>
        </div>
    </div>
    <br />
    <div class="panel panel-default" id="TablaPanel">
        <div class="panel-heading"></div>
        <div class="panel-body">
            <div id="tablaConsulta" class="table-responsive">
                <table id="id_tablaConsulta" class="table table-responsive table-striped table-bordered  dataTable no-footer" style="font-size: 14px; width: 100% !important;">
                    <thead>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var listadoParametro = new Array();

        $(document).ready(function () {
            window.servicio.ConsultaOpciones();
            hf_username = obtenerUsuario();
            $('#TablaPanel').hide();
            $('#tablaConsulta').hide();
            $('#colParametros').hide();
        });

        function obtenerUsuario() {
            var datosTabla = $("#tblHeader").find("span");
            var row = datosTabla[0].outerText;
            var start_pos = row.indexOf('Conectado como: ') + 16;
            var end_pos = row.indexOf('\nAplicación', start_pos);
            var usuario = row.substring(start_pos, end_pos);
            return usuario;
        }
    </script>

</asp:Content>




