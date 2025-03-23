using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Gestion_Finance.Classes
{
    public class Categorie
    {
		/// <summary>
		///Identifiant unique
		/// </summary>
		private int _id;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}
		/// <summary>
		/// Liste de depenses
		/// </summary>
		private List<Depenses> _listeDepenses;

		public List<Depenses> ListeDepenses
		{
			get { return _listeDepenses; }
			set { _listeDepenses = value; }
		}
		/// <summary>
		/// Le coût total des dépenses
		/// </summary>
		private decimal _coutTotal;

		public decimal CoutTotal
		{
			get { return _coutTotal; }
			set { _coutTotal = value; }
		}
     /// <summary>
	 /// Marge disponible pour les nouvelles dépenses
	 /// </summary>
        public decimal Marge
        {
            get
            {
                return LimiteDepenses - CoutTotal;
            }
        }
		/// <summary>
		/// Nom de la catégorie
		/// </summary>
        private string _nom;

		public string Nom
		{
			get { return _nom; }
			set { _nom = value; }
		}
		/// <summary>
		/// Limites dépenses
		/// </summary>
		private decimal _limiteDepenses;

		public decimal LimiteDepenses
		{
			get { return _limiteDepenses; }
			set { _limiteDepenses = value; }
		}
		/// <summary>
		/// Description de la dépense
		/// </summary>
		private string _description;

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}
	/// <summary>
	/// constructeur total
	/// </summary>
	/// <param name="id"></param>
	/// <param name="nom"></param>
	/// <param name="limiteDepenses"></param>
	/// <param name="description"></param>
        public Categorie(int id, string nom, decimal limiteDepenses, string description)
        {
			Id = id;
            Nom = nom;
            LimiteDepenses = limiteDepenses;
            Description = description;
			ListeDepenses= new List<Depenses>();
        }
		
        //public override string ToString()
        //{
        //	return $"{Nom.PadRight(30)}{LimiteDepenses,22:C2}";
        //      }
    }
}
