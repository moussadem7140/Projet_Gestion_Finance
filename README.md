# Projet_Gestion_Finance

> Application de gestion des finances personnelles (WPF / C# / MySQL)

---

## Présentation du projet

**Projet_Gestion_Finance** est une application de bureau développée en **C# avec WPF** (.NET 8).
Elle permet à un utilisateur de gérer ses finances personnelles en suivant :

- Ses **dépenses** (ponctuelles, hebdomadaires, mensuelles ou annuelles), réparties par catégories avec des plafonds de budget.
- Ses **projets d'épargne** (objectif financier, montant mis de côté périodiquement, progression).
- Son **profil** (revenu mensuel, identifiants, mot de passe sécurisé).

Des **graphiques dynamiques** (LiveCharts) offrent une vue synthétique des catégories de dépenses par rapport à leurs limites ainsi que de la répartition revenu/dépenses/projets.

---

## Stack technique et prérequis

| Technologie | Version |
|---|---|
| .NET / C# | 8.0 (net8.0-windows) |
| WPF | inclus dans .NET 8 Windows |
| MySQL | 8.x (serveur local) |
| MySqlConnector | 2.4.0 |
| Konscious.Security.Cryptography.Argon2 | 1.3.1 |
| LiveCharts.Wpf | 0.9.7 |
| Microsoft.Extensions.Configuration.Json | 9.0.2 |

**Prérequis :**

- Windows 10/11 (application WPF uniquement Windows)
- .NET 8 SDK ou Runtime installé
- Serveur MySQL local (ou distant)
- Visual Studio 2022+ recommandé (ou Rider / VS Code + CLI)

---

## Installation et exécution

### 1. Cloner le dépôt

```bash
git clone https://github.com/moussadem7140/Projet_Gestion_Finance.git
cd Projet_Gestion_Finance
```

### 2. Configurer la base de données

Créez la base de données MySQL et les tables nécessaires. Un exemple de script SQL minimal :

```sql
CREATE DATABASE IF NOT EXISTS gestion_Finance;
USE gestion_Finance;

CREATE TABLE utilisateurs (
    idutilisateurs INT AUTO_INCREMENT PRIMARY KEY,
    nom            VARCHAR(100) NOT NULL,
    prenom         VARCHAR(100) NOT NULL,
    mdp            TEXT NOT NULL,
    identifiant    VARCHAR(50) NOT NULL UNIQUE,
    mail           VARCHAR(150),
    salt           TEXT NOT NULL,
    Revenu         DECIMAL(15,2) DEFAULT 0
);

CREATE TABLE categorie (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    Utilisateur INT NOT NULL,
    Nom         VARCHAR(100) NOT NULL,
    Limite      DECIMAL(15,2) NOT NULL,
    Description TEXT,
    FOREIGN KEY (Utilisateur) REFERENCES utilisateurs(idutilisateurs)
);

CREATE TABLE depenses (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    Utilisateur INT NOT NULL,
    Nom         VARCHAR(100) NOT NULL,
    Cout        DECIMAL(15,2) NOT NULL,
    Categorie   INT NOT NULL,
    Date        DATETIME NOT NULL,
    Frequence   VARCHAR(20) NOT NULL,
    Obligatoire TINYINT(1) NOT NULL DEFAULT 0,
    FOREIGN KEY (Utilisateur) REFERENCES utilisateurs(idutilisateurs),
    FOREIGN KEY (Categorie) REFERENCES categorie(Id)
);

CREATE TABLE projets (
    Id          INT AUTO_INCREMENT PRIMARY KEY,
    Utilisateur INT NOT NULL,
    Nom         VARCHAR(100) NOT NULL,
    Date        DATETIME NOT NULL,
    Objectif    DECIMAL(15,2) NOT NULL,
    Cout        DECIMAL(15,2) NOT NULL,
    Frequence   VARCHAR(20) NOT NULL,
    FOREIGN KEY (Utilisateur) REFERENCES utilisateurs(idutilisateurs)
);
```

### 3. Configurer la connexion

Modifiez le fichier `Projet_Gestion_Finance/Classes/appsettings.json` :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;port=3306;Database=gestion_Finance;Uid=VOTRE_UTILISATEUR;Pwd='VOTRE_MOT_DE_PASSE'"
  }
}
```

### 4. Compiler et lancer

```bash
cd Projet_Gestion_Finance
dotnet build Projet_Gestion_Finance.sln /p:EnableWindowsTargeting=true
dotnet run --project Projet_Gestion_Finance/Projet_Gestion_Finance.csproj
```

Ou ouvrez la solution `Projet_Gestion_Finance.sln` dans Visual Studio et appuyez sur **F5**.

---

## Structure du projet

```
Projet_Gestion_Finance/
├── App.xaml / App.xaml.cs          # Point d'entrée WPF
├── login.xaml / login.xaml.cs      # Fenêtre de connexion
├── MainWindow.xaml / .cs           # Gestion des dépenses et catégories
│
├── Classes/
│   ├── Dal.cs                      # Couche d'accès aux données (toutes les requêtes SQL)
│   ├── Utils.cs                    # Utilitaires (hachage Argon2id, encodage Base64)
│   ├── UtilEnum.cs                 # Extensions d'énumération, écriture de fichier
│   └── appsettings.json            # Chaîne de connexion MySQL
│
├── Models/
│   ├── Categorie.cs                # Modèle catégorie de dépenses
│   ├── Depenses.cs                 # Modèle dépense (avec TypeFrequence)
│   ├── Projets.cs                  # Modèle projet d'épargne
│   ├── Utilisateur.cs              # Modèle utilisateur
│   ├── EtatFormulaire.cs           # Énumération des états de formulaire
│   ├── FormCategorie.xaml / .cs    # Formulaire CRUD catégorie
│   ├── FormDepense.xaml / .cs      # Formulaire CRUD dépense
│   ├── FormProjets.xaml / .cs      # Liste et gestion des projets
│   ├── FormManipulationProjet.xaml/.cs # Formulaire CRUD projet
│   ├── FrmErreur.xaml / .cs        # Fenêtre d'erreur / confirmation / notification
│   └── video.xaml / .cs            # Lecteur vidéo intégré
│
├── Views/
│   ├── Accueil.xaml / .cs          # Tableau de bord principal
│   └── Inscription.xaml / .cs      # Formulaire d'inscription / modification de profil
│
└── Ressource/
    ├── Icons/                      # Icônes de l'interface
    ├── argent.wav / bienvenu.wav   # Sons de navigation
    ├── ma_video.mp4                # Vidéo tutoriel intégrée
    └── ...                         # Images de fond
```

---

## Fonctionnalités implémentées

### 🔐 Authentification sécurisée

- **Connexion** : identification par un identifiant unique auto-généré (3 premières lettres du nom + 3 premières du prénom, en majuscules) et un mot de passe.
- **Mot de passe haché** avec l'algorithme **Argon2id** (via Konscious.Security.Cryptography) et un sel (SALT) cryptographique unique par utilisateur.
- Validation des champs (identifiant vide, inexistant, mot de passe incorrect).

### 📝 Inscription et gestion du profil

- **Inscription** d'un nouveau compte : saisie du nom, prénom, e-mail, revenu mensuel et mot de passe (avec confirmation).
- Génération automatique de l'identifiant de connexion.
- Validation complète : longueur minimale des champs, format e-mail, correspondance des mots de passe, revenu positif.
- **Modification du profil** : le formulaire d'inscription est réutilisé, pré-rempli avec les données existantes.

### 📂 Gestion des catégories

- **Créer**, **modifier** et **supprimer** des catégories de dépenses.
- Chaque catégorie possède un **nom**, une **limite mensuelle** (budget plafond) et une **description**.
- Validation : la somme de toutes les limites de catégories ne peut pas dépasser le revenu mensuel lors de la création.
- La suppression d'une catégorie est bloquée si elle contient des dépenses.

### 💸 Gestion des dépenses

- **Ajouter**, **modifier** et **supprimer** des dépenses.
- Chaque dépense est caractérisée par :
  - **Nom**
  - **Catégorie** (liste des catégories existantes)
  - **Coût**
  - **Date** de début
  - **Fréquence** : `Hebdomadaire`, `Mensuel`, `Annuel`, `Occasionnel`
  - **Obligatoire** (oui/non)
- Contrôles métier lors de l'ajout/modification :
  - La dépense ne peut pas dépasser la limite mensuelle de la catégorie.
  - Les dépenses récurrentes sont vérifiées sur le long terme (viabilité mensuelle).
- Validation des champs : nom (lettres uniquement), coût (entier positif), date, fréquence et catégorie obligatoires.
- Pour les dépenses **occasionnelles**, la date ne peut pas être dans le passé.

### 🔄 Développement des dépenses récurrentes

L'algorithme `DepensesPeriodes` étend automatiquement les dépenses récurrentes sur une période :
- **Hebdomadaire** : une occurrence par semaine dans la période.
- **Mensuel** : une occurrence par mois.
- **Annuel** : une occurrence par an.
- **Occasionnel** : occurrence unique à la date exacte.

### 🔍 Recherche et filtrage des dépenses

- Sélection d'une **période** (date de début / date de fin) via des sélecteurs de date.
- Filtrage par **catégorie** (ou toutes les catégories).
- Recherche par **nom** (insensible à la casse).

### 🖨️ Export des dépenses

- Génération d'un fichier texte **`Depenses.txt`** dans le répertoire d'exécution.
- Le fichier liste les dépenses de la période sélectionnée, regroupées par catégorie, avec nom, coût et date.

### 📊 Graphiques et tableau de bord

**Tableau de bord (Accueil) :**
- **Graphique en colonnes** : limite budgétaire vs dépenses réelles pour chaque catégorie du mois sélectionné.
- **Graphique en camembert** : répartition entre la marge libre, les dépenses et les projets (basée sur le revenu réel de l'utilisateur).
- Sélection d'une période personnalisée.

**Module Projets :**
- **Graphique en colonnes** : objectif vs avancement de chaque projet à une date choisie.

### 🎯 Gestion des projets d'épargne

- **Créer**, **modifier** et **supprimer** des projets.
- Chaque projet est défini par :
  - **Nom**
  - **Date** de début de l'épargne
  - **Objectif** (montant cible)
  - **Mise de côté** (`Cout`) par période
  - **Fréquence** : `Hebdomadaire`, `Mensuel`, `Annuel` (pas d'occasionnel)
- Calcul automatique de l'**avancement** (total mis de côté depuis la date de début jusqu'à aujourd'hui).
- Calcul automatique du **reste à atteindre** et de la **progression** en pourcentage.
- Validation : la mise de côté ne peut pas dépasser le budget disponible (revenu − somme des limites de catégories).

### 🎥 Tutoriel vidéo intégré

- Lecture en boucle d'une vidéo d'aide (`Ressource/ma_video.mp4`) depuis le tableau de bord.

---

## Fonctionnement global

### Flux utilisateur

```
Lancement
    │
    ▼
[Connexion]  ──── (nouveau compte) ──→  [Inscription]
    │                                         │
    │◄────────────────────────────────────────┘
    ▼
[Tableau de bord (Accueil)]
    │              │              │
    ▼              ▼              ▼
[Dépenses /    [Projets]     [Profil /
 Catégories]                 Déconnexion]
```

1. **Connexion / Inscription** : l'utilisateur s'authentifie ou crée un compte. Le mot de passe est haché avec Argon2id avant tout stockage.
2. **Tableau de bord** : affiche un résumé graphique des finances du mois courant (ou d'une période personnalisée).
3. **Gestion des dépenses** (MainWindow) : liste des dépenses et catégories avec CRUD complet, recherche et export.
4. **Gestion des projets** : suivi des objectifs d'épargne avec avancement calculé automatiquement.
5. **Déconnexion** : retour à la fenêtre de connexion.

### Flux des données

```
[UI (XAML / code-behind)]
        │  appels statiques
        ▼
    [Dal.cs]  ──────────────────► [Base MySQL]
        │
        ▼
  [Modèles : Utilisateur, Categorie, Depenses, Projets]
        │
        ▼
  [Utils.cs / UtilEnum.cs]  (hachage, encodage, export fichier)
```

---

## Configuration et données

### Fichier `Classes/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;port=3306;Database=gestion_Finance;Uid=<user>;Pwd='<password>'"
  }
}
```

> ⚠️ Ce fichier contient des identifiants de connexion. Ne le committez jamais avec de vraies valeurs dans un dépôt public. Ajoutez-le à `.gitignore` ou utilisez des variables d'environnement en production.

### Identifiant utilisateur

L'identifiant est généré automatiquement : **3 premières lettres du nom + 3 premières lettres du prénom** (en majuscules).
Exemple : Jean Dupont → `DUPSEA` → identifiant `DUPJEA`.

### Hachage Argon2id

- Parallélisme : 8 threads
- Itérations : 4
- Mémoire : 1 Go (1 048 576 Ko)

> L'authentification peut prendre quelques secondes en raison du coût intentionnel d'Argon2id.

---

## Points d'attention

| Point | Description |
|---|---|
| **Windows uniquement** | Application WPF, non portable sur macOS/Linux |
| **MySQL requis** | Aucune base embarquée ; un serveur MySQL doit être accessible |
| **Argon2id lent** | Délai intentionnel à la connexion (~1-2 s selon le matériel) |
| **Export texte** | Le fichier `Depenses.txt` est écrasé à chaque export (pas d'historique) |
| **Identifiant unique** | Deux utilisateurs avec le même début de nom/prénom auront un conflit d'identifiant |
| **appsettings.json** | Contient les credentials MySQL en clair ; à protéger en environnement de production |

---

## Pistes d'amélioration futures

1. **Gestion des conflits d'identifiant** : ajouter un suffixe numérique si l'identifiant est déjà pris.
2. **Validation du format e-mail** : utiliser une expression régulière ou la classe `MailAddress`.
3. **Internationalisation** : supporter les montants avec décimales (ex. `1234,56 $`).
4. **Thème sombre / clair** : améliorer l'expérience utilisateur.
5. **Notifications** : alerter l'utilisateur quand il approche d'une limite de catégorie.
6. **Exportation PDF / Excel** : remplacer l'export texte par un format plus riche.
7. **Graphiques interactifs** : afficher les détails au survol des barres/secteurs.
8. **Sécurisation de la configuration** : stocker la chaîne de connexion chiffrée ou via variables d'environnement.
9. **Tests unitaires** : couvrir la logique métier de `Dal.cs` et des modèles.
10. **Mode hors ligne** : mettre en cache les données localement (SQLite) pour une utilisation sans réseau.
