using GeoQuiz.ViewModels;

namespace GeoQuiz.Views;

public partial class CountryDetailPage : ContentPage
{
    public CountryDetailPage(CountryDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
        if (BindingContext is CountryDetailViewModel viewModel)
        {
            try
            {
                var uri = Shell.Current?.CurrentState?.Location;
                if (uri != null)
                {
                    var fullPath = uri.OriginalString;
                    var codeIndex = fullPath.IndexOf("code=");
                    if (codeIndex >= 0)
                    {
                        var codeStart = codeIndex + 5;
                        var code = fullPath.Substring(codeStart);
                        var ampIndex = code.IndexOf('&');
                        if (ampIndex > 0) code = code.Substring(0, ampIndex);
                        if (!string.IsNullOrEmpty(code))
                        {
                            _ = viewModel.LoadCountryAsync(code);
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}