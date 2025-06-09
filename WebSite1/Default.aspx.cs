using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Nada por ahora
    }

    protected void btnEntrar_Click(object sender, EventArgs e)
    {
        string token = txtToken.Text.Trim();

        if (string.IsNullOrEmpty(token))
            return; 

        
        bool esCorrecto = ValidarUsuario(token);

        if (esCorrecto)
        {
            string nombreUsuario = ObtenerNombre(token);
            string idUsuario = ObtenerID(token);

            // Guardamos en sesión
            Session["NombreUsuario"] = nombreUsuario;
            Session["IDUsuario"] = idUsuario;

            // Redirige al menú principal
            Response.Redirect("Home_Menu.aspx");
        }
        else
        {
            // Mostrar SweetAlert con mensaje de error
            string script = @"
                <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
                <script>
                    Swal.fire({
                        icon: 'error',
                        title: 'Token inválido',
                        text: 'El token ingresado no es válido',
                        confirmButtonColor: '#3085d6'
                    });
                </script>";
            ClientScript.RegisterStartupScript(this.GetType(), "TokenInvalido", script);
        }
    }

    private bool ValidarUsuario(string token)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT COUNT(*) FROM Usuarios WHERE contraseña = @Contraseña"; // Ajusta tabla/campo
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Contraseña", token);
            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }

    private string ObtenerNombre(string token)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Usuario FROM Usuarios WHERE contraseña = @Contraseña"; // Ajusta
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Contraseña", token);
            conn.Open();
            return cmd.ExecuteScalar()?.ToString();
        }
    }

    private string ObtenerID(string token)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["FastFoodConnection"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT IDUsuario FROM Usuarios WHERE contraseña = @Token"; // Ajusta
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Token", token);
            conn.Open();
            return cmd.ExecuteScalar()?.ToString();
        }
    }
}
