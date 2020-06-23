const uriIncapacidad = 'incapacidad';

function guardar() {

    const solicitudIncapacidad = crearSolicitudIncapacidad();

    fetch(`${uriIncapacidad}`, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(solicitudIncapacidad)
      }).then(response => response.json())
      .then(data => console.log(data))
      .catch(error => console.error('Unable to add incapacidad.', error));
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

    if(fechaIncial == '' || fechaIncial == undefined)
        return 0;

    return parseInt(fechaIncial.substring(0, 4));
}

function obtenerMes() {
    const fechaIncial = obtenerFecha();

    if(fechaIncial == '' || fechaIncial == undefined)
        return 0;

    return parseInt(fechaIncial.substring(5, 7));
}

function obtenerDia() {
    const fechaIncial = obtenerFecha();

    if(fechaIncial == '' || fechaIncial == undefined)
        return 0;

    return parseInt(fechaIncial.substring(8, 10));
}

function obtenerCantidadDias() {
    const cantidadDias = document.getElementById('cantidadDias');

    if(cantidadDias.value == '' || cantidadDias.value == undefined)
        return 0;
    
    return parseInt(cantidadDias.value);
}

function obtenerObservaciones() {
    const observaciones = document.getElementById('observaciones');

    return observaciones.value;
}

