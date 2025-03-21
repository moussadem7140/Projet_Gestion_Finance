using Projet_Gestion_Finance.Classes;
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
    public partial class Login : Window
    {
        public int User;
        Dictionary<string, Utilisateur> _dicoUtilisateurs = new Dictionary<string, Utilisateur>();
        public Login()
        {
            InitializeComponent();
            _dicoUtilisateurs = Dal.ObtenirUtilisateurs();

        }
        /// <summary>
        /// Permet à l'utilisateur d'ouvrir le formulaire pour créer un compre si il n'en a pas un
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (VerifierChamps())
            {
                string identifiant = txtIdConn.Text.ToUpper().Trim();
                Utilisateur utilisateur = _dicoUtilisateurs[identifiant];
                User= utilisateur.IdUnique;
                (new FrmErreur("Bonsoir "+utilisateur.Nom+" "+ utilisateur.Prenom, FrmErreur.EtatErreur.Connexion, User)).Show();
                this.Close();
            }
        }

        public bool VerifierChamps()
        {
            // Réinitialisation des messages d'erreur
            txtMsgErrid.Text = "";
            txtMsgErrMdp.Text = "";
            bool estValide = true;
            string identifiant = txtIdConn.Text.ToUpper().Trim();
            if (string.IsNullOrWhiteSpace(txtIdConn.Text))
            {
                txtMsgErrid.Text = "L'identifiant ne peut pas être vide";
                estValide = false;
            }
            else if (!_dicoUtilisateurs.ContainsKey(identifiant)){
                txtMsgErrid.Text = "L'identifiant n'existe pas";
                estValide = false;

            }
            if (string.IsNullOrWhiteSpace(txtMdpConn.Password))
            {
                txtMsgErrMdp.Text = "Le mot de passe ne peut pas être vide";
                estValide = false;
            }
            else
            {
                if (!_dicoUtilisateurs.ContainsKey(identifiant))
                {
                    txtMsgErrid.Text = "L'identifiant n'existe pas";
                    estValide = false;

                }
                else
                {
                    Utilisateur utilisateur = _dicoUtilisateurs[identifiant];
                    if (!Utils.EstMotDePasseCorrespond(txtMdpConn.Password, utilisateur.Salt, utilisateur.MDP))
                    {
                        txtMsgErrMdp.Text = "Le mot de passe ne correspond pas";
                        estValide = false;
                    }
                }
            }
            return estValide;
        }

        private void TextBlock_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Inscription inscription = new Inscription();
            inscription.ShowDialog();
            this.Close();
        }
    }
}
