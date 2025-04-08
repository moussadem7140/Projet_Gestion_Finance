using LiveCharts.Wpf;
using LiveCharts;
using Projet_Gestion_Finance.Classes;
using Projet_Gestion_Finance.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public static ChartValues<Decimal> limites = new ChartValues<Decimal>();
        public static ChartValues<Decimal> totals = new ChartValues<Decimal>();
        public static List<string> labels = new List<string>();
        public int User {  get; set; }
        public FormProjets()
        {
            InitializeComponent();
        }
        public FormProjets(int user)
        {
            InitializeComponent();
            User= user;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtpDate.SelectedDate = DateTime.Now;
            chargerListes();
        }
        private void chargerListes()
        {
            lstProjets.ItemsSource = null;
            List<Projets> cat = Dal.ObtenirListeProjets(dtpDate.SelectedDate.Value, User);
            lstProjets.ItemsSource = cat;
            foreach (Projets c in cat)
            {
                limites.Add(c.Objectif);
                totals.Add(c.Avancement);
                labels.Add(c.Nom);
            }
            //var limites = new ChartValues<double> { 500, 800, 300 };
            //var totals = new ChartValues<double> { 350, 600, 200 };
            //var labels = new List<string> { "Nourriture", "Voiture", "Loisirs" };
            SeriesCollection categories = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Objectif",
                    Values = limites,
                    DataLabels=true,
                 },
                new ColumnSeries
                {
                    Title = "Avancement",
                    Values = totals,
                    DataLabels =true,
                }
            };
            graphique.Series = categories;
            Labels.Labels = labels;

        }
        private void btnCreerCategorie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FormManipulationProjet formCreer = new FormManipulationProjet(null, EtatFormulaire.Créer, User);
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
        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Login l = new Login();
            l.Show();
            this.Close();
        }
     
    }
}
