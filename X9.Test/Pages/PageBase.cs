using ApplicationServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using X9.Domain.Enums;
using System.Configuration;
using X9.Domain.Helpers;
using System.Text.RegularExpressions;
using System.Threading;
using X9.Domain.Models;
using Handlenium;
using X9.Domain;

namespace X9.Test.Pages
{
	public partial class PageBase
	{
		tipo tipo = new tipo();
		TestPortaisServices services = new TestPortaisServices();
		private string _Erro = ConfigurationManager.AppSettings["screenshots"];
		Pessoa pessoa = new Pessoa();
		log_navegacao Log = new log_navegacao();

	
		public int _cont { get; set; }

		public bool fail { get; set; }
		public ChromeDriver OpenChrome()
		{
			try
			{
				ChromeOptions options = new ChromeOptions();
				options.AddArguments("test-type");
				options.AddArguments("start-maximized");
				options.AddArguments("--js-flags=--expose-gc");
				options.AddArguments("--enable-precise-memory-info");
				options.AddArguments("--disable-popup-blocking");
				options.AddArguments("--disable-default-apps");
				options.AddArguments("test-type=browser");
				options.AddArguments("disable-infobars");

				this.chromeDriver = new ChromeDriver(driverPath, options);
				this.chromeDriver.Manage().Window.Maximize();
				this.chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
				return chromeDriver;
			}
			catch (Exception e)
			{
				return null;
			}
		}
		public string InsertPhone()
		{
			return "41900000000";
		}
		public string InserTimeIn()
		{
			return  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}
		public string InserTimeFim()
		{
			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		public void GaveGood(ChromeDriver _chrome, Pessoa pessoa, log_navegacao log)
		{
			log.cpf = pessoa.CPF;
			log.idstatus = 1;
			log.data_fim = InserTimeFim();
			log.pagina = _chrome.Url.ToString();
			services.InsertLog(log);
		}
		
		public void Error(ChromeDriver _chrome, Pessoa pessoa, log_navegacao log)
		{
			try
			{
				log.cpf = pessoa.CPF;
				log.idstatus = 2;
				log.data_fim = InserTimeFim();
				log.pagina = _chrome.Url.ToString();
				var s = _chrome.GetScreenshot();
				string Id = services.InserLog(log).ToString();
				s.SaveAsFile(_Erro + "\\" + Id + "_Erro.Jpeg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
			}
			catch (Exception e)
			{
				if(log.idportal == Convert.ToInt32(Portal.skyprepago))
				{
					log.pagina = "ganhemaiscomoprepago.com.br";
					services.InsertLog(log);
				}
			}
		}
		public void Error404(ChromeDriver _chrome, Pessoa pessoa, log_navegacao log)
		{
			log.cpf = pessoa.CPF;
			log.idstatus = 3;
			log.data_fim = InserTimeFim();
			log.pagina = _chrome.Url.ToString();
			var s = _chrome.GetScreenshot();
			string Id = services.InserLog(log).ToString();
			s.SaveAsFile(_Erro + "\\" + Id + "_SiteForadoar.Jpeg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
		}

		public void Fail()
		{
			throw new Exception();
		}


		public void CloseChrome(ChromeDriver chromeDriver)
		{
			chromeDriver.Close();
		}

		public WebDriverWait Wait(IWebDriver webDriver)
		{

			return new WebDriverWait(webDriver, TimeSpan.FromSeconds(120));
		}
		public bool ExpectedUrl(ChromeDriver _chrome, string Url)
		{
			WebDriverWait wait = new WebDriverWait(_chrome, TimeSpan.FromSeconds(30));
			wait.Until(condition: SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains(Url));
			return true;
		}
		public bool ValidateNotFound(ChromeDriver _chrome, log_navegacao log)
		{
			if (!ValidatePage(_chrome, tipo.Id, "header", "Server Error", log, pessoa))
				return false;
			if (!ValidatePage(_chrome, tipo.XPath, "/html/body/h2", "Not Found", log, pessoa))
				return false;
			if (!ValidatePage(_chrome, tipo.XPath, "/html/body/h2", "Service Unavailable", log, pessoa))
				return false;
			if (!ValidatePage(_chrome, tipo.Id, "main-message", "Não é possível acessar esse site", log, pessoa))
				return false;
			if (!ValidatePage(_chrome, tipo.XPath, "//*[@id=\"L_10060_2\"]", "Network Access Message:", log, pessoa))
				return false;
			if (!ValidatePage(_chrome, tipo.XPath, "//*[@id=\"frame-line-0\"]/div[1]/span[1]/div", "Twig_Error_Runtime", log, pessoa))
				return false;
			if (!ValidatePage(_chrome, tipo.Id, "titulo", "Página não encontrada", log, pessoa))
				return false;
			if (!ValidatePage(_chrome, tipo.XPath, "//*[@id=\"main-message\"]/h1", "Não foi possível encontrar a página deste", log, pessoa))
				return false;
			if (!ValidatePage(_chrome, tipo.Id, "header", "Erro do Servidor", log, pessoa))
				return false;

			return true;
		}
		public bool ExpectedElement(ChromeDriver _chrome, string Element, tipo tipo)
		{
			WebDriverWait wait = new WebDriverWait(_chrome, TimeSpan.FromSeconds(30));
			switch (tipo)
			{
				case tipo.ClassName:
					wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName(Element)));
					return true;
				case tipo.Id:
					wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id(Element)));
					return true;
				case tipo.Name:
					wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Name(Element)));
					return true;
				case tipo.TagName:
					wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.TagName(Element)));
					return true;
				case tipo.XPath:
					wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(Element)));
					return true;
				case tipo.CssSelector:
					wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(Element)));
					return true;
				case tipo.LinkText:
					wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.LinkText(Element)));
					return true;
			}

			return false;
		}
		public bool ValidatePage(ChromeDriver _chrome, tipo tipo, string titulo, string text, log_navegacao log , Pessoa pessoa)
		{
			try
			{
				_chrome.Navigate().GoToUrl(log.pagina);
				log.cpf = pessoa.CPF;
				var pagenotfound = (dynamic)null;
				switch (tipo)
				{
					case tipo.ClassName:
						pagenotfound = AjustElement.AjustExceptionsByClassName(titulo, _chrome);
						break;
					case tipo.Id:
						pagenotfound = AjustElement.AjustExceptionsById(titulo, _chrome);
						break;
					case tipo.Name:
						pagenotfound = AjustElement.AjustExceptionsByName(titulo, _chrome);
						break;
					case tipo.TagName:
						pagenotfound = AjustElement.AjustExceptionsByTagName(titulo, _chrome);
						break;
					case tipo.XPath:
						pagenotfound = AjustElement.AjustExceptionsByXPath(titulo, _chrome);
						break;
					case tipo.CssSelector:
						pagenotfound = _chrome.FindElementByCssSelector(titulo);
						break;
					case tipo.LinkText:
						pagenotfound = _chrome.FindElementByLinkText(titulo);
						break;
				}

				if (pagenotfound.Text.Contains(text))
				{
					log.Exception = "Página Fora do Ar:" + _chrome.Url.ToString(); ;
					Error404(_chrome, pessoa, log);
					return false;
				}
				return false;
			}
			catch (Exception e)
			{
				return true;
			}
		}

	}

	
	public partial class PageBase
	{
		public PageBase() { }

		public PageBase(ChromeDriver chromeDriver) { }

		ChromeDriver chromeDriver;

		private string driverPath = ConfigurationManager.AppSettings["chromedriverPath"];

	}


}
