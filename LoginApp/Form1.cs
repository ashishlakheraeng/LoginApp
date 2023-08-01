using CsvHelper;
using CsvHelper.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WebDriverManager;

namespace LoginApp
{
    public partial class Form1 : Form
    {
        const string pwdKey = "wmdLoginPass";
        private IWebDriver _driver;

        public Form1()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Save_1(object sender, EventArgs e)
        {
            LoginDetail loginDetail = new LoginDetail();
            List<LoginDetail> listLoginDetail = new List<LoginDetail>();
            if (this.textBox1.Text != string.Empty && this.textBox2.Text != string.Empty)
            {
                loginDetail = new LoginDetail()
                {
                    UserName = this.textBox1.Text,
                    Password = this.textBox2.Text,
                    //WebSite = this.label4.Text,
                    WebSite = "https://practicetestautomation.com/practice-test-login/",
                };
                loginDetail.Password = EncryptDecrypt.Encrypt(loginDetail.Password, pwdKey);
            }
            listLoginDetail.Add(loginDetail);

            using (var writer = new StreamWriter("LoginInformation1.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(listLoginDetail);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Save_2(object sender, EventArgs e)
        {
            LoginDetail loginDetail = new LoginDetail();
            List<LoginDetail> listLoginDetail = new List<LoginDetail>();

            if (this.textBox3.Text != string.Empty && this.textBox4.Text != string.Empty)
            {
                loginDetail = new LoginDetail()
                {
                    UserName = this.textBox3.Text,
                    Password = this.textBox4.Text,
                    WebSite = this.label5.Text,
                };
                loginDetail.Password = EncryptDecrypt.Encrypt(loginDetail.Password, pwdKey);
            }

            listLoginDetail.Add(loginDetail);
            using (var writer = new StreamWriter("LoginInformation2.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(listLoginDetail);
            }
        }

        private void login_1(object sender, EventArgs e)
        {
            string csvFile = "LoginInformation1.csv";
            string userName, password, webSite;
            ReadCSVFile(csvFile, out userName, out password, out webSite);
            OpenWebUrl(webSite, userName, password);
        }

        private static void ReadCSVFile(string csvFile, out string userName, out string password, out string webSite)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Comment = '#',
                AllowComments = true,
                Delimiter = ",",
            };
            userName = string.Empty;
            password = string.Empty;
            webSite = string.Empty;
            using (var streamReader = File.OpenText(csvFile))
            using (var csvReader = new CsvReader(streamReader, csvConfig))
            {
                while (csvReader.Read())
                {
                    userName = csvReader.GetField(0);
                    password = csvReader.GetField(1);
                    webSite = csvReader.GetField(2);
                }
            }
            password = EncryptDecrypt.Decrypt(password, pwdKey);
        }

        private void login_2(object sender, EventArgs e)
        {

        }

        public void OpenWebUrl(string url, string userName, string password)
        {
            new DriverManager().SetUpDriver(
    "https://chromedriver.storage.googleapis.com/2.25/chromedriver_win32.zip",
    Path.Combine(Directory.GetCurrentDirectory(), "chromedriver.exe"),
    "chromedriver.exe"
);
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl(url);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10000);

            IWebElement inputAccount = _driver.FindElement(By.Id("username"));
            Thread.Sleep(2000);
            inputAccount.Clear();
            Thread.Sleep(2000);
            inputAccount.SendKeys(userName);
            Thread.Sleep(2000);

            IWebElement inputPassword = _driver.FindElement(By.Id("password"));

            inputPassword.Clear();
            Thread.Sleep(2000);
            inputPassword.SendKeys(password);
            Thread.Sleep(2000);

            //IWebElement submitButton = driver.FindElement(By.XPath("/html/body/div[2]/form/table/tbody/tr[4]/td[2]/input"));
            IWebElement submitButton = _driver.FindElement(By.Id("submit"));
            Thread.Sleep(2000);
            submitButton.Click();
            Thread.Sleep(2000);

            //driver.Quit();
        }
    }
}
