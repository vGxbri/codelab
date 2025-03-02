using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

//referencias MySQL
using MySql.Data;
using MySql.Data.MySqlClient;

namespace PaginaCursos
{
    public partial class index : System.Web.UI.Page
    {
        string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";
        private const int MAX_CHAPTERS = 10;
        private int chapterCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Usuario"] != null)
                {
                    // El usuario ha iniciado sesión
                    navbarNotLogged.Style["display"] = "none";
                    navbarLogged.Style["display"] = "flex";

                    // Verificar el estado de la suscripción del usuario
                    string email = Session["Usuario"].ToString();
                    string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "SELECT s.estado FROM suscripciones s JOIN estudiantes e ON s.estudiante_id = e.id WHERE e.correo_electronico = @correo_electronico AND s.estado = 'activa'";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@correo_electronico", email);

                        string estadoSuscripcion = (string)command.ExecuteScalar();

                        if (estadoSuscripcion == "activa")
                        {
                            btnPlanGratuito.Text = "Volver a Plan Básico";
                            btnPlanGratuito.Enabled = true;
                            btnPlanGratuito.CssClass = "btn btn-primary btnMejorar";

                            btnPlanPremium.Text = "Plan actual";
                            btnPlanPremium.Enabled = false;
                            btnPlanPremium.CssClass = "btn btn-outline-primary-mejorar";
                        }
                        else
                        {
                            btnPlanGratuito.Text = "Plan actual";
                            btnPlanGratuito.Enabled = false;
                            btnPlanGratuito.CssClass = "btn btn-outline-primary-mejorar";
                            
                            btnPlanPremium.Text = "Mejorar a Premium";
                            btnPlanPremium.Enabled = true;
                            btnPlanPremium.CssClass = "btn btn-secondary btnMejorar";
                        }
                    }
                }
                else
                {
                    // El usuario no ha iniciado sesión
                    navbarNotLogged.Style["display"] = "flex";
                    navbarLogged.Style["display"] = "none";
                }

                paginas.ActiveViewIndex = 0;
                //cargarCursos();
                cargarCursosPrincipal();
            }
        }

        protected void btnCerrarSesion2_Click(object sender, EventArgs e)
        {
            // Limpiar la sesión
            Session.Clear();
            Session.Abandon();

            // Redirigir a la página de inicio
            Response.Redirect("index.aspx");
        }

        protected void cargarCursosPrincipal()
        {
            string query = @"SELECT c.id, c.titulo, c.descripcion, c.img, i.nombre AS instructor
                            FROM cursos c
                            JOIN instructores i ON c.instructor_id = i.id
                            LIMIT 6";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    rptCursos3.DataSource = reader;
                    rptCursos3.DataBind();
                }
            }
            catch (Exception ex)
            {
                labelDebug.Text = "Error al cargar los cursos. Por favor, inténtelo de nuevo más tarde. " + ex.Message;
            }
        }

        protected void cargarCursos()
        {
            string email = Session["Usuario"].ToString();
            string query = @"SELECT c.id, c.titulo, c.descripcion, c.es_premium, c.img, i.nombre AS instructor,
                            (SELECT COUNT(*) FROM inscripciones WHERE estudiante_id = e.id AND curso_id = c.id) AS inscrito
                            FROM cursos c
                            JOIN instructores i ON c.instructor_id = i.id
                            JOIN estudiantes e ON e.correo_electronico = @correo_electronico";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@correo_electronico", email);
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    rptCursos.DataSource = reader;
                    rptCursos.DataBind();
                }
            }
            catch (Exception ex)
            {
                labelDebugCatalogo.Text = "Error al cargar los cursos. Por favor, inténtelo de nuevo más tarde. " + ex.Message;
            }
        }

        protected void rptCursos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hfCursoId = (HiddenField)e.Item.FindControl("hfCursoId");
                System.Web.UI.WebControls.Button btnUnirme = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnUnirme");

                bool inscrito = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "inscrito"));
                if (inscrito)
                {
                    btnUnirme.Text = "Ya inscrito";
                    btnUnirme.Enabled = false;
                    btnUnirme.CssClass = "btn btn-outline-primary-unirme";
                }
            }
        }

        protected void btnCatalogo_Click(object sender, EventArgs e)
        {
            paginas.ActiveViewIndex = 1;
            cargarCursos();
        }

        protected void btnMisCursos_Click(object sender, EventArgs e)
        {
            paginas.ActiveViewIndex = 2;
            CargarMisCursos();
            CheckIfInstructor();
        }

        protected void btnPlanes_Click(object sender, EventArgs e)
        {
            paginas.ActiveViewIndex = 3;
        }

        protected void btnPerfil_Click(object sender, EventArgs e)
        {
            paginas.ActiveViewIndex = 4;
            LoadProfileInformation();
        }

        protected void imgLogo_Click(object sender, ImageClickEventArgs e)
        {
            paginas.ActiveViewIndex = 0;
        }

        protected void btnConocerMas_Click(object sender, EventArgs e)
        {
            if (Session["Usuario"] != null)
            {
                paginas.ActiveViewIndex = 1;
                cargarCursos();
            }
            else
            {
                Response.Redirect("~/iniciar.aspx?view=register");
            }
        }

        protected void btnUnirme_Click(object sender, EventArgs e)
        {
            string email = Session["Usuario"].ToString();
            string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Obtener el ID del estudiante
                string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                // Verificar el estado de la suscripción del usuario
                string getSubscriptionQuery = "SELECT estado FROM suscripciones WHERE estudiante_id = @estudiante_id AND estado = 'activa'";
                MySqlCommand getSubscriptionCommand = new MySqlCommand(getSubscriptionQuery, connection);
                getSubscriptionCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                string estadoSuscripcion = (string)getSubscriptionCommand.ExecuteScalar();

                // Obtener el ID del curso
                System.Web.UI.WebControls.Button btnUnirme = (System.Web.UI.WebControls.Button)sender;
                RepeaterItem item = (RepeaterItem)btnUnirme.NamingContainer;
                HiddenField hfCursoId = (HiddenField)item.FindControl("hfCursoId");
                long cursoId = Convert.ToInt64(hfCursoId.Value);

                // Verificar si el curso es premium
                string getCursoQuery = "SELECT es_premium FROM cursos WHERE id = @curso_id";
                MySqlCommand getCursoCommand = new MySqlCommand(getCursoQuery, connection);
                getCursoCommand.Parameters.AddWithValue("@curso_id", cursoId);
                bool esPremium = Convert.ToBoolean(getCursoCommand.ExecuteScalar());

                if (esPremium && estadoSuscripcion != "activa")
                {
                    // Redirigir a la vista de planes si el curso es premium y el usuario no tiene una suscripción premium activa
                    paginas.ActiveViewIndex = 3;
                }
                else
                {
                    // Verificar si ya existe una inscripción para el estudiante y el curso
                    string checkInscripcionQuery = "SELECT COUNT(*) FROM inscripciones WHERE estudiante_id = @estudiante_id AND curso_id = @curso_id";
                    MySqlCommand checkInscripcionCommand = new MySqlCommand(checkInscripcionQuery, connection);
                    checkInscripcionCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                    checkInscripcionCommand.Parameters.AddWithValue("@curso_id", cursoId);
                    int inscripcionCount = Convert.ToInt32(checkInscripcionCommand.ExecuteScalar());

                    if (inscripcionCount == 0)
                    {
                        // Insertar una nueva fila en la tabla inscripciones
                        string insertInscripcionQuery = "INSERT INTO inscripciones (estudiante_id, curso_id, fecha_inscripcion) VALUES (@estudiante_id, @curso_id, NOW())";
                        MySqlCommand insertInscripcionCommand = new MySqlCommand(insertInscripcionQuery, connection);
                        insertInscripcionCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                        insertInscripcionCommand.Parameters.AddWithValue("@curso_id", cursoId);
                        insertInscripcionCommand.ExecuteNonQuery();
                    }

                    // Cargar el curso y cambiar la vista
                    cargarCurso(cursoId);
                    paginas.ActiveViewIndex = 5;
                    CheckCourseOwnership(cursoId);
                }
            }
        }

        protected void btnPlanGratuito_Click(object sender, EventArgs e)
        {
            string email = Session["Usuario"].ToString();
            string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Obtener el ID del estudiante
                string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                // Actualizar la suscripción a gratuita
                string updateQuery = "UPDATE suscripciones SET estado = 'inactiva' WHERE estudiante_id = @estudiante_id AND estado = 'activa'";
                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                updateCommand.ExecuteNonQuery();

                // Actualizar los botones
                btnPlanGratuito.Text = "Plan actual";
                btnPlanGratuito.Enabled = false;
                btnPlanGratuito.CssClass = "btn btn-outline-primary-mejorar";
                
                btnPlanPremium.Text = "Mejorar a Premium";
                btnPlanPremium.Enabled = true;
                btnPlanPremium.CssClass = "btn btn-secondary btnMejorar";
            }
        }

        protected void btnConfirmarTarjeta_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email = Session["Usuario"].ToString();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Get student ID
                    string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                    MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                    getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                    long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                    // Calculate new end date (30 days from now)
                    DateTime fechaFin = DateTime.Now.AddDays(30);

                    // Update subscription to premium and set end date
                    string updateQuery = "UPDATE suscripciones SET estado = 'activa', fecha_fin = @fecha_fin WHERE estudiante_id = @estudiante_id AND estado = 'inactiva'";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                    updateCommand.Parameters.AddWithValue("@fecha_fin", fechaFin);
                    updateCommand.ExecuteNonQuery();

                    // Update buttons
                    btnPlanGratuito.Text = "Volver a Plan Básico";
                    btnPlanGratuito.Enabled = true;
                    btnPlanGratuito.CssClass = "btn btn-primary btnMejorar";

                    btnPlanPremium.Text = "Plan actual";
                    btnPlanPremium.Enabled = false;
                    btnPlanPremium.CssClass = "btn btn-outline-primary-mejorar";

                    // Clear form fields
                    txtNumeroTarjeta.Text = string.Empty;
                    txtFechaExp.Text = string.Empty;
                    txtCVV.Text = string.Empty;
                    txtNombreTarjeta.Text = string.Empty;

                    // Add JavaScript to close the modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", 
                        "var modal = bootstrap.Modal.getInstance(document.getElementById('modalTarjeta')); modal.hide();", true);
                }
            }
        }

        private void cargarCurso(long cursoId)
        {
            string queryCurso = @"SELECT c.titulo, c.descripcion, c.fecha_creacion, c.img, i.nombre AS instructor, 
                                cap.titulo AS capitulo, cap.numero_capitulo, cap.id AS capitulo_id, c.id AS curso_id
                                FROM cursos c
                                JOIN instructores i ON c.instructor_id = i.id
                                JOIN capitulos cap ON c.id = cap.curso_id
                                WHERE c.id = @curso_id";

            string queryCapitulos = @"SELECT id, titulo, numero_capitulo, @curso_id AS curso_id
                                    FROM capitulos
                                    WHERE curso_id = @curso_id
                                    ORDER BY numero_capitulo";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string email = Session["Usuario"].ToString();
                    hfCursoId.Value = cursoId.ToString();

                    // Get estudiante_id first
                    string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                    MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                    getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                    long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                    // Initialize variables outside the if statement
                    long capituloId = 0;
                    bool isChapterViewed = false;

                    // Load course data
                    MySqlCommand commandCurso = new MySqlCommand(queryCurso, connection);
                    commandCurso.Parameters.AddWithValue("@curso_id", cursoId);
                    MySqlDataReader readerCurso = commandCurso.ExecuteReader();

                    if (readerCurso.Read())
                    {
                        capituloId = Convert.ToInt64(readerCurso["capitulo_id"]);
                        var cursoData = new List<object>
                        {
                            new
                            {
                                titulo = readerCurso["titulo"],
                                descripcion = readerCurso["descripcion"],
                                fecha_creacion = readerCurso["fecha_creacion"],
                                img = readerCurso["img"],
                                instructor = readerCurso["instructor"],
                                capitulo = readerCurso["capitulo"],
                                numero_capitulo = readerCurso["numero_capitulo"],
                                capitulo_id = readerCurso["capitulo_id"],
                                curso_id = readerCurso["curso_id"]
                            }
                        };

                        rptCurso.DataSource = cursoData;
                        rptCurso.DataBind();
                    }
                    readerCurso.Close();

                    // Check if first chapter is viewed using the capituloId variable
                    string checkViewedQuery = @"SELECT visto FROM capitulos_vistos 
                                            WHERE capitulo_id = @capitulo_id 
                                            AND estudiante_id = @estudiante_id";
                    MySqlCommand checkViewedCommand = new MySqlCommand(checkViewedQuery, connection);
                    checkViewedCommand.Parameters.AddWithValue("@capitulo_id", capituloId);
                    checkViewedCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                    
                    object result = checkViewedCommand.ExecuteScalar();
                    isChapterViewed = result != null && Convert.ToInt32(result) == 1;

                    // Update UI elements for viewed status
                    foreach (RepeaterItem item in rptCurso.Items)
                    {
                        LinkButton btnMarcarVisto = (LinkButton)item.FindControl("btnMarcarVisto");
                        System.Web.UI.WebControls.Label lblMarcarVisto = (System.Web.UI.WebControls.Label)btnMarcarVisto.FindControl("lblMarcarVisto");
                        System.Web.UI.HtmlControls.HtmlGenericControl svgOjoAbierto = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoAbierto");
                        System.Web.UI.HtmlControls.HtmlGenericControl svgOjoCerrado = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoCerrado");

                        if (isChapterViewed)
                        {
                            lblMarcarVisto.Text = "Visto";
                            svgOjoAbierto.Style["display"] = "none";
                            svgOjoCerrado.Style["display"] = "block";
                        }
                        else
                        {
                            lblMarcarVisto.Text = "Marcar como visto";
                            svgOjoAbierto.Style["display"] = "block";
                            svgOjoCerrado.Style["display"] = "none";
                        }
                    }

                    // Load chapters
                    MySqlCommand commandCapitulos = new MySqlCommand(queryCapitulos, connection);
                    commandCapitulos.Parameters.AddWithValue("@curso_id", cursoId);
                    MySqlDataReader readerCapitulos = commandCapitulos.ExecuteReader();

                    rptCapitulos.DataSource = readerCapitulos;
                    rptCapitulos.DataBind();
                }
            }
            catch (Exception ex)
            {
                labelDebugCursosIn.Text = "Error al cargar el curso. Por favor, inténtelo de nuevo más tarde. " + ex.Message;
            }
        }

        protected void btnMarcarVisto_Click(object sender, EventArgs e) {
            LinkButton btnMarcarVisto = (LinkButton)sender;
            System.Web.UI.WebControls.Label lblMarcarVisto = (System.Web.UI.WebControls.Label)btnMarcarVisto.FindControl("lblMarcarVisto");
            System.Web.UI.HtmlControls.HtmlGenericControl svgOjoAbierto = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoAbierto");
            System.Web.UI.HtmlControls.HtmlGenericControl svgOjoCerrado = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoCerrado");

            // Get capitulo_id and curso_id from hidden fields
            HiddenField hfCapituloId = (HiddenField)btnMarcarVisto.FindControl("hfCapituloId");
            HiddenField hfCursoId = (HiddenField)btnMarcarVisto.FindControl("hfCursoId");
            long capituloId = Convert.ToInt64(hfCapituloId.Value);
            long cursoId = Convert.ToInt64(hfCursoId.Value);
            string email = Session["Usuario"].ToString();

            if (lblMarcarVisto != null && svgOjoAbierto != null && svgOjoCerrado != null)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Get estudiante_id
                    string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                    MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                    getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                    long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                    if (lblMarcarVisto.Text == "Marcar como visto")
                    {
                        // Marcar como visto
                        string insertQuery = @"INSERT INTO capitulos_vistos (capitulo_id, curso_id, estudiante_id, visto) 
                                            VALUES (@capitulo_id, @curso_id, @estudiante_id, 1)
                                            ON DUPLICATE KEY UPDATE visto = 1";
                        
                        MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                        insertCommand.Parameters.AddWithValue("@capitulo_id", capituloId);
                        insertCommand.Parameters.AddWithValue("@curso_id", cursoId);
                        insertCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                        insertCommand.ExecuteNonQuery();

                        lblMarcarVisto.Text = "Visto";
                        svgOjoAbierto.Style["display"] = "none";
                        svgOjoCerrado.Style["display"] = "block";
                    }
                    else
                    {
                        // Marcar como no visto
                        string updateQuery = @"UPDATE capitulos_vistos 
                                            SET visto = 0 
                                            WHERE capitulo_id = @capitulo_id 
                                            AND curso_id = @curso_id 
                                            AND estudiante_id = @estudiante_id";

                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@capitulo_id", capituloId);
                        updateCommand.Parameters.AddWithValue("@curso_id", cursoId);
                        updateCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                        updateCommand.ExecuteNonQuery();

                        lblMarcarVisto.Text = "Marcar como visto";
                        svgOjoAbierto.Style["display"] = "block";
                        svgOjoCerrado.Style["display"] = "none";
                    }
                }
            }
        }

        protected void rptCapitulos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectCapitulo")
            {

                // Conseguir el id del capítulo seleccionado.
                LinkButton btnCapitulos = (LinkButton)e.Item.FindControl("btnCapitulos");
                HiddenField hfCapituloId = (HiddenField)e.Item.FindControl("hfCapituloId");
                string capituloId = hfCapituloId.Value;

                string[] args = e.CommandArgument.ToString().Split('|');
                string numeroCapitulo = args[0];
                string tituloCapitulo = args[1];

                string email = Session["Usuario"].ToString();
                bool isChapterViewed = false;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Get estudiante_id
                    string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                    MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                    getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                    long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                    // Verificar si el capítulo ha sido visto
                    string checkViewedQuery = @"SELECT visto FROM capitulos_vistos 
                                            WHERE capitulo_id = @capitulo_id 
                                            AND estudiante_id = @estudiante_id";
                    MySqlCommand checkViewedCommand = new MySqlCommand(checkViewedQuery, connection);
                    checkViewedCommand.Parameters.AddWithValue("@capitulo_id", capituloId);
                    checkViewedCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                    
                    object result = checkViewedCommand.ExecuteScalar();
                    isChapterViewed = result != null && Convert.ToInt32(result) == 1;
                }

                foreach (RepeaterItem item in rptCurso.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        // Update the chapter title
                        System.Web.UI.WebControls.Label lblCapCursoIn = (System.Web.UI.WebControls.Label)item.FindControl("lblCapCursoIn");
                        lblCapCursoIn.Text = $"{numeroCapitulo}.<span> {tituloCapitulo}</span>";

                        // Update the hidden field in the main course display
                        HiddenField mainHfCapituloId = (HiddenField)item.FindControl("hfCapituloId");
                        if (mainHfCapituloId != null)
                        {
                            mainHfCapituloId.Value = capituloId;
                        }

                        // Update the viewed status
                        LinkButton btnMarcarVisto = (LinkButton)item.FindControl("btnMarcarVisto");
                        System.Web.UI.WebControls.Label lblMarcarVisto = (System.Web.UI.WebControls.Label)btnMarcarVisto.FindControl("lblMarcarVisto");
                        System.Web.UI.HtmlControls.HtmlGenericControl svgOjoAbierto = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoAbierto");
                        System.Web.UI.HtmlControls.HtmlGenericControl svgOjoCerrado = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoCerrado");

                        if (isChapterViewed)
                        {
                            lblMarcarVisto.Text = "Visto";
                            svgOjoAbierto.Style["display"] = "none";
                            svgOjoCerrado.Style["display"] = "block";
                        }
                        else
                        {
                            lblMarcarVisto.Text = "Marcar como visto";
                            svgOjoAbierto.Style["display"] = "block";
                            svgOjoCerrado.Style["display"] = "none";
                        }
                    }
                }
            }
            else if (e.CommandName == "ShowDeleteModal")
            {
                // Store the chapter ID in the hidden field
                int chapterId = Convert.ToInt32(e.CommandArgument);
                hfDeleteChapterId.Value = chapterId.ToString();
                
                // Get the chapter number from the database
                int chapterNumber = 0;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT numero_capitulo FROM capitulos WHERE id = @chapterId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@chapterId", chapterId);
                    chapterNumber = Convert.ToInt32(cmd.ExecuteScalar());
                }
                
                // Update the modal message with the chapter number and show the modal
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowDeleteModal", $@"
                    setTimeout(function() {{
                        document.getElementById('deleteChapterMessage').innerText = '¿Estás seguro de que quieres eliminar el capítulo {chapterNumber}?';
                        var deleteModal = new bootstrap.Modal(document.getElementById('modalDeleteChapter'));
                        deleteModal.show();
                    }}, 100);", true);
            }
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            string email = Session["Usuario"].ToString();
            string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Obtener el ID del estudiante
                string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                // Verificar el estado de la suscripción del usuario
                string getSubscriptionQuery = "SELECT estado FROM suscripciones WHERE estudiante_id = @estudiante_id AND estado = 'activa'";
                MySqlCommand getSubscriptionCommand = new MySqlCommand(getSubscriptionQuery, connection);
                getSubscriptionCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                string estadoSuscripcion = (string)getSubscriptionCommand.ExecuteScalar();

                // Obtener el ID del curso
                System.Web.UI.WebControls.Button btnUnirme = (System.Web.UI.WebControls.Button)sender;
                RepeaterItem item = (RepeaterItem)btnUnirme.NamingContainer;
                HiddenField hfCursoId = (HiddenField)item.FindControl("hfCursoId");
                long cursoId = Convert.ToInt64(hfCursoId.Value);

                // Verificar si el curso es premium
                string getCursoQuery = "SELECT es_premium FROM cursos WHERE id = @curso_id";
                MySqlCommand getCursoCommand = new MySqlCommand(getCursoQuery, connection);
                getCursoCommand.Parameters.AddWithValue("@curso_id", cursoId);
                bool esPremium = Convert.ToBoolean(getCursoCommand.ExecuteScalar());

                if (esPremium && estadoSuscripcion != "activa")
                {
                    // Redirigir a la vista de planes si el curso es premium y el usuario no tiene una suscripción premium activa
                    paginas.ActiveViewIndex = 3;
                }
                else
                {
                    // Cargar el curso y cambiar la vista
                    cargarCurso(cursoId);
                    paginas.ActiveViewIndex = 5;
                    CheckCourseOwnership(cursoId);
                }
            }
        }
        
        protected void btnSalirCurso_Click(object sender, EventArgs e)
        {
            string email = Session["Usuario"].ToString();
            LinkButton btnSalirCurso = (LinkButton)sender;
            RepeaterItem item = (RepeaterItem)btnSalirCurso.NamingContainer;
            HiddenField hfCursoId = (HiddenField)item.FindControl("hfCursoId");
            long cursoId = Convert.ToInt64(hfCursoId.Value);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Get estudiante_id
                string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                // Delete enrollment
                string deleteInscripcionQuery = "DELETE FROM inscripciones WHERE estudiante_id = @estudiante_id AND curso_id = @curso_id";
                MySqlCommand deleteInscripcionCommand = new MySqlCommand(deleteInscripcionQuery, connection);
                deleteInscripcionCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                deleteInscripcionCommand.Parameters.AddWithValue("@curso_id", cursoId);
                deleteInscripcionCommand.ExecuteNonQuery();
            }

            // Reload MisCursos view
            CargarMisCursos();
        }

        private void CargarMisCursos()
        {
            string email = Session["Usuario"].ToString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT c.*, c.es_premium, instr.nombre AS nombre_instructor,
                                (SELECT COUNT(*) FROM capitulos WHERE curso_id = c.id) as total_capitulos,
                                (SELECT COUNT(*) FROM capitulos_vistos cv 
                                JOIN estudiantes e2 ON cv.estudiante_id = e2.id 
                                WHERE cv.curso_id = c.id 
                                AND e2.correo_electronico = @email 
                                AND cv.visto = 1) as capitulos_vistos
                                FROM cursos c 
                                INNER JOIN inscripciones i ON c.id = i.curso_id 
                                INNER JOIN estudiantes e ON i.estudiante_id = e.id
                                INNER JOIN instructores instr ON c.instructor_id = instr.id
                                WHERE e.correo_electronico = @email";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    rptMisCursos.DataSource = dt;
                    rptMisCursos.DataBind();

                    divSinCursos.Visible = false;
                    rptMisCursos.Visible = true;
                }
                else
                {
                    divSinCursos.Visible = true;
                    rptMisCursos.Visible = false;
                }
            }
        }

        private void CheckIfInstructor()
        {
            string email = Session["Usuario"].ToString();
            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) 
                                FROM instructores i 
                                JOIN estudiantes e ON i.estudiante_id = e.id 
                                WHERE e.correo_electronico = @email";
                
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                
                int count = Convert.ToInt32(command.ExecuteScalar());
                pnlInstructorButton.Visible = (count > 0);
            }
        }

        protected void btnActualizarEmail_Click(object sender, EventArgs e)
        {
            string currentEmail = Session["Usuario"].ToString();
            string newEmail = txtEmailPerfil.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if new email already exists
                string checkEmailQuery = "SELECT COUNT(*) FROM estudiantes WHERE correo_electronico = @nuevo_email AND correo_electronico != @email_actual";
                MySqlCommand checkEmailCommand = new MySqlCommand(checkEmailQuery, connection);
                checkEmailCommand.Parameters.AddWithValue("@nuevo_email", newEmail);
                checkEmailCommand.Parameters.AddWithValue("@email_actual", currentEmail);

                int emailExists = Convert.ToInt32(checkEmailCommand.ExecuteScalar());

                if (emailExists > 0)
                {
                    // Email already exists
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowEmailExistsToast", @"
                        document.addEventListener('DOMContentLoaded', function() {
                            var toastElement = document.getElementById('emailExistsToast');
                            var toast = new bootstrap.Toast(toastElement);
                            toast.show();
                        });
                    ", true);
                    return;
                }

                // Update email
                string updateQuery = "UPDATE estudiantes SET correo_electronico = @nuevo_email WHERE correo_electronico = @email_actual";
                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@nuevo_email", newEmail);
                updateCommand.Parameters.AddWithValue("@email_actual", currentEmail);
                updateCommand.ExecuteNonQuery();

                // Update session
                Session["Usuario"] = newEmail;
                
                // Clear the textbox
                txtEmailPerfil.Text = string.Empty;

                // After successful update
                Session["Usuario"] = newEmail;
                txtEmailPerfil.Text = string.Empty;

                // Show toast notification with proper initialization
                string script = @"
                    document.addEventListener('DOMContentLoaded', function() {
                        var toastElement = document.getElementById('emailToast');
                        var toast = new bootstrap.Toast(toastElement);
                        toast.show();
                    });
                ";
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowEmailToast", script, true);
            }
        }

        protected void btnActualizarPassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email = Session["Usuario"].ToString();
                string currentPassword = txtPasswordActual.Text;
                string newPassword = txtPasswordNueva.Text;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Verify current password
                    string checkPasswordQuery = "SELECT hash_contrasena FROM estudiantes WHERE correo_electronico = @email";
                    MySqlCommand checkPasswordCommand = new MySqlCommand(checkPasswordQuery, connection);
                    checkPasswordCommand.Parameters.AddWithValue("@email", email);
                    string storedHash = (string)checkPasswordCommand.ExecuteScalar();

                    if (BCrypt.Net.BCrypt.Verify(currentPassword, storedHash))
                    {
                        // Hash new password
                        string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

                        // Update password
                        string updateQuery = "UPDATE estudiantes SET hash_contrasena = @nueva_contrasena WHERE correo_electronico = @email";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@nueva_contrasena", newHashedPassword);
                        updateCommand.Parameters.AddWithValue("@email", email);
                        updateCommand.ExecuteNonQuery();

                        // Clear form
                        txtPasswordActual.Text = string.Empty;
                        txtPasswordNueva.Text = string.Empty;
                        txtPasswordConfirm.Text = string.Empty;

                        // Show success toast
                        string script = @"
                            document.addEventListener('DOMContentLoaded', function() {
                                var toastElement = document.getElementById('passwordToast');
                                var toast = new bootstrap.Toast(toastElement);
                                toast.show();
                            });
                        ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowPasswordToast", script, true);
                    }
                    else
                    {
                        // Show error toast for incorrect current password
                        string script = @"
                            document.addEventListener('DOMContentLoaded', function() {
                                var toastElement = document.getElementById('passwordErrorToast');
                                var toast = new bootstrap.Toast(toastElement);
                                toast.show();
                            });
                        ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowPasswordErrorToast", script, true);
                    }
                }
            }
        }

        private void LoadProfileInformation()
        {
            string email = Session["Usuario"].ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // First get the user's basic information
                string userQuery = @"SELECT 
                                    e.nombre, e.apellidos, e.fecha_creacion, e.correo_electronico,
                                    (SELECT COUNT(*) 
                                    FROM inscripciones i 
                                    JOIN cursos c ON i.curso_id = c.id
                                    WHERE i.estudiante_id = e.id 
                                    AND (SELECT COUNT(*) FROM capitulos WHERE curso_id = c.id) = 
                                        (SELECT COUNT(*) FROM capitulos_vistos cv 
                                        WHERE cv.curso_id = c.id 
                                        AND cv.estudiante_id = e.id 
                                        AND cv.visto = 1)
                                    ) as cursos_completados,
                                    CASE 
                                        WHEN EXISTS (
                                            SELECT 1 FROM suscripciones s 
                                            WHERE s.estudiante_id = e.id 
                                            AND s.estado = 'activa'
                                        ) THEN 'Premium'
                                        ELSE 'Básico'
                                    END as plan_actual
                                    FROM estudiantes e
                                    WHERE e.correo_electronico = @email";

                MySqlCommand userCmd = new MySqlCommand(userQuery, connection);
                userCmd.Parameters.AddWithValue("@email", email);

                using (MySqlDataReader userReader = userCmd.ExecuteReader())
                {
                    if (userReader.Read())
                    {
                        lblNombre.Text = userReader["nombre"].ToString();
                        lblApellidos.Text = userReader["apellidos"].ToString();
                        lblFechaCreacion.Text = Convert.ToDateTime(userReader["fecha_creacion"]).ToString("dd/MM/yyyy");
                        lblEmail.Text = userReader["correo_electronico"].ToString();
                        lblCursosCompletados.Text = userReader["cursos_completados"].ToString();
                        lblPlanActual.Text = userReader["plan_actual"].ToString();

                        if (userReader["plan_actual"].ToString() == "Premium")
                        {
                            lblPlanActual.CssClass = "negrita lbl premium";
                        }
                    }
                }

                // Now get the course history
                string historyQuery = @"SELECT 
                    c.titulo,
                    i.fecha_inscripcion,
                    (SELECT COUNT(*) FROM capitulos WHERE curso_id = c.id) as total_capitulos,
                    (SELECT COUNT(*) FROM capitulos_vistos cv 
                    WHERE cv.curso_id = c.id 
                    AND cv.estudiante_id = e.id 
                    AND cv.visto = 1) as capitulos_vistos
                FROM inscripciones i
                JOIN estudiantes e ON i.estudiante_id = e.id
                JOIN cursos c ON i.curso_id = c.id
                WHERE e.correo_electronico = @email
                ORDER BY i.fecha_inscripcion DESC";

                MySqlCommand historyCmd = new MySqlCommand(historyQuery, connection);
                historyCmd.Parameters.AddWithValue("@email", email);

                using (MySqlDataReader historyReader = historyCmd.ExecuteReader())
                {
                    rptHistorialCursos.DataSource = historyReader;
                    rptHistorialCursos.DataBind();
                }
            }
        }

        protected void btnGuardarCurso_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email = Session["Usuario"].ToString();
                string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";

                // Handle image upload
                string imageFileName = "";
                if (fuImagenCurso.HasFile)
                {
                    // Generate unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fuImagenCurso.FileName);
                    string savePath = Server.MapPath("~/imgs/cursos/") + uniqueFileName;
                    
                    // Create directory if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath("~/imgs/cursos/"));
                    
                    // Save the file
                    fuImagenCurso.SaveAs(savePath);
                    imageFileName = "imgs/cursos/" + uniqueFileName;
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // First, get instructor_id using the correct join
                    string queryInstructor = @"SELECT i.id 
                                            FROM instructores i 
                                            JOIN estudiantes e ON i.estudiante_id = e.id 
                                            WHERE e.correo_electronico = @email";
                    MySqlCommand cmdInstructor = new MySqlCommand(queryInstructor, connection);
                    cmdInstructor.Parameters.AddWithValue("@email", email);
                    object instructorIdResult = cmdInstructor.ExecuteScalar();

                    if (instructorIdResult == null)
                    {
                        // Handle case where user is not an instructor
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", 
                            "alert('No tienes permisos de instructor para crear cursos.');", true);
                        return;
                    }

                    int instructorId = Convert.ToInt32(instructorIdResult);

                    // Rest of the course creation code remains the same
                    string queryCurso = @"INSERT INTO cursos (titulo, descripcion, img, es_premium, instructor_id, fecha_creacion) 
                                        VALUES (@titulo, @descripcion, @img, @es_premium, @instructor_id, NOW())";
                    
                    MySqlCommand cmdCurso = new MySqlCommand(queryCurso, connection);
                    cmdCurso.Parameters.AddWithValue("@titulo", txtTituloCurso.Text);
                    cmdCurso.Parameters.AddWithValue("@descripcion", txtDescripcionCurso.Text);
                    cmdCurso.Parameters.AddWithValue("@img", imageFileName);
                    cmdCurso.Parameters.AddWithValue("@es_premium", chkCursoPremium.Checked ? 1 : 0);
                    cmdCurso.Parameters.AddWithValue("@instructor_id", instructorId);
                    cmdCurso.ExecuteNonQuery();

                    // Get the ID of the inserted course
                    long cursoId = cmdCurso.LastInsertedId;

                    // Insert initial chapters
                    string queryCapitulos = "INSERT INTO capitulos (curso_id, numero_capitulo, titulo) VALUES (@curso_id, @numero, @titulo)";
                    
                    // Insert the first 3 chapters from ASP.NET controls
                    InsertChapter(connection, cursoId, 1, txtCapitulo1.Text);
                    InsertChapter(connection, cursoId, 2, txtCapitulo2.Text);
                    InsertChapter(connection, cursoId, 3, txtCapitulo3.Text);

                    // Insert additional chapters from form collection
                    for (int i = 4; i <= 10; i++)
                    {
                        string chapterValue = Request.Form[$"chapter{i}"];
                        if (!string.IsNullOrEmpty(chapterValue))
                        {
                            InsertChapter(connection, cursoId, i, chapterValue);
                        }
                    }
                }

                // Clear form and close modal
                LimpiarFormularioCurso();
                ScriptManager.RegisterStartupScript(this, GetType(), "closeCourseModal", 
                    "$('#modalCrearCurso').modal('hide');", true);
            }
        }

        private void InsertChapter(MySqlConnection connection, long cursoId, int numero, string titulo)
        {
            string queryCapitulo = "INSERT INTO capitulos (curso_id, numero_capitulo, titulo) VALUES (@curso_id, @numero, @titulo)";
            MySqlCommand cmdCapitulo = new MySqlCommand(queryCapitulo, connection);
            cmdCapitulo.Parameters.AddWithValue("@curso_id", cursoId);
            cmdCapitulo.Parameters.AddWithValue("@numero", numero);
            cmdCapitulo.Parameters.AddWithValue("@titulo", titulo);
            cmdCapitulo.ExecuteNonQuery();
        }

        private void LimpiarFormularioCurso()
        {
            txtTituloCurso.Text = string.Empty;
            txtDescripcionCurso.Text = string.Empty;
            txtCapitulo1.Text = string.Empty;
            txtCapitulo2.Text = string.Empty;
            txtCapitulo3.Text = string.Empty;
            chkCursoPremium.Checked = false;
        }

        private void CheckCourseOwnership(long cursoId)
        {
            if (Session["Usuario"] != null)
            {
                string email = Session["Usuario"].ToString();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if user is course owner and chapter count
                    string query = @"SELECT 
                                    (SELECT COUNT(*) 
                                    FROM cursos c 
                                    JOIN instructores i ON c.instructor_id = i.id 
                                    JOIN estudiantes e ON i.estudiante_id = e.id 
                                    WHERE c.id = @cursoId AND e.correo_electronico = @email) as is_owner,
                                    (SELECT COUNT(*) 
                                    FROM capitulos 
                                    WHERE curso_id = @cursoId) as chapter_count";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@cursoId", cursoId);
                    cmd.Parameters.AddWithValue("@email", email);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool isOwner = Convert.ToInt32(reader["is_owner"]) > 0;
                            int chapterCount = Convert.ToInt32(reader["chapter_count"]);
                            pnlAddChapter.Visible = isOwner && chapterCount < 10;
                        }
                    }
                }
            }
        }

        protected void btnSaveNewChapter_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                long cursoId = Convert.ToInt64(hfCursoId.Value);
                string newChapterTitle = txtNewChapterTitle.Text;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // First check if the course already has 10 chapters
                    string countQuery = "SELECT COUNT(*) FROM capitulos WHERE curso_id = @cursoId";
                    MySqlCommand countCmd = new MySqlCommand(countQuery, connection);
                    countCmd.Parameters.AddWithValue("@cursoId", cursoId);
                    int chapterCount = Convert.ToInt32(countCmd.ExecuteScalar());

                    if (chapterCount >= 10)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", 
                            "alert('No se pueden añadir más capítulos. El límite es de 10 capítulos por curso.');", true);
                        return;
                    }

                    // If less than 10 chapters, proceed with insertion
                    string queryNextNumber = "SELECT COALESCE(MAX(numero_capitulo), 0) + 1 FROM capitulos WHERE curso_id = @cursoId";
                    MySqlCommand cmdNextNumber = new MySqlCommand(queryNextNumber, connection);
                    cmdNextNumber.Parameters.AddWithValue("@cursoId", cursoId);
                    int nextChapterNumber = Convert.ToInt32(cmdNextNumber.ExecuteScalar());

                    string queryInsert = "INSERT INTO capitulos (curso_id, numero_capitulo, titulo) VALUES (@cursoId, @numero, @titulo)";
                    MySqlCommand cmdInsert = new MySqlCommand(queryInsert, connection);
                    cmdInsert.Parameters.AddWithValue("@cursoId", cursoId);
                    cmdInsert.Parameters.AddWithValue("@numero", nextChapterNumber);
                    cmdInsert.Parameters.AddWithValue("@titulo", newChapterTitle);
                    cmdInsert.ExecuteNonQuery();

                    // Clear the textbox and close modal
                    txtNewChapterTitle.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", 
                        "$('#modalAddChapter').modal('hide');", true);

                    cargarCurso(cursoId);
                }
            }
        }

        protected bool IsCourseInstructor(int cursoId)
        {
            if (Session["Usuario"] == null)
                return false;

            string email = Session["Usuario"].ToString();
            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) 
                                FROM cursos c 
                                JOIN instructores i ON c.instructor_id = i.id 
                                JOIN estudiantes e ON i.estudiante_id = e.id 
                                WHERE c.id = @cursoId 
                                AND e.correo_electronico = @email";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@cursoId", cursoId);
                cmd.Parameters.AddWithValue("@email", email);

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        protected void btnConfirmDeleteChapter_Click(object sender, EventArgs e)
        {
            // Get the chapter ID from the hidden field
            int chapterId = Convert.ToInt32(hfDeleteChapterId.Value);
            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                
                // First, get the curso_id and numero_capitulo of the chapter to be deleted
                string getChapterInfoQuery = "SELECT curso_id, numero_capitulo FROM capitulos WHERE id = @chapterId";
                MySqlCommand getChapterInfoCmd = new MySqlCommand(getChapterInfoQuery, connection);
                getChapterInfoCmd.Parameters.AddWithValue("@chapterId", chapterId);
                
                int deletedChapterNumber = 0;
                long cursoId = 0;
                
                using (MySqlDataReader reader = getChapterInfoCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cursoId = Convert.ToInt64(reader["curso_id"]);
                        deletedChapterNumber = Convert.ToInt32(reader["numero_capitulo"]);
                    }
                }
                
                if (cursoId > 0)
                {
                    // Delete the chapter
                    string deleteQuery = "DELETE FROM capitulos WHERE id = @chapterId";
                    MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                    deleteCmd.Parameters.AddWithValue("@chapterId", chapterId);
                    deleteCmd.ExecuteNonQuery();
                    
                    // Update the chapter numbers for all chapters with higher numbers
                    string updateNumbersQuery = "UPDATE capitulos SET numero_capitulo = numero_capitulo - 1 " +
                                                "WHERE curso_id = @cursoId AND numero_capitulo > @deletedChapterNumber";
                    MySqlCommand updateNumbersCmd = new MySqlCommand(updateNumbersQuery, connection);
                    updateNumbersCmd.Parameters.AddWithValue("@cursoId", cursoId);
                    updateNumbersCmd.Parameters.AddWithValue("@deletedChapterNumber", deletedChapterNumber);
                    updateNumbersCmd.ExecuteNonQuery();
                    
                    // Also update any viewed chapters records that might reference higher chapter numbers
                    string updateViewedQuery = "UPDATE capitulos_vistos cv " +
                                            "JOIN capitulos c ON cv.capitulo_id = c.id " +
                                            "SET c.numero_capitulo = c.numero_capitulo - 1 " +
                                            "WHERE c.curso_id = @cursoId AND c.numero_capitulo > @deletedChapterNumber";
                    MySqlCommand updateViewedCmd = new MySqlCommand(updateViewedQuery, connection);
                    updateViewedCmd.Parameters.AddWithValue("@cursoId", cursoId);
                    updateViewedCmd.Parameters.AddWithValue("@deletedChapterNumber", deletedChapterNumber);
                    updateViewedCmd.ExecuteNonQuery();
                    
                    // Reload the course to update the UI
                    cargarCurso(cursoId);
                    
                    // Hide the modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideDeleteModal", 
                        "$('#modalDeleteChapter').modal('hide');", true);
                }
            }
        }
    }
}