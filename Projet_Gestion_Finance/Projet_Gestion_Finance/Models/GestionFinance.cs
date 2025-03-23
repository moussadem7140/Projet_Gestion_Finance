using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Gestion_Finance.Models
{
    public class GestionFinance
    {
		private Dictionary<string, byte[]> _dicoSalts;

		public Dictionary<string, byte[]> Dicosalts
		{
			get { return _dicoSalts; }
			set { _dicoSalts = value; }
		}

		private Dictionary<string,Utilisateur> _dicoUtilisateurs;

		public Dictionary<string,Utilisateur> DicoUtilisateurs
		{
			get { return _dicoUtilisateurs; }
			set { _dicoUtilisateurs = value; }
		}

        public GestionFinance()
        {
            DicoUtilisateurs = new Dictionary<string, Utilisateur>();
            Dicosalts = new Dictionary<string, byte[]>();
        }

        public GestionFinance(Dictionary<string, byte[]> dicosalts, Dictionary<string, Utilisateur> dicoUtilisateurs)
        {
            Dicosalts = dicosalts;
            DicoUtilisateurs = dicoUtilisateurs;
        }
    }
}
