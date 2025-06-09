using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class NuevaComanda : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["IDUsuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            CargarMesas();
        }
    }

    private void CargarMesas()
    {
        int idMeseroInt;
        if (!int.TryParse(Session["IDUsuario"].ToString(), out idMeseroInt))
        {
            Response.Write("IDUsuario no es numérico.");
            return;
        }

        string connStr = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;
        string query = @"SELECT IdMesa, Nombre, Estatus FROM MESAS WHERE IdMesero = @IdMesero AND Estatus = 'COCINA' ORDER BY Nombre";

        DataTable dtMesas = new DataTable();

        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@IdMesero", idMeseroInt);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            try
            {
                conn.Open();
                da.Fill(dtMesas);

                rptMesas.DataSource = dtMesas;
                rptMesas.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("Error al cargar mesas: " + ex.Message);
            }
        }
    }

    protected void btnConfirmarAgregar_Click(object sender, EventArgs e)
    {
        string idMesero = Session["IDUsuario"]?.ToString();

        string nombreMesa = hdnNombreMesa.Value;
        string strComensales = hdnComensales.Value;

        int comensales;
        if (string.IsNullOrEmpty(nombreMesa) || !int.TryParse(strComensales, out comensales) || comensales < 1)
        {
            // Podrías mostrar alerta aquí si quieres
            return;
        }

        string connStr = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;

        int idMesaNueva = 0;

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string insertQuery = @"
            INSERT INTO MESAS (Nombre, IdMesero, CantidadPersonas, Estatus)
            VALUES (@Nombre, @IdMesero, @CantidadPersonas, @Estatus);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";  // Para obtener el Id recién insertado

            using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Nombre", nombreMesa);
                cmd.Parameters.AddWithValue("@IdMesero", idMesero);
                cmd.Parameters.AddWithValue("@CantidadPersonas", comensales);
                cmd.Parameters.AddWithValue("@Estatus", "NUEVA");

                try
                {
                    conn.Open();
                 
                    idMesaNueva = (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "error", $"alert('Error al agregar mesa: {ex.Message}');", true);
                    return;
                }
            }
        }

        if (idMesaNueva > 0)
        {
          
            Response.Redirect($"NuevaMesa.aspx?id={idMesaNueva}");
        }
        else
        {
         
            ClientScript.RegisterStartupScript(this.GetType(), "error", "alert('No se pudo obtener el ID de la nueva mesa.');", true);
        }
    }

}
