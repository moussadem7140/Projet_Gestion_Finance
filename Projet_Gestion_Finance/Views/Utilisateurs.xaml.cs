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
using Projet_Gestion_Finance.Models;
namespace Projet_Gestion_Finance.Views
{
    /// <summary>
    /// Logique d'interaction pour Demarrages.xaml
    /// </summary>
    public partial class Utilisateurs : Window
    {
        public Utilisateur sUtilisateur;
        public Utilisateurs(Utilisateur u)
        {
            InitializeComponent();
            sUtilisateur= u;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNomIns.Text = sUtilisateur.Nom;
            txtPrenomIns.Text= sUtilisateur.Prenom;
            txtMailIns.Text= sUtilisateur.Mail;
            txtRevenuIns.Text = sUtilisateur.Revenue.ToString();
        }
        private void btnInscription_Click(object sender, RoutedEventArgs e)
        {
            if (ValiderChamps())
            {
                sUtilisateur.Nom = txtNomIns.Text.Trim();
                sUtilisateur.Prenom = txtPrenomIns.Text.Trim();
                sUtilisateur.Mail = txtMailIns.Text.Trim();
                sUtilisateur.MDP = Classes.Utils.HacherMotDePasse(txtMdpIns.Password.Trim(), sUtilisateur.Salt);
                sUtilisateur.Id = sUtilisateur.Nom.Substring(0, 3).Trim().ToUpper() + sUtilisateur.Prenom.Substring(0, 3).Trim().ToUpper();
                sUtilisateur.Revenue =decimal.Parse(txtRevenuIns.Text);
                //GestionFinance.Dicosalts.Add(id, salt);
                //GestionFinance.DicoUtilisateurs.Add(id, utilisateur);
                Dal.ModifierUtilisateur(sUtilisateur);
                DialogResult= true;
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
            else if (txtNomIns.Text.Length < 2)
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
            else if (txtMdpIns.Password != txtMdpConfIns.Password)
            {
                txtMdpConfInsError.Text = "Les mots de passe ne correspondent pas";
                estValide = false;
            }
            return estValide;
        }

    }
}
