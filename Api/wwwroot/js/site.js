const uriEmpleado = 'Empleado';
const empleados = [];

function llenarSelectEmpleados() {
    obtenerEmpleados();
}

function obtenerEmpleados() {
    fetch(uriEmpleado)
        .then(response => response.json())
        .then(data => agregarEmpleados(data))
        .catch(err => console.log("Error" + err));
}

function agregarEmpleados(data) {
    data.forEach(element => empleados.push(element));
    let select = document.getElementById('empleados');

    for (var i = 0; i < empleados.length; i++) {
        var option = document.createElement("option");
        option.value = empleados[i].id;
        option.text = `${empleados[i].nombres} ${empleados[i].apellidos}`;
        select.appendChild(option);
    }

    mostrarDetalleEmpleado();
}

function mostrarDetalleEmpleado() {
    const idEmpleado = obtenerValorEmpleadoSeleccionado();
    const empleado = obtenerEmpleado(idEmpleado);

    let empleadoNombres = document.getElementById('empleadoNombres');
    empleadoNombres.innerText = `${empleado.nombres} ${empleado.apellidos}`;

    let empleadoSalario = document.getElementById('empleadoSalario');
    empleadoSalario.innerText = `Tipo salario: ${transformarTipoSalario(empleado.tipoSalario)} - Salario: ${empleado.salario.cantidad} ${empleado.salario.moneda} - Salario diario: ${empleado.salarioDiario.cantidad} ${empleado.salarioDiario.moneda}  `;

    empleadoTipoSalario = document.getElementById('empleadoTipoSalario');
    empleadoTipoSalario.innerText = ``;

    consultarIncapacidades();
    consultarReconocimientosEconomicos();

}

function transformarTipoSalario(tipoSalario) {
    if (tipoSalario == 1)
        return 'Ley 50';
    else if (tipoSalario == 2)
        return 'Integral';
}

function obtenerValorEmpleadoSeleccionado() {
    let select = document.getElementById("empleados");

    return select.options[select.selectedIndex].value;
}

function obtenerEmpleado(id) {
    let empleado = empleados.filter(empleado => empleado.id == id);

    return empleado[0];
}


//Registrar incapacidad
function guardar() {

    const solicitudIncapacidad = crearSolicitudIncapacidad();

    const uriGuardarIncapacidades = construirUriIncapacidadPorTipo();
    console.log(uriGuardarIncapacidades);

    fetch('incapacidadenfermedadgeneralsalarioley50', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(solicitudIncapacidad)
    }).then(response => response.json())
        .then(data => console.log(data))
        .then(limpiarFormulario())
        .then(consultarIncapacidades())
        .then(consultarReconocimientosEconomicos())
        .catch(error => console.error('Unable to add incapacidad.', error));
}


function construirUriIncapacidadPorTipo() {
    const tipoSalario = empleados[0].tipoSalario.tipo;
    const idTipoIncapacidad = obtenerValorTipoIncapacidadSeleccionado();

    return 'incapacidadley50';
}

function crearSolicitudIncapacidad() {
    return {
        idEmpleado: obtenerValorEmpleadoSeleccionado(),
        tipoIncapacidad: obtenerValorTipoIncapacidadSeleccionado(),
        anio: obtenerAnio(),
        mes: obtenerMes(),
        dia: obtenerDia(),
        cantidadDias: obtenerCantidadDias(),
        observaciones: obtenerObservaciones()
    }
}

function obtenerValorEmpleadoSeleccionado() {
    const empleados = document.getElementById("empleados");

    return parseInt(empleados.options[empleados.selectedIndex].value);
}

function obtenerValorTipoIncapacidadSeleccionado() {
    const tiposIncapacidad = document.getElementById('tipoIncapacidad');

    return parseInt(tiposIncapacidad.options[tiposIncapacidad.selectedIndex].value);
}

function obtenerFecha() {
    const fechaIncial = document.getElementById('fechaInicial');

    return fechaIncial.value;
}

function obtenerAnio() {
    const fechaIncial = obtenerFecha();

    if (fechaIncial == '' || fechaIncial == undefined)
        return 0;

    return parseInt(fechaIncial.substring(0, 4));
}

function obtenerMes() {
    const fechaIncial = obtenerFecha();

    if (fechaIncial == '' || fechaIncial == undefined)
        return 0;

    return parseInt(fechaIncial.substring(5, 7));
}

function obtenerDia() {
    const fechaIncial = obtenerFecha();

    if (fechaIncial == '' || fechaIncial == undefined)
        return 0;

    return parseInt(fechaIncial.substring(8, 10));
}

function obtenerCantidadDias() {
    const cantidadDias = document.getElementById('cantidadDias');

    if (cantidadDias.value == '' || cantidadDias.value == undefined)
        return 0;

    return parseInt(cantidadDias.value);
}

function obtenerObservaciones() {
    const observaciones = document.getElementById('observaciones');

    return observaciones.value;
}

function limpiarFormulario() {
    const fechaIncial = document.getElementById('fechaInicial')
    const cantidadDias = document.getElementById('cantidadDias');
    const fechaFinal = document.getElementById('fechaFinal');
    const observaciones = document.getElementById('observaciones');

    fechaIncial.value = '';
    fechaFinal.innerText = '';
    cantidadDias.value = '';
    observaciones.value = '';
    alert("Se guardo la incapacidad");
}

//Consultar incapacidad
function consultarIncapacidades() {
    const uriIncapacidadConsulta = 'incapacidadconsulta';

    const empleados = document.getElementById("empleados");

    let idEmpleado = 1;

    if (empleados.options.length != 0)
        idEmpleado = empleados.options[empleados.selectedIndex].value;

    fetch(`${uriIncapacidadConsulta}/${idEmpleado}`)
        .then(response => response.json())
        .then(data => llenarTablaIncapacidades(data))
        .catch(err => console.log("Error" + err.message));
}


function llenarTablaIncapacidades(incapacidades) {
    const cuerpoTabla = document.getElementById('tabla-detalle-incapacidad');

    if (cuerpoTabla.rows.length > 0) {
        for (let i = 0; i < incapacidades.length; i++) {
            cuerpoTabla.innerHTML = '';
        }
    }

    for (const item of incapacidades) {
        let trElemento = document.createElement('tr');

        let idTdElemento = document.createElement('td');
        idTdElemento.innerHTML = item.id;
        trElemento.appendChild(idTdElemento);

        let tipoTdElemento = document.createElement('td');
        tipoTdElemento.innerHTML = item.tipo;
        trElemento.appendChild(tipoTdElemento);

        let fechaInicialTdElemento = document.createElement('td');
        fechaInicialTdElemento.innerHTML = item.fechaInicial;
        trElemento.appendChild(fechaInicialTdElemento);

        let fechaFinalTdElemento = document.createElement('td');
        fechaFinalTdElemento.innerHTML = item.fechaFinal;
        trElemento.appendChild(fechaFinalTdElemento);

        let cantidadDiasTdElemento = document.createElement('td');
        cantidadDiasTdElemento.innerHTML = item.cantidadDias;
        trElemento.appendChild(cantidadDiasTdElemento);

        let prorrogaTdElemento = document.createElement('td');
        let prorrogaBotonElemento = document.createElement('button');
        prorrogaTdElemento.appendChild(prorrogaBotonElemento);
        prorrogaBotonElemento.id = item.id;
        prorrogaBotonElemento.className = 'btn btn-outline-dark btn-sm';
        prorrogaBotonElemento.innerText = 'Prórroga';
        trElemento.appendChild(prorrogaTdElemento);

        cuerpoTabla.appendChild(trElemento);
    }

}

consultarIncapacidades();


const uriReconocimientoEconomico = 'reconocimientoEconomico';

//Consultar Reconocimiento Economico
function consultarReconocimientosEconomicos() {
    let empleados = document.getElementById("empleados");

    let idEmpleado = 1;

    if (empleados.options.length != 0)
        idEmpleado = empleados.options[empleados.selectedIndex].value;

    fetch(`${uriReconocimientoEconomico}/${idEmpleado}`)
        .then(response => response.json())
        .then(data => llenarTablaReconocimientosEconomicos(data))
        .catch(err => console.log("Error" + err.message));
}

function llenarTablaReconocimientosEconomicos(reconocimientosEconomicos) {
    const cuerpoTabla = document.getElementById('tabla-detalle-reconocimiento');

    if (cuerpoTabla.rows.length > 0) {
        for (let i = 0; i < reconocimientosEconomicos.length; i++) {
            cuerpoTabla.innerHTML = '';
        }
    }

    for (const item of reconocimientosEconomicos) {
        let trElemento = document.createElement('tr');

        let idIncapacidadTdElemento = document.createElement('td');
        idIncapacidadTdElemento.innerHTML = item.idIncapacidad;
        trElemento.appendChild(idIncapacidadTdElemento);

        let fechaInicialTdElemento = document.createElement('td');
        fechaInicialTdElemento.innerHTML = item.fechaInicial;
        trElemento.appendChild(fechaInicialTdElemento);

        let fechaFinalTdElemento = document.createElement('td');
        fechaFinalTdElemento.innerHTML = item.fechaFinal;
        trElemento.appendChild(fechaFinalTdElemento);

        let valorAPagarTdElemento = document.createElement('td');
        valorAPagarTdElemento.innerHTML = `${item.valorAPagar.cantidad} ${item.valorAPagar.moneda}`;
        trElemento.appendChild(valorAPagarTdElemento);

        let responsablePagoTdElemento = document.createElement('td');
        responsablePagoTdElemento.innerHTML = item.responsablePago;
        trElemento.appendChild(responsablePagoTdElemento);

        cuerpoTabla.appendChild(trElemento);
    }
}

consultarReconocimientosEconomicos();
