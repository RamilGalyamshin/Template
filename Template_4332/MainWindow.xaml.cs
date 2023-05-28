using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Template_4332
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void Galyamshin_4332_Click(object sender, RoutedEventArgs e) 
        {
            _4332_Galyamshin ramil = new _4332_Galyamshin();
            ramil.Show();
        }
        private void Mukhamadiyarov_4332_Click(Object sender, RoutedEventArgs e)
        {
            _4332_Mukhamadiyarov denies = new _4332_Mukhamadiyarov();
            denies.Show();
        }
    }
}
