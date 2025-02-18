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
        public static void CreerCategorie(Categorie categorie)
        {
            if (categorie is null)
                throw new ArgumentNullException(nameof(categorie), "Le produit ne peut être null");
            MySqlConnection cn = new MySqlConnection(_configuration.GetConnectionString(CONNECTION_STRING));
            try
            {
                cn.Open();
                string requete = "INSERT INTO categorie VALUES(@nom, @limite, @description)";

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

    }
}
