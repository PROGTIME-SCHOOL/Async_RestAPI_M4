using System;
using System.Collections.Generic;
using System.Diagnostics;                   // stopwatch
using System.Linq;
using System.Net;
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

namespace Async_RestAPI_M4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSync_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            LoadDataSync();

            stopwatch.Stop();

            var time = stopwatch.ElapsedMilliseconds;

            textBlockInfo.Text += $"\n\nTotal time: {time}";
        }

        private async void ButtonAsync_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            //await LoadDataAsync();   // lock
            await LoadDataAsyncParallel();  

            stopwatch.Stop();

            var time = stopwatch.ElapsedMilliseconds;

            textBlockInfo.Text += $"\n\nTotal time: {time}";
        }

        public async Task LoadDataAsync()
        {
            List<string> sites = PrepareLoadSites();

            foreach (var item in sites)
            {
                DataModel dataModel = await Task.Run(() => LoadSite(item));  // lock
                PrintInfo(dataModel);
            }

            
        }

        public async Task LoadDataAsyncParallel()
        {
            List<string> sites = PrepareLoadSites();

            List<Task<DataModel>> tasks = new List<Task<DataModel>>();

            foreach (var item in sites)
            {
                tasks.Add(Task.Run(() => LoadSite(item))); 
            }

            DataModel[] dataModels = await Task.WhenAll(tasks);   // lock

            foreach (var item in dataModels)
            {
                PrintInfo(item);
            }
        }

        public void LoadDataSync()
        {
            List<string> sites = PrepareLoadSites();

            foreach (var item in sites)
            {
                DataModel dataModel = LoadSite(item);
                PrintInfo(dataModel);
            }
        }

        private void PrintInfo(DataModel dataModel)
        {
            textBlockInfo.Text += $"\nUrl: {dataModel.Url}, Length: {dataModel.Data.Length}";
        }

        private List<string> PrepareLoadSites()
        {
            List<string> sites = new List<string>()
            {
                "https://google.com",
                "https://my.progtime.net"
                // ....
            };

            return sites;
        }

        private DataModel LoadSite(string site)
        {
            DataModel dataModel = new DataModel();

            dataModel.Url = site;

            WebClient webClient = new WebClient();
            dataModel.Data = webClient.DownloadString(site);

            // обновление дизайна из другого потока
            Dispatcher.BeginInvoke((Action)(() =>
            {
                textBlockInfo.Text = "Info";
            }));

            return dataModel;
        }
    }
}
