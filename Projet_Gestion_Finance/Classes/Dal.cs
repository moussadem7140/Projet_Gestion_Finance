using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projet_Gestion_Finance.Models;
using Microsoft.Extensions.Configuration;
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


    }
}
