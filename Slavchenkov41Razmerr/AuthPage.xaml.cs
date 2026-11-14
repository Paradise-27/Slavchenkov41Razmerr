using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Slavchenkov41Razmerr
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private bool isCaptchaRequired = false;
        private string currentCaptcha = "";
        public AuthPage()
        {
            InitializeComponent();
        }

        private void GenerateCaptcha()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            char c1 = chars[random.Next(chars.Length)];
            char c2 = chars[random.Next(chars.Length)];
            char c3 = chars[random.Next(chars.Length)];
            char c4 = chars[random.Next(chars.Length)];

            captchaOneWord.Text = c1.ToString();
            captchaTwoWord.Text = c2.ToString();
            captchaThreeWord.Text = c3.ToString();
            captchaFourWord.Text = c4.ToString();

            currentCaptcha = $"{c1}{c2}{c3}{c4}";
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Есть пустые поля");
                return;
            }


            if (isCaptchaRequired && CaptchaInput.Text != currentCaptcha)
            {
                MessageBox.Show("Капча введена неверно! Вход заблокирован на 10 секунд.");
                GenerateCaptcha();
                CaptchaInput.Text = "";

   
                LoginBtn.IsEnabled = false;
                await Task.Delay(10000);
                LoginBtn.IsEnabled = true;
                return;
            }

            User user = Slavchenkov41Entities.GetContext().User.ToList()
                .Find(p => p.UserLogin == login && p.UserPassword == password);

            if (user != null)
            {

                Login.Text = "";
                Password.Text = "";
                CaptchaInput.Text = "";
                isCaptchaRequired = false;
                CaptchaPanel.Visibility = Visibility.Collapsed;
                CaptchaInputPanel.Visibility = Visibility.Collapsed;

                Manager.MainFrame.Navigate(new ProductPage(user));
            }
            else
            {
                MessageBox.Show("Введены неверные данные");

                if (!isCaptchaRequired)
                {

                    isCaptchaRequired = true;
                    CaptchaPanel.Visibility = Visibility.Visible;
                    CaptchaInputPanel.Visibility = Visibility.Visible;
                    GenerateCaptcha();
                }
                else
                {

                    GenerateCaptcha();
                    CaptchaInput.Text = "";

                    LoginBtn.IsEnabled = false;
                    await Task.Delay(10000);
                    LoginBtn.IsEnabled = true;
                }
            }
        }

        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            Login.Text = "";
            Password.Text = "";
            CaptchaInput.Text = "";
            isCaptchaRequired = false;
            CaptchaPanel.Visibility = Visibility.Collapsed;
            CaptchaInputPanel.Visibility = Visibility.Collapsed;

            Manager.MainFrame.Navigate(new ProductPage(null));
        }
    }
}