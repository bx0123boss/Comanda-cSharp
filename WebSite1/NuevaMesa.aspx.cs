using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls; // Para ListItemType
using System.Web.UI;



/// <summary>
/// comentario de prueba :v 
/// </summary>
public partial class NuevaMesa : System.Web.UI.Page
{
    // Clase para almacenar producto agregado con observaciones
    [Serializable]
    public class ProductoAgregado
    {
        public Guid IdUnico { get; set; } = Guid.NewGuid(); // Identificador único para cada producto agregado
        public int IdInventario { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Observaciones { get; set; }
    }

    private const string SessionKeyProductosAgregados = "ProductosAgregados";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string idMesa = Request.QueryString["id"];

            if (string.IsNullOrEmpty(idMesa))
            {
                pnlMesa.Visible = false;
                return;
            }

            CargarDatosMesa(idMesa);
            CargarCategorias();

            // Limpiar repeticiones al cargar por primera vez
            rptSubCategorias.DataSource = null;
            rptSubCategorias.DataBind();

            rptProductos.DataSource = null;
            rptProductos.DataBind();

            // Aquí sí hacemos el bind para productos agregados, no lo limpiamos
            BindProductosAgregados();
        }
        else
        {
            // Guardar observaciones actuales
            GuardarObservacionesDesdeRepeater();

            // BindProductosAgregados debe ejecutarse siempre en postback para recrear controles
            BindProductosAgregados();
        }
    }


    private void GuardarObservacionesDesdeRepeater()
    {
        var productos = ObtenerProductosAgregadosDesdeSesion();

        foreach (RepeaterItem item in rptProductosAgregados.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                var hfIdUnico = (HiddenField)item.FindControl("hfIdUnico");
                var txtObs = (TextBox)item.FindControl("txtObservaciones");

                if (hfIdUnico != null && txtObs != null)
                {
                    Guid idUnico;
                    if (Guid.TryParse(hfIdUnico.Value, out idUnico))
                    {
                        var prod = productos.Find(p => p.IdUnico == idUnico);
                        if (prod != null)
                        {
                            prod.Observaciones = txtObs.Text;
                        }
                    }
                }
            }
        }

        GuardarProductosAgregadosEnSesion(productos);
    }

    private void GuardarProductosEnBaseDeDatos(int idMesa, int idMesero)
    {
        string connStr = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;

        var listaProductos = ObtenerProductosAgregadosDesdeSesion();

        foreach (var producto in listaProductos)
        {
            string sql = @"
            INSERT INTO [dbo].[ArticulosMesa] 
            ([IdInventario], [Cantidad], [Total], [Comentario], [IdMesa], [IdMesero], [FechaHora], [Estatus]) 
            VALUES 
            (@IdInventario, @Cantidad, @Total, @Comentario, @IdMesa, @IdMesero, @FechaHora, @Estatus)";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@IdInventario", producto.IdInventario);
                cmd.Parameters.AddWithValue("@Cantidad", 1); 
                cmd.Parameters.AddWithValue("@Total", producto.Precio); 
                cmd.Parameters.AddWithValue("@Comentario", producto.Observaciones ?? "");
                cmd.Parameters.AddWithValue("@IdMesa", idMesa);
                cmd.Parameters.AddWithValue("@IdMesero", idMesero);
                cmd.Parameters.AddWithValue("@FechaHora", DateTime.Now);
                cmd.Parameters.AddWithValue("@Estatus", "COCINA"); 

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    protected void btnGuardarPedido_Click(object sender, EventArgs e)
    {
        // Actualizar observaciones desde repeater a sesión
        GuardarObservacionesDesdeRepeater();

        // Obtener IdMesa desde label o donde lo tengas
        int idMesa = int.Parse(lblIdMesa.Text);

        // Obtener IdMesero desde sesión
        int idMesero = 0;
        if (Session["IDUsuario"] != null)
        {
            idMesero = Convert.ToInt32(Session["IDUsuario"]);
        }
        else
        {
            lblMensaje.Text = "Sesión expirada. Por favor, inicia sesión de nuevo.";
            return;
        }

        // Guardar productos en base de datos
        GuardarProductosEnBaseDeDatos(idMesa, idMesero);

        // Script para mostrar Swal y redirigir
        string script = @"
        Swal.fire({
            icon: 'success',
            title: 'Pedido guardado correctamente',
            showConfirmButton: false,
            timer: 2000
        }).then(() => {
            window.location = 'home_menu.aspx';
        });
    ";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "swalSuccess", script, true);
    }
    private void CargarDatosMesa(string idMesa)
    {
        string connStr = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string query = @"SELECT m.IdMesa, u.Usuario AS NombreMesero, m.Nombre AS NombreMesa, m.CantidadPersonas
                             FROM MESAS m
                             INNER JOIN USUARIOS u ON u.IdUsuario = m.IdMesero
                             WHERE m.IdMesa = @IdMesa";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdMesa", idMesa);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pnlMesa.Visible = true;
                        lblIdMesa.Text = reader["IdMesa"].ToString();
                        lblNombreMesero.Text = reader["NombreMesero"].ToString();
                        lblNombreMesa.Text = reader["NombreMesa"].ToString();
                        lblComensales.Text = reader["CantidadPersonas"].ToString();
                    }
                    else
                    {
                        pnlMesa.Visible = false;
                    }
                }
            }
        }
    }

    private void CargarCategorias()
    {
        string connStr = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string query = "SELECT IdCategoria, Nombre, Color, Letra FROM CATEGORIAS";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    rptCategorias.DataSource = reader;
                    rptCategorias.DataBind();
                }
            }
        }
    }

    protected void rptCategorias_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "VerSubcategorias")
        {
            int idCategoria = Convert.ToInt32(e.CommandArgument);
            CargarSubcategorias(idCategoria);

            rptProductos.DataSource = null;
            rptProductos.DataBind();
        }
    }

    private void CargarSubcategorias(int idCategoria)
    {
        string connStr = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string query = @"SELECT a.Nombre, a.Precio, b.Color, b.Letra, a.IdInventario
                             FROM INVENTARIO AS a
                             INNER JOIN CATEGORIAS AS b ON a.IdCategoria = b.IdCategoria
                             WHERE a.IdCategoria = @IdCategoria";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    rptSubCategorias.DataSource = reader;
                    rptSubCategorias.DataBind();
                }
            }
        }
    }

    protected void rptSubCategorias_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "VerProductos")
        {
            int idInventario = Convert.ToInt32(e.CommandArgument);
            CargarProductos(idInventario);
        }
    }

    private void CargarProductos(int idInventario)
    {
        string connStr = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string query = @"SELECT a.Nombre, a.Precio, b.Color, b.Letra, a.IdInventario
                             FROM INVENTARIO AS a
                             INNER JOIN CATEGORIAS AS b ON a.IdCategoria = b.IdCategoria
                             WHERE a.IdInventario = @IdInventario";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@IdInventario", idInventario);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    rptProductos.DataSource = reader;
                    rptProductos.DataBind();
                }
            }
        }
    }

    protected void rptCategorias_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var btn = (Button)e.Item.FindControl("btnCategoria");
            var data = (System.Data.IDataRecord)e.Item.DataItem;

            string color = data["Color"]?.ToString().Trim() ?? "";
            string letra = data["Letra"]?.ToString().Trim() ?? "";

            btn.BackColor = ParseColorOrDefault(color, System.Drawing.Color.Black);
            btn.ForeColor = ParseColorOrDefault(letra, System.Drawing.Color.White);
        }
    }

    protected void rptSubCategorias_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var btn = (Button)e.Item.FindControl("btnSubCategoria");
            var data = (System.Data.IDataRecord)e.Item.DataItem;

            string color = data["Color"]?.ToString().Trim() ?? "";
            string letra = data["Letra"]?.ToString().Trim() ?? "";

            btn.BackColor = ParseColorOrDefault(color, System.Drawing.Color.Black);
            btn.ForeColor = ParseColorOrDefault(letra, System.Drawing.Color.White);
        }
    }

    private System.Drawing.Color ParseColorOrDefault(string colorString, System.Drawing.Color defaultColor)
    {
        if (string.IsNullOrEmpty(colorString))
            return defaultColor;

        if (!colorString.StartsWith("#") && !IsKnownColorName(colorString))
        {
            colorString = "#" + colorString;
        }

        try
        {
            return System.Drawing.ColorTranslator.FromHtml(colorString);
        }
        catch
        {
            return defaultColor;
        }
    }

    private bool IsKnownColorName(string name)
    {
        try
        {
            var c = System.Drawing.Color.FromName(name);
            return c.IsKnownColor;
        }
        catch
        {
            return false;
        }
    }

    protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "AgregarProducto")
        {
            string[] args = e.CommandArgument.ToString().Split('|');
            if (args.Length == 3)
            {
                int idInventario = int.Parse(args[0]);
                string nombre = args[1];
                decimal precio = decimal.Parse(args[2]);

                var productos = ObtenerProductosAgregadosDesdeSesion();

                // Ya no evitamos duplicados para permitir varias entradas iguales
                productos.Add(new ProductoAgregado
                {
                    IdInventario = idInventario,
                    Nombre = nombre,
                    Precio = precio,
                    Observaciones = ""
                });

                GuardarProductosAgregadosEnSesion(productos);
                BindProductosAgregados();
            }
        }
    }

    protected void rptProductosAgregados_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "EliminarProducto")
        {
            Guid idUnico;
            if (Guid.TryParse(e.CommandArgument.ToString(), out idUnico))
            {
                var productos = ObtenerProductosAgregadosDesdeSesion();
                productos.RemoveAll(p => p.IdUnico == idUnico);
                GuardarProductosAgregadosEnSesion(productos);
                BindProductosAgregados();
            }
        }
    }

    private List<ProductoAgregado> ObtenerProductosAgregadosDesdeSesion()
    {
        if (Session[SessionKeyProductosAgregados] == null)
            Session[SessionKeyProductosAgregados] = new List<ProductoAgregado>();

        return (List<ProductoAgregado>)Session[SessionKeyProductosAgregados];
    }

    private void GuardarProductosAgregadosEnSesion(List<ProductoAgregado> productos)
    {
        Session[SessionKeyProductosAgregados] = productos;
    }

    private void BindProductosAgregados()
    {
        var productos = ObtenerProductosAgregadosDesdeSesion();
        rptProductosAgregados.DataSource = productos;
        rptProductosAgregados.DataBind();
    }





}
