using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Projet_Gestion_Finance.Classes;
using Projet_Gestion_Finance.classes;
namespace Projet_Gestion_Finance.Classes
{
    public class Depenses: IComparable
    {
		public enum TypeFrequence
        {
            Hebdomadaire,
            Mensuel,
            Annuel,
            Occasionnel
        }
        private string _nom;

		public string Nom
		{
			get { return _nom; }
			set { _nom = value; }
		}
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
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
        private decimal _coutMensuel;
        public decimal CoutMensuel
        {
            get
            {
                if (Frequence == TypeFrequence.Hebdomadaire)
                    return 5*Cout;
                return Cout;

            }
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

        public Depenses(int id, string nom, Categorie cat, decimal cout, DateTime date, TypeFrequence frequence, bool obligatoire)
        {
            Id = id;
            Nom = nom;
            Categorie = cat;
            Cout = cout;
            Date = date;
            Frequence = frequence;
            Obligatoire = obligatoire;
        }
        public Depenses(Depenses depense, DateTime date)   
        {
            Id = depense.Id;
            Nom = depense.Nom;
            Categorie = depense.Categorie;
            Cout = depense.Cout;
            Date = date;
            Frequence = depense.Frequence;
            Obligatoire = depense.Obligatoire;
        }

        public int CompareTo(object? obj)
        {
			if (obj == null)
                return 1;
            if (!(obj is Depenses))
                throw new ArgumentException("obj is not a Depense");
			
            return Date.CompareTo(((Depenses)obj).Date);	
        }
    }
}
