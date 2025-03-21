using Microsoft.VisualBasic;
using Projet_Gestion_Finance.Classes;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logique d'interaction pour FormCategorie.xaml
    /// </summary>
    public partial class FormCategorie : Window
    {
        private Categorie _categorie;

        public Categorie Categorie
        {
            get { return _categorie; }
            set { _categorie = value; }
        }
        private EtatFormulaire _etat;

        public EtatFormulaire Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }
        public int User { get; set; }
        public FormCategorie(Categorie categorie, EtatFormulaire etat, int user=0)
        {
            InitializeComponent();
            Categorie = categorie;
            Etat = etat;
            User = user;
        }
        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            Titre.Text = $"{Etat} une catégorie";
            btnCreer.Content = $"{Etat}";
            if (Etat != EtatFormulaire.Créer)
            {
                txtNom.Text = Categorie.Nom.ToString();
                txtLimite.Text = Categorie.LimiteDepenses.ToString();
                txtDescription.Text = Categorie.Description.ToString();
                if (Etat == EtatFormulaire.Supprimer)
                {
                    txtNom.IsEnabled = false;
                    txtLimite.IsEnabled = false;
                    txtDescription.IsEnabled = false;
                }
            }
        }
        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Etat)
                {
                    case EtatFormulaire.Créer:
                        if (validerCategorie())
                        {
                            Dal.CreerCategorie(new Categorie(0, txtNom.Text, decimal.Parse(txtLimite.Text), txtDescription.Text), User);
                            DialogResult = true;
                        }
                        break;
                    case EtatFormulaire.Modifier:
                        //string message1 = ValiderCourse();
                        if (validerCategorie())
                        {

                            Dal.ModifierCategorie(new Categorie(Categorie.Id, txtNom.Text, decimal.Parse(txtLimite.Text), txtDescription.Text), User);
                            DialogResult = true;
                        }

                        break;
                    case EtatFormulaire.Supprimer:
                        MessageBoxResult messageBoxResult = MessageBox.Show("Désirez-vous supprimer la Categorie",
                           "Suppression d'un contact", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            Dal.SupprimerCategorie(Categorie);
                            DialogResult = true;
                        }
                        else
                            DialogResult = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Action Invalide", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        /// <summary>
        /// Permet de valider le formulaire
        /// </summary>
        /// <returns>True si l formulaire est valide et false false</returns>
        private bool validerCategorie()
        {
            bool valide = true;
            if (string.IsNullOrEmpty(txtNom.Text))
            {
                txtNomErreur.Content = "Saisissez un nom";
                txtNom.BorderBrush = Brushes.Red;
                valide = false;
            }
            else
            {
                txtNomErreur.Content = "";
                txtNom.BorderBrush = null;

            }
            if (String.IsNullOrEmpty(txtLimite.Text) || !uint.TryParse(txtLimite.Text, out uint a))
            {
                txtLimiteErreur.Content = "Saisiser une limite(nombre positif)kk";
                txtLimite.BorderBrush = Brushes.Red;
                valide = false;

            }
            else
            {
                txtLimiteErreur.Content = "";
                txtLimite.BorderBrush = null;
            }
            if (String.IsNullOrEmpty(txtDescription.Text))
            {
                txtDescriptionErreur.Content = "Saisiser une limite";
                txtDescription.BorderBrush = Brushes.Red;
                valide = false;

            }
            else
            {
                txtDescriptionErreur.Content = "";
                txtDescription.BorderBrush = null;
            }

            return valide;
        }
    }
}
