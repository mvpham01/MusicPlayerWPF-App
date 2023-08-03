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
using System.IO;
using System.Globalization;
using static WpfAppNagivator.Page1;

namespace WpfAppNagivator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string LinkFile = @"C:\Users\84338\Music";
        public MainWindow()
        {
            InitializeComponent();
            string configFilePath = @"C:\Users\84338\AppData\Local\PvmMussicApp\config.txt";
            if (File.Exists(configFilePath))
            {

                LinkFile = File.ReadAllText(configFilePath);
            }
            else
            {
                LinkFile = @"C:\Users\84338\Music";

                File.WriteAllText(configFilePath, LinkFile, Encoding.UTF8);
            }
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            click1(sender, e);
        }

        private void click1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page1();
        }
        private void click2(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new Page2();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public class CreationDateProperty : DependencyObject
        {
            public static readonly DependencyProperty CreationDatePropertyProperty =
                DependencyProperty.RegisterAttached("CreationDateProperty", typeof(DateTime), typeof(CreationDateProperty));

            public static void SetCreationDate(DependencyObject element, DateTime value)
            {
                element.SetValue(CreationDatePropertyProperty, value);
            }

            public static DateTime GetCreationDate(DependencyObject element)
            {
                return (DateTime)element.GetValue(CreationDatePropertyProperty);
            }
        }
       
    }
}
