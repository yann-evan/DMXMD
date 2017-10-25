using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DmxControlLib.Hardware;
using DmxControlLib.Utility;
using DmxUserControlLib;
using Microsoft.Win32;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace WpfApplication2
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public APC40 _APC40;
        public APC40Mapping _APC40Map;

        public OpenFileDialog opendial;
        public SaveFileDialog savedial; 

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                _APC40 = new APC40();
                _APC40Map = new APC40Mapping("newMap");

                MapName_TextBox.Text = _APC40Map.name;

                _APC40.open();

                _APC40.AnimatedStartAnimation();

                _APC40.LinkMapping(_APC40Map);

                opendial = new OpenFileDialog();
                opendial.Filter = "Mapping Setting (.APC40map)|*.APC40map";


                savedial = new SaveFileDialog();
                savedial.Filter = "Mapping Setting (.APC40map)|*.APC40map";
                savedial.DefaultExt = ".APC40map";
                savedial.AddExtension = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _APC40.resetLed();
            _APC40.close();
        }

        private void ConfigLed_BT_Valid_Click(object sender, EventArgs e)
        {
            if(sender is APC40LedConf)
            {
                APC40LedConf apcconf = sender as APC40LedConf;
                foreach(int sel in MatriceLed.SelectedBT)
                {
                    int ind = _APC40Map.RGBBT.FindIndex(x => x.ID == sel);
                    _APC40Map.RGBBT[ind].Type = apcconf.BTType;
                    _APC40Map.RGBBT[ind].onColor = apcconf.ONRGBColor;
                    _APC40Map.RGBBT[ind].offColor = apcconf.OFFRGBColor;
                    _APC40Map.RGBBT[ind].onFlashing = apcconf.ONFlash;
                    _APC40Map.RGBBT[ind].offFlashing = apcconf.OFFFlash;
                    _APC40Map.RGBBT[ind].Groupe = apcconf.Groupe;
                }

                _APC40.LinkMapping(_APC40Map);
            }
        }

        private void New_Button_Click(object sender, RoutedEventArgs e)
        {
            _APC40Map = new APC40Mapping("newMap");
            _APC40.LinkMapping(_APC40Map);

            MapName_TextBox.Text = _APC40Map.name;
        }

        private void Open_button_Click(object sender, RoutedEventArgs e)
        {
            Nullable<bool> result = opendial.ShowDialog();

            if(result == true)
            {
                BinaryFormatter format = new BinaryFormatter();
                Stream STRE = opendial.OpenFile();

                _APC40Map = (APC40Mapping)format.Deserialize(STRE);

                _APC40.LinkMapping(_APC40Map);
                MapName_TextBox.Text = _APC40Map.name;

                STRE.Close();
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            savedial.FileName = MapName_TextBox.Text;
            Nullable<bool> result = savedial.ShowDialog();

            if(result == true)
            {
                BinaryFormatter format = new BinaryFormatter();
                Stream STRE = savedial.OpenFile();

                format.Serialize(STRE, _APC40Map);

                STRE.Close();

            }
        }
    }
}
