using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Windows.Threading;


namespace WpfAppNagivator
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>

    public partial class Page1 : Page
    {
        private List<string> musicList = new List<string>(); 
       
        public Page1()
        {
            InitializeComponent();
            LoadMusicList();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
            PlayButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Collapsed;
            wplayer.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(wplayer_PlayStateChange);
           
        }
        private WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        public class MusicFileViewModel
        {
            public string FileName { get; set; }
            public DateTime CreationDate { get; set; }
        }
        private String LinkFile()
        {
            string configFilePath = @"C:\Users\84338\AppData\Local\PvmMussicApp\config.txt";
            String LinkFile = File.ReadAllText(configFilePath);
            return LinkFile;
        }
        private void LoadMusicList()
        {
           
            string searchPath = LinkFile(); 
            string[] mp3Files = Directory.GetFiles(searchPath, "*.mp3", SearchOption.AllDirectories);

            foreach (string file in mp3Files)
            {
                string fileName = System.IO.Path.GetFileName(file);
                DateTime creationDate = File.GetCreationTime(file);
                MusicListBox.Items.Add(new MusicFileViewModel { FileName = fileName, CreationDate = creationDate });
                musicList.Add(System.IO.Path.GetFileName(file));
            }
        }
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MusicListBox.SelectedItem != null && MusicListBox.SelectedItem is MusicFileViewModel selectedMusic)
            {
                
                string selectedFilePath = LinkFile()+"\\"+selectedMusic.FileName;
                
                wplayer.URL = selectedFilePath;
                NowPlayingText.Text = selectedMusic.FileName;
                wplayer.controls.play();
                MusicProgressBar.Value = 0;
                timer.Start();
            }
            else
            {
                MessageBox.Show("Please select a music file from the list.");
            }
            PauseButton.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Collapsed;
        }
        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            wplayer.controls.pause();
            NowPlayingText.Text = "";
            timer.Stop();
            PlayButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Collapsed;
        }
        private DispatcherTimer timer;
        private static Uri Source;

        private void wplayer_PlayStateChange(int newState)
        {
           
            if ((WMPLib.WMPPlayState)newState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                PauseButton.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Visible;
                NowPlayingText.Text = "...";
               MusicProgressBar.Value = 0;
                timer.Stop();
            }
        }
    
        private void Timer_Tick(object sender, EventArgs e)
        {

            if (wplayer.currentMedia != null)
            {
                double totalTime = wplayer.currentMedia.duration;
                double currentPosition = wplayer.controls.currentPosition;
                double progress = (currentPosition / totalTime) * 100;
                MusicProgressBar.Value = progress;
            }
        }

        private void NextMusicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MusicListBox.Items.Count > 1)
            {
                int selectedIndex = MusicListBox.SelectedIndex;
                if (selectedIndex < MusicListBox.Items.Count - 1)
                {
                    ListBoxItem nextItem = MusicListBox.ItemContainerGenerator.ContainerFromIndex(selectedIndex + 1) as ListBoxItem;
                    if (nextItem != null)
                    {
                        nextItem.IsSelected = true;
                        MusicListBox.ScrollIntoView(nextItem);

                        if (MusicListBox.SelectedItem is MusicFileViewModel selectedMusic)
                        {
                            string selectedFilePath = LinkFile() + "\\" + selectedMusic.FileName;
                            wplayer.URL = selectedFilePath;
                            NowPlayingText.Text = selectedMusic.FileName;
                            wplayer.controls.play();
                            MusicProgressBar.Value = 0;
                            timer.Start();
                        }
                    }
                }
            }
        }


        private void BackMusicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MusicListBox.Items.Count > 1)
            {
                int selectedIndex = MusicListBox.SelectedIndex;
                if (selectedIndex > 1)
                {
                    ListBoxItem nextItem = MusicListBox.ItemContainerGenerator.ContainerFromIndex(selectedIndex - 1) as ListBoxItem;
                    if (nextItem != null)
                    {
                        nextItem.IsSelected = true;
                        MusicListBox.ScrollIntoView(nextItem);
                        if (MusicListBox.SelectedItem is MusicFileViewModel selectedMusic)
                        {
                            string selectedFilePath = LinkFile() + "\\" + selectedMusic.FileName;
                            wplayer.URL = selectedFilePath;
                            NowPlayingText.Text = selectedMusic.FileName;
                            wplayer.controls.play();
                            MusicProgressBar.Value = 0;
                            timer.Start();
                        }
                    }
                }
            }
        }
        private Random random = new Random();
        private ListBoxItem FindListBoxItemByContent(ListBox listBox, string content)
        {
            foreach (var item in listBox.Items)
            {
                if (item is ListBoxItem listBoxItem && listBoxItem.Content.ToString() == content)
                {
                    return listBoxItem;
                }
            }
            return null;
        }
        private void RandomMusicBtn_Click(object sender, RoutedEventArgs e)
        {
            if (musicList.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, musicList.Count);
                string selectedMusic = musicList[randomIndex];

              
                ListBoxItem randomItem = FindListBoxItemByContent(MusicListBox, selectedMusic);
                if (randomItem != null)
                {
                    randomItem.IsSelected = true;
                    MusicListBox.ScrollIntoView(randomItem);
                }
                string selectedFilePath = @"C:\Users\84338\Music\" + selectedMusic;
                wplayer.URL = selectedFilePath;
                NowPlayingText.Text = selectedMusic;
                wplayer.controls.play();
                MusicProgressBar.Value = 0;
                timer.Start();
            }
            else
            {
                MessageBox.Show("The music list is empty. Please load music files first.");
            }
           

             PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Visible;
        }
    }
    //*********************************************************************//
    public class TextTrimmingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                double maxWidth = 100;
                int maxLength = 70;

                if (text.Length > maxLength)
                {
                    return text.Substring(0, maxLength) + "...";
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SmoothProgressBar : Control
    {
        private double _minimum;
        private double _maximum;
        private double _value;
        private Color _progressBarColor;

        static SmoothProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SmoothProgressBar), new FrameworkPropertyMetadata(typeof(SmoothProgressBar)));
        }

        public SmoothProgressBar()
        {
            _minimum = 0;
            _maximum = 100;
            _value = 0;
            _progressBarColor = Colors.Blue;
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(SmoothProgressBar), new PropertyMetadata(0.0));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }



        public static readonly DependencyProperty ProgressBarColorProperty = DependencyProperty.Register(
            "ProgressBarColor", typeof(Color), typeof(SmoothProgressBar), new PropertyMetadata(Colors.Blue));

        public Color ProgressBarColor
        {
            get { return (Color)GetValue(ProgressBarColorProperty); }
            set { SetValue(ProgressBarColorProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
       "Maximum", typeof(double), typeof(SmoothProgressBar), new PropertyMetadata(100.0));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(SmoothProgressBar), new PropertyMetadata(0.0, OnValueChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // ...

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SmoothProgressBar progressBar = (SmoothProgressBar)d;
            progressBar.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            double percent = (Value - Minimum) / (Maximum - Minimum);

            Rect progressBarRect = new Rect(0, 0, RenderSize.Width * percent, RenderSize.Height);

            drawingContext.DrawRectangle(new SolidColorBrush(ProgressBarColor), null, progressBarRect);
        }
    }
}

