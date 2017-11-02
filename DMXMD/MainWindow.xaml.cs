using System;
using System.Windows;
using DmxControlLib.Hardware;
using DmxControlLib.Utility;
using DmxUserControlLib;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

namespace WpfApplication2
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// APC40
        /// </summary>
        public APC40 _APC40;

        /// <summary>
        /// Mapping de l'apc40
        /// </summary>
        public APC40Mapping _APC40Map;

        /// <summary>
        /// Fenetre d'ouverture de fichier
        /// </summary>
        public OpenFileDialog opendial;

        /// <summary>
        /// fenetre de sauvegarde de fichier
        /// </summary>
        public SaveFileDialog savedial; 


        /// <summary>
        /// MAIN WIN
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Log.open();
            Log.writeLine(DateTime.Now.ToString());

            bool connexionOK = false;
            while(!connexionOK)
            {
                try
                {
                    _APC40 = new APC40();                                                   //Init APC40
                    _APC40Map = new APC40Mapping("newMap");                                 //Init Mapping APC40

                    MapName_TextBox.Text = _APC40Map.name;                                  //récuperation du nom de la sauvegarde

                    _APC40.open();                                                          //Connexion de l'APC40

                    _APC40.AnimatedStartAnimation();                                        //Animation de l'APC40

                    _APC40.LinkMapping(_APC40Map);                                          //Link entre l'APC40 et le mapping

                    opendial = new OpenFileDialog();                                        //Nouvelle fenetre d'ouverture de fichier
                    opendial.Filter = "Mapping Setting (.APC40map)|*.APC40map";             //Afficher que les fichier .APC40map


                    savedial = new SaveFileDialog();                                        //Nouvelle fenetre de sauvegarde de fichier
                    savedial.Filter = "Mapping Setting (.APC40map)|*.APC40map";             //Afficher que les fichier .APC40map
                    savedial.DefaultExt = ".APC40map";                                      //Extension du fichier
                    savedial.AddExtension = true;                                           //Ajout de l'extension dans tout les cas

                    connexionOK = true;
                }
                catch (Exception ex)
                {
                    //Si Erreur
                    if (MessageBox.Show(ex.Message + "\nréessayer ?", "Erreur", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        connexionOK = false;
                    }
                    else
                    {
                        Environment.Exit(16);
                    }
                }
            }
        }

        /// <summary>
        /// Si Fermeture de la fenetre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (_APC40 != null)
                {
                    if (_APC40.IsOpen())
                    {
                        _APC40.resetLed(); //RAZ des leds
                        _APC40.close();    //deconnexion de l'apc40
                    }
                }
            }
            catch
            {
                MessageBox.Show("Erreur pendant la fermeture");
            }
                    
        }

        /// <summary>
        /// Si Appui sur le bouton Valide du module de configuration des leds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigLed_BT_Valid_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender is APC40LedConf)
                {
                    APC40LedConf apcconf = sender as APC40LedConf;  //Recuperation de toute les information du module de configuration des leds
                    foreach (int sel in MatriceLed.SelectedBT)      //tri des info
                    {
                        int ind = _APC40Map.RGBBT.FindIndex(x => x.ID == sel);
                        _APC40Map.RGBBT[ind].Type = apcconf.BTType;
                        _APC40Map.RGBBT[ind].Groupe = apcconf.Groupe;

                        _APC40Map.RGBBT[ind].offprimaryColor = apcconf.OFFRGBPrimaryColor;
                        _APC40Map.RGBBT[ind].offsecondaryColor = apcconf.OFFRGBSecondaryColor;
                        _APC40Map.RGBBT[ind].offFlashingtype = apcconf.OFFBlinkingType;
                        _APC40Map.RGBBT[ind].offFlashingspeed = apcconf.OFFBlinkingSpeed;

                        _APC40Map.RGBBT[ind].onprimaryColor = apcconf.ONRGBPrimaryColor;
                        _APC40Map.RGBBT[ind].onsecondaryColor = apcconf.ONRGBSecondaryColor;
                        _APC40Map.RGBBT[ind].onFlashingtype = apcconf.ONBlinkingType;
                        _APC40Map.RGBBT[ind].onFlashingspeed = apcconf.ONBlinkingSpeed;
                    }

                    _APC40.LinkMapping(_APC40Map);  //envoi vers le mapping de l'apc40
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Impossible de Communiquer avec l'APC40\nL'application va s'arreter");
                Environment.Exit(15);
            }
            
        }

        /// <summary>
        /// Si appui sur le bouton de nouveau mapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _APC40Map = new APC40Mapping("newMap");
                _APC40.LinkMapping(_APC40Map);

                MapName_TextBox.Text = _APC40Map.name;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur pendant la creation d'un nouveau mapping :\n" + ex.Message + "\nveuillez réesayer");
            }
        }

        /// <summary>
        /// Si appui sur le nouton de'ouverture d'un mapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Nullable<bool> result = opendial.ShowDialog();

                if (result == true)
                {
                    BinaryFormatter format = new BinaryFormatter();
                    Stream STRE = opendial.OpenFile();

                    _APC40Map = (APC40Mapping)format.Deserialize(STRE);

                    _APC40.LinkMapping(_APC40Map);
                    MapName_TextBox.Text = _APC40Map.name;

                    STRE.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erreur pendant l'ouverture d'un nouveau mapping :\n" + ex.Message + "\nveuillez réesayer");
            }
            
        }

        /// <summary>
        /// Si appui sur le bouton de sauvegarde d'un mapping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                savedial.FileName = MapName_TextBox.Text;
                Nullable<bool> result = savedial.ShowDialog();

                if (result == true)
                {
                    BinaryFormatter format = new BinaryFormatter();
                    Stream STRE = savedial.OpenFile();

                    format.Serialize(STRE, _APC40Map);

                    STRE.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur pendant l'ouverture d'un nouveau mapping :\n" + ex.Message + "\nveuillez réesayer");
            }
            
        }
    }
}
 