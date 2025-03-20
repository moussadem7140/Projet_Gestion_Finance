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
    /// <summary>
    /// Logique d'interaction pour FormProjets.xaml
    /// </summary>
    public partial class FormProjets : Window
    {
        public FormProjets()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtpDate.SelectedDate = DateTime.Now;
            chargerListes();
        }
        private void chargerListes()
        {
            lstProjets.ItemsSource = null;
            List<Projets> cat = Dal.ObtenirListeProjets(dtpDate.SelectedDate.Value);
            lstProjets.ItemsSource = cat;
        }
        private void btnCreerCategorie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FormManipulationProjet formCreer = new FormManipulationProjet(null, EtatFormulaire.Créer);
                formCreer.ShowDialog();
                chargerListes();
            }
            catch (Exception ex)
            {
                FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Erreur);
                f.ShowDialog();
            }
        }

        private void btnModifierCategorie_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (lstProjets.SelectedItem == null)
                {
                    throw new Exception("Veuillez sélectionner un projet");
                }
                FormManipulationProjet formModifier = new FormManipulationProjet((Projets)lstProjets.SelectedItem, EtatFormulaire.Modifier);
                formModifier.ShowDialog();
                chargerListes();
            }
            catch (Exception ex)
            {
                FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Erreur);
                f.ShowDialog();
            }

        }

        private void btnSupprimerCategorie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstProjets.SelectedItem == null)
                {
                    throw new Exception("Veuillez sélectionner un projet");
                }
                FormManipulationProjet formSupprimer = new FormManipulationProjet((Projets)lstProjets.SelectedItem, EtatFormulaire.Supprimer);
                formSupprimer.ShowDialog();
                chargerListes();
            }
            catch (Exception ex)
            {
                FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Erreur);
                f.ShowDialog();
            }

        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        { 
            chargerListes();
        }

        }
}
