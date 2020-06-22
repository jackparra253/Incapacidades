const uriFechas = 'CalcularFechas';

function obtenerFechaFinal() {
    const fecha = llenarFecha();
    const cantidadDiasValor = obtenerValorCantidadDias();

    if (fecha.anio != 0 && fecha.mes != 0 && fecha.dia != 0 && cantidadDiasValor != 0)
        calcularFechas(fecha.anio, fecha.mes, fecha.dia, cantidadDiasValor);
}

function llenarFecha() {
    const fechaIncial = document.getElementById('fechaInicial');

    const fechaBase = fechaIncial.value;

    if (fechaBase == '' || fechaBase == undefined)
        return { anio: 0, mes: 0, dia: 0 }

    const anio = fechaBase.substring(0, 4);
    const mes = fechaBase.substring(5, 7);
    const dia = fechaBase.substring(8, 10);

    return { anio: anio, mes: mes, dia: dia }
}

function obtenerValorCantidadDias() {
    const cantidadDias = document.getElementById('cantidadDias');
    const cantidadDiasValor = cantidadDias.value

    if (cantidadDiasValor == '' || cantidadDiasValor == undefined)
        return 0;

    return cantidadDiasValor;
}

function calcularFechas(anio, mes, dia, cantidadDias) {
    fetch(construirUri(anio, mes, dia, cantidadDias))
        .then(response => response.json())
        .then(data => llenarFechaFinal(data))
        .catch(err => console.log("Error" + err.message));
}

function llenarFechaFinal(fechaFinal) {
    const fechaFinalInput = document.getElementById('fechaFinal');

    const fecha = fechaFinal.substring(0,10);

    fechaFinalInput.innerText = fecha;
}

function construirUri(anio, mes, dia, cantidadDias){
    return `${uriFechas}/?anio=${anio}&mes=${mes}&dia=${dia}&cantidadDias=${cantidadDias}`;
}