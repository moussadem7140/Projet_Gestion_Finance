using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Gestion_Finance.Classes
{
    public class Categorie
    {
		private int _id;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}
		private List<Depenses> _listeDepenses;

		public List<Depenses> ListeDepenses
		{
			get { return _listeDepenses; }
			set { _listeDepenses = value; }
		}

		private decimal _coutTotal;

		public decimal CoutTotal
		{
			get { return _coutTotal; }
			set { _coutTotal = value; }
		}
        private decimal _coutTotalMensuel;

        public decimal CoutTotalMensuel
		{
			get { return _coutTotalMensuel; }
			set { _coutTotalMensuel = value; }
		}
        public decimal Marge
        {
            get
            {
                return LimiteDepenses - CoutTotal;
            }
        }

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
