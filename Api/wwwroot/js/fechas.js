const RUTA_CALCULAR_FECHAS = 'CalcularFechas';

function obtenerFechaFinal() {
    const fechaInicial = fechaInicialSeleccionada();
    const cantidadDias = cantidadDiasSeleccionada();

    if (fechaInicial.anio === 0 || cantidadDias === 0)
        return;

    pedirJson(construirRutaDeCalculoDeFechas(fechaInicial, cantidadDias))
        .then(mostrarFechaFinal)
        .catch(mostrarError);
}

function construirRutaDeCalculoDeFechas(fechaInicial, cantidadDias) {
    return `${RUTA_CALCULAR_FECHAS}/?anio=${fechaInicial.anio}`
        + `&mes=${fechaInicial.mes}`
        + `&dia=${fechaInicial.dia}`
        + `&cantidadDias=${cantidadDias}`;
}

function mostrarFechaFinal(fechaFinal) {
    document.getElementById('fechaFinal').textContent = fechaFinal.substring(0, 10);
}
