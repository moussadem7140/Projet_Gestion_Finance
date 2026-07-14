using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;

namespace Projet_Gestion_Finance.Classes
{
    public static class Utils
    {
        /// <summary>
        /// Permet de générer le SALT pour un nouvel utilisateur
        /// </summary>
        /// <returns>Un SALT</returns>
        public static byte[] CreerSALT()
        {
            byte[] buffer = new byte[16];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(buffer);
            return buffer;
        }

        /// <summary>
        /// Combine un mot de passe clair + un salt et retourne une concaténation hachée du mot de passe
        /// Cet appel est lourd, car il s'exécute en multi threads (Bloc mémoire de 1 Go)
        /// </summary>
        /// <param name="password">Mot de passe clair d'un utilisateur</param>
        /// <param name="salt">SALT d'un utilisateur</param>
        /// <returns>Hash du mot de passe (mot de passe haché)</returns>
        public static byte[] HacherMotDePasse(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 1024 * 1024
            };

            return argon2.GetBytes(16);
        }

        /// <summary>
        /// Reteste le hachage de la concaténation d'un mot de passe clair + SALT et compare avec une version hachée fournie
        /// Si les 2 correspondent, c'est que le mot de passe clair + SALT correspondent à la version hachée fournie,
        /// donc le mot de passe correspond (true) ou pas (false)
        /// </summary>
        /// <param name="password">Mot de passe en clair</param>
        /// <param name="salt">SALT</param>
        /// <param name="motDePasseDicoUtilisateur">Mot de passe provenant du dictionnaire d'utilisateurs</param>
        /// <returns></returns>
        public static bool EstMotDePasseCorrespond(string password, byte[] salt, byte[] motDePasseDicoUtilisateur)
        {
            return motDePasseDicoUtilisateur.SequenceEqual(HacherMotDePasse(password, salt));
        }
        /// <summary>
        /// Convertit un tableau de bytes en chaîne de caractères Base64
        /// </summary>
        /// <param name="salt">Tableau de bytes</param>
        /// <returns>Chaîne de caractères Base64</returns>
        public static string ConvertirByteSaltEnString(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }
        /// <summary>
        /// Convertit une chaîne de caractères Base64 en tableau de bytes
        /// </summary>
        /// <param name="saltString">Chaîne de caractères Base64</param>
        /// <returns>Tableau de bytes</returns>
        public static byte[] ConvertirStringEnByteSalt(string saltString)
        {
            return Convert.FromBase64String(saltString);
        }

    }
}
