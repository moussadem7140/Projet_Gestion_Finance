using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Gestion_Finance.Classes
{
    public class Depenses: IComparable
    {
		public enum TypeFrequence
        {
            Hebdomadaire,
            Mensuel,
            Annuel
        }
        private string _nom;

		public string Nom
		{
			get { return _nom; }
			set { _nom = value; }
		}
		private decimal _cout;
		private Categorie _categorie;

		public Categorie Categorie
		{
			get { return _categorie; }
			set { _categorie = value; }
		}

		public decimal Cout
		{
			get { return _cout; }
			set { _cout = value; }
		}
		private DateTime _date;

		public DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}
		private TypeFrequence _frequence;

		public TypeFrequence Frequence
		{
			get { return _frequence; }
			set { _frequence = value; }
		}
		private bool _obligatoire;

		public bool Obligatoire
		{
			get { return _obligatoire; }
			set { _obligatoire = value; }
		}
		public Depenses()
		{

		}
        public Depenses(string nom, Categorie cat, decimal cout, DateTime date, TypeFrequence frequence, bool obligatoire)
        {
            Nom = nom;
            Categorie = cat;
            Cout = cout;
            Date = date;
            Frequence = frequence;
            Obligatoire = obligatoire;
        }

        public int CompareTo(object? obj)
        {
			if (obj == null)
                return 1;
            if (!(obj is Depenses))
                throw new ArgumentException("obj is not a Depenses");
			
            return Date.CompareTo(((Depenses)obj).Date);	
        }
    }
}
