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

    using System.Windows.Threading;
    namespace flappy_bird_2
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
        {
            DispatcherTimer GameTimer = new DispatcherTimer();

            double speed = 1.0;
            double Points;
            int Gravity = 8;
            bool Over;
            Rect supermanBox;

            public MainWindow()
            {
                InitializeComponent();
                GameTimer.Tick += MainEventTimer;
                GameTimer.Interval = TimeSpan.FromMilliseconds(20);
                StartGame();
            }
            //method event trong game
            private void MainEventTimer(object sender, EventArgs e)
            {
                //label cho điểm
                txtPoints.Content = "Points: " + Points;
                //khởi tạo nhân vật superman
                supermanBox = new Rect(Canvas.GetLeft(superman), Canvas.GetTop(superman), superman.Width - 10, superman.Height);
                //set vị trí superman ở giữa
                Canvas.SetTop(superman, Canvas.GetTop(superman) + Gravity);
                //nếu superman bay cao quá hoặc rớt thì die
                if (Canvas.GetTop(superman) < -30 || Canvas.GetTop(superman) > 460)
                {
                    EndGame();
                }
                //move pipe
                foreach (var x in Mygame.Children.OfType<Image>())
                {
                //time increase speed
                if (GameTimer.Interval.TotalSeconds % 10 == 0)
                {
                    speed += 0.2; // the increment value as needed
                }
                if (GameTimer.Interval.TotalSeconds % 10 == 0)
                {
                    speed = Math.Min(speed + 0.1, 8.0); // đảm bảo không tốc độ không tăng quá mức 
                }
                if ((string)x.Tag == "obs1" ||  (string)x.Tag == "obs2" || (string)x.Tag == "obs3")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - (5 * speed));

                        if (Canvas.GetLeft(x) < -100) 
                        {
                            Canvas.SetLeft(x, 800);
                            //points get each pipe is 0.5 
                            Points += .5; 
                        }
                        //Rect: xử lý va chạm các hình khác
                        //tạo đối tượng mới biểu diễn hình hộp va chạm 
                    Rect PillarHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                        if (supermanBox.IntersectsWith(PillarHitBox))
                        {
                            EndGame();
                        }
                        if ((string)x.Tag == "cloud")
                        {

                            Canvas.SetLeft(x, Canvas.GetLeft(x) - 8);

                            if (Canvas.GetLeft(x) < -250)
                            {
                                Canvas.SetLeft(x, 550);

                                Points += .5;
                            }
                        }
                    }
                }
            }

            //method khi bay len
            private void Down(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Space)
                {
                    superman.RenderTransform = new RotateTransform(-20, superman.Width / 2, superman.Height / 2);
                    Gravity = -8;
                }

                if (e.Key == Key.R && Over == true)
                {
                //dong nay de tat hien thi game over
                Ending.Visibility = Visibility.Collapsed;
                FinalPoints.Visibility = Visibility.Collapsed;
                reset.Visibility = Visibility.Collapsed;
                StartGame();
                }
            }
            //method bay
            private void Up(object sender, KeyEventArgs e)
            {
                //chuyển gốc độ khi nhân vật rơi xuống 
                superman.RenderTransform = new RotateTransform(5, superman.Width / 2, superman.Height / 2);
                Gravity = 8;
            }

            //bat dau vao tro choi
            private void StartGame()
            {

                Mygame.Focus();

                int temp = 300;

                Points = 0;

                Over = false;

                Canvas.SetTop(superman, 190);

            //duyệt qua các hình ảnh trong Mygame 
                foreach (var x in Mygame.Children.OfType<Image>())
                {
                    //set vị trí của các pipe
                    if ((string)x.Tag == "obs1")
                    {
                        Canvas.SetLeft(x, 500);
                    }
                    if ((string)x.Tag == "obs2")
                    {
                        Canvas.SetLeft(x, 800);
                    }
                    if ((string)x.Tag == "obs3")
                    {
                        Canvas.SetLeft(x, 1100);
                    }

                    if ((string)x.Tag == "clouds")
                    {
                        Canvas.SetLeft(x, 400 + temp);
                        temp = 800;
                    }

                    GameTimer.Start();
                }
            }

        //method kết thúc game  
        private void EndGame()
        {
            //hiển thị hình ảnh game over và điểm số đạt được 
            Ending.Visibility = Visibility.Visible;
            FinalPoints.Visibility = Visibility.Visible;
            FinalPoints.Content = $"Your highest points:{Points}";
            reset.Visibility = Visibility.Visible;
            GameTimer.Stop();
            Over = true;
           
        }
    }
}
