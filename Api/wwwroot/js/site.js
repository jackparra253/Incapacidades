const RUTA_EMPLEADOS = 'Empleado';
const RUTA_INCAPACIDADES = 'Incapacidad';
const RUTA_CONSULTA_INCAPACIDADES = 'IncapacidadConsulta';
const RUTA_RECONOCIMIENTOS = 'ReconocimientoEconomico';

const TIPOS_DE_SALARIO = { 1: 'Ley 50', 2: 'Integral' };

let empleados = [];

function iniciar() {
    pedirJson(RUTA_EMPLEADOS)
        .then(guardarEmpleados)
        .then(llenarSelectDeEmpleados)
        .then(mostrarDetalleEmpleado)
        .catch(mostrarError);
}

function pedirJson(ruta) {
    return fetch(ruta)
        .then(exigirRespuestaExitosa)
        .then(respuesta => respuesta.json());
}

function exigirRespuestaExitosa(respuesta) {
    if (!respuesta.ok)
        throw new Error(`${respuesta.status} ${respuesta.statusText}`);

    return respuesta;
}

function guardarEmpleados(empleadosRecibidos) {
    empleados = empleadosRecibidos;
}

function llenarSelectDeEmpleados() {
    const select = document.getElementById('empleados');
    select.innerHTML = '';

    empleados.forEach(empleado => {
        const opcion = document.createElement('option');
        opcion.value = empleado.id;
        opcion.textContent = `${empleado.nombres} ${empleado.apellidos}`;
        select.appendChild(opcion);
    });
}

function empleadoSeleccionado() {
    const id = idEmpleadoSeleccionado();

    return empleados.find(empleado => empleado.id === id);
}

function idEmpleadoSeleccionado() {
    const select = document.getElementById('empleados');

    if (select.options.length === 0)
        return 0;

    return parseInt(select.options[select.selectedIndex].value);
}

function mostrarDetalleEmpleado() {
    const empleado = empleadoSeleccionado();

    if (empleado === undefined)
        return Promise.resolve();

    document.getElementById('empleadoNombres').textContent = `${empleado.nombres} ${empleado.apellidos}`;
    document.getElementById('empleadoSalario').textContent = descripcionDelSalario(empleado);

    return refrescarTablas();
}

function descripcionDelSalario(empleado) {
    const tipo = TIPOS_DE_SALARIO[empleado.tipoSalario.tipo];

    return `Tipo salario: ${tipo}`
        + ` - Salario: ${formatearDinero(empleado.salario)}`
        + ` - Salario diario: ${formatearDinero(empleado.salarioDiario)}`;
}

function formatearDinero(dinero) {
    return `${dinero.cantidad} ${dinero.moneda}`;
}

function guardar() {
    enviarSolicitudIncapacidad(construirSolicitudIncapacidad())
        .then(limpiarFormulario)
        .then(refrescarTablas)
        .then(() => mostrarMensaje('Se guardó la incapacidad'))
        .catch(mostrarError);
}

function enviarSolicitudIncapacidad(solicitudIncapacidad) {
    return fetch(RUTA_INCAPACIDADES, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(solicitudIncapacidad)
    }).then(exigirRespuestaExitosa);
}

function construirSolicitudIncapacidad() {
    const fechaInicial = fechaInicialSeleccionada();

    return {
        idEmpleado: idEmpleadoSeleccionado(),
        tipoIncapacidad: tipoIncapacidadSeleccionado(),
        anio: fechaInicial.anio,
        mes: fechaInicial.mes,
        dia: fechaInicial.dia,
        cantidadDias: cantidadDiasSeleccionada(),
        observaciones: document.getElementById('observaciones').value
    };
}

function tipoIncapacidadSeleccionado() {
    const select = document.getElementById('tipoIncapacidad');

    return parseInt(select.options[select.selectedIndex].value);
}

function fechaInicialSeleccionada() {
    const valor = document.getElementById('fechaInicial').value;

    if (!valor)
        return { anio: 0, mes: 0, dia: 0 };

    return {
        anio: parseInt(valor.substring(0, 4)),
        mes: parseInt(valor.substring(5, 7)),
        dia: parseInt(valor.substring(8, 10))
    };
}

function cantidadDiasSeleccionada() {
    const valor = document.getElementById('cantidadDias').value;

    if (!valor)
        return 0;

    return parseInt(valor);
}

function limpiarFormulario() {
    document.getElementById('fechaInicial').value = '';
    document.getElementById('fechaFinal').textContent = '';
    document.getElementById('cantidadDias').value = '';
    document.getElementById('observaciones').value = '';
}

function refrescarTablas() {
    const idEmpleado = idEmpleadoSeleccionado();

    return Promise.all([
        consultarIncapacidades(idEmpleado),
        consultarReconocimientosEconomicos(idEmpleado)
    ]);
}

function consultarIncapacidades(idEmpleado) {
    return pedirJson(`${RUTA_CONSULTA_INCAPACIDADES}/${idEmpleado}`)
        .then(incapacidades => llenarTabla('tabla-detalle-incapacidad', incapacidades, filaDeIncapacidad));
}

function filaDeIncapacidad(incapacidad) {
    return [
        incapacidad.id,
        incapacidad.tipo,
        incapacidad.fechaInicial,
        incapacidad.fechaFinal,
        incapacidad.cantidadDias,
        formatearDinero(incapacidad.totalAPagar),
        botonDeProrroga(incapacidad.id)
    ];
}

function botonDeProrroga(idIncapacidad) {
    const boton = document.createElement('button');
    boton.id = idIncapacidad;
    boton.className = 'btn btn-outline-dark btn-sm';
    boton.textContent = 'Prórroga';

    return boton;
}

function consultarReconocimientosEconomicos(idEmpleado) {
    return pedirJson(`${RUTA_RECONOCIMIENTOS}/${idEmpleado}`)
        .then(reconocimientos => llenarTabla('tabla-detalle-reconocimiento', reconocimientos, filaDeReconocimiento));
}

function filaDeReconocimiento(reconocimiento) {
    return [
        reconocimiento.idIncapacidad,
        reconocimiento.fechaInicial,
        reconocimiento.fechaFinal,
        formatearDinero(reconocimiento.valorAPagar),
        reconocimiento.responsablePago
    ];
}

function llenarTabla(idCuerpoTabla, filas, construirColumnas) {
    const cuerpoTabla = document.getElementById(idCuerpoTabla);
    cuerpoTabla.innerHTML = '';

    filas.forEach(fila => cuerpoTabla.appendChild(crearFila(construirColumnas(fila))));
}

function crearFila(columnas) {
    const tr = document.createElement('tr');

    columnas.forEach(columna => {
        const td = document.createElement('td');

        if (columna instanceof Node)
            td.appendChild(columna);
        else
            td.textContent = columna;

        tr.appendChild(td);
    });

    return tr;
}

function mostrarMensaje(texto) {
    const mensaje = document.getElementById('mensaje');
    mensaje.className = 'alert alert-success';
    mensaje.textContent = texto;
}

function mostrarError(error) {
    const mensaje = document.getElementById('mensaje');
    mensaje.className = 'alert alert-danger';
    mensaje.textContent = `No se pudo completar la operación: ${error.message}`;
}
