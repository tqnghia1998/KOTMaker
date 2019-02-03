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
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Org.Mentalis.Files;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace KOTMaker
{
    public partial class Interface : Window
    {
        #region Import from C++
        [StructLayout(LayoutKind.Sequential)]
        public struct tagPOINT
        {
            public uint X;
            public uint Y;
        }
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(uint X, uint Y);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out tagPOINT pPoint);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr window);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int GetPixel(IntPtr dc, uint x, uint y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string className, string windowName);
        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);
        #endregion

        IntPtr hDC = GetWindowDC(GetDesktopWindow());
        List<TextBox> ListTextBox = new List<TextBox>();
        IniReader ini;

        public Interface()
        {
            InitializeComponent();
            ListTextBox.Add(textBox01);
            ListTextBox.Add(textBox02);
            ListTextBox.Add(textBox03);
            ListTextBox.Add(textBox04);
            ListTextBox.Add(textBox05);
            ListTextBox.Add(textBox06);
            ListTextBox.Add(textBox07);
            ListTextBox.Add(textBox08);
            ListTextBox.Add(textBox09);
            ListTextBox.Add(textBox10);
            ListTextBox.Add(textBox11);
            ListTextBox.Add(textBox12);
            ListTextBox.Add(textBox13);
            ListTextBox.Add(textBox14);
            ListTextBox.Add(textBox15);
            ListTextBox.Add(textBox16);
            ListTextBox.Add(textBox17);
            ListTextBox.Add(textBox18);
            ListTextBox.Add(textBox19);
            ListTextBox.Add(textBox20);
            ListTextBox.Add(textBox21);
            ListTextBox.Add(textBox22);
            ListTextBox.Add(textBox23);
            ListTextBox.Add(textBox24);
            ListTextBox.Add(textBox25);
            ListTextBox.Add(textBox26);
            ListTextBox.Add(textBox27);
            ListTextBox.Add(textBox28);
            ListTextBox.Add(textBox29);
            ListTextBox.Add(textBox30);
            ini = new IniReader(System.IO.Path
                .GetDirectoryName(Assembly
                .GetEntryAssembly().Location)
                + @"\marcos.ini");
            refreshList();
            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.RealTime;
        }

        /// <summary>
        /// Refresh list of marco
        /// </summary>
        public void refreshList()
        {
            listView.Items.Clear();
            IEnumerator e = ini.GetSectionNames().GetEnumerator();
            while (e.MoveNext()) listView.Items.Add(e.Current);
        }

        /// <summary>
        /// Increase or decrease value by mouse wheel
        /// </summary>
        private void textBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var textBox = sender as TextBox;
            int value = 0;
            int.TryParse(textBox.Text, out value);
            value += (e.Delta > 0) ? 5 : -5;
            textBox.Text = (value == 0) ? string.Empty : value.ToString();
        }

        /// <summary>
        /// TextBox only accept number value
        /// </summary>
        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        /// <summary>
        /// Auto enable or disable TextBox
        /// </summary>
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            var nameTBx = textBox.Name.Substring(7);
            int indexTB = int.Parse(nameTBx);
            var isEnabl = textBox.Text.Length > 0;
            var destiNa = !isEnabl ? 30 : indexTB < 30 ? indexTB + 1 : 30;
            while (indexTB < destiNa) ListTextBox[indexTB++].IsEnabled = isEnabl;
        }

        /// <summary>
        /// Auto empty TextBox if double click
        /// </summary>
        private void textBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // (sender as TextBox).Text = string.Empty;
        }

        /// <summary>
        /// Choose a marco
        /// </summary>
        private void listView_Selected(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedItem == null) return;
            string section = listView.SelectedItem.ToString();

            string strValues = ini.ReadString(section, "values");
            string[] token = strValues.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            int i = -1;
            while (++i < token.Length) ListTextBox[i].Text = token[i];
            for (int j = 29; i <= j; j--) ListTextBox[j].Text = string.Empty;
        }

        /// <summary>
        /// Open ini file
        /// </summary>
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(ini.Filename);
        }

        /// <summary>
        /// Refresh list marco
        /// </summary>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            refreshList();
        }

        /// <summary>
        /// Save a marco
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string keyValue = string.Empty;
            for (int i = 0; i < 30; i++)
            {
                if (ListTextBox[i].Text.Length == 0) break;
                keyValue += ListTextBox[i].Text + ',';
            }
            if (keyValue.Length > 0)
            {
                keyValue = keyValue.Remove(keyValue.Length - 1);
            }
            Saving saveWindow = new Saving(ini, null != listView.SelectedItem
                ? listView.SelectedItem.ToString() : string.Empty, keyValue);
            saveWindow.Owner = this;
            saveWindow.Show();
        }

        /// <summary>
        /// Run a marco
        /// </summary>
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Focus to KOT
            IntPtr handle = FindWindow(null, "King of Thieves");
            SetForegroundWindow(handle);

            // Get values from TextBoxes
            int[] values = new int[30];
            for (int i = 0; i < 30; i++)
            {
                int defaultValue = 0;
                if (ListTextBox[i].IsEnabled) int.TryParse(ListTextBox[i].Text, out defaultValue);
                values[i] = defaultValue;
            }

            // Initial variables
            int index = 0;
            tagPOINT cursor;
            Stopwatch stopwatch = new Stopwatch();

            // Wait for shift key
            Thread beginThread = new Thread(delegate () {
                while (System.Windows.Forms.Control.ModifierKeys != System.Windows.Forms.Keys.Shift);
            });
            beginThread.Start();
            beginThread.Join();

            // Click to start
            GetCursorPos(out cursor);
            SetCursorPos(cursor.X - 80, cursor.Y - 80);
            int oldPixel = GetPixel(hDC, cursor.X, cursor.Y);
            mouse_event(0x08 | 0x10, 0, 0, 0, 0);
            
            // Immeadiatly first click (Spinner Close To Wall)
            if (values[index] < 0)
            {
                Thread.Sleep(-values[index++]);
                mouse_event(0x08 | 0x10, 0, 0, 0, 0);
            }

            // Wait for thief to run
            while (oldPixel == GetPixel(hDC, cursor.X, cursor.Y)) stopwatch.Restart();
            int init = (int) stopwatch.ElapsedMilliseconds;

            // Calculate delay
            values[index] = Math.Abs(values[index] - init + (init > 24 ? 24 : 8));

            // Running
            Thread checkThread = null;
            Thread runThread = new Thread(delegate () {
                while (values[index] > 0)
                {
                    while (stopwatch.ElapsedMilliseconds <= values[index]);
                    stopwatch.Restart();
                    mouse_event(0x08 | 0x10, 0, 0, 0, 0);
                    if (++index == 30) break;
                }
                if (checkThread != null) checkThread.Abort();
            });
            runThread.Start();

            // Check if want to stop
            checkThread = new Thread(delegate () {
                while (System.Windows.Forms.Control.ModifierKeys != System.Windows.Forms.Keys.Control);
                runThread.Abort();
            });
            checkThread.Start();
            textBox30.Text = init.ToString();
        }
    }
}