using Projet_Gestion_Finance.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Gestion_Finance.Models
{
    public class Projets
    {
		private int _id;

		public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
		private string _nom;

		public string Nom
		{
			get { return _nom; }
			set { _nom = value; }
		}
		private decimal _cout;

		public decimal Cout
		{
			get { return _cout; }
			set { _cout = value; }
		}
		private Depenses.TypeFrequence _frequence;

		public int Frequence
        {
            get { return (int)_frequence; }
            set { _frequence = (Depenses.TypeFrequence)value; }
        }
		private DateTime _date;

		public DateTime Date
		{
			get { return _date; }
			set { _date = value; }
		}

        public Projets(int id, string nom, DateTime date, decimal cout, int frequence)
        {
			Id = id;
            Nom = nom;
            Cout = cout;
            Frequence = frequence;
            Date = date;
        }
    }
}
