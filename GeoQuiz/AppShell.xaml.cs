using GeoQuiz.Views;

namespace GeoQuiz;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute("country-detail", typeof(CountryDetailPage));
	}
}
