<%@ Page Title="Nueva Comanda" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NuevaComanda.aspx.cs" Inherits="NuevaComanda" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2 style="text-align:center; margin-bottom:30px; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; color:#333;">
        Gestión de Mesas
    </h2>

    <style>
        /* (Mantén aquí el CSS previo para el diseño, omitido para abreviar) */
    </style>

    <!-- FontAwesome -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

    <!-- SweetAlert2 -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <div class="mesas-container">
        <asp:Repeater ID="rptMesas" runat="server">
            <ItemTemplate>
                <div class="mesa-card" onclick="window.location.href='DetalleMesa.aspx?id=<%# Eval("IdMesa") %>';">
                    <i class="fas fa-utensils mesa-icon" title="Mesa"></i>
                    <div class="mesa-numero">
                        Mesa <%# Eval("Nombre") %>
                    </div>
                    <div class="mesa-estatus">
                        <%# Eval("Estatus") %>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div class="btn-container" style="margin-top:40px; text-align:center;">
        <!-- Botón para lanzar SweetAlert -->
        <asp:Button ID="btnAgregarMesa" runat="server" Text="+ Agregar Nueva Mesa" CssClass="btn-primary" OnClientClick="return mostrarSweetAlert();" UseSubmitBehavior="false" />
    </div>

    <!-- Campo oculto para capturar número de comensales y hacer postback -->
    <asp:HiddenField ID="hdnComensales" runat="server" />
    <asp:HiddenField ID="hdnNombreMesa" runat="server" />


    <!-- Botón oculto para enviar postback tras aceptar -->
    <asp:Button ID="btnConfirmarAgregar" runat="server" OnClick="btnConfirmarAgregar_Click" Style="display:none;" />

<script type="text/javascript">
    function mostrarSweetAlert() {
        // Nombre del mesero desde servidor (pasado a JS)
        var nombreMesero = '<%= Session["NombreUsuario"] != null ? Session["NombreUsuario"].ToString() : "Mesero" %>';

        Swal.fire({
            title: 'Agregar nueva mesa',
            html:
                '<p><strong>Mesero:</strong> ' + nombreMesero + '</p>' +
                '<input id="inputNombreMesa" type="text" class="swal2-input" placeholder="Nombre de la mesa">' +
                '<input id="inputComensales" type="number" min="1" max="20" class="swal2-input" placeholder="Número de comensales">',
            icon: 'info',
            showCancelButton: true,
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar',
            preConfirm: () => {
                const nombreMesa = Swal.getPopup().querySelector('#inputNombreMesa').value.trim();
                const comensales = Swal.getPopup().querySelector('#inputComensales').value;

                if (!nombreMesa) {
                    Swal.showValidationMessage('Por favor ingresa el nombre de la mesa');
                } else if (!comensales || comensales < 1) {
                    Swal.showValidationMessage('Por favor ingresa un número válido de comensales');
                }

                return { nombreMesa, comensales };
            }
        }).then((result) => {
            if (result.isConfirmed) {
                // Guardamos ambos valores en dos hidden fields (hay que agregar otro)
                document.getElementById('<%= hdnNombreMesa.ClientID %>').value = result.value.nombreMesa;
                document.getElementById('<%= hdnComensales.ClientID %>').value = result.value.comensales;

                // Lanzar postback para guardar en servidor
                document.getElementById('<%= btnConfirmarAgregar.ClientID %>').click();
            }
        });

        // Cancelar postback normal del botón
        return false;
    }
</script>


</asp:Content>
