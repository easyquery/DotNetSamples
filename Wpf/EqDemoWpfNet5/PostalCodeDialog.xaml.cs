using System.Collections.Generic;
using System.Windows;

namespace EqDemo
{
    /// <summary>
    /// Interaction logic for PostalCodeDialog.xaml
    /// </summary>
    public partial class PostalCodeDialog : Window
    {
        public PostalCodeDialog() {
            InitializeComponent();
            DataContext = this;
            listView.SelectedIndex = 0;
        }


        public  IList<District> Districts {
            get {
                return _districts;
            }
        }
            
        private IList<District> _districts =  new List<District> {
            new District("District 1", "10001"),
            new District("District 2", "10002"),
            new District("District 3", "10003"),
            new District("District 4", "10004"),
            new District("District 5", "10005"),
            new District("District 6", "10006"),
            new District("District 7", "10007"),
            new District("District 8", "10008")
        };

        public District SelectedDistrict {
            get {
                return listView.SelectedItem as District;
            }
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }
    }


    public class District {
        public string Name { get; set; }
        public string Code { get; set; }

        public District(string name, string code) {
            this.Name = name;
            this.Code = code;
        }

    }
}
