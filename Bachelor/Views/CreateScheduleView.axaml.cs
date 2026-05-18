using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Bachelor.ViewModels;

namespace Bachelor.Views;

public partial class CreateScheduleView : UserControl
{
    public CreateScheduleView()
    {
        InitializeComponent();
    }
    private async void BrowseFileOnClick(object sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open TSP File",
            AllowMultiple = false
        });
        
        if (files.Count >= 1 && DataContext is CreateScheduleViewModel vm)
            vm.FilePath = files[0].Path.LocalPath;
    }
}