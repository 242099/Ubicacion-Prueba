using System.Text.Json;
using System.Text;

namespace UbicacionApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnActivarUbicacionClicked(object sender, EventArgs e)
    {
        try
        {
            // 1. Verificar y pedir permisos de GPS al usuario en tiempo de ejecución
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permiso denegado", "La aplicación necesita acceso a la ubicación para funcionar.", "OK");
                return;
            }

            // 2. Obtener la ubicación actual del dispositivo
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);

            if (location != null)
            {
                // 3. Pintar los datos en la tabla (Grid) en la parte inferior
                lblLatitud.Text = location.Latitude.ToString();
                lblLongitud.Text = location.Longitude.ToString();

                // 4. Enviar las coordenadas a tu Minimal API para que las guarde en SQL Server
                await EnviarCoordenadasASqlServer(location.Latitude, location.Longitude);
            }
            else
            {
                await DisplayAlert("Aviso", "No se pudo obtener la señal GPS.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task EnviarCoordenadasASqlServer(double latitud, double longitud)
    {
        var httpClient = new HttpClient();

        // Objeto que coincide con el modelo de la API
        var datosUbicacion = new
        {
            Latitud = latitud,
            Longitud = longitud
        };

        var json = JsonSerializer.Serialize(datosUbicacion);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // REEMPLAZA "TU_IP_AQUI" por tu IPv4 real (ej: 192.168.1.50) y mantén el puerto 5019
        string urlApi = "http://192.168.60.60:5019/api/ubicaciones";

        try
        {
            var response = await httpClient.PostAsync(urlApi, content);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Ubicación obtenida y guardada en SQL Server correctamente.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "La API respondió pero hubo un problema al guardar.", "OK");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Error de Conexión", "No se pudo conectar con la PC. Revisa tu IP y que la API esté encendida.", "OK");
        }
    }
}