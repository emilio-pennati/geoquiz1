using CommunityToolkit.Mvvm.ComponentModel;

namespace GeoQuiz.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "Search";
}