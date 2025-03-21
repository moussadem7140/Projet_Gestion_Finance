using Projet_Gestion_Finance.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Gestion_Finance.Models
{
    public class Utilisateur
    {
		private decimal _revenue;

		public decimal Revenue
		{
			get { return _revenue; }
			set { _revenue = value; }
		}

		private string _nom;

		public string Nom
		{
			get { return _nom; }
			set {
				if(string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom ne peut pas être vide");
                }
                _nom = value.Trim(); }
		}

		private string _prenom;

		public string Prenom
		{
			get { return _prenom; }
			set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le prénom ne peut pas être vide");
                }
                _prenom = value.Trim(); }
		}

		private string _mail;

		public string Mail
		{
			get { return _mail; }
			set { _mail = value; }
		}

		private string _id;

		public string Id
		{
			get { return _id; }
			set {
				if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("L'identifiant ne peut pas être vide");
                }
                _id = value;
            }
		}

		private List<Depenses> _lstDepenses;

		public List<Depenses> LstDepenses
		{
			get { return _lstDepenses; }
			set { _lstDepenses = value; }
		}

		private byte[] _mdp;

        public byte[] MDP
		{
			get { return _mdp; }
			set { _mdp = value; }
		}
		/// <summary>
		/// Constructeur sans paramètre d'un utilisateur
		/// </summary>
        public Utilisateur()
        {

        }

        public Utilisateur(string nom, string prenom, string id, byte[] mDP,string mail)
        {
            Nom = nom;
            Prenom = prenom;
            Id = id;
            MDP = mDP;
            Mail = mail;
            LstDepenses = new List<Depenses>();
            Revenue = 0;
        }
    }
}
