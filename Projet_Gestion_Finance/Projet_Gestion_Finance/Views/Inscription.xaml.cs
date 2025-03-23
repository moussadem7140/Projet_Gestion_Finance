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
    /// Logique d'interaction pour Inscription.xaml
    /// </summary>
    public partial class Inscription : Window
    {
        public Inscription()
        {
            InitializeComponent();
        }

        private void btnInscription_Click(object sender, RoutedEventArgs e)
        {
            if (ValiderChamps())
            {
                string nom = txtNomIns.Text.Trim();
                string prenom = txtPrenomIns.Text.Trim();
                string mail = txtMailIns.Text.Trim();
                string mdp = txtMdpIns.Password.Trim();
                string id = nom.Substring(0, 3).Trim().ToUpper() + prenom.Substring(0, 3).Trim().ToUpper();

                byte[] salt = Classes.Utils.CreerSALT();
                Utilisateur utilisateur = new Utilisateur(nom, prenom, id, Classes.Utils.HacherMotDePasse(mdp, salt), mail,salt);
                try
                {
                    Dal.AjouterUtilisateur(utilisateur);
                    
                }
                catch (Exception ex)
                {
                    FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Connexion);
                    f.ShowDialog();
                }
            }
        }

        public bool ValiderChamps()
        {
            // Réinitialisation des messages d'erreur
            txtNomInsError.Text = string.Empty;
            txtPrenomInsError.Text = string.Empty;
            txtMailInsError.Text = string.Empty;
            txtMdpInsError.Text = string.Empty;
            txtMdpConfInsError.Text = string.Empty;
            bool estValide = true;
            if (string.IsNullOrWhiteSpace(txtNomIns.Text))
            {
                txtNomInsError.Text = "Le nom ne peut pas être vide";
                estValide = false;
            }
            else if(txtNomIns.Text.Length < 2)
            {
                txtNomInsError.Text = "Le nom doit contenir au moins 2 caractères";
                estValide = false;
            }
            if (string.IsNullOrWhiteSpace(txtPrenomIns.Text))
            {
                txtPrenomInsError.Text = "Le prénom ne peut pas être vide";
                estValide = false;
            }
            else if (txtPrenomIns.Text.Length < 2)
            {
                txtPrenomInsError.Text = "Le prénom doit contenir au moins 2 caractères";
                estValide = false;
            }
            if (string.IsNullOrWhiteSpace(txtMailIns.Text))
            {
                txtMailInsError.Text = "Le mail ne peut pas être vide";
                estValide = false;
            }
            else if (!txtMailIns.Text.Contains("@") || !txtMailIns.Text.Contains("."))
            {
                txtMailInsError.Text = "Le mail doit être valide";
                estValide = false;
            }
            if (string.IsNullOrWhiteSpace(txtMdpIns.Password))
            {
                txtMdpInsError.Text = "Le mot de passe ne peut pas être vide";
                estValide = false;
            }
            else if(txtMdpIns.Password != txtMdpConfIns.Password)
            {
                txtMdpConfInsError.Text = "Les mots de passe ne correspondent pas";
                estValide = false;
            }
            return estValide;
        }

        private void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login Login = new Login();
            Login.ShowDialog();
            this.Close();
        }
    }
}
