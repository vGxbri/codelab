using System;
using System.Web.UI;
using System.Web.UI.WebControls;

//referencias MySQL
using MySql.Data.MySqlClient;

namespace PaginaCursos
{
    public partial class iniciar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string view = Request.QueryString["view"];
                if (view == "register")
                {
                    mvAuth.ActiveViewIndex = 1;
                }
                else
                {
                    mvAuth.ActiveViewIndex = 0;
                }
            }
        }

        protected void btnRegistro_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string nombre = txtNombre.Text;
                string apellidos = txtApellidos.Text;
                string email = txtEmailRegister.Text;
                string password = txtPasswordRegister.Text;

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Comprobar si el correo electrónico ya está registrado
                    string checkQuery = "SELECT COUNT(*) FROM estudiantes WHERE correo_electronico = @correo_electronico";
                    MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@correo_electronico", email);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (count > 0)
                    {
                        // Mostrar mensaje de error si el correo ya está registrado
                        labelErrorRegister.Text = "El correo electrónico ya está registrado.";
                    }
                    else
                    {
                        // Insertar el nuevo usuario
                        string query = "INSERT INTO estudiantes (apellidos, correo_electronico, nombre, hash_contrasena) VALUES (@apellidos, @correo_electronico, @nombre, @hash_contrasena)";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@apellidos", apellidos);
                        command.Parameters.AddWithValue("@correo_electronico", email);
                        command.Parameters.AddWithValue("@nombre", nombre);
                        command.Parameters.AddWithValue("@hash_contrasena", hashedPassword);

                        command.ExecuteNonQuery();

                        // Obtener el ID del nuevo usuario
                        long estudianteId = command.LastInsertedId;

                        // Insertar una suscripción gratuita por defecto
                        string suscripcionQuery = "INSERT INTO suscripciones (estado, estudiante_id, fecha_inicio, fecha_fin) VALUES ('inactiva', @estudiante_id, NOW(), NOW())";
                        MySqlCommand suscripcionCommand = new MySqlCommand(suscripcionQuery, connection);
                        suscripcionCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);

                        suscripcionCommand.ExecuteNonQuery();

                        // Guardar el usuario en la sesión
                        Session["Usuario"] = email;

                        Response.Redirect("index.aspx");
                    }
                }
            }
    }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                labelErrorLogin.Text = string.Empty;
                
                string email = txtEmailLogin.Text;
                string password = txtPasswordLogin.Text;

                // Conexión a la base de datos
                string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT hash_contrasena FROM estudiantes WHERE correo_electronico = @correo_electronico";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@correo_electronico", email);

                    connection.Open();
                    string storedHash = (string)command.ExecuteScalar();

                    if (storedHash != null && BCrypt.Net.BCrypt.Verify(password, storedHash))
                    {
                        Session["Usuario"] = email;
                        Response.Redirect("index.aspx");
                    }
                    else
                    {
                        labelErrorLogin.Text = "Correo electrónico o contraseña incorrectos";
                    }
                }
            }
        }

        protected void lnkShowRegister_Click(object sender, EventArgs e)
        {
            mvAuth.ActiveViewIndex = 1;
        }

        protected void lnkShowLogin_Click(object sender, EventArgs e)
        {
            mvAuth.ActiveViewIndex = 0;
        }

        protected void redirigirPagina(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        protected void ValidateTerms(object source, ServerValidateEventArgs args)
        {
            args.IsValid = chkTerminos.Checked;
        }
    }
}