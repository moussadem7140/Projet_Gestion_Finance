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
using Projet_Gestion_Finance.Classes;
using Projet_Gestion_Finance.Models;
using System.Media;
using LiveCharts.Wpf;
using LiveCharts;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Projet_Gestion_Finance.Views
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        public static ChartValues<Decimal> limites = new ChartValues<Decimal>();
        public static ChartValues<Decimal> totals = new ChartValues<Decimal>();
        public static List<string> labels = new List<string>();
        private Utilisateur utilisateur;

        public Utilisateur Utilisateur
        {
            get { return utilisateur; }
            set { utilisateur = value; }
        }

        public Accueil(Utilisateur utilisateur)
        {
            Utilisateur = utilisateur;
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void StackPanel_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            MainWindow C = new MainWindow(Utilisateur.IdUnique);
            C.Show();
            this.Close();
            var player = new SoundPlayer("Ressource/argent.wav");
            player.Play();
        }

        private void StackPanel_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            FormProjets p = new FormProjets(Utilisateur.IdUnique);
            p.Show();
            this.Close();
            var player = new SoundPlayer("Ressource/argent.wav");
            player.Play();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Utilisateur;
            txtInitiales.Text = " " + Utilisateur.Prenom[0].ToString().ToUpper() + Utilisateur.Nom[0].ToString().ToUpper();
            txtNom.Text = "   Salut " + Utilisateur.Nom.ToUpper() + " " + Utilisateur.Prenom.ToUpper()+",";
            //txtRevenu.Text = "$" + " " + Utilisateur.Revenue.ToString();
            var player = new SoundPlayer("Ressource/bienvenu.wav");
            player.Play();
            chargerListes(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)));
        }
        /// <summary>
        /// Permet de gérer l'événement de clic sur l'image de déconnexion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Login g = new Login();
            g.ShowDialog();
            this.Close();

        }
        /// <summary>
        /// Permet de gérer l'événement de clic sur l'image de modification de profile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
          Inscription i = new Inscription(Utilisateur);
            i.Show();
        }
        private void Image_MouseDown2(object sender, MouseButtonEventArgs e)
        {
           video v = new video();
            v.Show();
        }
        private void chargerListes(DateTime dateDebut,DateTime dateFin)
        {
            graphique.Series = null;
            Labels.Labels = null;
            totals.Clear();
            limites.Clear();
            labels.Clear();
            //List<Categorie> cat = Dal.ObtenirListeCategories(utilisateur.IdUnique, new DateTime((int)CombosAnnee.SelectedItem, ComboMois.SelectedIndex + 1, 1));
            //lstCategorie.ItemsSource = cat;
            List<Depenses> dep = Dal.ObtenirListeDepenses(dateDebut, dateFin, utilisateur.IdUnique);
            //lstDepenses.ItemsSource = dep;
            //btnImprimerDepense.IsEnabled = true;
            //print.IsEnabled = true;
            //List<Categorie> cat1 = Dal.ObtenirListeCategories(utilisateur.IdUnique);
            //cat1.Add(new Categorie(1200, "Toutes", 0, "s"));
            //foreach (Categorie c in cat1)
            //{
            //    cboCategories.Items.Add(c.Nom);
            //}
            foreach (Categorie c in Dal.ObtenirListeCategories(utilisateur.IdUnique, dateDebut))
            {
                limites.Add(c.LimiteDepenses);
                totals.Add(c.CoutTotal);
                labels.Add(c.Nom);
            }
            decimal total = 0;
            decimal total1 = 0;
            foreach (Depenses d in dep)
            {
                total += d.Cout;
            }
            foreach (Projets p in Dal.ObtenirListeProjets(dateFin, utilisateur.IdUnique))
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
                    Title = "Marge libre",
                    Values = new ChartValues<decimal>{ utilisateur.Revenue - total - total1 },
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Depenses",
                    Values =new ChartValues<decimal>{total},
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Projets",
                    Values = new ChartValues<decimal> { total1 },
                    DataLabels = true
                }
            };

            depensesRevenues.Series = ListeDepensesRevenus;
            graphique.Series = categories;
            Labels.Labels = labels;

        }

        private void btnValider_Click(object sender, RoutedEventArgs e)
        {
            if (dtpDebutPeriode.SelectedDate is null || dtpFinPeriode.SelectedDate is null)
            {
                MessageBox.Show("Veuillez choisir une date");
            }
            else
            {
                chargerListes(dtpDebutPeriode.SelectedDate.Value, dtpFinPeriode.SelectedDate.Value);
            }
        }
    }
}
