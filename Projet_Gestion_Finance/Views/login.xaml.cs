using Projet_Gestion_Finance.Models;
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

namespace Projet_Gestion_Finance.Views
{
    /// <summary>
    /// Logique d'interaction pour login.xaml
    /// </summary>
    public partial class login : Window
    {
        //Dictionary<string, Utilisateur> _dicoUtilisateur = new Dictionary<string, Utilisateur>();
        //Dictionary<string, byte[]> _dicoSalts = new Dictionary<string, byte[]>();
        private GestionFinance gestionFinance;

        public GestionFinance GestionFinance
        {
            get { return gestionFinance; }
            set { gestionFinance = value; }
        }

        public login(GestionFinance gestionFinance)
        {
            InitializeComponent();
            GestionFinance = gestionFinance;
            
        }
        /// <summary>
        /// Permet à l'utilisateur d'ouvrir le formulaire pour créer un compre si il n'en a pas un
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //public bool VerifierChamps()
        //{
        //    //bool estValide = true;
        //    //if (string.IsNullOrWhiteSpace(txtIdconn.Text))
        //    //{
        //    //    txtMsgErrid.Text = "Le nom ne peut pas être vide";
        //    //    estValide = false;
        //    //}
        //    //else if ()
        //    //if (string.IsNullOrWhiteSpace(txtMdp.Password))
        //    //{
        //    //    return "Le mot de passe ne peut pas être vide";
        //    //}
        //    //return null;
        //}
    }
}
