using System;
using Avalonia.Controls;
using Bachelor.ViewModels;

namespace Bachelor.Views;
// author Jeppe and Clement
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var vm = new MainWindowViewModel(this);
        DataContext = vm;
    }
    
}