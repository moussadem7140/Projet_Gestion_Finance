using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace Projet_Gestion_Finance.Models
{
    /// <summary>
    /// Logique d'interaction pour video.xaml
    /// </summary>
    public partial class video : Window
    {
        public video()
        {
            InitializeComponent();
            // Chemin vers la vidéo (relatif au dossier du projet)
            string chemin = "Ressource/ma_video.mp4";

            if (File.Exists(chemin))
            {
                Uri uri = new Uri(Path.GetFullPath(chemin));
                videoPlayer.Source = uri;
                videoPlayer.Play();
            }

        }

        // Rejoue la vidéo automatiquement à la fin
        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Position = TimeSpan.Zero;
            videoPlayer.Play();
        }
    }
}
