using Microsoft.Extensions.Logging;
using GeoQuiz.ViewModels;
using GeoQuiz.Views;
using GeoQuiz.Services;

namespace GeoQuiz;

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
			})
			.Services
			.AddSingleton<ICountryService, CountryService>()
			.AddSingleton<IQuizService, QuizService>()
			.AddTransient<SearchViewModel>()
			.AddTransient<SearchPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}