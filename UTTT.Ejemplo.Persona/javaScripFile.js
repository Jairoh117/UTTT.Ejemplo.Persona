const txtClaveUnica = document.querySelector('#txtClaveUnica');
const ddlSexo = document.querySelector('#ddlSexo');
const txtNombre = document.querySelector('#txtNombre');
const txtAPaterno = document.querySelector('#txtAPaterno');
const txtAMaterno = document.querySelector('#txtAMaterno');
const dtFechaNacimiento = document.querySelector('#HiddenField1');
const errores = document.querySelector('.alert-light');
const erroresParent = document.querySelector('#errores').firstElementChild;
const txtEmail = document.querySelector('#TextBox1');
const txtCP = document.querySelector('#TextBox3');
const txtRFC = document.querySelector('#TextBox2');
const lblAction = document.querySelector('#lblAccion');
const emailRegex = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
const rfcRegex = /^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3})?$/;
let isValid = false;
let erroresArray = [];


function myFunction() {
    document.getElementById("demo").innerHTML = "My First JavaScript Function";
}

const closeAlert = (fromBtnAgregar = true) => {
    if (fromBtnAgregar) {
        erroresParent.classList.remove('visible');
    } else {
        erroresParent.classList.add('visible');
    }
}

function validate() {

    console.log("hola papu");
    if (validateForm()) {
        return true;
    }
    if (erroresArray.length > 0) {
        let erroresStr = '';
        erroresArray.forEach((errorObj) => {
            erroresStr += `
                <strong>Algo no esta correcto!</strong><br><strong> ${errorObj.message}</strong>
                <br><button type="button" class="btn btn-dark" onClick="closeAlert(false)">Entendido</button>
            `;
        });
        errores.innerHTML = erroresStr;
        closeAlert();
    }
    return false;
}


const validTemp = () => {
    const today = new Date();
    const strDate = dtFechaNacimiento.value.split(' ')[0].split('/');
    // development
    // const personDate = new Date(`${strDate[1]}/${strDate[0]}/${strDate[2]}`);
    // end
    const personDate = new Date(`${strDate[0]}/${strDate[1]}/${strDate[2]}`);
    const validEmail = emailRegex.test(txtEmail.value.toLowerCase());
    const validCP = /^([0-9])*$/.test(txtCP.value) && txtCP.value.length === 5;
    const validRFC = rfcRegex.test(txtRFC.value);
    return ((ddlSexo.value > 0 && txtClaveUnica.value.length > 0 && txtNombre.value.length > 0 && txtAPaterno.value.length > 0
        && dtFechaNacimiento.value.length > 0) && (/^([0-9])*$/.test(txtClaveUnica.value) && txtClaveUnica.value.length === 3)
        && (txtNombre.value.length >= 3 && txtNombre.value.length <= 15) && (txtAPaterno.value.length >= 3 && txtAPaterno.value.length <= 15)
        && (txtAMaterno.value.length >= 3 && txtAMaterno.value.length <= 15)&& ((today.getFullYear() - personDate.getFullYear()) >= 18))
        && validEmail && validCP && validRFC;
}

const validateForm = () => {
    erroresArray = [];
    const today = new Date();
    const strDate = dtFechaNacimiento.value.split(' ')[0].split('/');
    // dev enviroment
    // const personDate = new Date(`${strDate[1]}/${strDate[0]}/${strDate[2]}`);
    // end dev-e
    const personDate = new Date(`${strDate[0]}/${strDate[1]}/${strDate[2]}`);
    if (Number.parseInt(ddlSexo.value) < 0) {
        erroresArray.push({
         
            message: 'El campo sexo es requerido.'
        });
        return false;
    }
    if (txtClaveUnica.value.length === 0) {
        erroresArray.push({
           
            message: 'El campo clave única es requerido.'
        });
        return false;
    }
    if (txtNombre.value.length === 0) {
        erroresArray.push({
         
            message: 'El campo nombre es requerido.'
        });
        return false;
    }
    if (txtAPaterno.value.length === 0) {
        erroresArray.push({
           
            message: 'El campo apellido paterno es requerido.'
        });
        return false;
    }
    if (txtAMaterno.value.length === 0) {
        erroresArray.push({

            message: 'El campo apellido Materno es requerido.'
        });
        return false;
    }
    if (dtFechaNacimiento.value.length === 0) {
        erroresArray.push({
         
            message: 'La fecha de nacimiento es requerida.'
        });
        return false;
    }
    if (!(/^([0-9])*$/.test(txtClaveUnica.value)) && txtClaveUnica.value.length > 0) {
        erroresArray.push({
            
            message: 'La clave única debe ser un número'
        });
        return false;
    }
    if (txtClaveUnica.value.length !== 3) {
        erroresArray.push({
           
            message: 'La clave única debe tener una longitud de 3 caracteres.'
        });
        return false;
    }
    if (txtNombre.value.length <=3 ) {
        erroresArray.push({
            
            message: 'El campo nombre debe tener mas de 2 caracteres.'
        });
        return false;
    }
    if (txtAPaterno.value.length <= 3) {
        erroresArray.push({
           
            message: 'El campo Apellido paterno debe tener mas de 2 caracteres'
        });
        return false;
    }
    if (txtAMaterno.value.length <= 3) {
        erroresArray.push({
           
            message: 'El campo Apellido materno debe tener mas de 2 caracteres'
        });
        return false;
    }

    if (today.getFullYear() - personDate.getFullYear() < 18) {
        erroresArray.push({
         
            message: 'Solo mayores de Edad'
        });
        return false;
    }
    if (txtEmail.value.length === 0) {
        erroresArray.push({

            message: 'El campo Correo es requerido.'
        });
        return false;
    }
    if (txtRFC.value.length === 0) {
        erroresArray.push({

            message: 'El campo RFC es requerido.'
        });
        return false;
    }
    if (txtCP.value.length === 0) {
        erroresArray.push({

            message: 'El campo Codigo postal es requerido.'
        });
        return false;
    }
    if (!emailRegex.test(txtEmail.value.toLowerCase())) {
        erroresArray.push({
         
            message: 'El campo email no es válido.'
        });
        return false;
    }
    if (!(/^([0-9])*$/.test(txtCP.value.length > 0 ? txtCP.value : 'abcd'))) {
        erroresArray.push({
           
            message: 'El campo código postal debe ser número'
        });
        return false;
    }
    if (txtCP.value.length !== 5) {
        erroresArray.push({
          
            message: 'El campo código postal debe tener una longitud de 5 caracteres.'
        });
        return false;
    }
    if (!rfcRegex.test(txtRFC.value)) {
        erroresArray.push({
         
            message: 'El formato del RFC no es válido.'
        });
        return false;
    }
    return true;
}


txtRFC.addEventListener('keyup', function () {

    this.value = this.value.toUpperCase();
    if (rfcRegex.test(this.value)) {
        this.className = '';
        isValid = true;
    } else {
        this.className = 'error';
        isValid = false;
    }
});

txtCP.addEventListener('keyup', function () {

    if (/^([0-9])*$/.test(this.value) && this.value.length === 5) {
        this.className = '';
        isValid = true;
    } else {
        this.className = 'error';
        isValid = false;
    }
});

txtNombre.addEventListener('keyup', function () {
   
    if (this.value.length >= 3 && this.value.length <= 15) {
        this.className = '';
        isValid = true;
    } else {
        this.className = 'error';
        isValid = false;
    }
});

txtAPaterno.addEventListener('keyup', function () {
   
    if (this.value.length >= 3 && this.value.length <= 15) {
        this.className = '';
        isValid = true;
    } else {
        this.className = 'error';
        isValid = false;
    }
});



txtEmail.addEventListener('keyup', function () {
   
    if (emailRegex.test(this.value.toLowerCase())) {
        this.className = '';
        isValid = true;
    } else {
        this.className = 'error';
        isValid = false;
    }
});



txtClaveUnica.addEventListener('keyup', function () {

    if (/^([0-9])*$/.test(this.value) && this.value.length === 3) {
        this.className = '';
        isValid = true;
    } else {
        this.className = 'error';
        isValid = false;
    }
});