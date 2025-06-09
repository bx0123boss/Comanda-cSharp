using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de ConexionSQL
/// </summary>

public class ConexionSQL
    {
        public static SqlConnection ObtenerConexion()
        {
            string cadena = "Data Source=192.168.20.41;Initial Catalog=FastFood;User ID=admin2;Password=admin";
            return new SqlConnection(cadena);
        }
    }
