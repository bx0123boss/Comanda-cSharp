<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8" />
    <title>Login</title>

    <!-- Tipografía -->
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;500;700&display=swap" rel="stylesheet" />

    <!-- Estilos embebidos -->
    <style>
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            font-family: 'Montserrat', sans-serif;
            min-height: 100vh;
            background: linear-gradient(135deg, #1f1c2c, #928dab);
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }

        form {
            width: 100%;
            max-width: 400px;
            padding: 0 20px;
        }

        .container {
            background-color: #ffffff;
            padding: 40px 30px;
            border-radius: 16px;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
            text-align: center;
            width: 100%;
        }

        .logo {
            width: 100px;
            margin-bottom: 25px;
        }

        h2 {
            margin-bottom: 25px;
            font-weight: 600;
            color: #2c3e50;
        }

        label {
            display: block;
            text-align: left;
            font-weight: 500;
            margin: 15px 0 5px;
            color: #2c3e50;
        }

        .input-group {
            margin-bottom: 20px;
        }

        .input-control {
            width: 100%;
            padding: 12px 15px;
            border: 1px solid #ccc;
            border-radius: 8px;
            font-size: 16px;
            transition: border-color 0.3s ease;
        }

        .input-control:focus {
            outline: none;
            border-color: #27ae60;
        }

        .btn {
            width: 100%;
            background-color: #27ae60;
            color: white;
            border: none;
            padding: 14px;
            font-size: 16px;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .btn:hover {
            background-color: #219150;
        }

        .footer {
            text-align: center;
            padding: 20px;
            font-size: 14px;
            color: #ecf0f1;
        }

        @media (max-width: 500px) {
            .container {
                padding: 30px 20px;
            }
        }
    </style>

    <!-- SweetAlert2 -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>

<body>
    <form id="loginForm" runat="server">
        <div class="container">
            <asp:Image ID="imgLogo" runat="server" CssClass="logo" ImageUrl="~/Imagenes/LOGO.png" />
            <h2>Bienvenido</h2>

            <label for="txtToken">Token</label>
            <div class="input-group">
                <asp:TextBox ID="txtToken" runat="server" TextMode="Password" CssClass="input-control" />
            </div>

            <asp:Button ID="btnEntrar" runat="server" Text="Entrar" CssClass="btn" OnClick="btnEntrar_Click" />
        </div>

        <div class="footer">
            <p>&copy; 2025 Jaegersoft. Todos los derechos reservados.</p>
        </div>
    </form>

    <script>
        document.getElementById('<%= btnEntrar.ClientID %>').addEventListener('click', function (e) {
            var token = document.getElementById('<%= txtToken.ClientID %>').value;
            if (token.trim() === '') {
                e.preventDefault();
                Swal.fire({
                    icon: 'error',
                    title: 'Campo vacío',
                    text: 'Por favor, ingrese el token.',
                    confirmButtonColor: '#27ae60'
                });
            }
        });
    </script>
</body>
</html>
