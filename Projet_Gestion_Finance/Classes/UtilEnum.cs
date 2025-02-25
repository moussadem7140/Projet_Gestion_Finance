


using Projet_Gestion_Finance.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Projet_Gestion_Finance.Models;

namespace  Projet_Gestion_Finance.classes
{
    /// <summary>
    /// Classe utilitaire pour les énumérations. Fournit des méthodes permettant d'obtenir les
    /// descriptions associées aux constantes des énumérations lorsque celles-ci sont disponibles.
    /// NOTE POUR LES ÉTUDIANTS : N'essayez pas de comprendre le code suivant.
    /// </summary>
    public static class UtilEnum
    {
     

        /// <summary>
        /// Extension permettant d'obtenir la description pour une constante d'une énumération, si disponible.
        /// S'il n'y a pas de description associée à la constante de l'énumération, permet d'obtenir la valeur
        /// de celle-ci.
        /// Source : https://msmvps.com/blogs/deborahk/archive/2009/07/10/enum-binding-to-the-description-attribute.aspx
        /// </summary>
        /// <param name="currentEnum">Énumération pour laquelle on désire obtenir une description.</param>
        /// <returns>
        /// Description associée à la constante de l'énumération ou bien la valeur s'il n'y a pas de description.
        /// </returns>
        public static string GetDescription(this Enum currentEnum)
        {
            string description;
            DescriptionAttribute da;

            FieldInfo fi = currentEnum.GetType().GetField(currentEnum.ToString());
            da = (DescriptionAttribute) Attribute.GetCustomAttribute(fi, typeof (DescriptionAttribute));
            if (da != null)
                description = da.Description;
            else
                description = currentEnum.ToString();

            return description;
        }
        public static int GetSemainesRestantes(DateTime date)
        {
            // Trouver le dernier jour du mois
            DateTime dernierJourMois = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            // Nombre de jours restants
            int joursRestants = (dernierJourMois - date).Days;

            // Convertir en semaines
            int semaines = (int)Math.Ceiling(joursRestants / 7.0);

            return semaines;
        }

        /// <summary>
        /// Permet d'obtenir toutes les descriptions associées aux constantes d'une énumération, si disponible.
        /// S'il n'y a pas de description associée à une constante de l'énumération, permet d'obtenir
        /// la valeur de celle-ci.
        /// </summary>
        /// <returns>
        /// Les descriptions associées aux constantes de l'énumération ou bien les valeurs
        /// s'il n'y a pas de description.
        /// </returns>
        public static string[] GetAllDescriptions<T>()
        {
            Type enumType = typeof (T);
            List<String> lesDescriptions = new List<String>();
            foreach (Enum valeur in Enum.GetValues(enumType))
            {
                lesDescriptions.Add(valeur.GetDescription());
            }
            return lesDescriptions.ToArray();
        }

        public static void EcrireDepensesDansFichier(List<Depenses> depenses, DateTime debut, DateTime fin)
        {
         
            string dossierBin = AppDomain.CurrentDomain.BaseDirectory;
            string cheminFichier = Path.Combine(dossierBin, "Depenses.txt");

           
            var depensesParCategorie = depenses.GroupBy(d => d.Categorie.Nom);

            using (StreamWriter writer = new StreamWriter(cheminFichier, false)) 
            {
                writer.WriteLine();
                writer.WriteLine("====================================================");
                writer.WriteLine($"Inventaire des depenses du { debut:dd/MM/yyyy} au { fin:dd/MM/yyyy}");
                writer.WriteLine("====================================================");
                writer.WriteLine();
                writer.WriteLine();
                foreach (var categorie in depensesParCategorie)
                {
                    writer.WriteLine($"================{categorie.Key}======================");

                    foreach (var depense in categorie)
                    {
                        writer.WriteLine($"------{depense.Nom} ------");
                        writer.WriteLine($"Coût : {depense.Cout}$");
                        writer.WriteLine($"Date : {depense.Date:dd/MM/yyyy}");
                        writer.WriteLine();
                    }
                    writer.WriteLine("====================================================");
                    writer.WriteLine();
                }
            }
        }
    }
}
