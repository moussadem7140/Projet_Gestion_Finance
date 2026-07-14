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
using Projet_Gestion_Finance.Models;
using static Projet_Gestion_Finance.Models.FrmErreur;

namespace Projet_Gestion_Finance.Views
{
    /// <summary>
    /// Logique d'interaction pour Inscription.xaml
    /// </summary>
    public partial class Inscription : Window
    {
        public bool Modification { get; set; }
        public Utilisateur User { get; set; }
        public bool t {  get; set; }
        public Inscription()
        {
            InitializeComponent();
        }
        public Inscription(Utilisateur user)
        {
            InitializeComponent();
            User= user;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           if(User is not null)
            {
                txtblkTitre.Text = "Modification";
                txtNomIns.Text = User.Nom;
                txtPrenomIns.Text= User.Prenom;
                txtMailIns.Text= User.Mail;
                txtRevenuIns.Text= User.Revenue.ToString()+"$";
                btnInscription.Content = "Modifier";
                p.Text = "";
                s.Text = "";
            }
        }
        private void btnInscription_Click(object sender, RoutedEventArgs e)
        {

            if (ValiderChamps())
            {
                if (User is null)
                {
                    string nom = txtNomIns.Text.Trim();
                    string prenom = txtPrenomIns.Text.Trim();
                    string mail = txtMailIns.Text.Trim();
                    string mdp = txtMdpIns.Password.Trim();
                    string id = nom.Substring(0, 3).Trim().ToUpper() + prenom.Substring(0, 3).Trim().ToUpper();
                    decimal revenu = decimal.Parse(txtRevenuIns.Text.Replace("$", "").Trim());
                    byte[] salt = Classes.Utils.CreerSALT();
                    //GestionFinance.Dicosalts.Add(id, salt);
                    Utilisateur utilisateur = new Utilisateur(0, nom, prenom, id, Classes.Utils.HacherMotDePasse(mdp, salt), mail, salt);
                    utilisateur.Revenue = revenu;
                    //GestionFinance.DicoUtilisateurs.Add(id, utilisateur);
                    Dal.AjouterUtilisateur(utilisateur);
   
                    FrmErreur erreur = new FrmErreur($"Compte créé avec succès \n Votre Identifiant : {utilisateur.Id}", FrmErreur.EtatErreur.Inscripton);
                    erreur.ShowDialog();
                    if (erreur.DialogResult == true)
                    {
                        erreur.Close();
                    }
                    this.Close();
                }
                else
                {
                    string nom = txtNomIns.Text.Trim();
                    string prenom = txtPrenomIns.Text.Trim();
                    string mail = txtMailIns.Text.Trim();
                    string mdp = txtMdpIns.Password.Trim();
                    string id = nom.Substring(0, 3).Trim().ToUpper() + prenom.Substring(0, 3).Trim().ToUpper();
                    decimal revenu = decimal.Parse(txtRevenuIns.Text.Replace("$", "").Trim());
                    byte[] salt = Classes.Utils.CreerSALT();
                    //GestionFinance.Dicosalts.Add(id, salt);
                    Utilisateur utilisateur = new Utilisateur(User.IdUnique, nom, prenom, id, Classes.Utils.HacherMotDePasse(mdp, salt), mail, salt);
                    utilisateur.Revenue = revenu;
                    User = utilisateur;
                    //GestionFinance.DicoUtilisateurs.Add(id, utilisateur);
                    Dal.ModifierUtilisateur(User);
                    (new Accueil(User)).Show();
                    this.Close();
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
            txtRevenuInsError.Text = string.Empty;
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
            string revenuBrut = txtRevenuIns.Text.Replace("$", "").Trim();
            if (string.IsNullOrWhiteSpace(revenuBrut) || !decimal.TryParse(revenuBrut, out decimal revenuVal) || revenuVal < 0)
            {
                txtRevenuInsError.Text = "Saisissez un revenu valide (nombre positif)";
                estValide = false;
            }
            else
            {
                txtRevenuInsError.Text = string.Empty;
            }
            return estValide;
        }

        private void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }
    }
}
