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
        private int contCapitulos = 1;

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
                        string query = @"SELECT s.estado 
                                        FROM suscripciones s 
                                        JOIN estudiantes e 
                                        ON s.estudiante_id = e.id 
                                        WHERE e.correo_electronico = @correo_electronico 
                                        AND s.estado = 'activa'";
                        
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@correo_electronico", email);

                        string estadoSuscripcion = (string)command.ExecuteScalar();

                        // Actualizar los botones de los planes
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
            // Seleccionar 6 cursos (página principal)
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

                    rptCursos6.DataSource = reader;
                    rptCursos6.DataBind();
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

            // Seleccionar todos los cursos
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
                // Si el curso es premium y el usuario no tiene una suscripción premium activa, deshabilitar el botón
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
            verificarInstructor();
        }

        protected void btnPlanes_Click(object sender, EventArgs e)
        {
            paginas.ActiveViewIndex = 3;
        }

        protected void btnPerfil_Click(object sender, EventArgs e)
        {
            paginas.ActiveViewIndex = 4;
            cargarInfoPerfil();
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
                    verificarCreadorCurso(cursoId);
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

                    // Conseguir el id del estudiante
                    string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                    MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                    getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                    long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                    // Calcular nueva fecha de fin (30 días desde ahora)
                    DateTime fechaFin = DateTime.Now.AddDays(30);

                    // Actualizar la suscripción a premium y establecer la fecha de fin
                    string updateQuery = "UPDATE suscripciones SET estado = 'activa', fecha_fin = @fecha_fin WHERE estudiante_id = @estudiante_id AND estado = 'inactiva'";
                    MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                    updateCommand.Parameters.AddWithValue("@fecha_fin", fechaFin);
                    updateCommand.ExecuteNonQuery();

                    // Actualizar botones y limpiar campos
                    btnPlanGratuito.Text = "Volver a Plan Básico";
                    btnPlanGratuito.Enabled = true;
                    btnPlanGratuito.CssClass = "btn btn-primary btnMejorar";

                    btnPlanPremium.Text = "Plan actual";
                    btnPlanPremium.Enabled = false;
                    btnPlanPremium.CssClass = "btn btn-outline-primary-mejorar";

                    txtNumeroTarjeta.Text = string.Empty;
                    txtFechaExp.Text = string.Empty;
                    txtCVV.Text = string.Empty;
                    txtNombreTarjeta.Text = string.Empty;

                    // Cerrar el modal (de la forma que indica bootstrap)
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

                    // Conseguir el id del estudiante
                    string getIdQuery = "SELECT id FROM estudiantes WHERE correo_electronico = @correo_electronico";
                    MySqlCommand getIdCommand = new MySqlCommand(getIdQuery, connection);
                    getIdCommand.Parameters.AddWithValue("@correo_electronico", email);
                    long estudianteId = Convert.ToInt64(getIdCommand.ExecuteScalar());

                    long capituloId = 0;
                    bool capituloVisto = false;

                    // Cargar datos del curso
                    MySqlCommand commandCurso = new MySqlCommand(queryCurso, connection);
                    commandCurso.Parameters.AddWithValue("@curso_id", cursoId);
                    MySqlDataReader readerCurso = commandCurso.ExecuteReader();

                    if (readerCurso.Read())
                    {
                        capituloId = Convert.ToInt64(readerCurso["capitulo_id"]);
                        // Lista con todos los datos del curso
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

                    // Verificar si el primer capítulo está visto
                    string checkViewedQuery = @"SELECT visto FROM capitulos_vistos 
                                            WHERE capitulo_id = @capitulo_id 
                                            AND estudiante_id = @estudiante_id";
                    MySqlCommand checkViewedCommand = new MySqlCommand(checkViewedQuery, connection);
                    checkViewedCommand.Parameters.AddWithValue("@capitulo_id", capituloId);
                    checkViewedCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                    
                    object result = checkViewedCommand.ExecuteScalar();
                    capituloVisto = result != null && Convert.ToInt32(result) == 1;

                    // Actualizar el estado del botón "Marcar como visto"
                    foreach (RepeaterItem item in rptCurso.Items)
                    {
                        LinkButton btnMarcarVisto = (LinkButton)item.FindControl("btnMarcarVisto");
                        System.Web.UI.WebControls.Label lblMarcarVisto = (System.Web.UI.WebControls.Label)btnMarcarVisto.FindControl("lblMarcarVisto");
                        System.Web.UI.HtmlControls.HtmlGenericControl svgOjoAbierto = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoAbierto");
                        System.Web.UI.HtmlControls.HtmlGenericControl svgOjoCerrado = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoCerrado");

                        if (capituloVisto)
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

                    // Cargar los capítulos
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

            // Conseguir capitulo_id y curso_id de los campos ocultos (HiddenFields)
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
                bool capituloVisto = false;

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
                    capituloVisto = result != null && Convert.ToInt32(result) == 1;
                }

                // Para cada item en rptCurso
                foreach (RepeaterItem item in rptCurso.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        // Actualizar el título del capítulo
                        System.Web.UI.WebControls.Label lblCapCursoIn = (System.Web.UI.WebControls.Label)item.FindControl("lblCapCursoIn");
                        lblCapCursoIn.Text = $"{numeroCapitulo}.<span> {tituloCapitulo}</span>";

                        // Actualziar el hidden field
                        HiddenField mainHfCapituloId = (HiddenField)item.FindControl("hfCapituloId");
                        if (mainHfCapituloId != null)
                        {
                            mainHfCapituloId.Value = capituloId;
                        }

                        // Actualizar el estado del botón "Marcar como visto"
                        LinkButton btnMarcarVisto = (LinkButton)item.FindControl("btnMarcarVisto");
                        System.Web.UI.WebControls.Label lblMarcarVisto = (System.Web.UI.WebControls.Label)btnMarcarVisto.FindControl("lblMarcarVisto");
                        System.Web.UI.HtmlControls.HtmlGenericControl svgOjoAbierto = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoAbierto");
                        System.Web.UI.HtmlControls.HtmlGenericControl svgOjoCerrado = (System.Web.UI.HtmlControls.HtmlGenericControl)btnMarcarVisto.FindControl("svgOjoCerrado");

                        if (capituloVisto)
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
            else if (e.CommandName == "mostrarModalEliminarCap")
            {
                // Guardar el ID del capítulo en el campo oculto
                int chapterId = Convert.ToInt32(e.CommandArgument);
                hfDeleteChapterId.Value = chapterId.ToString();
                
                // Conseguir el número del capítulo
                int chapterNumber = 0;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT numero_capitulo FROM capitulos WHERE id = @chapterId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@chapterId", chapterId);
                    chapterNumber = Convert.ToInt32(cmd.ExecuteScalar());
                }
                
                // Actualizar el mensaje del modal con el número del capítulo y mostrar el modal
                ScriptManager.RegisterStartupScript(this, GetType(), "mostrarModalEliminarCap", $@"
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
                    verificarCreadorCurso(cursoId);
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

                // Eliminar la inscripción del estudiante al curso
                string eliminarInscripcionQuery = "DELETE FROM inscripciones WHERE estudiante_id = @estudiante_id AND curso_id = @curso_id";
                MySqlCommand eliminarInscripcionCommand = new MySqlCommand(eliminarInscripcionQuery, connection);
                eliminarInscripcionCommand.Parameters.AddWithValue("@estudiante_id", estudianteId);
                eliminarInscripcionCommand.Parameters.AddWithValue("@curso_id", cursoId);
                eliminarInscripcionCommand.ExecuteNonQuery();
            }

            // Regargar vista de mis cursos
            CargarMisCursos();
        }

        private void CargarMisCursos()
        {
            string email = Session["Usuario"].ToString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // Query para obtener los cursos en los que está inscrito el estudiante
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

                // Si hay cursos, mostrar el repeater, si no, mostrar el div de sin cursos
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

        // Función para verificar si el usuario es instructor
        private void verificarInstructor()
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
                // Si es instructor, mostrar el panel del instructor con el botón para crear curso
                pnlInstructorButton.Visible = (count > 0);
            }
        }

        protected void btnActualizarEmail_Click(object sender, EventArgs e)
        {
            string emailActual = Session["Usuario"].ToString();
            string emailNuevo = txtEmailPerfil.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si el nuevo correo ya existe
                string checkEmailQuery = "SELECT COUNT(*) FROM estudiantes WHERE correo_electronico = @nuevo_email AND correo_electronico != @email_actual";
                MySqlCommand checkEmailCommand = new MySqlCommand(checkEmailQuery, connection);
                checkEmailCommand.Parameters.AddWithValue("@nuevo_email", emailNuevo);
                checkEmailCommand.Parameters.AddWithValue("@email_actual", emailActual);

                int emailExists = Convert.ToInt32(checkEmailCommand.ExecuteScalar());

                if (emailExists > 0)
                {
                    // El correo ya existe (mostrar toast)
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowEmailExistsToast", @"
                        document.addEventListener('DOMContentLoaded', function() {
                            var toastElement = document.getElementById('emailExistsToast');
                            var toast = new bootstrap.Toast(toastElement);
                            toast.show();
                        });
                    ", true);
                    return;
                }

                // Actualizar correo
                string updateQuery = "UPDATE estudiantes SET correo_electronico = @nuevo_email WHERE correo_electronico = @email_actual";
                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@nuevo_email", emailNuevo);
                updateCommand.Parameters.AddWithValue("@email_actual", emailActual);
                updateCommand.ExecuteNonQuery();

                Session["Usuario"] = emailNuevo;
                txtEmailPerfil.Text = string.Empty;

                 // Después de actualizar el correo, mostrar un toast
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
                string actualPassword = txtPasswordActual.Text;
                string nuevaPassword = txtPasswordNueva.Text;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar contraseña actual
                    string checkPasswordQuery = "SELECT hash_contrasena FROM estudiantes WHERE correo_electronico = @email";
                    MySqlCommand checkPasswordCommand = new MySqlCommand(checkPasswordQuery, connection);
                    checkPasswordCommand.Parameters.AddWithValue("@email", email);
                    string storedHash = (string)checkPasswordCommand.ExecuteScalar();

                    // Si coinciden (utilizando BCrypt)
                    if (BCrypt.Net.BCrypt.Verify(actualPassword, storedHash))
                    {
                        // Hashear nueva contraseña
                        string nuevaHashedPassword = BCrypt.Net.BCrypt.HashPassword(nuevaPassword);

                        // Actualizar la contraseña
                        string updateQuery = "UPDATE estudiantes SET hash_contrasena = @nueva_contrasena WHERE correo_electronico = @email";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@nueva_contrasena", nuevaHashedPassword);
                        updateCommand.Parameters.AddWithValue("@email", email);
                        updateCommand.ExecuteNonQuery();

                        // Limpiar campos
                        txtPasswordActual.Text = string.Empty;
                        txtPasswordNueva.Text = string.Empty;
                        txtPasswordConfirm.Text = string.Empty;

                        // Mostrar notificación de éxito (toast)
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
                        // Mostrar notificación de error (toast)
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

        private void cargarInfoPerfil()
        {
            string email = Session["Usuario"].ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Conseguir toda la información del usuario
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

                // Conseguir el historial de cursos
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

        protected void btnCrearCursoModal_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email = Session["Usuario"].ToString();
                string connectionString = "DataBase=codelab;DataSource=localhost;user=root;Port=3306";

                // Manejar la imagen
                string imageFileName = "";
                if (fuImagenCurso.HasFile)
                {
                    // Generar un nombre único para la imagen
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fuImagenCurso.FileName);
                    string savePath = Server.MapPath("~/imgs/cursos/") + uniqueFileName;
                    
                    // Guardar el archivo
                    fuImagenCurso.SaveAs(savePath);
                    imageFileName = "imgs/cursos/" + uniqueFileName;
                }

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Conseguir el ID del instructor
                    string queryInstructor = @"SELECT i.id 
                                            FROM instructores i 
                                            JOIN estudiantes e ON i.estudiante_id = e.id 
                                            WHERE e.correo_electronico = @email";
                    MySqlCommand cmdInstructor = new MySqlCommand(queryInstructor, connection);
                    cmdInstructor.Parameters.AddWithValue("@email", email);
                    object instructorIdResult = cmdInstructor.ExecuteScalar();

                    int instructorId = Convert.ToInt32(instructorIdResult);

                    // Insertar el curso en la base de datos
                    string queryCurso = @"INSERT INTO cursos (titulo, descripcion, img, es_premium, instructor_id, fecha_creacion) 
                                        VALUES (@titulo, @descripcion, @img, @es_premium, @instructor_id, NOW())";
                    
                    MySqlCommand cmdCurso = new MySqlCommand(queryCurso, connection);
                    cmdCurso.Parameters.AddWithValue("@titulo", txtTituloCurso.Text);
                    cmdCurso.Parameters.AddWithValue("@descripcion", txtDescripcionCurso.Text);
                    cmdCurso.Parameters.AddWithValue("@img", imageFileName);
                    cmdCurso.Parameters.AddWithValue("@es_premium", chkCursoPremium.Checked ? 1 : 0);
                    cmdCurso.Parameters.AddWithValue("@instructor_id", instructorId);
                    cmdCurso.ExecuteNonQuery();

                    // Conseguir el ID del curso recién insertado
                    long cursoId = cmdCurso.LastInsertedId;

                    // Insertar los capítulos
                    string queryCapitulos = "INSERT INTO capitulos (curso_id, numero_capitulo, titulo) VALUES (@curso_id, @numero, @titulo)";
                    
                    // Inertar los primeros 3 capítulos desde controles ASP.NET
                    insertarCapitulo(connection, cursoId, 1, txtCapitulo1.Text);
                    insertarCapitulo(connection, cursoId, 2, txtCapitulo2.Text);
                    insertarCapitulo(connection, cursoId, 3, txtCapitulo3.Text);

                    // Insertar los otros capítulos desde el formulario
                    for (int i = 4; i <= 10; i++)
                    {
                        string valorCap = Request.Form[$"chapter{i}"];
                        if (!string.IsNullOrEmpty(valorCap))
                        {
                            insertarCapitulo(connection, cursoId, i, valorCap);
                        }
                    }
                }

                // Limpiar el formulario y mostrar un mensaje de éxito
                LimpiarFormularioCurso();
                ScriptManager.RegisterStartupScript(this, GetType(), "closeCourseModal", 
                    "$('#modalCrearCurso').modal('hide');", true);
            }
        }

        private void insertarCapitulo(MySqlConnection connection, long cursoId, int numero, string titulo)
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

        private void verificarCreadorCurso(long cursoId)
        {
            if (Session["Usuario"] != null)
            {
                string email = Session["Usuario"].ToString();

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar si el usuario es el creador del curso y el número de capítulos
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
                            // Si el usuario es el creador del curso y tiene menos de 10 capítulos, 
                            // mostrar el botón
                            bool isOwner = Convert.ToInt32(reader["is_owner"]) > 0;
                            int contCapitulos = Convert.ToInt32(reader["chapter_count"]);
                            pnlAddChapter.Visible = isOwner && contCapitulos < 10;
                        }
                    }
                }
            }
        }

        protected void btnGuardarNuevoCap_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                long cursoId = Convert.ToInt64(hfCursoId.Value);
                string newChapterTitle = txtNewChapterTitle.Text;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Primero verificamos si el curso ya tiene 10 capítulos
                    string countQuery = "SELECT COUNT(*) FROM capitulos WHERE curso_id = @cursoId";
                    MySqlCommand countCmd = new MySqlCommand(countQuery, connection);
                    countCmd.Parameters.AddWithValue("@cursoId", cursoId);
                    int contCapitulos = Convert.ToInt32(countCmd.ExecuteScalar());

                    if (contCapitulos >= 10)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", 
                            "alert('No se pueden añadir más capítulos. El límite es de 10 capítulos por curso.');", true);
                        return;
                    }

                    // Si tiene menos de 10 capítulos, procedemos con el insert
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

                    // Limpiar el textbox y cerrar el modal
                    txtNewChapterTitle.Text = string.Empty;
                    ScriptManager.RegisterStartupScript(this, GetType(), "closeModal", 
                        "$('#modalAddChapter').modal('hide');", true);

                    cargarCurso(cursoId);
                }
            }
        }

        protected bool esInstructorDelCurso(int cursoId)
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

                // Si la cuenta es mayor que 0, el usuario es un instructor del curso
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        protected void btnConfirmarEliminarCap_Click(object sender, EventArgs e)
        {
            // Conseguir el ID del capítulo desde el campo oculto
            int chapterId = Convert.ToInt32(hfDeleteChapterId.Value);
            
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Primero, obtener el curso_id y numero_capitulo del capítulo a eliminar
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
                    // ELiminar el capítulo
                    string deleteQuery = "DELETE FROM capitulos WHERE id = @chapterId";
                    MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, connection);
                    deleteCmd.Parameters.AddWithValue("@chapterId", chapterId);
                    deleteCmd.ExecuteNonQuery();
                    
                    // Actualizar el número de capítulos para todos los capítulos con números más altos
                    string updateNumbersQuery = "UPDATE capitulos SET numero_capitulo = numero_capitulo - 1 " +
                                                "WHERE curso_id = @cursoId AND numero_capitulo > @deletedChapterNumber";
                    MySqlCommand updateNumbersCmd = new MySqlCommand(updateNumbersQuery, connection);
                    updateNumbersCmd.Parameters.AddWithValue("@cursoId", cursoId);
                    updateNumbersCmd.Parameters.AddWithValue("@deletedChapterNumber", deletedChapterNumber);
                    updateNumbersCmd.ExecuteNonQuery();
                    
                    // También actualiza cualquier registro de capítulos vistos que pueda referenciar números de capítulo más altos
                    string updateViewedQuery = "UPDATE capitulos_vistos cv " +
                                            "JOIN capitulos c ON cv.capitulo_id = c.id " +
                                            "SET c.numero_capitulo = c.numero_capitulo - 1 " +
                                            "WHERE c.curso_id = @cursoId AND c.numero_capitulo > @deletedChapterNumber";
                    MySqlCommand updateViewedCmd = new MySqlCommand(updateViewedQuery, connection);
                    updateViewedCmd.Parameters.AddWithValue("@cursoId", cursoId);
                    updateViewedCmd.Parameters.AddWithValue("@deletedChapterNumber", deletedChapterNumber);
                    updateViewedCmd.ExecuteNonQuery();
                    
                    // Recargar el curso
                    cargarCurso(cursoId);
                    
                    // Cerrar el modal
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideDeleteModal", 
                        "$('#modalDeleteChapter').modal('hide');", true);
                }
            }
        }

        protected void btnConfirmarEliminarCurso_Click(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
                return;

            string email = Session["Usuario"].ToString();
            long cursoId = Convert.ToInt64(hfCursoId.Value);

            // Verificar que el usuario sea el instructor del curso
            if (!esInstructorDelCurso(Convert.ToInt32(cursoId)))
                return;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // Usar una transacción para asegurar que todas las operaciones se completen o ninguna
                MySqlTransaction transaction = connection.BeginTransaction();
                
                try
                {
                    // 1. Eliminar los registros de capítulos vistos
                    string deleteCapVistos = "DELETE FROM capitulos_vistos WHERE curso_id = @cursoId";
                    MySqlCommand cmdCapVistos = new MySqlCommand(deleteCapVistos, connection, transaction);
                    cmdCapVistos.Parameters.AddWithValue("@cursoId", cursoId);
                    cmdCapVistos.ExecuteNonQuery();
                    
                    // 2. Eliminar los capítulos
                    string deleteCapitulos = "DELETE FROM capitulos WHERE curso_id = @cursoId";
                    MySqlCommand cmdCapitulos = new MySqlCommand(deleteCapitulos, connection, transaction);
                    cmdCapitulos.Parameters.AddWithValue("@cursoId", cursoId);
                    cmdCapitulos.ExecuteNonQuery();
                    
                    // 3. Eliminar las inscripciones
                    string deleteInscripciones = "DELETE FROM inscripciones WHERE curso_id = @cursoId";
                    MySqlCommand cmdInscripciones = new MySqlCommand(deleteInscripciones, connection, transaction);
                    cmdInscripciones.Parameters.AddWithValue("@cursoId", cursoId);
                    cmdInscripciones.ExecuteNonQuery();
                    
                    // 4. Eliminar el curso
                    string deleteCurso = "DELETE FROM cursos WHERE id = @cursoId";
                    MySqlCommand cmdCurso = new MySqlCommand(deleteCurso, connection, transaction);
                    cmdCurso.Parameters.AddWithValue("@cursoId", cursoId);
                    cmdCurso.ExecuteNonQuery();
                    
                    // Confirmar todas las operaciones
                    transaction.Commit();
                    
                    // Redirigir al catálogo
                    Response.Redirect("index.aspx?view=catalogo");
                }
                catch (Exception ex)
                {
                    // Si hay algún error, deshacer todas las operaciones
                    transaction.Rollback();
                    
                    // Mostrar mensaje de error (puedes personalizar esto)
                    ScriptManager.RegisterStartupScript(this, GetType(), "errorEliminarCurso", 
                        "alert('Error al eliminar el curso: " + ex.Message.Replace("'", "\\'") + "');", true);
                }
            }
        }
    }
}