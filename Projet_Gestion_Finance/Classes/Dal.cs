using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projet_Gestion_Finance.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Projet_Gestion_Finance.Classes;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using Microsoft.VisualBasic;
using Projet_Gestion_Finance.classes;
namespace Projet_Gestion_Finance.Models
{

    public static class Dal
    {
        public const string APPSTTINGS_FILE = "appsettings.json";
        public const string CONNECTION_STRING = "DefaultConnection";
        private static IConfiguration _configuration;

        /// <summary>
        /// Constructeur static permettant de charger les configurations de l'application
        /// </summary>
        static Dal()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile(APPSTTINGS_FILE, false, true).Build();

        }
        /// <summary>
        /// Permet d'obtenir la liste des categorie
        /// </summary>
        /// <param name="mois">le mois dans lequelle nous allons produire les categories</param>
        public static List<Categorie> ObtenirListeCategories(DateTime? mois=null)
        {
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            List<Categorie> Categories = new List<Categorie>();
            try
            {
                cn.Open();
                //  public Depenses(string nom, Categorie cat, decimal cout, DateTime date, TypeFrequence frequence, bool obligatoire)

                string requete = "SELECT c.Id, c.Nom, Limite, Description FROM categorie c order by Id";

                MySqlCommand cmd = new MySqlCommand(requete, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (mois is null)
                {
                    while (dr.Read())
                    {
                        Categorie Categorie = new Categorie(dr.GetUInt16(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3));
                        Categories.Add(Categorie);
                    }
                }
                else
                {
                    while (dr.Read())
                    {
                        Categorie Categorie = new Categorie(dr.GetUInt16(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3));
                        Categorie.CoutTotal = totalCategorie(Categorie, mois.Value, 0);
                        Categories.Add(Categorie);
                    }
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
            return Categories;
        }
        /// <summary>
        /// Permet d'obtenir la liste de depenses dans une periode
        /// </summary>
        /// <param name="depart">debut de la periode</param>
        /// <param name="arrive">fin de la periode</param>
        public static List<Depenses> ObtenirListeDepenses(DateTime depart, DateTime arrive)
        {
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            List<Depenses> depenses = new List<Depenses>();
            try
            {
                cn.Open();
                //  public Depenses(string nom, Categorie cat, decimal cout, DateTime date, TypeFrequence frequence, bool obligatoire)

                string requete = "SELECT d.Nom, d.Categorie, d.Cout, d.Date, d.Frequence, d.Obligatoire, d.Id FROM depenses d Order by Date";

                MySqlCommand cmd = new MySqlCommand(requete, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Depenses depense = new Depenses(dr.GetUInt16(6), dr.GetString(0), ObtenirCategorie(dr.GetUInt16(1)), dr.GetDecimal(2), dr.GetDateTime(3), (Depenses.TypeFrequence)Enum.Parse(typeof(Depenses.TypeFrequence), dr.GetString(4)), dr.GetBoolean(5));
                    depenses.Add(depense);
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
            return DepensesPeriodes(depenses, depart, arrive);
        }
        /// <summary>
        /// Permet de créer une catégorie
        /// </summary>
        /// <param name="categorie">Categorie qui doit être créer</param>
        /// <exception cref="ArgumentNullException">Lance une exception si la categorie est null</exception>

        public static void CreerCategorie(Categorie categorie)
        {
            if (categorie is null)
                throw new ArgumentNullException(nameof(categorie), "La categorie ne peut être null");
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            try
            {
                cn.Open();
                string requete = "INSERT INTO categorie VALUES(null, @nom, @limite, @description)";

                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@nom", categorie.Nom);
                cmd.Parameters.AddWithValue("@limite", categorie.LimiteDepenses);
                cmd.Parameters.AddWithValue("@description", categorie.Description);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }

        }
        /// <summary>
        /// Permet de modifier une categorie
        /// </summary>
        /// <param name="categorie">Categorie qui doit être modifier</param>
        /// <exception cref="ArgumentNullException">Lance une exception si la categorie est null</exception>
        public static void ModifierCategorie(Categorie categorie)
        {
            if (categorie is null)
                throw new ArgumentNullException(nameof(categorie), "Veillez choisir une Categorie");
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));


            try
            {
                cn.Open();
                string requete2 = "UPDATE categorie SET Nom=@nom, Limite=@limite, Description=@description Where id= @id";
                MySqlCommand cmd = new MySqlCommand(requete2, cn);
                cmd = new MySqlCommand(requete2, cn);
                cmd.Parameters.AddWithValue("@id", categorie.Id);
                cmd.Parameters.AddWithValue("@nom", categorie.Nom);
                cmd.Parameters.AddWithValue("@limite", categorie.LimiteDepenses);
                cmd.Parameters.AddWithValue("@description", categorie.Description);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
        }
        /// <summary>
        /// Permet de supprimer une categorie
        /// </summary>
        /// <param name="categorie">Categorie qui doit être supprimer</param>
        /// <returns>True si la categorie est supprimée et false sinon</returns>
        /// <exception cref="ArgumentNullException">Lance une exception si la categorie est null</exception>
        /// <exception cref="InvalidOperationException">Lance une exception si la categorie contient au moins une dépense</exception>
        public static bool SupprimerCategorie(Categorie categorie)
        {
            if (categorie is null)
                throw new ArgumentNullException(nameof(categorie), "Veillez choisir une categorie");
            if(ObtenirlesDepensesCategorie(categorie).Count > 0)
            {
                throw new InvalidOperationException("La catégorie contient des dépenses");
            }
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));


            try
            {
                cn.Open();
                string requete = "DELETE FROM categorie WHERE Id = @id";
                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@id", categorie.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
            return true;
        }
        /// <summary>
        /// Permet d'ajouter une dépense
        /// </summary>
        /// <param name="depense">la depense qui doit être ajouter</param>
        /// <exception cref="ArgumentNullException">Lance une exception si la depense est null</exception>
        /// <exception cref="InvalidOperationException">Lance une exception si le coût de la dépense est trop élévé</exception>
        public static void AjouterDepense(Depenses depense)
        {
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            try
            {
            int p = depense.Frequence== Depenses.TypeFrequence.Hebdomadaire? UtilEnum.GetSemainesRestantes(depense.Date) : 1;
            if (depense is null)
                throw new ArgumentNullException(nameof(depense), "La depenses ne peut être null");
            if(totalCategorie(depense.Categorie, new DateTime(depense.Date.Year, depense.Date.Month, 1), 0) + p*depense.Cout> depense.Categorie.LimiteDepenses)
                throw new InvalidOperationException("La depense dépasse la limite mensuelle de la catégorie");
            if(ObtenirDepensesGénéralesMensuel(depense.Categorie, 0) + depense.CoutMensuel> depense.Categorie.LimiteDepenses && depense.Frequence!= Depenses.TypeFrequence.Occasionnel)
                    throw new InvalidOperationException("La depense n'est pas tenable sur le long terme.");
                cn.Open();
                //public Depenses(string nom, int cat, decimal cout, DateTime date, TypeFrequence frequence, bool obligatoire)
                string requete = "INSERT INTO depenses VALUES(@id, @nom, @cout, @categorie, @date, @frequence, @obligatoire)";

                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@id", depense.Id);
                cmd.Parameters.AddWithValue("@nom", depense.Nom);
                cmd.Parameters.AddWithValue("@categorie", depense.Categorie.Id);
                cmd.Parameters.AddWithValue("@cout", depense.Cout);
                cmd.Parameters.AddWithValue("@date", depense.Date);
                cmd.Parameters.AddWithValue("@frequence", depense.Frequence);
                cmd.Parameters.AddWithValue("@obligatoire", depense.Obligatoire);
                cmd.ExecuteNonQuery();
                ModifierCategorie(new Categorie(depense.Categorie.Id, depense.Categorie.Nom, depense.Categorie.LimiteDepenses, depense.Categorie.Description));
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }

        }
        /// <summary>
        /// Permet de modifier une dépense
        /// </summary>
        /// <param name="depense">la depense qui doit être modifier</param>
        /// <exception cref="ArgumentNullException">Lance une exception si la depense est null</exception>
        /// <exception cref="InvalidOperationException">Lance une exception si le coût de la dépense est trop élévé</exception>

        public static void ModifierDepense(Depenses depense)
        {
            int p = depense.Frequence == Depenses.TypeFrequence.Hebdomadaire ? UtilEnum.GetSemainesRestantes(depense.Date) : 1;
            if (depense is null)
                throw new ArgumentNullException(nameof(depense), "Veillez choisir une depense");
            if (totalCategorie(depense.Categorie, new DateTime(depense.Date.Year, depense.Date.Month, 1), depense.Id) + p*depense.Cout > depense.Categorie.LimiteDepenses)
                throw new InvalidOperationException("La depense dépasse la limite mensuelle de la catégorie");
            if (ObtenirDepensesGénéralesMensuel(depense.Categorie, depense.Id) + depense.CoutMensuel > depense.Categorie.LimiteDepenses)
                throw new InvalidOperationException("La depense n'est pas tenable sur le long terme.");
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));


            try
            {
                cn.Open();
                string requete2 = "UPDATE depenses SET Nom=@nom, Categorie=@categorie, Cout=@cout, Date=@date, Frequence= @frequence, Obligatoire=@obligatoire  Where Id=@id";
                MySqlCommand cmd = new MySqlCommand(requete2, cn);
                cmd = new MySqlCommand(requete2, cn);
                cmd.Parameters.AddWithValue("@id", depense.Id);
                cmd.Parameters.AddWithValue("@nom", depense.Nom);
                cmd.Parameters.AddWithValue("@categorie", depense.Categorie.Id);
                cmd.Parameters.AddWithValue("@cout", depense.Cout);
                cmd.Parameters.AddWithValue("@date", depense.Date);
                cmd.Parameters.AddWithValue("@frequence", depense.Frequence);
                cmd.Parameters.AddWithValue("@obligatoire", depense.Obligatoire);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
        }
        /// <summary>
        /// Permet de supprimer une dépense
        /// </summary>
        /// <param name="depense">la depense qui doit être supprimer</param>
        /// <exception cref="ArgumentNullException">Lance une exception si la depense est null</exception>
        public static bool Supprimerdepense(Depenses depense)
        {
            if (depense is null)
                throw new ArgumentNullException(nameof(depense), "Veillez choisir une categoie");

            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));


            try
            {
                cn.Open();
                string requete = "DELETE FROM depenses WHERE Id = @id";
                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@id", depense.Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
            return true;
        }

        /// <summary>
        /// Permet d'obtenir une categorie par son Id
        /// </summary>
        /// <param name="id">Id de la categorie</param>
        /// <returns>La categorie trouvée ou null si elle n'existe pas</returns>
        public static Categorie ObtenirCategorie(int id)
        {
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            Categorie c = null;
            try
            {
                cn.Open();
                String requete = "SELECT Id, Nom, Limite, Description FROM categorie WHERE Id=@id";
                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    c = new Categorie(dr.GetInt32(0), dr.GetString(1), dr.GetDecimal(2), dr.GetString(3));
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
            return c;

        }
        /// <summary>
        /// Permet d'obtenir une dépense par son Id
        /// </summary>
        /// <param name="id">Id de la dépense</param>
        /// <returns>La dépense trouvée ou null si elle n'existe pas</returns>
        public static Depenses ObtenirDepense(int id)
        {
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            Depenses depense=null;
            try
            {
                cn.Open();
                string requete = "SELECT d.Nom, d.Categorie, d.Cout, d.Date, d.Frequence, d.Obligatoire, d.Id FROM depenses d Where id =@id";
                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    depense = new Depenses(dr.GetUInt16(6), dr.GetString(0), ObtenirCategorie(dr.GetUInt16(1)), dr.GetDecimal(2), dr.GetDateTime(3), (Depenses.TypeFrequence)Enum.Parse(typeof(Depenses.TypeFrequence), dr.GetString(4)), dr.GetBoolean(5));
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
            return depense;

        }
        /// <summary>
        /// Permet d'obtenir les dépenses d'une categorie
        /// </summary>
        /// <param name="categorie">Caategorie des dépenses</param>
        /// <returns></returns>
        public static List<Depenses> ObtenirlesDepensesCategorie(Categorie categorie)
        {
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            List<Depenses> depenses = new List<Depenses>();
            try
            {
                cn.Open();
                string requete = "SELECT d.Nom, d.Categorie, d.Cout, d.Date, d.Frequence, d.Obligatoire, d.Id FROM depenses d where Categorie=@id Order by Date";
                MySqlCommand cmd = new MySqlCommand(requete, cn);
                cmd.Parameters.AddWithValue("@id", categorie.Id);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                        Depenses depense = new Depenses(dr.GetUInt16(6), dr.GetString(0), ObtenirCategorie(dr.GetUInt16(1)), dr.GetDecimal(2), dr.GetDateTime(3), (Depenses.TypeFrequence)Enum.Parse(typeof(Depenses.TypeFrequence), dr.GetString(4)), dr.GetBoolean(5));
                    depenses.Add(depense);
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
            return depenses;
        }
       /// <summary>
       /// Permet d'obtenir la somme des dépenses d'une categorie pendant un mois
       /// </summary>
       /// <param name="Categorie">la categorie des dépenses</param>
       /// <param name="mois">le mois dans lequel on recherche</param>
       /// <param name="Id">l'id de la depense a exclure si modification</param>
       /// <returns>retourne le total</returns>
        private static decimal totalCategorie(Categorie Categorie, DateTime mois, int Id)
        {

            decimal total = 0;
            foreach (Depenses depense in DepensesPeriodes(ObtenirlesDepensesCategorie(Categorie), mois, new DateTime(mois.Year, mois.Month, DateTime.DaysInMonth(mois.Year, mois.Month))))
            {
                if(Id !=  depense.Id) 
                    total += depense.Cout;
            }
            return total;
        }
        /// <summary>
        /// Permet d'obtenir les dépenses dans une periode données
        /// </summary>
        /// <param name="d">Listes des dépenses générales</param>
        /// <param name="depart">debut de la periode</param>
        /// <param name="arrive">fin de la periode</param>
        /// <returns>retourne la liste filtrée</returns>
        public static List<Depenses> DepensesPeriodes(List<Depenses> d, DateTime depart, DateTime arrive)
        {
            List<Depenses> depenses = new List<Depenses>();
            foreach (Depenses depense in d)
            {

                DateTime departSemaine = depart;
                DateTime arriveSemaine = arrive;
                DateTime debut = depense.Date;
                if (depense.Frequence== Depenses.TypeFrequence.Hebdomadaire)
                {
                    int i = 0;
                    while(debut.AddDays(i * 7).Date <= arriveSemaine.Date)
                    {
                        if(debut.AddDays(i*7).Date>=departSemaine.Date)
                            depenses.Add(new Depenses(depense, debut.AddDays(i*7)));
                        i++;
                    }
                }
                else if(depense.Frequence == Depenses.TypeFrequence.Mensuel)
                {
                    int i = 0;
                    while (debut.AddMonths(i).Date <= arriveSemaine.Date)
                    {
                        if (debut.AddMonths(i).Date >= departSemaine.Date)
                            depenses.Add(new Depenses(depense, debut.AddMonths(i)));
                        i++;
                    }
                }
                else if (depense.Frequence == Depenses.TypeFrequence.Occasionnel)
                {
                    if (depense.Date.Date >= depart.Date && depense.Date.Date <= arrive.Date)
                    {
                        depenses.Add(depense);
                    }
                }
                else 
                {
                    int i = 0;
                    while (debut.AddYears(i).Date <= arriveSemaine.Date)
                    {
                        if (debut.AddYears(i).Date >= departSemaine.Date)
                            depenses.Add(new Depenses(depense, debut.AddYears(i)));
                        i++;
                    }
                }
                
            }
            return depenses;

        }
        /// <summary>
        /// Permet d'avoir les dépenses générales
        /// </summary>
        /// <param name="categorie">Categorie des dépenses</param>
        /// <param name="Id">Id de deépense a exclure si modification</param>
        /// <returns></returns>
        public static decimal ObtenirDepensesGénéralesMensuel(Categorie categorie, int Id)
        {
            List<Depenses> liste= ObtenirlesDepensesCategorie(categorie);
            decimal total= 0;
            foreach(Depenses depense in liste)
            {
                if(depense.Id != Id && depense.Frequence!=Depenses.TypeFrequence.Occasionnel)
                {
                    if (depense.Frequence == Depenses.TypeFrequence.Hebdomadaire)
                        total += 5 * depense.Cout;
                    else
                        total += depense.Cout;
                }
            }
            return total;
        }
        /// <summary>
        /// Permet d'obtenir la liste de projets
        /// </summary>
        public static List<Projets> ObtenirListeProjets()
        {
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            List<Projets> projets = new List<Projets>();
            try
            {
                cn.Open();
                //        public Projets(int id, string nom, DateTime date, decimal objectif, decimal cout, Depenses.TypeFrequence frequence)

                string requete = "SELECT  d.Id, d.Nom, d.Date, d.Objectif, d.Cout d.Frequence FROM projets d Order by Date";

                MySqlCommand cmd = new MySqlCommand(requete, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Projets projet = new Projets(dr.GetInt32(0), dr.GetString(1), dr.GetDateTime(2) , dr.GetDecimal(3), dr.GetDecimal(4), (Depenses.TypeFrequence)Enum.Parse(typeof(Depenses.TypeFrequence), dr.GetString(5)));
                    projets.Add(projet);
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (cn is not null && cn.State == System.Data.ConnectionState.Open)
                    cn.Close();
            }
            return projets;
        }


    }
}
