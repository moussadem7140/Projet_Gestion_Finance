using Projet_Gestion_Finance.Classes;
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
using Microsoft.VisualBasic;
using System.Xml;
namespace Projet_Gestion_Finance.Models
{
    /// <summary>
    /// Logique d'interaction pour FormDepense.xaml
    /// </summary>
    public partial class FormDepense : Window
    {
        private Depenses _depense;

        public Depenses Depense
        {
            get { return _depense; }
            set { _depense = value; }
        }
        private EtatFormulaire _etat;

        public EtatFormulaire Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }
        public int User {  get; set; }
        public FormDepense(Depenses depense, EtatFormulaire etat, int user=0)
        {
            InitializeComponent();
            Depense = depense;
            Etat = etat;
            User = user;
        }
        /// <summary>
        /// Permet de charger les combobox du formulaire
        /// </summary>
        private void ChargerCbx()
        {
            string[] vect1 = UtilEnum.GetAllDescriptions<Depenses.TypeFrequence>();
            List<Categorie> cat = Dal.ObtenirListeCategories(User);
            for (int i = 0; i < cat.Count; i++)
            {
                cbxCategorie.Items.Add(cat[i].Nom);
            }
            for (int i = 0; i < vect1.Length; i++)
            {
                cbxFrequence.Items.Add(vect1[i]);
            }
        }
        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChargerCbx();
                Titre.Text = $"{Etat} une dépense";
                btnModifier.Content = $"{Etat}";
                if (Etat != EtatFormulaire.Ajouter)
                {
                    txtNom.Text = Depense.Nom.ToString();
                    txtCout.Text = Depense.Cout.ToString();
                    dtpDate.SelectedDate = Depense.Date;
                    cbxFrequence.SelectedIndex = (int)Depense.Frequence;
                    cbxCategorie.SelectedIndex = cbxCategorie.Items.IndexOf(Depense.Categorie.Nom);
                    chkOblig.IsChecked = Depense.Obligatoire;
                    if (Etat == EtatFormulaire.Supprimer)
                    {
                        txtNom.IsEnabled = false;
                        txtCout.IsEnabled = false;
                        dtpDate.IsEnabled = false;
                        cbxFrequence.IsEnabled = false;
                        cbxCategorie.IsEnabled = false;
                        chkOblig.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Action Invalide", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                switch (Etat)
                {
                    case EtatFormulaire.Ajouter:
                       
                        if (validerDepense())
                        {
                            Dal.AjouterDepense(new Depenses(0, txtNom.Text, Dal.ObtenirCategorie(cbxCategorie.SelectedIndex + 1), decimal.Parse(txtCout.Text), dtpDate.SelectedDate.Value, (Depenses.TypeFrequence)cbxFrequence.SelectedIndex, chkOblig.IsChecked.Value), User);
                            DialogResult = true;
                        }
                        break;
                    case EtatFormulaire.Modifier:
                        if (validerDepense())
                        {
                            Dal.ModifierDepense(new Depenses(Depense.Id, txtNom.Text, Dal.ObtenirCategorie(cbxCategorie.SelectedIndex+1), decimal.Parse(txtCout.Text), dtpDate.SelectedDate.Value, (Depenses.TypeFrequence)cbxFrequence.SelectedIndex, chkOblig.IsChecked.Value));
                            DialogResult = true;
                        }
                        break;
                    case EtatFormulaire.Supprimer:
                        FrmErreur formErreur = new FrmErreur("Voulez-vous vraiment supprimer cette dépense?", FrmErreur.EtatErreur.Avertissement);
                        formErreur.ShowDialog();
                        if (formErreur.DialogResult == true)
                        {
                            Dal.Supprimerdepense(Depense);
                            DialogResult = true;
                        }
                        else
                            DialogResult = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                txtCoutErreur.Content = ex.Message;
                txtCout.BorderBrush = Brushes.Red;
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
        private bool validerDepense()
        {
            bool valide = true;
            if (string.IsNullOrEmpty(txtNom.Text))
            {
                txtNomErreur.Content = "Saisissez un nom";
                txtNom.BorderBrush = Brushes.Red;
                valide = false;
            }
            else if (!txtNom.Text.All(char.IsLetter))
            {
                txtNomErreur.Content = "Les valeurs numériques ne sont pas acceptées";
                txtNom.BorderBrush = Brushes.Red;
                valide = false;
            }
            else
            {
                txtNomErreur.Content = "";
                txtNom.BorderBrush = null;

            }
            if (String.IsNullOrEmpty(txtCout.Text) || !uint.TryParse(txtCout.Text, out uint a))
            {
                txtCoutErreur.Content = "Saisiser un nombre positif";
                txtCout.BorderBrush = Brushes.Red;
                valide = false;

            }
            else
            {
                txtCoutErreur.Content = ""; 
                txtCout.BorderBrush = null;
            }
            if (dtpDate.SelectedDate == null)
            {
                dtpDateErreur.Content = "Sasisez une date";
                dtpDate.BorderBrush = Brushes.Red;
                valide = false;
            }
            else if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date && (Depenses.TypeFrequence)(cbxFrequence.SelectedIndex)==Depenses.TypeFrequence.Occasionnel)
            {
                dtpDateErreur.Content = "La date ne peut pas être dans le passé pour une dépense occasionelle";
                dtpDate.BorderBrush = Brushes.Red;
                valide = false;
            }
            else
            {
                dtpDateErreur.Content = ""; 
                dtpDate.BorderBrush = null;
            }
            if (cbxFrequence.SelectedIndex == -1)
            {
                cbxFrequenceErreur.Content = "Choisissez une frequence";
                cbxFrequence.BorderBrush = Brushes.Red;
                valide = false;
            }
            else
                cbxFrequenceErreur.Content = "";
            if (cbxCategorie.SelectedIndex == -1)
            {
                txtCategorieErreur.Content = "Choisissez une categorie";
                cbxCategorie.BorderBrush = Brushes.Red;
                valide = false;
            }
            else
                txtCategorieErreur.Content = "";
            return valide;
        }
    }
}

