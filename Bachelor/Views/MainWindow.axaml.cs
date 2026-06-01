using System;
using Avalonia.Controls;
using Bachelor.ViewModels;

namespace Bachelor.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var vm = new MainWindowViewModel(this);
        DataContext = vm;
    }
    
}