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
using System.Windows.Shapes;

namespace Projet_Gestion_Finance.Models
{
    /// <summary>
    /// Logique d'interaction pour FrmErreur.xaml
    /// </summary>
    public partial class FrmErreur : Window
    {
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public FrmErreur(string message)
        {
            InitializeComponent();
            Message = message;
        }
        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            lblErreur.Text = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;   
        }
    }
    }
