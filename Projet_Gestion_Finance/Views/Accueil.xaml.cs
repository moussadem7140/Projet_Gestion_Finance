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
using Projet_Gestion_Finance.Classes;
using Projet_Gestion_Finance.Models;

namespace Projet_Gestion_Finance.Views
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        private Utilisateur utilisateur;

        public Utilisateur Utilisateur
        {
            get { return utilisateur; }
            set { utilisateur = value; }
        }

        public Accueil(Utilisateur utilisateur)
        {
            Utilisateur = utilisateur;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void StackPanel_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void StackPanel_MouseDown_2(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Utilisateur;
            txtInitiales.Text = " " + Utilisateur.Prenom[0].ToString().ToUpper() + Utilisateur.Nom[0].ToString().ToUpper();
            //txtNom.Text = " " + Utilisateur.Nom.ToUpper() + " " + Utilisateur.Prenom.ToUpper();
            //txtRevenu.Text = "$" + " " + Utilisateur.Revenue.ToString();
        }
        /// <summary>
        /// Permet de gérer l'événement de clic sur l'image de déconnexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        /// <summary>
        /// Permet de gérer l'événement de clic sur l'image de modification de profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
