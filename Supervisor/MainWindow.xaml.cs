using System;
using System.Windows;
using Supervisor.ViewModel;

namespace Supervisor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }

        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            (this.DataContext as MainWindowViewModel)?.SaveSubjectList();
        }
    }
}
