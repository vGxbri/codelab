<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="PaginaCursos.index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>CodeLab</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
    <link rel="icon" href="imgs/logo16.png" type="image/png" />
    <link rel="stylesheet" href="css/styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <nav class="navbar navbar-expand-lg navbarBefore">
            <div class="container-fluid">
                <asp:ImageButton ID="imgLogo" runat="server" ImageUrl="imgs/logo-full.png" CssClass="imgLogo" OnClick="imgLogo_Click" />
                
                <!-- Botón para mostrar el menú (en tablets/móviles) -->
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarContent" aria-controls="navbarContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon">
                        <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30" fill="#ffffff" viewBox="0 0 16 16">
                            <path fill-rule="evenodd" d="M2.5 12a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5zm0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5z"/>
                        </svg>
                    </span>
                </button>
                
                <!-- Envolver la navegación en un div colapsable -->
                <div class="collapse navbar-collapse" id="navbarContent">
                    <ul class="navbar-nav ms-auto" id="navbarNotLogged" style="display: flex;" runat="server">
                        <li class="nav-item btn-sesion">
                            <asp:LinkButton ID="LinkButton1" runat="server" CssClass="nav-link" PostBackUrl="~/iniciar.aspx?view=login">Iniciar Sesión</asp:LinkButton>
                        </li>
                        <li class="nav-item btn-sesion">
                            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="nav-link unirme" PostBackUrl="~/iniciar.aspx?view=register">Unirme</asp:LinkButton>
                        </li>
                    </ul>

                    <ul class="navbar-nav ms-auto" id="navbarLogged" style="display: none;" runat="server">
                        <li class="nav-item btn-sesion">
                            <asp:LinkButton ID="btnCatalogo" runat="server" CssClass="nav-link" OnClick="btnCatalogo_Click">Catálogo</asp:LinkButton>
                        </li>
                        <li class="nav-item btn-sesion">
                            <asp:LinkButton ID="btnMisCursos" runat="server" CssClass="nav-link" OnClick="btnMisCursos_Click">Mis Cursos</asp:LinkButton>
                        </li>
                        <li class="nav-item btn-sesion">
                            <asp:LinkButton ID="btnPlanes" runat="server" CssClass="nav-link" OnClick="btnPlanes_Click">Planes</asp:LinkButton>
                        </li>
                        <li class="nav-item btn-sesion">
                            <asp:LinkButton ID="btnPerfil" runat="server" CssClass="nav-link" OnClick="btnPerfil_Click">Perfil</asp:LinkButton>
                        </li>
                        <li class="nav-item btn-sesion">
                            <asp:LinkButton ID="btnCerrarSesion" runat="server" CssClass="nav-link unirme" data-bs-toggle="modal" data-bs-target="#modalCerrarSesion">Cerrar sesión</asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
            <hr style="margin-top: 24px;">
        </nav>

        <!-- Contenedor principal -->
        <div class="container">
            <asp:MultiView ID="paginas" runat="server" ActiveViewIndex="0">
                <!-- Vista de la página principal -->
                <asp:View ID="pagPrincipal" runat="server">
                    <div class="divSlogan">
                        <h1 class="slogan">CodeLab, la web con los <b class="titDestacado">mejores cursos</b> de todo internet.</h1>
                    </div>
                    <div class="divSlogan">
                        <h3 class="fraseExotica">Creada por profesionales, para futuros profesionales.</h3>
                    </div>
                    <div class="divDatos row">
                        <div class="dato1 col-12 col-sm-4">
                            <h2 class="titDato">+5,000</h2>
                            <p class="txtDato">usuarios diarios</p>
                        </div>
                        <div class="dato2 col-12 col-sm-4">
                            <h2 class="titDato">+30</h2>
                            <p class="txtDato">profesores titulados</p>
                        </div>
                        <div class="dato3 col-12 col-sm-4">
                            <h2 class="titDato">+100</h2>
                            <p class="txtDato">cursos disponibles</p>
                        </div>
                    </div>
                    <br />
                    <div class="divCursosDisponibles">
                        <h2 class="dato1 text-center">Cursos disponibles</h2>
                        <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" viewBox="0 0 24 24" class="svgFlecha">
                            <a class="flechaCursos" href="#cursosDisponibles3"><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m4 11l8 3l8-3"/></a>
                        </svg>
                    </div>

                    <!-- TARJETAS DE CURSOS (6) -->
                    <asp:label ID="labelDebug" CssClass="d-flex justify-content-center mb-4" runat="server" ForeColor="White" />
                    <div class="row row-cols-1 row-cols-md-3 g-4 mb-4" id="cursosDisponibles3">
                        <asp:Repeater ID="rptCursos6" runat="server">
                            <ItemTemplate>
                                <div class="col">
                                    <div class="card h-100">
                                        <img src='<%# Eval("img") %>' class="card-img-top" alt="...">
                                        <div class="card-body">
                                            <h4 class="card-title"><%# Eval("titulo") %></h4>
                                            <p class="card-text"><%# Eval("descripcion") %></p>
                                            <hr>
                                            <p class="card-text nomInstructor">Instructor: <%# Eval("instructor") %></p>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="d-flex justify-content-center mt-4">
                        <asp:LinkButton ID="btnConocerMas" runat="server" CssClass="conocerMas" OnClick="btnConocerMas_Click">Conocer más</asp:LinkButton>
                    </div>

                </asp:View>
                
                <!-- Vista de la página del catálogo -->
                <asp:View ID="pagCatalogo" runat="server">
                    <div class="divCursosDisponibles">
                        <h2 class="tit1 text-center">Cursos disponibles</h2>
                        <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" viewBox="0 0 24 24" class="svgFlecha">
                            <a class="flechaCursos" href="#cursosDisponibles"><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m4 11l8 3l8-3"/></a>
                        </svg>
                    </div>

                    <!-- TARJETAS DE CURSOS (TODAS) -->
                    <asp:label ID="labelDebugCatalogo" CssClass="d-flex justify-content-center mb-4" runat="server" ForeColor="White" />
                    <div class="row row-cols-1 row-cols-md-3 g-4 mb-4" id="cursosDisponibles">
                        <asp:Repeater ID="rptCursos" runat="server" OnItemDataBound="rptCursos_ItemDataBound">
                            <ItemTemplate>
                                <div class="col">
                                    <div class="card h-100">
                                        <img src='<%# Eval("img") %>' class="card-img-top" alt="...">
                                        <div class="card-body d-flex flex-column h-100">
                                            <div class="d-flex mb-1">
                                                <%# Convert.ToBoolean(Eval("es_premium")) ? "<svg xmlns='http://www.w3.org/2000/svg' width='32' height='32' viewBox='0 0 24 24' class='svgEstrella'><path fill='currentColor' d='m12 14.95l2.775 2.1q.3.2.6.013t.175-.538L14.5 13.05l2.725-1.95q.3-.225.175-.563t-.475-.337H13.6l-1.125-3.65Q12.35 6.2 12 6.2t-.475.35L10.4 10.2H7.075q-.35 0-.475.338t.175.562L9.5 13.05l-1.05 3.475q-.125.35.175.538t.6-.013zM12 22q-2.075 0-3.9-.788t-3.175-2.137T2.788 15.9T2 12t.788-3.9t2.137-3.175T8.1 2.788T12 2t3.9.788t3.175 2.137T21.213 8.1T22 12t-.788 3.9t-2.137 3.175t-3.175 2.138T12 22'/></svg>" : "" %>
                                                <h4 class="card-title"><%# Eval("titulo") %></h4>
                                            </div>
                                            <div class="flex-grow-1">
                                                <p class="card-text"><%# Eval("descripcion") %></p>
                                                <p class="card-text nomInstructor">Instructor: <%# Eval("instructor") %></p>
                                            </div>
                                            <div class="mt-auto">
                                                <div class="espaciadorGeneral espaciadorGeneralCatalogo"></div>
                                                <asp:HiddenField ID="hfCursoId" runat="server" Value='<%# Eval("id") %>' />
                                                <asp:Button ID="btnUnirme" OnClick="btnUnirme_Click" CssClass="btn btn-primary btnUnirme" runat="server" Text="Unirme" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:View>

                <!-- Vista de la página de mis cursos -->
                <asp:View ID="pagMisCursos" runat="server">
                    <div class="divCursosDisponibles">
                        <h2 class="tit1">Mis Cursos</h2>
                        <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" viewBox="0 0 24 24" class="svgFlecha">
                            <a class="flechaCursos" href="#misCursos"><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m4 11l8 3l8-3"/></a>
                        </svg>
                    </div>

                    <asp:label ID="labelDebugMisCursos" CssClass="d-flex justify-content-center mb-4" runat="server" ForeColor="White" />
                    <div class="row row-cols-1 row-cols-md-3 g-4 mb-4" id="misCursos">
                        <asp:Panel ID="divSinCursos" runat="server" CssClass="col-12 col-md-12 d-flex justify-content-center text-center">
                            <div class="w-100">
                                <h3 class="text-white mb-3">No tienes ningún curso guardado</h3>
                                <p class="text-muted mb-4">¡Descubre nuevos cursos y comienza tu viaje de aprendizaje!</p>
                                <div class="espaciadorGeneral espaciadorGeneralMisCursos"></div>
                                <asp:LinkButton ID="btnIrCatalogo" runat="server" CssClass="btn btn-primary btnUnirme" style="margin-top: 16px;" OnClick="btnCatalogo_Click" Text="Explorar catálogo"></asp:LinkButton>
                            </div>
                        </asp:Panel>

                        <asp:Repeater ID="rptMisCursos" runat="server">
                            <ItemTemplate>
                                <div class="col-md-6 col-lg-4">
                                    <div class="h-100 cardMisCursos">
                                        <div class="d-flex flex-column h-100">
                                            <div class="card-tit-miscursos">
                                                <div class="d-flex mb-1 align-items-center">
                                                    <%# Convert.ToBoolean(Eval("es_premium")) ? "<svg xmlns='http://www.w3.org/2000/svg' width='32' height='32' viewBox='0 0 24 24' class='svgEstrella'><path fill='currentColor' d='m12 14.95l2.775 2.1q.3.2.6.013t.175-.538L14.5 13.05l2.725-1.95q.3-.225.175-.563t-.475-.337H13.6l-1.125-3.65Q12.35 6.2 12 6.2t-.475.35L10.4 10.2H7.075q-.35 0-.475.338t.175.562L9.5 13.05l-1.05 3.475q-.125.35.175.538t.6-.013zM12 22q-2.075 0-3.9-.788t-3.175-2.137T2.788 15.9T2 12t.788-3.9t2.137-3.175T8.1 2.788T12 2t3.9.788t3.175 2.137T21.213 8.1T22 12t-.788 3.9t-2.137 3.175t-3.175 2.138T12 22'/></svg>" : "" %>
                                                    <h4 class="card-title"><%# Eval("titulo") %></h4>
                                                </div>
                                            </div>
                                            <div class="espaciadorGeneral espaciadorGeneralMisCursos"></div>
                                            <div class="card-body-miscursos flex-grow-1">
                                                <p class="card-text card-desc"><%# Eval("descripcion") %></p>
                                                <p class="card-text card-instructor negrita">Instructor: <%# Eval("nombre_instructor") %></p>
                                                
                                                <div class="progress mb-3" style="height: 8px;">
                                                    <div class="progress-bar" role="progressbar" 
                                                         style='<%# "width:" + (Convert.ToInt32(Eval("total_capitulos")) == 0 ? "0" : ((Convert.ToInt32(Eval("capitulos_vistos")) * 100.0) / Convert.ToInt32(Eval("total_capitulos"))).ToString("0")) + "%" %>' 
                                                         aria-valuenow='<%# Convert.ToInt32(Eval("total_capitulos")) == 0 ? 0 : ((Convert.ToInt32(Eval("capitulos_vistos")) * 100.0) / Convert.ToInt32(Eval("total_capitulos"))) %>' 
                                                         aria-valuemin="0" 
                                                         aria-valuemax="100">
                                                    </div>
                                                </div>
                                                <p class="text-muted" style="font-size: 0.9em;">
                                                    <%# (Convert.ToInt32(Eval("total_capitulos")) == 0 ? "0" : ((Convert.ToInt32(Eval("capitulos_vistos")) * 100.0) / Convert.ToInt32(Eval("total_capitulos"))).ToString("0")) %>% completado
                                                    (<%# Eval("capitulos_vistos") %>/<%# Eval("total_capitulos") %> capítulos)
                                                </p>
                                            </div>
                                            <div class="espaciadorGeneral espaciadorGeneralMisCursos mt-auto"></div>
                                            <div class="d-flex justify-content-between">
                                                <asp:Button ID="btnContinuar" OnClick="btnContinuar_Click" CssClass="btn btn-primary btnContinuar" runat="server" Text="Continuar curso" CommandArgument='<%# Eval("id") %>' />
                                                <asp:HiddenField ID="hfCursoId" runat="server" Value='<%# Eval("id") %>' />
                                                <asp:LinkButton ID="btnSalirCurso" OnClick="btnSalirCurso_Click" CssClass="btn btn-primary btnContinuar" runat="server" CommandArgument='<%# Eval("id") %>'>
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
                                                        <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                                        <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                                                    </svg>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <!-- Solo visto por instructores (crear curso) -->
                    <asp:Panel ID="pnlInstructorButton" runat="server" Visible="false">
                        <div class="espaciadorGeneral mb-4"></div>
                        <div class="d-flex justify-content-center mb-4">
                            <asp:LinkButton ID="btnCrearCurso" runat="server" CssClass="btn btn-primary btnUnirme" data-bs-toggle="modal" data-bs-target="#modalCrearCurso">Crear nuevo curso</asp:LinkButton>
                        </div>
                    </asp:Panel>
                </asp:View>

                <!-- Vista de la página de los planes -->
                <asp:View ID="pagPlanes" runat="server">
                    <div class="container mt-4">
                        <h1 class="tit1 text-center">Actualizar a Premium</h1>
                        <h4 class="text-center descPlanes">Aquí tienes una muestra de los planes que tenemos disponibles actualmente.</h4>

                        <div class="row justify-content-center gy-4 mt-4 mt-md-0">
                            <!-- Plan Básico -->
                            <div class="planes col-12 col-lg-5 mx-3" id="plan1">
                                <div class="divTitPlanes">
                                    <h4 class="titPlanes">Plan Básico</h4>
                                </div>
                                <hr>
                                <div class="divPrecioPlanes">
                                    <h2 class="precioPlanes">0,00€ <span style="font-size: 0.7em;"> / mes</span></h2>
                                </div>
                                <div class="divVentajasPlanes">
                                    <ul class="ulVentajasPlanes">
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Acceso a cursos gratuitos de nivel inicial.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Material de apoyo descargable.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Clases en video con opción de subtítulos.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Soporte técnico en la plataforma.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Actualizaciones constantes.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256"><path d="M205.66,194.34a8,8,0,0,1-11.32,11.32L128,139.31,61.66,205.66a8,8,0,0,1-11.32-11.32L116.69,128,50.34,61.66A8,8,0,0,1,61.66,50.34L128,116.69l66.34-66.35a8,8,0,0,1,11.32,11.32L139.31,128Z"></path></svg>
                                            <li class="ventajasPlanes">Acceso ilimitado a todos los cursos.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256"><path d="M205.66,194.34a8,8,0,0,1-11.32,11.32L128,139.31,61.66,205.66a8,8,0,0,1-11.32-11.32L116.69,128,50.34,61.66A8,8,0,0,1,61.66,50.34L128,116.69l66.34-66.35a8,8,0,0,1,11.32,11.32L139.31,128Z"></path></svg>
                                            <li class="ventajasPlanes">Sesiones de mentoría personalizada.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256"><path d="M205.66,194.34a8,8,0,0,1-11.32,11.32L128,139.31,61.66,205.66a8,8,0,0,1-11.32-11.32L116.69,128,50.34,61.66A8,8,0,0,1,61.66,50.34L128,116.69l66.34-66.35a8,8,0,0,1,11.32,11.32L139.31,128Z"></path></svg>
                                            <li class="ventajasPlanes">Certificaciones oficiales verificadas.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256"><path d="M205.66,194.34a8,8,0,0,1-11.32,11.32L128,139.31,61.66,205.66a8,8,0,0,1-11.32-11.32L116.69,128,50.34,61.66A8,8,0,0,1,61.66,50.34L128,116.69l66.34-66.35a8,8,0,0,1,11.32,11.32L139.31,128Z"></path></svg>
                                            <li class="ventajasPlanes">Modo sin conexión.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256"><path d="M205.66,194.34a8,8,0,0,1-11.32,11.32L128,139.31L61.66,205.66a8,8,0,0,1-11.32-11.32L116.69,128L50.34,61.66A8,8,0,0,1,61.66,50.34L128,116.69l66.34-66.35a8,8,0,0,1,11.32,11.32L139.31,128Z"></path></svg>
                                            <li class="ventajasPlanes">Acceso anticipado a nuevos contenidos.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256"><path d="M205.66,194.34a8,8,0,0,1-11.32,11.32L128,139.31L61.66,205.66a8,8,0,0,1-11.32-11.32L116.69,128L50.34,61.66A8,8,0,0,1,61.66,50.34L128,116.69l66.34-66.35a8,8,0,0,1,11.32,11.32L139.31,128Z"></path></svg>
                                            <li class="ventajasPlanes">Garantía de devolución de 7 días.</li>
                                        </div>
                                    </ul>
                                    <asp:Button ID="btnPlanGratuito" runat="server" CssClass="btn btn-outline-primary-mejorar" Text="Plan actual" Enabled="false" OnClick="btnPlanGratuito_Click"/>
                                </div>
                            </div>

                            <!-- Plan Premium -->
                            <div class="planes col-12 col-lg-5 mx-3" id="plan2">
                                <div class="divTitPlanes">
                                    <h4 class="titPlanes">Plan Premium</h4>
                                </div>
                                <hr>
                                <div class="divPrecioPlanes">
                                    <h2 class="precioPlanes">9,99€ <span style="font-size: 0.7em;"> / mes</span></h2>
                                </div>
                                <div class="divVentajasPlanes">
                                    <ul class="ulVentajasPlanes">
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Acceso a cursos gratuitos de nivel inicial.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Material de apoyo descargable.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Clases en video con opción de subtítulos.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Soporte técnico en la plataforma.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Actualizaciones constantes.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Acceso ilimitado a todos los cursos.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Sesiones de mentoría personalizada.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Certificaciones oficiales verificadas.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Modo sin conexión.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Acceso anticipado a nuevos contenidos.</li>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="#ffffff" viewBox="0 0 256 256">
                                                <path d="M229.66,77.66l-128,128a8,8,0,0,1-11.32,0l-56-56a8,8,0,0,1,11.32-11.32L96,188.69,218.34,66.34a8,8,0,0,1,11.32,11.32Z"></path>
                                            </svg>
                                            <li class="ventajasPlanes">Garantía de devolución de 7 días.</li>
                                        </div>
                                    </ul>
                                    <asp:LinkButton ID="btnPlanPremium" runat="server" CssClass="btn btn-primary btnMejorar" Text="Mejorar a premium" data-bs-toggle="modal" data-bs-target="#modalTarjeta" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>

                <!-- Vista de la página del perfil -->
                <asp:View ID="pagPerfil" runat="server">
                    <div class="container mt-4">
                        <h1 class="tit1 text-center">Datos Personales</h1>
                        
                        <div class="row mt-5">
                            <!-- Columna Izquierda -->
                            <div class="col-md-6">
                                <!-- Información del Perfil -->
                                <div class="planes p-4 mb-4">
                                    <h4 class="titPlanes mb-4">Información del Perfil</h4>
                                    <div class="espaciadorGeneral mb-4"></div>
                                    
                                    <p class="text-muted datos mb-2">Nombre: <asp:Label ID="lblNombre" runat="server" CssClass="negrita lbl" /></p>
                                    <p class="text-muted datos mb-2">Apellidos: <asp:Label ID="lblApellidos" runat="server" CssClass="negrita lbl" /></p>
                                    <p class="text-muted datos mb-2">Miembro desde: <asp:Label ID="lblFechaCreacion" runat="server" CssClass="negrita lbl" /></p>
                                    <p class="text-muted datos mb-2">Correo: <asp:Label ID="lblEmail" runat="server" CssClass="negrita lbl" /></p>
                                    <p class="text-muted datos mb-2">Cursos completados: <asp:Label ID="lblCursosCompletados" runat="server" CssClass="negrita lbl" /></p>
                                    <p class="text-muted datos mb-2">Plan Actual: <asp:Label ID="lblPlanActual" runat="server" CssClass="negrita lbl" /></p>
                                </div>

                                <!-- Historial de Cursos -->
                                <div class="planes p-4">
                                    <h4 class="titPlanes mb-4">Historial de Cursos</h4>
                                    <div class="espaciadorGeneral mb-4"></div>
                                
                                    <asp:Repeater ID="rptHistorialCursos" runat="server">
                                        <ItemTemplate>
                                            <div class="curso-historial mb-4">
                                                <div class="d-flex justify-content-between align-items-center">
                                                    <div>
                                                        <h5 class="mb-2"><%# Eval("titulo") %></h5>
                                                        <p class="text-muted mb-0">Unido: <%# Convert.ToDateTime(Eval("fecha_inscripcion")).ToString("dd/MM/yyyy") %></p>
                                                    </div>
                                                    <span class="badge <%# Convert.ToInt32(Eval("capitulos_vistos")) == Convert.ToInt32(Eval("total_capitulos")) ? "bg-success" : "bg-warning" %>">
                                                        <%# Convert.ToInt32(Eval("capitulos_vistos")) == Convert.ToInt32(Eval("total_capitulos")) ? "Completado" : "En progreso" %>
                                                    </span>
                                                </div>
                                                <div class="espaciadorGeneral mt-3"></div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>

                            <!-- Columna Derecha -->
                            <div class="col-md-6">
                                <!-- Cambiar Correo Electrónico -->
                                <div class="planes p-4 mb-4">
                                    <h4 class="titPlanes mb-4">Cambiar Correo Electrónico</h4>
                                    <div class="espaciadorGeneral mb-4"></div>

                                    <div class="form-floating mb-4">
                                        <asp:TextBox ID="txtEmailPerfil" runat="server" CssClass="form-control" placeholder="Correo Electrónico" ValidationGroup="EmailUpdate" />
                                        <label for="txtEmailPerfil">Nuevo Correo Electrónico</label>
                                        <asp:RequiredFieldValidator ID="rfvEmailPerfil" runat="server" 
                                            ControlToValidate="txtEmailPerfil" 
                                            ErrorMessage="El correo electrónico es obligatorio" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="EmailUpdate" />
                                        <asp:RegularExpressionValidator ID="revEmailPerfil" runat="server" 
                                            ControlToValidate="txtEmailPerfil" 
                                            ErrorMessage="Formato de correo electrónico no válido" 
                                            ValidationExpression="\w+([-+.'']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="EmailUpdate" />
                                    </div>
                                    
                                    <asp:Button ID="btnActualizarEmail" runat="server" 
                                        Text="Actualizar Correo" 
                                        CssClass="btn btn-primary btnMejorar" 
                                        OnClick="btnActualizarEmail_Click"
                                        ValidationGroup="EmailUpdate" />
                                </div>

                                <!-- Cambiar Contraseña -->
                                <div class="planes p-4">
                                    <h4 class="titPlanes mb-4">Cambiar Contraseña</h4>
                                    <div class="espaciadorGeneral mb-4"></div>

                                    <div class="form-floating mb-4">
                                        <asp:TextBox ID="txtPasswordActual" TextMode="Password" runat="server" CssClass="form-control" placeholder="Contraseña Actual" ValidationGroup="PasswordUpdate" />
                                        <label for="txtPasswordActual">Contraseña Actual</label>
                                        <asp:RequiredFieldValidator ID="rfvPasswordActual" runat="server" 
                                            ControlToValidate="txtPasswordActual" 
                                            ErrorMessage="La contraseña actual es obligatoria" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="PasswordUpdate" />
                                    </div>
                                    
                                    <div class="form-floating mb-4">
                                        <asp:TextBox ID="txtPasswordNueva" TextMode="Password" runat="server" CssClass="form-control" placeholder="Nueva Contraseña" ValidationGroup="PasswordUpdate" />
                                        <label for="txtPasswordNueva">Nueva Contraseña</label>
                                        <asp:RequiredFieldValidator ID="rfvPasswordNueva" runat="server" 
                                            ControlToValidate="txtPasswordNueva" 
                                            ErrorMessage="La nueva contraseña es obligatoria" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="PasswordUpdate" />
                                    </div>
                                    
                                    <div class="form-floating mb-4">
                                        <asp:TextBox ID="txtPasswordConfirm" TextMode="Password" runat="server" CssClass="form-control" placeholder="Confirmar Nueva Contraseña" ValidationGroup="PasswordUpdate" />
                                        <label for="txtPasswordConfirm">Confirmar Nueva Contraseña</label>
                                        <asp:RequiredFieldValidator ID="rfvPasswordConfirm" runat="server" 
                                            ControlToValidate="txtPasswordConfirm" 
                                            ErrorMessage="La confirmación de contraseña es obligatoria" 
                                            CssClass="text-danger" 
                                            Display="Dynamic"
                                            ValidationGroup="PasswordUpdate" />
                                        <asp:CompareValidator ID="cvPassword" runat="server" 
                                            ControlToValidate="txtPasswordConfirm"
                                            ControlToCompare="txtPasswordNueva"
                                            ErrorMessage="Las contraseñas no coinciden"
                                            CssClass="text-danger"
                                            Display="Dynamic"
                                            ValidationGroup="PasswordUpdate" />
                                    </div>
                                    
                                    <asp:Button ID="btnActualizarPassword" runat="server" 
                                        Text="Actualizar Contraseña" 
                                        CssClass="btn btn-primary btnMejorar" 
                                        OnClick="btnActualizarPassword_Click"
                                        ValidationGroup="PasswordUpdate" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>

                <!-- Vista de la página cuando entras un curso -->
                <asp:View ID="pagCursoIn" runat="server">
                    <asp:Label ID="labelDebugCursosIn" CssClass="d-flex justify-content-center mb-4" runat="server" ForeColor="White" />
                    <div class="container-fluid px-4">
                        <div class="row">
                            <!-- Columna Izquierda -->
                            <div class="col-lg-8 mb-4 mb-lg-0">
                                <div class="divLeftCursoIn">
                                    <asp:Repeater ID="rptCurso" runat="server">
                                        <ItemTemplate>
                                            <div class="imgCursoContainer mb-4">
                                                <asp:Image ID="imgCursoIn" runat="server" CssClass="imgCursoIn img-fluid w-100" ImageUrl='<%# Eval("img") %>' alt="Curso" />
                                                <button type="button" class="btnPlay" onclick="playVideo()">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="72" height="72" fill="#ffffff" class="bi bi-play-circle" viewBox="0 0 16 16">
                                                        <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM6.271 5.055a.5.5 0 0 0-.771.422v5.046a.5.5 0 0 0 .771.422l4.5-2.523a.5.5 0 0 0 0-.844l-4.5-2.523z"/>
                                                    </svg>
                                                </button>
                                            </div>
                                            <div class="d-flex flex-column flex-sm-row justify-content-between align-items-start align-items-sm-center gap-3 mb-4">
                                                <h2 class="capCursoIn mb-0"><asp:Label ID="lblCapCursoIn" runat="server" Text='<%# Eval("numero_capitulo") + ".<span> " + Eval("capitulo") + "</span>" %>' /></h2>
                                                <asp:LinkButton ID="btnMarcarVisto" OnClick="btnMarcarVisto_Click" CssClass="btn btn-primary btnMarcarVisto d-flex align-items-center" runat="server">
                                                    <asp:HiddenField ID="hfCapituloId" runat="server" Value='<%# Eval("capitulo_id") %>' />
                                                    <asp:HiddenField ID="hfCursoId" runat="server" Value='<%# Eval("curso_id") %>' />
                                                    <div class="d-flex align-items-center">
                                                        <svg runat="server" id="svgOjoAbierto" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="me-2" viewBox="0 0 16 16"><path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8M1.173 8a13.133 13.133 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13.133 13.133 0 0 1 14.828 8c-.058.087-.122.183-.195.288-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5c-2.12 0-3.879-1.168-5.168-2.457A13.134 13.134 0 0 1 1.172 8z"/><path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5M4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0"/></svg>
                                                        <svg runat="server" id="svgOjoCerrado" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="me-2" viewBox="0 0 16 16" style="display: none;"><path d="M13.359 11.238C15.06 9.72 16 8 16 8s-3-5.5-8-5.5a7.028 7.028 0 0 0-2.79.588l.77.771A5.944 5.944 0 0 1 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13.134 13.134 0 0 1 14.828 8c-.058.087-.122.183-.195.288-.335.48-.83 1.12-1.465 1.755-.165.165-.337.328-.517.486z"/><path d="M11.297 9.176a3.5 3.5 0 0 0-4.474-4.474l.823.823a2.5 2.5 0 0 1 2.829 2.829zm-2.943 1.299.822.822a3.5 3.5 0 0 1-4.474-4.474l.823.823a2.5 2.5 0 0 0 2.829 2.829"/><path d="M3.35 5.47c-.18.16-.353.322-.518.487A13.134 13.134 0 0 0 1.172 8l.195.288c.335.48.83 1.12 1.465 1.755C4.121 11.332 5.881 12.5 8 12.5c.716 0 1.39-.133 2.02-.36l.77.772A7.029 7.029 0 0 1 8 13.5C3 13.5 0 8 0 8s.939-1.721 2.641-3.238l.708.709zm10.296 8.884-12-12 .708-.708 12 12-.708.708"/></svg>
                                                        <asp:label ID="lblMarcarVisto" runat="server" Text="Marcar como visto" />
                                                    </div>
                                                </asp:LinkButton>
                                            </div>
                                            <div class="espaciadorGeneral mb-4"></div>
                                            <h1 class="titCursoIn mb-3"><%# Eval("titulo") %></h1>
                                            <h3 class="instructorCursoIn mb-3">Instructor: <span class="negrita"><%# Eval("instructor") %></span></h3>
                                            <p class="descCursoIn mb-3"><%# Eval("descripcion") %></p>
                                            <p class="fechaCursoIn mb-3"><%# Eval("fecha_creacion") %></p>
                                            <asp:LinkButton ID="btnEliminarCurso" runat="server" 
                                                            CssClass="btn btn-primary btnMarcarVisto" 
                                                            Visible='<%# esInstructorDelCurso(Convert.ToInt32(Eval("curso_id"))) %>'
                                                            data-bs-toggle="modal" 
                                                            data-bs-target="#modalEliminarCurso">
                                                <div class="d-flex align-items-center justify-content-center h-100">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="me-2" viewBox="0 0 16 16">
                                                        <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                                        <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                                                    </svg>
                                                    <span class="d-none d-sm-inline">Eliminar curso</span>
                                                </div>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>

                            <!-- Columna Derecha -->
                            <div class="col-lg-4">
                                <div class="divRightCursoIn">
                                    <asp:HiddenField ID="hfCursoId" runat="server" />
                                    <div class="list-group mb-4">
                                        <asp:Repeater ID="rptCapitulos" runat="server" OnItemCommand="rptCapitulos_ItemCommand">
                                            <ItemTemplate>
                                                <div class="d-flex align-items-center mb-2">
                                                    <asp:LinkButton ID="btnCapitulos" CssClass="capitulos d-flex align-items-center flex-grow-1 p-3" runat="server" CommandName="SelectCapitulo" CommandArgument='<%# Eval("numero_capitulo") + "|" + Eval("titulo") %>' Font-Underline="false">
                                                        <asp:HiddenField ID="hfCapituloId" runat="server" Value='<%# Eval("id") %>' />
                                                        <h2 class="numCapitulo mb-0 me-3"><%# Eval("numero_capitulo") %></h2>
                                                        <h3 class="nomCapitulo mb-0"><%# Eval("titulo") %></h3>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDeleteChapter" runat="server" 
                                                                    CssClass="btn btn-primary btnMarcarVisto btnDeleteChapter ms-2" 
                                                                    CommandName="mostrarModalEliminarCap"
                                                                    CommandArgument='<%# Eval("id") %>'
                                                                    Visible='<%# esInstructorDelCurso(Convert.ToInt32(Eval("curso_id"))) %>'>
                                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" viewBox="0 0 16 16">
                                                            <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/>
                                                        </svg>
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <asp:Panel ID="pnlAddChapter" CssClass="pnlAddChapter" runat="server">
                                        <asp:Button ID="btnAddChapterCourse" runat="server" 
                                                    CssClass="btn btn-primary btnMarcarVisto w-100"
                                                    Text="Añadir Capítulo"
                                                    OnClientClick="return false;"
                                                    data-bs-toggle="modal" 
                                                    data-bs-target="#modalAddChapter" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>  
                </asp:View>
            </asp:MultiView>
        </div>

        <!-- Pie de página -->
        <footer class="footer">
			<div class="container">
				<div class="row justify-content-center">
					<div class="col-md-12 text-center">
                        <div class="d-flex justify-content-center mb-4">
                            <asp:ImageButton ID="imgLogoFooter" runat="server" ImageUrl="imgs/logo-full.png" CssClass="imgLogoFooter" OnClick="imgLogo_Click" />
                        </div>
                        <hr />
                        <div class="d-flex justify-content-center">
                            <svg viewbox="0 0 512 512" preserveAspectRatio="xMidYMid meet" width="42" height="42" class="socialMedia-icons"><path d="M211.9 197.4h-36.7v59.9h36.7V433.1h70.5V256.5h49.2l5.2-59.1h-54.4c0 0 0-22.1 0-33.7 0-13.9 2.8-19.5 16.3-19.5 10.9 0 38.2 0 38.2 0V82.9c0 0-40.2 0-48.8 0 -52.5 0-76.1 23.1-76.1 67.3C211.9 188.8 211.9 197.4 211.9 197.4z"></path></svg>
                            <svg viewbox="0 0 512 512" preserveAspectRatio="xMidYMid meet" width="42" height="42" class="socialMedia-icons"><path d="M256 109.3c47.8 0 53.4 0.2 72.3 1 17.4 0.8 26.9 3.7 33.2 6.2 8.4 3.2 14.3 7.1 20.6 13.4 6.3 6.3 10.1 12.2 13.4 20.6 2.5 6.3 5.4 15.8 6.2 33.2 0.9 18.9 1 24.5 1 72.3s-0.2 53.4-1 72.3c-0.8 17.4-3.7 26.9-6.2 33.2 -3.2 8.4-7.1 14.3-13.4 20.6 -6.3 6.3-12.2 10.1-20.6 13.4 -6.3 2.5-15.8 5.4-33.2 6.2 -18.9 0.9-24.5 1-72.3 1s-53.4-0.2-72.3-1c-17.4-0.8-26.9-3.7-33.2-6.2 -8.4-3.2-14.3-7.1-20.6-13.4 -6.3-6.3-10.1-12.2-13.4-20.6 -2.5-6.3-5.4-15.8-6.2-33.2 -0.9-18.9-1-24.5-1-72.3s0.2-53.4 1-72.3c0.8-17.4 3.7-26.9 6.2-33.2 3.2-8.4 7.1-14.3 13.4-20.6 6.3-6.3 12.2-10.1 20.6-13.4 6.3-2.5 15.8-5.4 33.2-6.2C202.6 109.5 208.2 109.3 256 109.3M256 77.1c-48.6 0-54.7 0.2-73.8 1.1 -19 0.9-32.1 3.9-43.4 8.3 -11.8 4.6-21.7 10.7-31.7 20.6 -9.9 9.9-16.1 19.9-20.6 31.7 -4.4 11.4-7.4 24.4-8.3 43.4 -0.9 19.1-1.1 25.2-1.1 73.8 0 48.6 0.2 54.7 1.1 73.8 0.9 19 3.9 32.1 8.3 43.4 4.6 11.8 10.7 21.7 20.6 31.7 9.9 9.9 19.9 16.1 31.7 20.6 11.4 4.4 24.4 7.4 43.4 8.3 19.1 0.9 25.2 1.1 73.8 1.1s54.7-0.2 73.8-1.1c19-0.9 32.1-3.9 43.4-8.3 11.8-4.6 21.7-10.7 31.7-20.6 9.9-9.9 16.1-19.9 20.6-31.7 4.4-11.4 7.4-24.4 8.3-43.4 0.9-19.1 1.1-25.2 1.1-73.8s-0.2-54.7-1.1-73.8c-0.9-19-3.9-32.1-8.3-43.4 -4.6-11.8-10.7-21.7-20.6-31.7 -9.9-9.9-19.9-16.1-31.7-20.6 -11.4-4.4-24.4-7.4-43.4-8.3C310.7 77.3 304.6 77.1 256 77.1L256 77.1z"></path><path d="M256 164.1c-50.7 0-91.9 41.1-91.9 91.9s41.1 91.9 91.9 91.9 91.9-41.1 91.9-91.9S306.7 164.1 256 164.1zM256 315.6c-32.9 0-59.6-26.7-59.6-59.6s26.7-59.6 59.6-59.6 59.6 26.7 59.6 59.6S288.9 315.6 256 315.6z"></path><circle cx="351.5" cy="160.5" r="21.5"></circle></svg>
                            <svg viewbox="0 0 512 512" preserveAspectRatio="xMidYMid meet" width="42" height="42" class="socialMedia-icons"><path d="M186.4 142.4c0 19-15.3 34.5-34.2 34.5 -18.9 0-34.2-15.4-34.2-34.5 0-19 15.3-34.5 34.2-34.5C171.1 107.9 186.4 123.4 186.4 142.4zM181.4 201.3h-57.8V388.1h57.8V201.3zM273.8 201.3h-55.4V388.1h55.4c0 0 0-69.3 0-98 0-26.3 12.1-41.9 35.2-41.9 21.3 0 31.5 15 31.5 41.9 0 26.9 0 98 0 98h57.5c0 0 0-68.2 0-118.3 0-50-28.3-74.2-68-74.2 -39.6 0-56.3 30.9-56.3 30.9v-25.2H273.8z"></path></svg>
                            <svg viewbox="0 0 512 512" preserveAspectRatio="xMidYMid meet" width="42" height="42" class="socialMedia-icons"><path d="M419.6 168.6c-11.7 5.2-24.2 8.7-37.4 10.2 13.4-8.1 23.8-20.8 28.6-36 -12.6 7.5-26.5 12.9-41.3 15.8 -11.9-12.6-28.8-20.6-47.5-20.6 -42 0-72.9 39.2-63.4 79.9 -54.1-2.7-102.1-28.6-134.2-68 -17 29.2-8.8 67.5 20.1 86.9 -10.7-0.3-20.7-3.3-29.5-8.1 -0.7 30.2 20.9 58.4 52.2 64.6 -9.2 2.5-19.2 3.1-29.4 1.1 8.3 25.9 32.3 44.7 60.8 45.2 -27.4 21.4-61.8 31-96.4 27 28.8 18.5 63 29.2 99.8 29.2 120.8 0 189.1-102.1 185-193.6C399.9 193.1 410.9 181.7 419.6 168.6z"></path></svg>
                            <svg viewbox="0 0 512 512" preserveAspectRatio="xMidYMid meet" width="42" height="42" class="socialMedia-icons"><path d="M422.6 193.6c-5.3-45.3-23.3-51.6-59-54 -50.8-3.5-164.3-3.5-215.1 0 -35.7 2.4-53.7 8.7-59 54 -4 33.6-4 91.1 0 124.8 5.3 45.3 23.3 51.6 59 54 50.9 3.5 164.3 3.5 215.1 0 35.7-2.4 53.7-8.7 59-54C426.6 284.8 426.6 227.3 422.6 193.6zM222.2 303.4v-94.6l90.7 47.3L222.2 303.4z"></path></svg>
                        </div>
					</div>
				</div>
				<div class="row mt-4">
					<div class="col-md-12 text-center">
						<p class="copyright">
					  <b>Copyright &copy;<script>document.write(new Date().getFullYear());</script></b> Todos los derechos reservados | Página creada por <b>Gabriel Almarcha Martínez</b></a>
					</div>
				</div>
			</div>
		</footer>

   <!-- Modal Cerrar Sesión -->
   <div class="modal fade" id="modalCerrarSesion" tabindex="-1" aria-labelledby="titModalCS" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content mc-cerrarsesion">
                <div class="modal-body">¿Estás seguro de que quieres cerrar sesión?</div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-cancelar" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnCerrarSesion2" runat="server" CssClass="btn btn-danger btn-cerrarSesion" Text="Cerrar Sesión" OnClick="btnCerrarSesion2_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Crear Curso -->
    <div class="modal fade" id="modalCrearCurso" tabindex="-1" aria-labelledby="modalCrearCursoLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-xl">
            <div class="modal-content planes">
                <div class="modal-header border-0">
                    <h5 class="modal-title titPlanes" id="modalCrearCursoLabel">Crear Nuevo Curso</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="espaciadorGeneral mb-4"></div>
                <div class="modal-body">
                    <div class="row">
                        <!-- Columna Izquierda -->
                        <div class="col-md-6">
                            <div class="form-floating mb-4">
                                <asp:TextBox ID="txtTituloCurso" runat="server" CssClass="form-control inputDark" placeholder="Título" />
                                <label for="txtTituloCurso" class="text-muted">Título del curso</label>
                                <asp:RequiredFieldValidator ID="rfvTituloCurso" runat="server" 
                                    ControlToValidate="txtTituloCurso"
                                    ErrorMessage="El título es requerido" 
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="CrearCurso" />
                            </div>
                            
                            <div class="form-floating mb-4">
                                <asp:TextBox ID="txtDescripcionCurso" runat="server" CssClass="form-control inputDark" TextMode="MultiLine" Style="height: 100px" placeholder="Descripción" />
                                <label for="txtDescripcionCurso" class="text-muted">Descripción</label>
                                <asp:RequiredFieldValidator ID="rfvDescripcionCurso" runat="server" 
                                    ControlToValidate="txtDescripcionCurso"
                                    ErrorMessage="La descripción es requerida" 
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="CrearCurso" />
                            </div>

                            <div class="form-check mb-4">
                                <asp:CheckBox ID="chkCursoPremium" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label text-muted" for="chkCursoPremium">
                                    Curso Premium
                                </label>
                            </div>
                        
                            <div class="mb-4">
                                <label class="form-label text-muted">Imagen del curso</label>
                                <asp:FileUpload ID="fuImagenCurso" runat="server" CssClass="form-control inputDark" />
                                <asp:RequiredFieldValidator ID="rfvImagenCurso" runat="server" 
                                    ControlToValidate="fuImagenCurso"
                                    ErrorMessage="La imagen es requerida" 
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="CrearCurso" />
                            </div>
                        </div>
                        
                        <!-- Columna derecha -->
                        <div class="col-md-6">
                            <div id="chaptersContainer">
                                <!-- 3 capítulos iniciales -->
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtCapitulo1" runat="server" CssClass="form-control inputDark chapter-input" placeholder="Capítulo 1" />
                                    <label for="txtCapitulo1" class="text-muted">Capítulo 1</label>
                                    <asp:RequiredFieldValidator ID="rfvCapitulo1" runat="server" 
                                        ControlToValidate="txtCapitulo1"
                                        ErrorMessage="El título del capítulo es requerido" 
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ValidationGroup="CrearCurso" />
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtCapitulo2" runat="server" CssClass="form-control inputDark chapter-input" placeholder="Capítulo 2" />
                                    <label for="txtCapitulo2" class="text-muted">Capítulo 2</label>
                                    <asp:RequiredFieldValidator ID="rfvCapitulo2" runat="server" 
                                        ControlToValidate="txtCapitulo2"
                                        ErrorMessage="El título del capítulo es requerido" 
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ValidationGroup="CrearCurso" />
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtCapitulo3" runat="server" CssClass="form-control inputDark chapter-input" placeholder="Capítulo 3" />
                                    <label for="txtCapitulo3" class="text-muted">Capítulo 3</label>
                                    <asp:RequiredFieldValidator ID="rfvCapitulo3" runat="server" 
                                        ControlToValidate="txtCapitulo3"
                                        ErrorMessage="El título del capítulo es requerido" 
                                        CssClass="text-danger"
                                        Display="Dynamic"
                                        ValidationGroup="CrearCurso" />
                                </div>
                            </div>
                            <div class="d-flex justify-content-end">
                                <button type="button" id="btnAddChapter" class="btn btn-primary btnMejorar" onclick="addChapter(); return false;">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-plus-circle me-2" viewBox="0 0 16 16">
                                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16"/>
                                        <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4"/>
                                    </svg>
                                    Añadir capítulo
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer border-0">
                    <asp:Button ID="btnCrearCursoModal" runat="server" 
                        Text="Crear curso" 
                        CssClass="btn btn-primary btnMejorar" 
                        OnClick="btnCrearCursoModal_Click"
                        ValidationGroup="CrearCurso" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Tarjeta de Crédito -->
    <div class="modal fade" id="modalTarjeta" tabindex="-1" aria-labelledby="modalTarjetaLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content planes">
                <div class="modal-header border-0">
                    <h5 class="modal-title titPlanes" id="modalTarjetaLabel">Datos de pago</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="espaciadorGeneral mb-4"></div>
                <div class="modal-body modal-tarjetacredito">
                    <div class="form-floating mb-4">
                        <asp:TextBox ID="txtNumeroTarjeta" runat="server" CssClass="form-control inputDark" placeholder="Número de tarjeta" MaxLength="16" />
                        <label for="txtNumeroTarjeta" class="text-muted">Número de tarjeta</label>
                        <asp:RequiredFieldValidator ID="rfvNumeroTarjeta" runat="server" 
                            ControlToValidate="txtNumeroTarjeta"
                            ErrorMessage="El número de tarjeta es requerido" 
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="Tarjeta" />
                        <asp:RegularExpressionValidator ID="revNumeroTarjeta" runat="server" 
                            ControlToValidate="txtNumeroTarjeta"
                            ErrorMessage="Número de tarjeta inválido" 
                            ValidationExpression="^[0-9]{16}$"
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="Tarjeta" />
                    </div>
                    
                    <div class="row mb-4">
                        <div class="col-md-6 mb-4 mb-md-0">
                            <div class="form-floating">
                                <asp:TextBox ID="txtFechaExp" runat="server" CssClass="form-control inputDark" placeholder="MM/YY" MaxLength="5" Style="height: 58px;" />
                                <label for="txtFechaExp" class="text-muted">Fecha Exp. (MM/YY)</label>
                                <asp:RequiredFieldValidator ID="rfvFechaExp" runat="server" 
                                    ControlToValidate="txtFechaExp"
                                    ErrorMessage="La fecha de expiración es requerida" 
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="Tarjeta" />
                                <asp:RegularExpressionValidator ID="revFechaExp" runat="server" 
                                    ControlToValidate="txtFechaExp"
                                    ErrorMessage="Formato inválido (MM/YY)" 
                                    ValidationExpression="^(0[1-9]|1[0-2])\/([0-9]{2})$"
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="Tarjeta" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-floating">
                                <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control inputDark" placeholder="CVV" MaxLength="3" />
                                <label for="txtCVV" class="text-muted">CVV</label>
                                <asp:RequiredFieldValidator ID="rfvCVV" runat="server" 
                                    ControlToValidate="txtCVV"
                                    ErrorMessage="El CVV es requerido" 
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="Tarjeta" />
                                <asp:RegularExpressionValidator ID="revCVV" runat="server" 
                                    ControlToValidate="txtCVV"
                                    ErrorMessage="CVV inválido" 
                                    ValidationExpression="^[0-9]{3}$"
                                    CssClass="text-danger"
                                    Display="Dynamic"
                                    ValidationGroup="Tarjeta" />
                            </div>
                        </div>
                    </div>
                
                    <div class="form-floating mb-4">
                        <asp:TextBox ID="txtNombreTarjeta" runat="server" CssClass="form-control inputDark" placeholder="Nombre en la tarjeta" />
                        <label for="txtNombreTarjeta" class="text-muted">Nombre en la tarjeta</label>
                        <asp:RequiredFieldValidator ID="rfvNombreTarjeta" runat="server" 
                            ControlToValidate="txtNombreTarjeta"
                            ErrorMessage="El nombre es requerido" 
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="Tarjeta" />
                    </div>
                </div>
                <div class="modal-footer border-0">
                    <asp:Button ID="btnConfirmarTarjeta" runat="server" 
                        Text="Confirmar pago" 
                        CssClass="btn btn-primary btnMejorar" 
                        OnClick="btnConfirmarTarjeta_Click"
                        ValidationGroup="Tarjeta" />
                </div>
            </div>
        </div>
    </div>
        
    <!-- Modal Añadir Capítulo -->
    <div class="modal fade" id="modalAddChapter" tabindex="-1" aria-labelledby="modalAddChapterLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content planes">
                <div class="modal-header border-0">
                    <h5 class="modal-title titPlanes" id="modalAddChapterLabel">Añadir Capítulo</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="espaciadorGeneral mb-4"></div>
                <div class="modal-body">
                    <div class="form-floating mb-3">
                        <asp:TextBox ID="txtNewChapterTitle" runat="server" CssClass="form-control inputDark" placeholder="Título del capítulo" />
                        <label for="txtNewChapterTitle" class="text-muted">Título del capítulo</label>
                        <asp:RequiredFieldValidator ID="rfvNewChapterTitle" runat="server" 
                            ControlToValidate="txtNewChapterTitle"
                            ErrorMessage="El título del capítulo es requerido" 
                            CssClass="text-danger"
                            Display="Dynamic"
                            ValidationGroup="AddChapter" />
                    </div>
                </div>
                <div class="modal-footer border-0">
                    <asp:Button ID="btnGuardarNuevoCap" runat="server" 
                        Text="Añadir capítulo"
                        CssClass="btn btn-primary btnMejorar" 
                        OnClick="btnGuardarNuevoCap_Click"
                        ValidationGroup="AddChapter" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Eliminar Capítulo -->
    <div class="modal fade" id="modalDeleteChapter" tabindex="-1" aria-labelledby="modalDeleteChapterLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content mc-cerrarsesion">
                <div class="modal-body">
                    <span id="deleteChapterMessage">¿Estás seguro de que quieres eliminar este capítulo?</span>
                    <asp:HiddenField ID="hfDeleteChapterId" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-cancelar" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnConfirmarEliminarCap" runat="server" 
                                CssClass="btn btn-primary btnMejorar" 
                                Text="Eliminar" 
                                OnClick="btnConfirmarEliminarCap_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Eliminar Curso -->
    <div class="modal fade" id="modalEliminarCurso" tabindex="-1" aria-labelledby="modalEliminarCursoLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content mc-cerrarsesion">
                <div class="modal-body">¿Estás seguro de que quieres eliminar este curso? Esta acción eliminará todos los capítulos, inscripciones y no se puede deshacer.</div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary btn-cancelar" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnConfirmarEliminarCurso" runat="server" 
                                CssClass="btn btn-primary btnMejorar" 
                                Text="Eliminar Curso" 
                                OnClick="btnConfirmarEliminarCurso_Click" />
                </div>              
            </div>
        </div>
    </div>

    <!-- Notificación Toast -->
    <div class="toast-container position-fixed bottom-0 end-0 p-3">
        <div id="emailToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-header">
                <strong class="me-auto">CodeLab</strong>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body d-flex align-items-center">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-check-circle-fill me-2" viewBox="0 0 16 16">
                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                </svg>
                ¡Correo electrónico actualizado correctamente!
            </div>
        </div>
        <div id="passwordToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-header">
                <strong class="me-auto">CodeLab</strong>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body d-flex align-items-center">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-check-circle-fill me-2" viewBox="0 0 16 16">
                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0m-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                </svg>
                ¡Contraseña actualizada correctamente!
            </div>
        </div>
        <div id="passwordErrorToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-header">
                <strong class="me-auto">CodeLab</strong>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body d-flex align-items-center">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-exclamation-circle-fill me-2" viewBox="0 0 16 16">
                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M8 4a.905.905 0 0 0-.9.995l.35 3.507a.552.552 0 0 0 1.1 0l.35-3.507A.905.905 0 0 0 8 4m.002 6a1 1 0 1 0 0 2 1 1 0 0 0 0-2"/>
                </svg>
                La contraseña actual no es correcta
            </div>
        </div>
        <div id="emailExistsToast" class="toast" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="toast-header">
                <strong class="me-auto">CodeLab</strong>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body d-flex align-items-center">
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-exclamation-circle-fill me-2" viewBox="0 0 16 16">
                    <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0M8 4a.905.905 0 0 0-.9.995l.35 3.507a.552.552 0 0 0 1.1 0l.35-3.507A.905.905 0 0 0 8 4m.002 6a1 1 0 1 0 0 2 1 1 0 0 0 0-2"/>
                </svg>
                Este correo ya está registrado en otra cuenta
            </div>
        </div>
    </div>
    
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" crossorigin="anonymous"></script>

    <script type="text/javascript">
        let contCapitulos = 3;
        const MAX_CHAPTERS = 10;

        // Función para añadir capítulo en el formulario, ya que en c# daba problemas.
        // Esta función solo hace que se añada un capítulo en el formulario, no realmente.
        function addChapter() {
            if (contCapitulos >= MAX_CHAPTERS) {
                alert('Has alcanzado el límite máximo de capítulos (10)');
                return;
            }

            contCapitulos++;
            const container = document.getElementById('chaptersContainer');
            
            const div = document.createElement('div');
            div.className = 'form-floating mb-3 position-relative';
            div.innerHTML = `
                <input type="text" class="form-control inputDark chapter-input" 
                    id="chapter${contCapitulos}" name="chapter${contCapitulos}" 
                    placeholder="Capítulo ${contCapitulos}" />
                <label class="text-muted">Capítulo ${contCapitulos}</label>
                <span class="text-danger validation-message" style="display: none;">El título del capítulo es requerido</span>
                <button type="button" class="btn btn-link position-absolute top-50 end-0 translate-middle-y text-danger" 
                        style="padding: 0.375rem;" onclick="removeChapter(this)">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-x-circle" viewBox="0 0 16 16">
                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16"/>
                        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708"/>
                    </svg>
                </button>
            `;
            
            container.appendChild(div);

            const input = div.querySelector('.chapter-input');
            input.addEventListener('input', function() {
                this.nextElementSibling.nextElementSibling.style.display = 'none';
            });
        }

        // Actualizar el click del botón btnCrearCursoModal para validar todos los capítulos
        document.getElementById('<%= btnCrearCursoModal.ClientID %>').addEventListener('click', function(e) {
            const chapters = document.querySelectorAll('.chapter-input');
            let isValid = true;

            chapters.forEach(chapter => {
                if (!chapter.value.trim()) {
                    const validationMessage = chapter.parentElement.querySelector('.validation-message');
                    if (validationMessage) {
                        validationMessage.style.display = 'block';
                    }
                    isValid = false;
                }
            });

            if (!isValid) {
                e.preventDefault();
            }
        });
        
        // Función para eliminar capítulo en el formulario, ya que en c# daba problemas.
        // Esta función solo hace que se elimine un capítulo en el formulario, no realmente.
        function removeChapter(button) {
            button.closest('.form-floating').remove();
            contCapitulos--;
            
            // Renumerar capítulos restantes
            const chapters = document.querySelectorAll('.chapter-input');
            chapters.forEach((chapter, index) => {
                const label = chapter.nextElementSibling;
                label.textContent = `Capítulo ${index + 1}`;
            });
        }
    </script>
</body>
</html>
