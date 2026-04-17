using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace AutoConnectLinkedIn
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private bool _isPasswordVisible = false;

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                // ẩn pass
                PasswordBox.Password = PasswordTextBox.Text;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                // hiện pass
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                PasswordTextBox.Visibility = Visibility.Visible;
            }

            _isPasswordVisible = !_isPasswordVisible;

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            //string password = PasswordBox.Password;
            string password = _isPasswordVisible
                ? PasswordTextBox.Text
                : PasswordBox.Password;


            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập Email và Password!");
                return;
            }

            // clear UI ngay khi bấm
            //EmailBox.Text = string.Empty;
            //PasswordBox.Password = string.Empty;
            EmailBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;
            PasswordTextBox.Text = string.Empty;

            PasswordTextBox.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Visible;
            _isPasswordVisible = false;
            EmailBox.IsEnabled = false;
            PasswordBox.IsEnabled = false;
            StartButton.IsEnabled = false;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            ChromeDriver driver = new ChromeDriver(options);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 1. Mở LinkedIn
            driver.Navigate().GoToUrl("https://www.linkedin.com/");

            // 2. Login
            wait.Until(d => d.FindElement(By.CssSelector("a[href*='login']"))).Click();

            wait.Until(d => d.FindElement(By.Id("username"))).SendKeys(email);
            wait.Until(d => d.FindElement(By.Id("password"))).SendKeys(password);

            wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

            // 3. Đợi kiểm tra lỗi
            await Task.Delay(4000);

            bool isLoginError = false;
            try
            {
                var errorDiv = driver.FindElement(By.Id("error-for-password"));
                if (errorDiv.Displayed)
                    isLoginError = true;
            }
            catch { }

            // Log fail
            if (isLoginError)
            {
                LogService.LogLogin(email, false);

                driver.Quit();

                MessageBox.Show(
                    "❌ Sai email hoặc mật khẩu. Vui lòng nhập lại!",
                    "Đăng nhập thất bại",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

                EmailBox.IsEnabled = true;
                PasswordBox.IsEnabled = true;
                StartButton.IsEnabled = true;
                return;
            }

            // Log success
            LogService.LogLogin(email, true);

            // 4. My Network
            wait.Until(d => d.FindElement(By.CssSelector("a[href*='mynetwork']"))).Click();
            await Task.Delay(5000);

            // 5. Lấy Connect buttons
            var connectButtons = driver.FindElements(
                By.XPath("//button[.//span[text()='Connect']]")
            );

            Random rd = new Random();
            int target = rd.Next(10, 21);
            int count = 0;

            foreach (var btn in connectButtons)
            {
                if (count >= target)
                    break;

                try
                {
                    var card = btn.FindElement(By.XPath("./ancestor::div[@role='listitem']"));

                    // NAME
                    string name = "";
                    try
                    {
                        name = card
                            .FindElement(By.XPath(".//span[@aria-hidden='true']"))
                            .Text.Trim();
                    }
                    catch { }

                    // PROFILE LINK
                    string profileUrl = "";
                    try
                    {
                        profileUrl = card
                            .FindElement(By.CssSelector("a[href*='/in/']"))
                            .GetAttribute("href");
                    }
                    catch { }

                    string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    // log từng người
                    LogService.LogConnectedPeople(
                        $"{name} | {profileUrl} | {time}"
                    );

                    // click connect
                    ((IJavaScriptExecutor)driver)
                        .ExecuteScript("arguments[0].scrollIntoView(true);", btn);

                    btn.Click();
                    count++;

                    await Task.Delay(rd.Next(1500, 3000));
                }
                catch
                {
                    continue;
                }
            }

            // log tổng số connect
            LogService.LogConnect(email, count);

            await Task.Delay(1000);
            driver.Quit();
            Application.Current.Shutdown();
        }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    //Console.OutputEncoding = System.Text.Encoding.UTF8;
        //    string email = EmailBox.Text;
        //    string password = PasswordBox.Password;

        //    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        //    {
        //        MessageBox.Show("Vui lòng nhập Email và Password!");
        //        return;
        //    }
        //    EmailBox.Text = string.Empty;
        //    PasswordBox.Password = string.Empty;

        //    ChromeOptions options = new ChromeOptions();
        //    options.AddArgument("--start-maximized");

        //    ChromeDriver driver = new ChromeDriver(options);

        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        //    // 1. Mở LinkedIn
        //    driver.Navigate().GoToUrl("https://www.linkedin.com/");

        //    // 2. Nhấn nút "Welcome back hoặc login"
        //    IWebElement loginBtn = wait.Until(d =>
        //        d.FindElement(By.CssSelector("a[href*='login']"))
        //    );
        //    loginBtn.Click();

        //    // 3. Nhập username từ UI
        //    IWebElement emailInput = wait.Until(d => d.FindElement(By.Id("username")));
        //    emailInput.Clear();
        //    emailInput.SendKeys(email);

        //    // 4. Nhập password từ UI
        //    IWebElement passwordInput = wait.Until(d => d.FindElement(By.Id("password")));
        //    passwordInput.Clear();
        //    passwordInput.SendKeys(password);

        //    // 5. (Tuỳ chọn) Bấm nút Sign in
        //    IWebElement signInBtn = wait.Until(d =>
        //        d.FindElement(By.CssSelector("button[type='submit']"))
        //    );
        //    signInBtn.Click();
        //    Console.WriteLine("Đã ấn nút đăng nhập");

        //    // ĐỢI TỐI ĐA 4 GIÂY ĐỂ KIỂM TRA LỖI ĐĂNG NHẬP
        //    await Task.Delay(4000);

        //    bool isLoginError = false;

        //    try
        //    {
        //        // Tìm lỗi đăng nhập
        //        var errorDiv = driver.FindElement(By.Id("error-for-password"));
        //        if (errorDiv.Displayed)
        //        {
        //            isLoginError = true;
        //        }
        //    }
        //    catch
        //    {
        //        // không tìm thấy → nghĩa là không có lỗi
        //    }

        //    // Nếu sai email hoặc mật khẩu
        //    if (isLoginError)
        //    {
        //        LogService.LogLogin(email, false);

        //        driver.Quit();

        //        MessageBox.Show(
        //            "❌ Sai email hoặc mật khẩu. Vui lòng nhập lại!",
        //            "Đăng nhập thất bại",
        //            MessageBoxButton.OK,
        //            MessageBoxImage.Error
        //        );

        //        EmailBox.Text = "";
        //        PasswordBox.Password = "";
        //        return;
        //    }



        //    // 6. **Click "My Network"**
        //    IWebElement myNetwork = wait.Until(d =>
        //        d.FindElement(By.CssSelector("a[href*='mynetwork']"))
        //    );
        //    myNetwork.Click();
        //    Console.WriteLine("đã ấn vào network");


        //    // 7. Chờ 5s
        //    await Task.Delay(5000);

        //    // 8. Lấy danh sách tất cả nút Connect
        //    IReadOnlyCollection<IWebElement> connectButtons = driver.FindElements(By.XPath("//button[.='Connect' or contains(., 'Connect')]"));

        //    // 9. Chọn ngẫu nhiên số lượng cần Connect (10–20)
        //    Random rd = new Random();
        //    int target = rd.Next(10, 21); // từ 10 đến 20

        //    int count = 0;
        //    List<string> connectedPeople = new List<string>();

        //    foreach (var btn in connectButtons)
        //    {
        //        if (count >= target)
        //            break;

        //        try
        //        {
        //            // 1. Lấy card cha (LinkedIn layout mới)
        //            var card = btn.FindElement(By.XPath("./ancestor::div[@role='listitem']"));

        //            // 2. Lấy TÊN người
        //            string name = "";
        //            try
        //            {
        //                var nameEl = card.FindElement(By.XPath(".//span[@aria-hidden='true']"));
        //                name = nameEl.Text.Trim();
        //            }
        //            catch { }

        //            // 3. Lấy LINK profile
        //            string profileUrl = "";
        //            try
        //            {
        //                var linkEl = card.FindElement(By.CssSelector("a[href*='/in/']"));
        //                profileUrl = linkEl.GetAttribute("href");
        //            }
        //            catch { }

        //            // 4. Thời gian
        //            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //            // 5. Format
        //            string line = $"{name} | {profileUrl} | {timeStamp}";
        //            connectedPeople.Add(line);

        //            // 6. Scroll → Connect
        //            ((IJavaScriptExecutor)driver)
        //                .ExecuteScript("arguments[0].scrollIntoView(true);", btn);

        //            btn.Click();
        //            count++;

        //            await Task.Delay(rd.Next(1500, 3000));  // tránh bị detect
        //        }
        //        catch
        //        {
        //            continue;
        //        }
        //    }

        //    // 7. LƯU FILE KHÔNG GHI ĐÈ – APPEND TIẾP DỮ LIỆU
        //    string folder = @"D:\Auto\AutoConnectLinkedIn\";

        //    // tạo folder nếu chưa có
        //    if (!Directory.Exists(folder))
        //        Directory.CreateDirectory(folder);

        //    // file txt
        //    string filePath = Path.Combine(folder, "connected_people.txt");

        //    // ghi nối tiếp
        //    File.AppendAllLines(filePath, connectedPeople);


        //    // 10. Sau khi đủ 10–20 người → tự động thoát
        //    await Task.Delay(1000);

        //    driver.Quit();     // đóng chrome
        //    Application.Current.Shutdown();  // thoát WPF app
        //}
    }
}
