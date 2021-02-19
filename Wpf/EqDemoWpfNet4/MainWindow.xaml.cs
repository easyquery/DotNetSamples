
using System.Windows;

namespace EqDemo {

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _NavigationFrame.Navigate(new MainPage());
        }

    }
}
