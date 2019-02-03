using Org.Mentalis.Files;
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
using System.Windows.Shapes;

namespace KOTMaker
{
    /// <summary>
    /// Interaction logic for Saving.xaml
    /// </summary>
    public partial class Saving : Window
    {
        IniReader iniFile;
        string keyValue;

        public Saving(IniReader ini, string section, string key)
        {
            InitializeComponent();
            textBoxSave.Text = section;
            textBoxSave.SelectAll();
            try { textBoxSave.Focus(); }
            catch (Exception) {}
            keyValue = key;
            iniFile = ini;
        }
        
        /// <summary>
        /// Save button
        /// </summary>
        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            iniFile.Write(textBoxSave.Text, "values", keyValue);
            Interface main = Owner as Interface;
            main.refreshList();
            Close();
        }

        /// <summary>
        /// Enter to save
        /// </summary>
        private void textBoxSave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) btnSaveAs_Click(null, null);
        }
    }
}
