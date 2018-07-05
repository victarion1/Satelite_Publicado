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
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title text-center" id="tituloCard"></h3>
                </div>
                <div class="panel-body">
                    <div id="cuerpoCard"></div>
                    <div id="btnEjecutar"></div>
                    <br />
                    <div id="modalOpciones" title="Alerta">
                        <br />
                        <p id="mensaje"></p>
                    </div>
                    <div id="modalError" title="Alerta">
                        <br />
                        <p id="mensajeError"></p>
                    </div>
                    <div id="tablaConsulta" class="table-responsive">
                        <table id="id_tablaConsulta" class="datatable table table-striped table-bordered compact">
                            <thead>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="panel-footer">
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var listadoParametro = new Array();


        $(document).ready(function () {
          
            window.servicio.ConsultaOpciones();

            $('#tablaConsulta').hide();
        });
    </script>

</asp:Content>

