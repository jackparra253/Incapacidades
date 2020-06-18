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

}

function transformarTipoSalario(tipoSalario){
    if(tipoSalario == 1)
        return 'Ley 50';
    else if(tipoSalario == 2)
        return 'Integral';
}

function obtenerValorEmpleadoSeleccionado(){
    let select = document.getElementById("empleados");

    return select.options[select.selectedIndex].value;
}

function obtenerEmpleado(id){
    let empleado = empleados.filter(empleado => empleado.id == id);

    return empleado[0];
}