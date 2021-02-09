#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTTT.Ejemplo.Linq.Data.Entity;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Collections;
using UTTT.Ejemplo.Persona.Control;
using UTTT.Ejemplo.Persona.Control.Ctrl;
using System.Text.RegularExpressions;

#endregion

namespace UTTT.Ejemplo.Persona
{
    public partial class PersonaManager : System.Web.UI.Page
    {
        #region Variables

        private SessionManager session = new SessionManager();
        private int idPersona = 0;
        private UTTT.Ejemplo.Linq.Data.Entity.Persona baseEntity;
        private DataContext dcGlobal = new DcGeneralDataContext();
        private int tipoAccion = 0;
        private Regex CorreExpresion = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        private Regex RfcExpresion = new Regex(@"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3})?$");


        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
           
            try
            {
                
                this.Response.Buffer = true;
                this.session = (SessionManager)this.Session["SessionManager"];
                this.idPersona = this.session.Parametros["idPersona"] != null ?
                    int.Parse(this.session.Parametros["idPersona"].ToString()) : 0;
                if (this.idPersona == 0)
                {
                    this.baseEntity = new Linq.Data.Entity.Persona();
                    this.tipoAccion = 1;
                }
                else
                {
                    this.baseEntity = dcGlobal.GetTable<Linq.Data.Entity.Persona>().First(c => c.id == this.idPersona);
                    this.tipoAccion = 2;
                }
//---------------------------------------------------------------------------------------
                if (!this.IsPostBack)
                {
                    if (this.session.Parametros["baseEntity"] == null)
                    {
                        this.session.Parametros.Add("baseEntity", this.baseEntity);
                    }
                    List<CatSexo> lista = dcGlobal.GetTable<CatSexo>().ToList();
                    CatSexo catTemp = new CatSexo();
                    catTemp.id = -1;
                    catTemp.strValor = "Seleccionar";
                    lista.Insert(0, catTemp);
                    this.ddlSexo.DataTextField = "strValor";
                    this.ddlSexo.DataValueField = "id";
                    this.ddlSexo.DataSource = lista;
                    this.ddlSexo.DataBind();

                    this.ddlSexo.SelectedIndexChanged += new EventHandler(ddlSexo_SelectedIndexChanged);
                    this.ddlSexo.AutoPostBack = true;
                    if (this.idPersona == 0)
                    {
                   
                        this.lblAccion.Text = "Agregar";
                        this.HiddenField1.Value = null;
                        DateTime date = new DateTime(2003, 1, 9);
                        this.Calendar1.TodaysDate = date;
                        this.Calendar1.SelectedDate = date;
                        
                    }
                    else
                    {
                       
                       
                        this.lblAccion.Text = "Editar";

                   
                        this.txtNombre.Text = this.baseEntity.strNombre;
                        this.txtAPaterno.Text = this.baseEntity.strAPaterno;
                        this.txtAMaterno.Text = this.baseEntity.strAMaterno;
                        this.txtClaveUnica.Text = this.baseEntity.strClaveUnica;                     
                        DateTime? nacimiento = this.baseEntity.fechaNacimiento;
                        this.TextBox1.Text = this.baseEntity.correo;
                        this.TextBox2.Text = this.baseEntity.rfc.Trim();
                        this.TextBox3.Text = this.baseEntity.codigoPostal.ToString();
                        if (nacimiento != null)
                        {
                            this.Calendar1.TodaysDate = (DateTime)nacimiento;
                            this.Calendar1.SelectedDate = (DateTime)nacimiento;
                            this.HiddenField1.Value = nacimiento.ToString();

                        }
                        else
                        {
                            DateTime time = new DateTime(2003, 1, 9);
                            this.Calendar1.TodaysDate = time;
                            this.Calendar1.SelectedDate = time;
                        }
                        this.setItemEdit(ref this.ddlSexo, baseEntity.CatSexo.strValor);

                    }
                }

            }
            catch (Exception _e)
            {
                this.showMessage("Ha ocurrido un problema al cargar la página");
                this.Response.Redirect("~/ErrorPage/colorlib-error-404-12/index.html", false);
            }

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {

            if (!IsValid)
            {
                return;
            }


            try
            {
                DataContext dcGuardar = new DcGeneralDataContext();
                UTTT.Ejemplo.Linq.Data.Entity.Persona persona = new Linq.Data.Entity.Persona();
                int i = 0;
                if (this.idPersona == 0)
                {
                    persona.strClaveUnica = this.txtClaveUnica.Text.Trim();
                    persona.strNombre = this.txtNombre.Text.Trim();
                    persona.strAMaterno = this.txtAMaterno.Text.Trim();
                    persona.strAPaterno = this.txtAPaterno.Text.Trim();
                    persona.idCatSexo = int.Parse(this.ddlSexo.Text);
                    persona.fechaNacimiento = this.Calendar1.SelectedDate.Date;
                    persona.correo = this.TextBox1.Text.Trim();
                    persona.codigoPostal = this.TextBox3.Text.Trim();
                    persona.rfc = this.TextBox2.Text.Trim();
                    
                    String mensaje = String.Empty;
                   
                    if (!this.validacion(persona, ref mensaje))
                    {
                        this.Label1.Text = mensaje;
                        this.Label1.Visible = true;
                        return;
                    }
                    if (!this.sqlInjectionValida(ref mensaje))
                    {
                        this.Label1.Text = mensaje;
                        this.Label1.Visible = true;
                        return;
                    }
                    if (!this.htmlInjectionValida(ref mensaje))
                    {
                        this.Label1.Text = mensaje;
                        this.Label1.Visible = true;
                        return;
                    }


                    dcGuardar.GetTable<UTTT.Ejemplo.Linq.Data.Entity.Persona>().InsertOnSubmit(persona);
                    dcGuardar.SubmitChanges();
                    this.showMessage("El registro se agrego correctamente.");
                    this.Response.Redirect("~/PersonaPrincipal.aspx", false);

                }
                if (this.idPersona > 0)
                {
                    persona = dcGuardar.GetTable<UTTT.Ejemplo.Linq.Data.Entity.Persona>().First(c => c.id == idPersona);
                    persona.strClaveUnica = this.txtClaveUnica.Text.Trim();
                    persona.strNombre = this.txtNombre.Text.Trim();
                    persona.strAMaterno = this.txtAMaterno.Text.Trim();
                    persona.strAPaterno = this.txtAPaterno.Text.Trim();
                    persona.idCatSexo = int.Parse(this.ddlSexo.Text);
                    persona.fechaNacimiento = this.Calendar1.SelectedDate.Date;
                    persona.correo = this.TextBox1.Text.Trim();
                    persona.rfc = this.TextBox2.Text.Trim();
                    persona.codigoPostal = this.TextBox3.Text.Trim();
                    String mensaje = String.Empty;
                    if (!this.validacion(persona, ref mensaje))
                    {
                        this.Label1.Text = mensaje;
                        this.Label1.Visible = true;
                        return;
                    }
                    if (!this.sqlInjectionValida(ref mensaje))
                    {
                        this.Label1.Text = mensaje;
                        this.Label1.Visible = true;
                        return;
                    }
                    if (!this.htmlInjectionValida(ref mensaje))
                    {
                        this.Label1.Text = mensaje;
                        this.Label1.Visible = true;
                        return;
                    }


                    dcGuardar.SubmitChanges();
                    this.showMessage("El registro se edito correctamente.");
                    this.Response.Redirect("~/PersonaPrincipal.aspx", false);
                }
            }
            catch (Exception _e)
            {
                CtrlEmail email = new CtrlEmail();

                email.sendEmail(_e.Message, "PersonaManager.aspx.cs", "error en btnAceptar_Click", _e.GetType().ToString());

                this.Response.Redirect("~/ErrorPage/colorlib-error-404-12/index.html", false);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Response.Redirect("~/PersonaPrincipal.aspx", false);
            }
            catch (Exception _e)
            {
                this.showMessage("Ha ocurrido un error inesperado");
            }
        }

        protected void ddlSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idSexo = int.Parse(this.ddlSexo.Text);
                Expression<Func<CatSexo, bool>> predicateSexo = c => c.id == idSexo;
                predicateSexo.Compile();
                List<CatSexo> lista = dcGlobal.GetTable<CatSexo>().Where(predicateSexo).ToList();
                CatSexo catTemp = new CatSexo();
                this.ddlSexo.DataTextField = "strValor";
                this.ddlSexo.DataValueField = "id";
                this.ddlSexo.DataSource = lista;
                this.ddlSexo.DataBind();
            }
            catch (Exception)
            {
                this.showMessage("Ha ocurrido un error inesperado");
            }
        }

        #endregion

        #region Metodos
        public void setItem(ref DropDownList _control, String _value)
        {
            foreach (ListItem item in _control.Items)
            {
                if (item.Value == _value)
                {
                    item.Selected = true;
                    break;
                }
            }
            _control.Items.FindByText(_value).Selected = true;
        }
        public void setItemEdit(ref DropDownList _control, String _value)
        {
            foreach (ListItem item in _control.Items)
            {
                if (item.Value != _value)
                {
                    item.Enabled = false;
                    break;
                }
            }
            _control.Items.FindByText(_value).Selected = true;
        }
        /// <summary>
        /// Validación datos
        /// </summary>
        /// <param name="_persona"></param>
        /// <param name="_mensaje"></param>
        /// <returns></returns>
        public bool validacion(UTTT.Ejemplo.Linq.Data.Entity.Persona _persona, ref String _mensaje)
        {
            int i = 0;
            if (_persona.idCatSexo < 0)
            {
                _mensaje = "Elige un sexo";
                return false;
            }
            if (_persona.strClaveUnica.Equals(String.Empty))
            {
                _mensaje = "Clave unica requerida";
                return false;
            }
            if (!int.TryParse(_persona.strClaveUnica, out i))
            {
                _mensaje = "Solo numeros en clave unica";
                return false;
            }
            if (int.Parse(_persona.strClaveUnica) < 100 || int.Parse(_persona.strClaveUnica) > 999)
            {
                _mensaje = "clave unica rango limitado";
                return false;
            }
            if (_persona.strClaveUnica.Length > 3 || _persona.strClaveUnica.Length < 3)
            {
                _mensaje = "clave única 3 caracteres";
                return false;
            }
            if (_persona.strNombre.Equals(String.Empty))
            {
                _mensaje = "Campo nombre vacío";
                return false;
            }
            if (_persona.strNombre.Length > 50)
            {
                _mensaje = "Nombre sobrepasa los caracteres";
                return false;
            }
            if (_persona.strAPaterno.Equals(String.Empty))
            {
                _mensaje = "Campo apellido paterno vacio";
                return false;
            }
            if (_persona.strAPaterno.Length > 50)
            {
                _mensaje = "Campo apellido paterno sobrepasa los caracteres";
                return false;
            }
            if (_persona.strAMaterno.Length > 50)
            {
                _mensaje = "La longitud de caracteres del campo apellido materno rebasa lo permitido.";
                return false;
            }

            if (!CorreExpresion.IsMatch(_persona.correo))
            {
                _mensaje = "El correo electrónico no es válido";
                return false;
            }
            if (_persona.correo.Length > 100)
            {
                _mensaje = "El correo electrónico no tiene el numero de caracteres permitidos.";
                return false;
            }
            if (_persona.codigoPostal.ToString().Length != 5)
            {
                _mensaje = "Codigo postal solo 5 caracteres";
                return false;
            }
            if (!this.RfcExpresion.IsMatch(_persona.rfc))
            {
                _mensaje = "El formato del campo RFC no es válido";
                return false;
            }
            if (_persona.rfc.Length > 13)
            {
                _mensaje = " campo RFC sobrepasa los caracteres permitidos.";
                return false;
            }
            if (_persona.fechaNacimiento.Equals(String.Empty))
            {
                _mensaje = "El campo fecha de nacimiento es requerido";
                return false;
            }
            var time = DateTime.Now - _persona.fechaNacimiento.Value.Date;
            if (time.Days < 6570)
            {
                _mensaje = "Solo mayores de edad";
                return false;
            }
            return true;
        }
        public bool sqlInjectionValida(ref String _mensaje)
        {
            CtrlValidaInjection valida = new CtrlValidaInjection();
            String _mensajeFuncion = String.Empty;
            if (valida.sqlInjectionValida(this.txtClaveUnica.Text.Trim(), ref _mensajeFuncion, "Clave Única", ref this.txtClaveUnica))
            {
                _mensaje = _mensajeFuncion;
                return false;
            }
            if (valida.sqlInjectionValida(txtNombre.Text.Trim(), ref _mensajeFuncion, "Nombre", ref this.txtNombre))
            {
                _mensaje = _mensajeFuncion;
                return false;
            }
            if (valida.sqlInjectionValida(this.txtAPaterno.Text.Trim(), ref _mensajeFuncion, "Apellido Paterno", ref this.txtAPaterno))
            {
                _mensaje = _mensajeFuncion;
                return false;
            }
            if (valida.sqlInjectionValida(this.txtAMaterno.Text.Trim(), ref _mensajeFuncion, "Apellido Materno", ref this.txtAMaterno))
            {
                _mensaje = _mensajeFuncion;
                return false;
            }

            if (valida.sqlInjectionValida(this.TextBox1.Text.Trim(), ref _mensajeFuncion, "Email", ref this.TextBox1))
            {
                _mensaje = _mensajeFuncion;
                return false;
            }
            if (valida.sqlInjectionValida(this.TextBox3.Text.Trim(), ref _mensajeFuncion, "Código Postal", ref this.TextBox3))
            {
                _mensaje = _mensajeFuncion;
                return false;
            }
            if (valida.sqlInjectionValida(this.TextBox2.Text.Trim(), ref _mensajeFuncion, "RFC", ref this.TextBox2))
            {
                _mensaje = _mensajeFuncion;
                return false;
            }
            return true;
        }

        public bool htmlInjectionValida(ref String _mensaje)
        {
            validaHtml valida = new validaHtml();
            String mensajeFuncion = String.Empty;
            if (valida.htmlInjectionValida(this.txtClaveUnica.Text.Trim(), ref mensajeFuncion, "Clave Única", ref this.txtClaveUnica))
            {
                _mensaje = mensajeFuncion;
                return false;
            }
            if (valida.htmlInjectionValida(this.txtNombre.Text.Trim(), ref mensajeFuncion, "Nombre", ref this.txtNombre))
            {
                _mensaje = mensajeFuncion;
                return false;
            }
            if (valida.htmlInjectionValida(this.txtAPaterno.Text.Trim(), ref mensajeFuncion, "Apellido Paterno", ref this.txtAPaterno))
            {
                _mensaje = mensajeFuncion;
                return false;
            }
            if (valida.htmlInjectionValida(this.txtAMaterno.Text.Trim(), ref mensajeFuncion, "Apellido Materno", ref this.txtAMaterno))
            {
                _mensaje = mensajeFuncion;
                return false;
            }

            if (valida.htmlInjectionValida(this.TextBox1.Text.Trim(), ref mensajeFuncion, "Email", ref this.TextBox1))
            {
                _mensaje = mensajeFuncion;
                return false;
            }
            if (valida.htmlInjectionValida(this.TextBox3.Text.Trim(), ref mensajeFuncion, "Código postal", ref this.TextBox3))
            {
                _mensaje = mensajeFuncion;
                return false;
            }
            if (valida.htmlInjectionValida(this.TextBox2.Text.Trim(), ref mensajeFuncion, "RFC", ref this.TextBox2))
            {
                _mensaje = mensajeFuncion;
                return false;
            }
            return true;
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            Page.ClientScript.GetPostBackEventReference(this, string.Empty);
            Calendar1.SelectedDate.ToString();
            this.HiddenField1.Value = this.Calendar1.SelectedDate.ToString();
           
        }
        protected void CustomValidator2_ServerValidate1(object source, ServerValidateEventArgs args)
        {
            int sexIndex = int.Parse(this.ddlSexo.SelectedValue);
            args.IsValid = sexIndex > 0;
        }
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.TextBox3.Text.Trim().Length == 5;
        }













        #endregion

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {

            DateTime date = new DateTime(2003, 1, 9);
            this.HiddenField1.Value = this.Calendar1.SelectedDate.ToString();
            string a =  date.ToString();
            if ( HiddenField1.Value == a)
            {
                args.IsValid = false;
            }

        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.txtClaveUnica.Text.Trim().Length == 3;
        }

        protected void CustomValidator5_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.txtNombre.Text.Trim().Length >= 3;
        }

        protected void CustomValidator8_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.txtAPaterno.Text.Trim().Length >= 3;
        }

        protected void CustomValidator9_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.txtAMaterno.Text.Trim().Length >= 3;
        }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
           

            //.Attributes.Add("href:", "javascript:_doPostBack('Calendar1', '" + "&" + e.Day.Date + " &" + "')");
            //oDate = Request.Form("__EVENTARGUMENT")
        }
    }
}