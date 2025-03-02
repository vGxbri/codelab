<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="iniciar.aspx.cs" Inherits="PaginaCursos.iniciar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CodeLab</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="icon" href="imgs/logo16.png" type="image/png" />
    <link rel="stylesheet" href="css/styles-iniciar.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid vh-100">
            <div class="row h-100">
                <!-- Columna izquierda para la imagen -->
                <div class="col-md-6 d-flex" style="padding: 0;">
                    <img src="imgs/backLogin.jpg" alt="login-image" id="imgLogin" />
                    <div class="divLogo d-flex justify-content-center align-items-center">
                        <img src="imgs/logo-full.png" alt="logo-full" id="logo-full" class="" />
                    </div>
                </div>
                
                <!-- Columna derecha para el formulario -->
                <div class="col-md-6 d-flex align-items-center justify-content-center">
                    <div class="login-container">
                        <div class="volver" onclick="paginaPrincipal()">
                            <asp:LinkButton ID="redirectPrincipal" runat="server" OnClick="redirigirPagina" class="d-flex" Style="color: inherit; text-decoration: none;" CausesValidation="false">
                                <svg width="32" height="32" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="white" color="#000"><path d="M0 0h24v24H0z" fill="none"></path><path d="M20 11H7.83l5.59-5.59L12 4l-8 8 8 8 1.41-1.41L7.83 13H20v-2z"></path></svg>
                                <h4 style="margin: 1px 0 5px 5px;">Volver</h4>
                            </asp:LinkButton>
                        </div>

                        <div class="separador mb-3"></div>
                        
                        <asp:MultiView ID="mvAuth" runat="server" ActiveViewIndex="0">
                            <!-- Vista de Inicio de Sesión -->
                            <asp:View ID="vLogin" runat="server">
                                <h1 class="mb-4 titIniciar">Iniciar Sesión</h1>
                                
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtEmailLogin" runat="server" CssClass="form-control" placeholder="Correo Electrónico" />
                                    <label for="txtEmailLogin">Correo Electrónico</label>
                                    <asp:RequiredFieldValidator ID="rfvEmailLogin" runat="server" ControlToValidate="txtEmailLogin" ErrorMessage="El correo electrónico es obligatorio" CssClass="text-danger" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revEmailLogin" runat="server" ControlToValidate="txtEmailLogin" ErrorMessage="Formato de correo electrónico no válido" ValidationExpression="\w+([-+.'']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtPasswordLogin" runat="server" TextMode="Password" CssClass="form-control" placeholder="Contraseña" />
                                    <label for="txtPasswordLogin">Contraseña</label>
                                    <asp:RequiredFieldValidator ID="rfvPasswordLogin" runat="server" ControlToValidate="txtPasswordLogin" ErrorMessage="La contraseña es obligatoria" CssClass="text-danger" Display="Dynamic" />
                                    <button class="btn btn-outline-secondary position-absolute top-0 end-0 mt-2 me-2" type="button" onclick="togglePassword('txtPasswordLogin')">
                                        <i class="fas fa-eye"></i>
                                    </button>
                                </div>

                                <asp:label ID="labelErrorLogin" CssClass="d-flex mb-2 text-danger" runat="server" />
                                
                                <div class="mb-3">
                                    <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
                                </div>
                                <div class="text-center">
                                    <p>¿No tienes cuenta? 
                                        <asp:LinkButton ID="lnkShowRegister" runat="server" OnClick="lnkShowRegister_Click" CssClass="text-decoration-none registrate" CausesValidation="false">Regístrate</asp:LinkButton>
                                    </p>
                                </div>
                            </asp:View>

                            <!-- Vista de Registro -->
                            <asp:View ID="vRegister" runat="server">
                                <h2 class="mb-4 titIniciar">Crear cuenta</h2>
                                <div class="row">
                                    <div class="col mb-3">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Nombre" />
                                            <label for="txtNombre">Nombre</label>
                                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio" CssClass="text-danger" Display="Dynamic" />
                                        </div>
                                    </div>
                                    <div class="col mb-3">
                                        <div class="form-floating">
                                            <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control" placeholder="Apellidos" />
                                            <label for="txtApellidos">Apellidos</label>
                                            <asp:RequiredFieldValidator ID="rfvApellidos" runat="server" ControlToValidate="txtApellidos" ErrorMessage="Los apellidos son obligatorios." CssClass="text-danger" Display="Dynamic" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtEmailRegister" runat="server" CssClass="form-control" placeholder="Email" />
                                    <label for="txtEmailRegister">Email</label>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmailRegister" ErrorMessage="El email es obligatorio" CssClass="text-danger" Display="Dynamic" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailRegister" ErrorMessage="Formato de email no válido" ValidationExpression="\w+([-+.'']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger" Display="Dynamic" />
                                </div>
                                <div class="form-floating mb-3 position-relative">
                                    <asp:TextBox ID="txtPasswordRegister" runat="server" TextMode="Password" CssClass="form-control" placeholder="Contraseña" />
                                    <label for="txtPasswordRegister">Contraseña</label>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPasswordRegister" ErrorMessage="La contraseña es obligatoria" CssClass="text-danger" Display="Dynamic" />
                                    <button class="btn btn-outline-secondary position-absolute top-0 end-0 mt-2 me-2" type="button" onclick="togglePassword('txtPasswordRegister')">
                                        <i class="fas fa-eye"></i>
                                    </button>
                                </div>
                                <div class="form-floating mb-3 position-relative">
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirmar Contraseña" />
                                    <label for="txtConfirmPassword">Confirmar Contraseña</label>
                                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="La confirmación de la contraseña es obligatoria" CssClass="text-danger" Display="Dynamic" />
                                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPasswordRegister" ErrorMessage="Las contraseñas no coinciden" CssClass="text-danger" Display="Dynamic" />
                                    <button class="btn btn-outline-secondary position-absolute top-0 end-0 mt-2 me-2" type="button" onclick="togglePassword('txtConfirmPassword')">
                                        <i class="fas fa-eye"></i>
                                    </button>
                                </div>
                                <div class="mb-3">
                                    <asp:CheckBox ID="chkTerminos" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="<%= chkTerminos.ClientID %>">
                                        Acepto los términos y condiciones
                                    </label>
                                    <asp:CustomValidator ID="cvTerminos" runat="server" ErrorMessage="Debe aceptar los términos y condiciones" CssClass="text-danger" Display="Dynamic" OnServerValidate="ValidateTerms" />
                                </div>

                                <asp:label ID="labelErrorRegister" CssClass="d-flex mb-2 text-danger" runat="server" />

                                <div class="mb-3">
                                    <asp:Button ID="btnRegistro" runat="server" Text="Crear cuenta" CssClass="btn btn-primary w-100" OnClick="btnRegistro_Click" />
                                </div>
                                <div class="text-center">
                                    <p>¿Ya tienes cuenta? 
                                        <asp:LinkButton ID="lnkShowLogin" runat="server" OnClick="lnkShowLogin_Click" CssClass="text-decoration-none registrate" CausesValidation="false">Inicia sesión</asp:LinkButton>
                                    </p>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function togglePassword(controlId) {
            var x = document.getElementById(controlId);
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        }
    </script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>