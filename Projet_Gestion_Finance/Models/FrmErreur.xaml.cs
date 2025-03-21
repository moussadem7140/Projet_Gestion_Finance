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
            Connexion,
            Inscripton
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
        private int _user;

        public int User
        {
            get { return _user; }
            set { _user = value; }
        }

        public FrmErreur(string message, EtatErreur etat, int user=0)
        {
            InitializeComponent();
            Message = message;
            Etat = etat;
            User=user;
        }
        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            if(Etat == EtatErreur.Avertissement)
            {
                lblTitre.Foreground = Brushes.Orange;
                lblTitre.Text = "Avertissement";
            }
            else if(Etat == EtatErreur.Connexion)
            {
                lblTitre.Foreground = Brushes.Green;
                lblTitre.Text = "Connexion réussie";
                btn.Content = "Gérer mes dépenses";
                btn1.Content = "Gérer mes projets";
            }
            else if(Etat==EtatErreur.Inscripton)
            {

                lblTitre.Foreground = Brushes.Green;
                lblTitre.Text = "Inscription réussi";
            }

            lblErreur.Text = Message;
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (Etat == EtatErreur.Connexion)
            {
                (new MainWindow(User)).ShowDialog();
                this.Close();
            }
            else
                DialogResult = true;

        }
        private void Button_Annuler_Click(object sender, RoutedEventArgs e)
        {
            if (Etat == EtatErreur.Connexion)
            {
                (new FormProjets(User)).Show();
                this.Close();
            }
            else
                DialogResult = false;
        }

    }
    }
