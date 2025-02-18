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

        public FormCategorie(Categorie categorie, EtatFormulaire etat)
        {
            InitializeComponent();
            Categorie = categorie;
            Etat = etat;
        }
        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
           
            Titre.Text = $"{Etat} d'une Categorie";
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
           
        }

        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
