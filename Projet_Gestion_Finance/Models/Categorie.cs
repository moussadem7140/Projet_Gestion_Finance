using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Gestion_Finance.Classes
{
    public class Categorie
    {
		private string _nom;

		public string Nom
		{
			get { return _nom; }
			set { _nom = value; }
		}
		private decimal _limiteDepenses;

		public decimal LimiteDepenses
		{
			get { return _limiteDepenses; }
			set { _limiteDepenses = value; }
		}
		private string _description;

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

        public Categorie(string nom, decimal limiteDepenses)
        {
            Nom = nom;
            LimiteDepenses = limiteDepenses;
            Description = $"{nom}:  ";	
        }
    }
}
