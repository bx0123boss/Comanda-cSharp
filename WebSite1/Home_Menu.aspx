<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home_Menu.aspx.cs" Inherits="Home_Menu" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="UTF-8" />
    <title>Dashboard - Jaegersoft</title>
    <link href="https://fonts.googleapis.com/css2?family=Montserrat:wght@400;500;700&display=swap" rel="stylesheet" />
    <link href="CSS/Hextech.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Iconos FontAwesome CDN para los íconos del menú -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
</head>
    <header>
  <div class="logo">
    <img src="Imagenes/LOGO.png" alt="Logo Jaegersoft" />
    Comandas Jaegersoft
  </div>
</header>
<body>
    <form id="form1" runat="server">
        <div class="sidebar" id="sidebar">
            <div class="logo">
                Jaegersoft
            </div>
            <ul class="menu">
                <li onclick="location.href='Home_Menu.aspx'"><i class="fas fa-tachometer-alt"></i><span>Dashboard</span></li>
<li onclick="location.href='NuevaComanda.aspx'"><i class="fas fa-plus-circle"></i><span>Nueva Comanda</span></li>
<li onclick="location.href='GestionarComandas.aspx'"><i class="fas fa-clipboard-list"></i><span>Gestionar Comandas</span></li>
<li onclick="location.href='Notificaciones.aspx'"><i class="fas fa-bell"></i><span>Notificaciones</span></li>
<li onclick="location.href='Logout.aspx'"><i class="fas fa-sign-out-alt"></i><span>Cerrar sesión</span></li>

            </ul>
        </div>

        <div class="main-content" id="mainContent">
            <h1>Bienvenido al Panel de Control</h1>
            <p>Este es el área principal donde puedes agregar contenido dinámico según el usuario y sus permisos.</p>
            <!-- Aquí puedes incluir más contenido o controles ASP.NET -->
        </div>

        <div class="toggle-btn" id="toggleBtn" title="Mostrar/Ocultar menú">
            <i class="fas fa-bars"></i>
        </div>
    </form>

    <script>
        const sidebar = document.getElementById('sidebar');
        const toggleBtn = document.getElementById('toggleBtn');

        // Toggle sidebar ancho en escritorio
        toggleBtn.addEventListener('click', () => {
            if(window.innerWidth > 768) {
                sidebar.classList.toggle('collapsed');
            } else {
                sidebar.classList.toggle('active');
            }
        });

        // Cerrar menú en móvil si se hace click fuera
        document.addEventListener('click', function(event) {
            const isClickInside = sidebar.contains(event.target) || toggleBtn.contains(event.target);
            if (!isClickInside && window.innerWidth <= 768 && sidebar.classList.contains('active')) {
                sidebar.classList.remove('active');
            }
        });

        // Ajustar el menú al cambiar tamaño ventana
        window.addEventListener('resize', () => {
            if(window.innerWidth > 768) {
                sidebar.classList.remove('active');
            } else {
                sidebar.classList.remove('collapsed');
            }
        });
    </script>
</body>
</html>
