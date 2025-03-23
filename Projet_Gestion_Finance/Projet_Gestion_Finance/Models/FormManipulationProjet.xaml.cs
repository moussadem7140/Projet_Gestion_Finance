using Projet_Gestion_Finance.classes;
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

namespace Projet_Gestion_Finance.Models
{
    public partial class FormManipulationProjet : Window
    {
        private Projets _projet;
        public Projets Projet
        {
            get { return _projet; }
            set { _projet = value; }
        }
        private EtatFormulaire _etat;

        public EtatFormulaire Etat
        {
            get { return _etat; }
            set { _etat = value; }
        }

        public FormManipulationProjet(Projets projet, EtatFormulaire etat)
        {
            InitializeComponent();
            Projet = projet;
            Etat = etat;
        }
        /// <summary>
        /// Permet de charger les combobox du formulaire
        /// </summary>
        private void ChargerCbx()
        {
            string[] vect1 = UtilEnum.GetAllDescriptions<Depenses.TypeFrequence>();
            for (int i = 0; i < vect1.Length; i++)
            {
                if (vect1[i] != "Occasionnel")
                    cbxFrequence.Items.Add(vect1[i]);
            }
        }
        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ChargerCbx();
                Titre.Text = $"{Etat} un projet";
                btnCreer.Content = $"{Etat}";
                if (Etat != EtatFormulaire.Créer)
                {
                    txtNom.Text = Projet.Nom.ToString();
                    txtCout.Text = Projet.Cout.ToString();
                    txtObjectif.Text = Projet.Objectif.ToString();
                    dtpDate.SelectedDate = Projet.Date;
                    cbxFrequence.SelectedIndex = (int)Projet.Frequence;
                    if (Etat == EtatFormulaire.Supprimer)
                    {
                        txtNom.IsEnabled = false;
                        txtCout.IsEnabled = false;
                        dtpDate.IsEnabled = false;
                        cbxFrequence.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Action Invalide", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }
        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                switch (Etat)
                {
                    case EtatFormulaire.Créer:

                        if (validerProjet())
                        {
                            Dal.CreerProjet(new Projets(0, txtNom.Text, dtpDate.SelectedDate.Value, decimal.Parse(txtObjectif.Text), decimal.Parse(txtCout.Text), (Depenses.TypeFrequence)cbxFrequence.SelectedIndex));
                            DialogResult = true;
                        }
                        break;
                    case EtatFormulaire.Modifier:
                        if (validerProjet())
                        {
                            FrmErreur formErreur1 = new FrmErreur("Voulez-vous vraiment Modifier ce projet? Notez que Si vous modifier un Projet toutes les progression seront perdues.", FrmErreur.EtatErreur.Avertissement);
                            formErreur1.ShowDialog();
                            if (formErreur1.DialogResult == true)
                            {
                                Dal.ModifierProjet(new Projets(Projet.Id, txtNom.Text, dtpDate.SelectedDate.Value, decimal.Parse(txtObjectif.Text), decimal.Parse(txtCout.Text), (Depenses.TypeFrequence)cbxFrequence.SelectedIndex));
                                DialogResult = true;
                            }
                            else
                                DialogResult = false;
                        }
                        break;
                    case EtatFormulaire.Supprimer:
                        FrmErreur formErreur = new FrmErreur("Voulez-vous vraiment supprimer ce projet?", FrmErreur.EtatErreur.Avertissement);
                        formErreur.ShowDialog();
                        if (formErreur.DialogResult==true)
                        {
                            Dal.SupprimerProjet(Projet);
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
        private bool validerProjet()
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
            if (String.IsNullOrEmpty(txtObjectif.Text) || !uint.TryParse(txtObjectif.Text, out uint b))
            {
                txtObjectifErreur.Content = "Saisiser un nombre positif";
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
            return valide;
        }
    }
}
