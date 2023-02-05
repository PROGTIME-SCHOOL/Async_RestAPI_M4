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

using RestApi_Library;

namespace RestApi_Comic
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int maxNum = 0;
        int currentNum = 0;

        public MainWindow()
        {
            InitializeComponent();

            btnNext.IsEnabled = false;

            ApiHelper.Init();
        }

        private async Task LoadImage(int num = 0)
        {
            // API
            var comic = await WorkProcess.Load(num);

            currentNum = comic.Num;

            if (num == 0)
            {
                maxNum = comic.Num;
            }

            imageComic.Source = new BitmapImage(new Uri(comic.Img));
        }

        private async void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (currentNum > 1)
            {
                currentNum--;
                btnNext.IsEnabled = true;

                await LoadImage(currentNum);

                if (currentNum == 1)
                {
                    btnPrev.IsEnabled = false;
                }
            }
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentNum < maxNum)
            {
                currentNum++;

                btnPrev.IsEnabled = true;

                await LoadImage(currentNum);

                if (currentNum == maxNum)
                {
                    btnNext.IsEnabled = false;
                }
            }


        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadImage();
        }
    }
}
