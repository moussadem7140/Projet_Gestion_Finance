using Projet_Gestion_Finance.Classes;
using Projet_Gestion_Finance.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using Projet_Gestion_Finance.classes;
using Projet_Gestion_Finance.Views;
using LiveCharts.Wpf;
using LiveCharts;
using Microsoft.VisualBasic;

namespace Projet_Gestion_Finance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public static ChartValues<Decimal> limites = new ChartValues<Decimal>();
        public static ChartValues<Decimal> totals = new ChartValues<Decimal>();
        public static List<string>  labels = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

        }

        public int User { get; set; }
        public MainWindow(int user)
        {
            InitializeComponent();
            User = user;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            ChargerMois();
            chargerListes();
           
        }
        /// <summary>
        /// Permet de remplir toutes les liste de départ du programme
        /// </summary>
        private void chargerListes()
        {
            lstDepenses.ItemsSource = null;
            lstCategorie.ItemsSource = null;
            graphique.Series = null;
            Labels.Labels = null;
            totals.Clear();
            limites.Clear();
            labels.Clear();
            cboCategories.Items.Clear();
            List<Categorie> cat = Dal.ObtenirListeCategories(User, new DateTime((int)CombosAnnee.SelectedItem, ComboMois.SelectedIndex + 1, 1));
            lstCategorie.ItemsSource = cat;
            List<Depenses> dep = Dal.ObtenirListeDepenses(dtpDebutPeriode.SelectedDate.Value, dtpFinPeriode.SelectedDate.Value, User);
            lstDepenses.ItemsSource = dep;
            btnImprimerDepense.IsEnabled = true;
            print.IsEnabled = true;
            List<Categorie> cat1 = Dal.ObtenirListeCategories(User);
            cat1.Add(new Categorie(1200, "Toutes", 0, "s"));
            foreach (Categorie c in cat1)
            {
                cboCategories.Items.Add(c.Nom);
            }
            foreach (Categorie c in Dal.ObtenirListeCategories(User, new DateTime((int)CombosAnnee.SelectedItem, ComboMois.SelectedIndex + 1, 1)))
            {
                limites.Add(c.LimiteDepenses);
                totals.Add(c.CoutTotal);
                labels.Add(c.Nom);
            }
            decimal total = 0;
            decimal total1 = 0;
            foreach(Depenses d in dep)
            {
                total += d.Cout;
            }
            foreach(Projets p in Dal.ObtenirListeProjets(dtpFinPeriode.SelectedDate.Value, User))
            {
                total1 += p.Cout;
            }
            //var limites = new ChartValues<double> { 500, 800, 300 };
            //var totals = new ChartValues<double> { 350, 600, 200 };
            //var labels = new List<string> { "Nourriture", "Voiture", "Loisirs" };
            SeriesCollection categories = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Limite",
                    Values = limites,
                    DataLabels=true,
                 },
                new ColumnSeries
                {
                    Title = "Utilisé",
                    Values = totals,
                    DataLabels =true,
                }
            };
            SeriesCollection ListeDepensesRevenus = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Marge de revenue non utilisées",
                    //Dal.ObtenirUtilisateur(User).Revenue
                    Values = new ChartValues<decimal>{ 5000 - total -total1 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Parts utilisées par les Depenses ",
                    Values =new ChartValues<decimal>{total},
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Parts utilisées par les projets",
                    Values = new ChartValues<decimal> { total1 },
                    DataLabels = true
                }
            };

            depensesRevenues.Series= ListeDepensesRevenus;
            graphique.Series = categories;
            Labels.Labels = labels;

        }
        /// <summary>
        /// Permet d'initialiser les jours dans les date pickers
        /// </summary>
        private void ChargerMois()
        {
            string[] moisNoms = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            ComboMois.ItemsSource = moisNoms;
            ComboMois.SelectedIndex = DateTime.Now.Month - 1;
            CombosAnnee.ItemsSource = Enumerable.Range(2025, 20).ToList();
            CombosAnnee.SelectedIndex = 0;
            moisCategorie.Text = ComboMois.SelectedItem.ToString() + " " + CombosAnnee.SelectedItem.ToString();
            dtpDebutPeriode.SelectedDate = DateTime.Now;
            dtpFinPeriode.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }
        private void btnCreerCategorie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FormCategorie formCreer = new FormCategorie(null, EtatFormulaire.Créer, User);
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
                if (lstCategorie.SelectedItem == null)
                {
                    throw new Exception("Veuillez sélectionner une Categorie");
                }
                FormCategorie formModifier = new FormCategorie((Categorie)lstCategorie.SelectedItem, EtatFormulaire.Modifier);
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
                if (lstCategorie.SelectedItem == null)
                {
                    throw new Exception("Veuillez sélectionner une Categorie");
                }
                FormCategorie formSupprimer = new FormCategorie((Categorie)lstCategorie.SelectedItem, EtatFormulaire.Supprimer);
                formSupprimer.ShowDialog();
                chargerListes();
            }
            catch (Exception ex)
            {
                FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Erreur);
                f.ShowDialog();
            }

        }

        private void btnAjouterDepense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FormDepense formAjouter = new FormDepense(null, EtatFormulaire.Ajouter, User);
                formAjouter.ShowDialog();

            }
            catch (Exception ex)
            {
                FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Erreur);
                f.ShowDialog();
            }
            chargerListes();
        }

        private void btnModifierDepense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstDepenses.SelectedItem == null)
                {
                    throw new Exception("Veuillez sélectionner une dépense");
                }
                FormDepense formModifier = new FormDepense(Dal.ObtenirDepense(((Depenses)lstDepenses.SelectedItem).Id), EtatFormulaire.Modifier);
                formModifier.ShowDialog();
                chargerListes();
            }
            catch (Exception ex)
            {
                FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Erreur);
                f.ShowDialog();
            }

        }

        private void btnSupprimerDepense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstDepenses.SelectedItem == null)
                {
                    throw new Exception("Veuillez sélectionner une dépense");
                }
                FormDepense formSupprimer = new FormDepense(Dal.ObtenirDepense(((Depenses)lstDepenses.SelectedItem).Id), EtatFormulaire.Supprimer);
                formSupprimer.ShowDialog();
                chargerListes();
            }
            catch (Exception ex)
            {
                FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Erreur);
                f.ShowDialog();
            }
        }

        private void btnRechercherDepense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpFinPeriode.SelectedDate.Value.Date <= dtpDebutPeriode.SelectedDate.Value.Date)
                {
                    throw new InvalidOperationException("La date de fin doit être postérieure à la date de début");
                }
                lstDepenses.ItemsSource = null;
                List<Depenses> r = new List<Depenses>();
                List<Depenses> dep = Dal.ObtenirListeDepenses(dtpDebutPeriode.SelectedDate.Value, dtpFinPeriode.SelectedDate.Value, User);
                if (cboCategories.SelectedItem is null && txtRechercher is null)
                    chargerListes();
                else if (cboCategories.SelectedItem is null || cboCategories.SelectedItem.ToString() == "Toutes")
                {
                    foreach (Depenses d in dep)
                    {
                        if (d.Nom.ToLower().Contains(txtRechercher.Text.ToLower()))
                            r.Add(d);
                    }

                }
                else if (txtRechercher.Text is null)
                {
                    foreach (Depenses d in dep)
                    {
                        if (d.Categorie.Nom == cboCategories.SelectedItem.ToString())
                            r.Add(d);
                    }
                }
                else
                {
                    foreach (Depenses d in dep)
                    {
                        if (d.Categorie.Nom == cboCategories.SelectedItem.ToString() && d.Nom.ToLower().Contains(txtRechercher.Text))
                            r.Add(d);
                    }
                }
                lstDepenses.ItemsSource = r;
                btnImprimerDepense.IsEnabled = true;
                print.IsEnabled = true;
            }
            catch (Exception ex)
            {
                FrmErreur f = new FrmErreur(ex.Message, FrmErreur.EtatErreur.Erreur);
                f.ShowDialog();
            }

        }
        private void btn(object sender, RoutedEventArgs e)
        {
            chargerListes();
            //lstCategorie.ItemsSource = null;
            //List<Categorie> cat = Dal.ObtenirListeCategories(User, new DateTime((int)CombosAnnee.SelectedItem, ComboMois.SelectedIndex + 1, 1));
            //lstCategorie.ItemsSource = cat;
        }

        private void btnImprimerDepense_Click(object sender, RoutedEventArgs e)
        {
            UtilEnum.EcrireDepensesDansFichier(lstDepenses.Items.Cast<Depenses>().ToList(), dtpDebutPeriode.SelectedDate.Value, dtpFinPeriode.SelectedDate.Value);
            btnImprimerDepense.IsEnabled = false;
            print.IsEnabled = false;
            MessageBox.Show("Le Fichier a bien été mise à jour", "Impression", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Accueil l = new Accueil(Dal.ObtenirUtilisateur(User));
            l.Show();
            this.Close();
        }

      
        //Statistiques


    }
}
