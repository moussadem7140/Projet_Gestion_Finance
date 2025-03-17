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

        public Depenses.TypeFrequence Frequence
        {
            get { return _frequence; }
            set { _frequence = value; }
        }
        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        private decimal _objectif;

        public decimal Objectif
        {
            get { return _objectif; }
            set { _objectif = value; }
        }

        public Projets(int id, string nom, DateTime date, decimal objectif, decimal cout, Depenses.TypeFrequence frequence)
        {
            Id = id;
            Nom = nom;
            Cout = cout;
            Objectif = objectif;
            Frequence = frequence;
            Date = date;
        }
        public decimal Avancement(DateTime date)
        {
            decimal avancement = 0;
            if (Frequence == Depenses.TypeFrequence.Mensuel)
            {
                int i = 0;
                while (date.AddMonths(i) <= Date)
                {
                    avancement += Cout;
                    i++;
                }
            }
            if (Frequence == Depenses.TypeFrequence.Hebdomadaire)
            {
                int i = 0;
                while (date.AddDays(i * 7) <= Date)
                {
                    avancement += Cout;
                    i++;
                }
            }
            else
            {
                int i = 0;
                while (date.AddYears(i) <= Date)
                {
                    avancement += Cout;
                    i++;
                }
            }
            return avancement;
        }
    }
}