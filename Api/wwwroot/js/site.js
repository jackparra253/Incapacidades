const uri = 'WeatherForecast';

function get() {
    fetch(uri)
    .then(response => response.json())
    .then(data => _displayItems(data))
    .catch(error => console.error('Unable to get WeatherForecast.', error));
}