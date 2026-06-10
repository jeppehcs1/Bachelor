using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;

namespace Bachelor.ViewModels;
// author Jeppe
public partial class LogViewModel : ViewModelBase
{
    public ObservableCollection<string> LogMessages { get; } = new();

    public LogViewModel()
    {
        WeakReferenceMessenger.Default.Register<string>(this, (r, m) =>
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                LogMessages.Add($"[{DateTime.Now:HH:mm:ss}] {m}"));
        });
    }
}