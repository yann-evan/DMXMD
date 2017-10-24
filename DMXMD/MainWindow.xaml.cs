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
                _APC40Map = new APC40Mapping("new Mapping");

                _APC40.open();

                _APC40.AnimatedStartAnimation();

                _APC40.LinkMapping(_APC40Map);

                opendial = new OpenFileDialog();
                opendial.Filter = "Mapping Setting (.map)|*.map";


                savedial = new SaveFileDialog();
                savedial.Filter = "Mapping Setting (.map)|*.map";
                savedial.DefaultExt = ".map";
                savedial.AddExtension = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MatriceLedAPC40_BT_click(object sender, BTClickEventArgs e)
        {
               
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
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
                }

                _APC40.LinkMapping(_APC40Map);
            }
        }
    }
}
