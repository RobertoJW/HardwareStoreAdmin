using Microsoft.Extensions.Logging;
using HardwareStoreAdmin.Servicios;

namespace HardwareStoreAdmin;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Material-Icons-Regular.ttf", "MaterialIcons");
				fonts.AddFont("merriweather.ttf", "PersonalizadoMerri");
            });

		// Sentencia fundamental para establecer conexion con la base de datos y realizar operaciones. 
		builder.Services.AddSingleton<ApiService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
