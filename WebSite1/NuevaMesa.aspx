<%@ Page Title="Nueva Comanda" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NuevaMesa.aspx.cs" Inherits="NuevaMesa" %>




<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
     <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <h2>Detalle de la Mesa</h2>

    <asp:Panel ID="pnlMesa" runat="server" Visible="false" CssClass="mesa-panel">
        <p><strong>Id Mesa:</strong> <asp:Label ID="lblIdMesa" runat="server" /></p>
        <p><strong>Mesero:</strong> <asp:Label ID="lblNombreMesero" runat="server" /></p>
        <p><strong>Nombre de la Mesa:</strong> <asp:Label ID="lblNombreMesa" runat="server" /></p>
        <p><strong>Número de Comensales:</strong> <asp:Label ID="lblComensales" runat="server" /></p>
    </asp:Panel>

    <h3>Categorías</h3>
    <asp:Repeater ID="rptCategorias" runat="server" OnItemCommand="rptCategorias_ItemCommand" OnItemDataBound="rptCategorias_ItemDataBound">
        <ItemTemplate>
            <asp:Button 
                ID="btnCategoria" 
                runat="server" 
                Text='<%# Eval("Nombre") %>' 
                CommandName="VerSubcategorias" 
                CommandArgument='<%# Eval("IdCategoria") %>' 
                CssClass="categoria-btn" />
        </ItemTemplate>
    </asp:Repeater>

    <h3>Subcategorías (Productos)</h3>
    <asp:Repeater ID="rptSubCategorias" runat="server" OnItemCommand="rptSubCategorias_ItemCommand" OnItemDataBound="rptSubCategorias_ItemDataBound">
        <ItemTemplate>
            <asp:Button 
                ID="btnSubCategoria" 
                runat="server" 
                Text='<%# Eval("nombre") %>' 
                CommandName="VerProductos" 
                CommandArgument='<%# Eval("IdInventario") %>' 
                CssClass="subcategoria-btn" />
        </ItemTemplate>
    </asp:Repeater>

    <h3>Productos</h3>
    <asp:Repeater ID="rptProductos" runat="server" OnItemCommand="rptProductos_ItemCommand">
        <HeaderTemplate>
            <table class="productos-tabla">
                <tr>
                    <th>Nombre</th>
                    <th>Precio</th>
                    <th>Acción</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("nombre") %></td>
                <td>$<%# Eval("Precio", "{0:N2}") %></td>
                <td>
                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CommandName="AgregarProducto"
                        CommandArgument='<%# Eval("IdInventario") + "|" + Eval("nombre") + "|" + Eval("Precio") %>'
                        CssClass="agregar-btn" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <h3>Productos Agregados</h3>
    <asp:Repeater ID="rptProductosAgregados" runat="server" OnItemCommand="rptProductosAgregados_ItemCommand">
        <ItemTemplate>
            <asp:HiddenField ID="hfIdUnico" runat="server" Value='<%# Eval("IdUnico") %>' />
            <div style="border:1px solid #ccc; padding:5px; margin-bottom:5px;">
                <span><%# Eval("Nombre") %> - $<%# Eval("Precio") %></span><br />
                <asp:TextBox ID="txtObservaciones" runat="server" Text='<%# Eval("Observaciones") %>' TextMode="MultiLine" Rows="2" Columns="30" CssClass="observaciones-text" />
                
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Button ID="btnGuardarPedido" runat="server" Text="Guardar Pedido" OnClick="btnGuardarPedido_Click" CssClass="guardar-btn" />
<asp:Label ID="lblMensaje" runat="server" ForeColor="Green" />

    <style>
        .mesa-panel {
            border: 1px solid #ccc;
            padding: 10px;
            margin-bottom: 20px;
            background-color: #f9f9f9;
            border-radius: 8px;
        }

        .categoria-btn, .subcategoria-btn {
            margin: 5px;
            padding: 10px 15px;
            font-size: 16px;
            border-radius: 8px;
            border: none;
            cursor: pointer;
        }

        .productos-tabla {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }

        .productos-tabla th, .productos-tabla td {
            padding: 8px;
            border: 1px solid #ddd;
            text-align: left;
        }

        .productos-tabla th {
            background-color: #eee;
        }

        .agregar-btn {
            background-color: #4CAF50;
            color: white;
            border-radius: 5px;
            border: none;
            padding: 5px 10px;
            cursor: pointer;
        }

        .observaciones-text {
            width: 100%;
            padding: 4px;
        }

        .eliminar-btn {
            background-color: #f44336;
            color: white;
            border: none;
            padding: 5px 10px;
            cursor: pointer;
            border-radius: 5px;
            margin-top: 5px;
        }
    </style>
</asp:Content>
