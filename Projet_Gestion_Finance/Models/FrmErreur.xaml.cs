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
    /// 
    public partial class FrmErreur : Window
    {
        public enum EtatErreur
        {
            Erreur,
            Avertissement,
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public EtatErreur _etat;
        public EtatErreur Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }
        public FrmErreur(string message, EtatErreur etat)
        {
            InitializeComponent();
            Message = message;
            Etat = etat;
        }
        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            if(Etat == EtatErreur.Avertissement)
            {
                lblTitre.Foreground = Brushes.Orange;
                lblTitre.Text = "Avertissement";
            }
            lblErreur.Text = Message;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;   
        }
        private void Button_Annuler_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        
    }
    }
