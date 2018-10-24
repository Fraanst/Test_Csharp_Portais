using ApplicationServices;
using Handlenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using X9.Domain.Helpers;
using X9.Domain.Models;
using X9.Test.Pages;
using X9.Domain.Enums;
using OpenQA.Selenium.Support.UI;
using X9.Domain;

namespace X9.Test.Workflow
{ 
	public class Recovery
	{
		PageBase pagebase = new PageBase();
		SocialNetWorks socialNetworks = new SocialNetWorks();
		Pessoa pessoa = new Pessoa();
		log_navegacao log = new log_navegacao();
		TestPortaisServices service = new TestPortaisServices();
		public string Name = "Test_Recovery";


		public void FullFlow(ChromeDriver _chrome)
		{
			try
			{
				log.data_ini = pagebase.InserTimeIn();
				log.idportal = Convert.ToInt32(Portal.Humana);
				pessoa.CPF = service.GetCPF(log);
				pessoa.Phone = pagebase.InsertPhone();
				if (_chrome.Url.Equals(new Constants().Humana_url))
				{
					pessoa.Email = "recoveryhumana@mailinator.com";
					if (!pagebase.ValidateNotFound(_chrome, log))
						return;
					Login(_chrome);
					Thread.Sleep(4000);
					if (pagebase.ExpectedUrl(_chrome, "pay-info/debt-info"))
					{
						NegotiateDebt(_chrome, log);
						reportpayment(_chrome, log);
						CustomerService(_chrome, log);
					}
					Thread.Sleep(1000);
					AjustClick.ClickByXPath("/html/body/header/div/div[1]/div[1]/a/i", _chrome);
					pagebase.GaveGood(_chrome, pessoa, log);
					AjustClick.ClickByXPath("//*[@id=\"slide-out\"]/li[4]/a", _chrome);
				}
				else if (_chrome.Url.Equals(new Constants().Digital_url))
				{
					log.idportal = Convert.ToInt32(Portal.RecoveryDigital);
					pessoa.Email = "recoveryDigital@mailinator.com";
					pagebase.ValidatePage(_chrome, tipo.XPath, "//*[@id=\"main-message\"]/h1", "Não foi possível encontrar a página deste", log, pessoa);
					Login(_chrome);
					Thread.Sleep(4000);
					if (pagebase.ExpectedUrl(_chrome, "pay-info/debt-info"))
					{
						NegotiateDebt(_chrome, log);
						reportpayment(_chrome, log);
						CustomerService(_chrome, log);
					}
					Thread.Sleep(1000);
					AjustClick.ClickByXPath("/html/body/header/div/div[1]/div[1]/a/i", _chrome);
					pagebase.GaveGood(_chrome, pessoa, log);
					AjustClick.ClickByXPath("//*[@id=\"slide-out\"]/li[4]/a", _chrome);
				}
			}
			catch (Exception e)
			{
				pagebase.Error(_chrome, pessoa, log);
				throw;
			}
		}

		public void Login(ChromeDriver _chrome)
		{
			AjustSendKeys.SendKeysById("cpf", pessoa.CPF, _chrome);
			Thread.Sleep(1000);
			AjustSendKeys.SendKeysById("email", pessoa.Email, _chrome);
			AjustSendKeys.SendKeysById("phone", pessoa.Phone, _chrome);
			AjustClick.ClickById("send-login", _chrome);
		}
		public void NegotiateDebt(ChromeDriver _chrome, log_navegacao log)
		{
			try
			{
				_chrome.FindElementByCssSelector("body > main > section > div > div:nth-child(4) > button:nth-child(1)").Click();
				ValidatePayment(_chrome, log);
				pagebase.GaveGood(_chrome, pessoa, log);
				AjustClick.ClickByXPath("//*[@id=\"btn_voltar\"]", _chrome);
			}
			catch (Exception e)
			{
				_chrome.FindElementByCssSelector("body > main > section > div > div:nth-child(3) > button:nth-child(1)").Click();
				ValidatePayment(_chrome, log);
				pagebase.GaveGood(_chrome, pessoa, log); ;
				AjustClick.ClickByXPath("//*[@id=\"btn_voltar\"]", _chrome);
			}
		}
		public void ValidatePayment(ChromeDriver _chrome, log_navegacao log)
		{
			try
			{
				if (pagebase.ExpectedUrl(_chrome, "pay-info/methods"))
				{
					AjustClick.ClickById("btn_pagamento_a_vista", _chrome);
					if (pagebase.ExpectedUrl(_chrome, "payment-cash"))
						PaymentCash(_chrome);
					else
						pagebase.Fail();
					AjustClick.ClickById("btn_pagamento_parcelado", _chrome);
					if (_chrome.Url.Contains("payment-parcel"))
					{
						if (log.idportal != 8)
						{
							AjustClick.ClickByXPath("/html/body/main/section/div/div[2]/table/tbody/tr[1]/td/div/input", _chrome);
							//WebDriverWait wait = new WebDriverWait(_chrome, TimeSpan.FromSeconds(30));
							//wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("")));
							//_chrome.FindElementByCssSelector("#numero-parcelas-send > option:nth-child(2)").Click();
							//AjustClick.ClickByXPath("//*[@id=\"register-bank-slip\"]/p[1]/label", _chrome);
							//AjustClick.ClickByXPath("//*[@id=\"confirmar-parcelamento\"]", _chrome);
							//AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[3]", _chrome);
							pagebase.GaveGood(_chrome, pessoa, log);
							AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[3]", _chrome);
						}
						else
						{
							AjustClick.ClickByXPath("/html/body/main/section/div/div[2]/table/tbody/tr[1]/td/div/input", _chrome);
							pagebase.GaveGood(_chrome, pessoa, log);
							AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[3]", _chrome);
						}
					}
					else
						pagebase.Fail();
				}
				else
					pagebase.Fail();
			}
			catch (Exception e)
			{
				pagebase.Error(_chrome, pessoa, log);
			}
		}
		public void PaymentCash(ChromeDriver _chrome)
		{
			if (_chrome.Url.Contains("regularizarecovery.com.br"))
			{
				try
				{
					AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[1]", _chrome);
					Thread.Sleep(1000);
					_chrome.FindElementByCssSelector("#modal-mensagem > div.modal-footer > button").Click();
					pagebase.GaveGood(_chrome, pessoa, log);
					AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[3]", _chrome);
				}
				catch (Exception e)
				{
					_chrome.FindElementByCssSelector("body>div.modal-overlay").Click();
					pagebase.GaveGood(_chrome, pessoa, log);
					AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[3]", _chrome);
				}
			}
			else
			{
				try
				{
					AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/a", _chrome);
					Thread.Sleep(1000);
					AjustClick.ClickByXPath("//*[@id=\"register-bank-slip\"]/p[1]/label", _chrome);
					pagebase.GaveGood(_chrome, pessoa, log);
					AjustClick.ClickByXPath("//*[@id=\"enviar-data-pagamento\"]", _chrome);
					_chrome.Navigate().Back();
					AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[2]", _chrome);
				}
				catch (Exception e)
				{
				}
			}

		}
		public void reportpayment(ChromeDriver _chrome, log_navegacao log)
		{
			try
			{
				_chrome.FindElementByCssSelector("body > main > section > div > div:nth-child(4) > button:nth-child(2)").Click();
				_reportpayment(_chrome, log);

			}
			catch (Exception)
			{
				AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[2]", _chrome);
				_reportpayment(_chrome, log);
			}
		}

		public void _reportpayment(ChromeDriver _chrome, log_navegacao log)
		{
			Thread.Sleep(1000);
			if (_chrome.Url.Contains("pay-info/schedule"))
			{
				AjustClick.ClickByXPath("//*[@id=\"form-alega-pagamento\"]/div[1]/div/div[1]/img[2]", _chrome);
				AjustClick.ClickByXPath("//*[@id=\"alega-pagamento-send\"]", _chrome);
				pagebase.GaveGood(_chrome, pessoa, log);
				AjustClick.ClickById("btn-fechar", _chrome);
				AjustClick.ClickByXPath("//*[@id=\"form-alega-pagamento\"]/div[2]/button[2]", _chrome);
			}
			else
				pagebase.Fail();
		}
		public void CustomerService(ChromeDriver _chrome, log_navegacao log)
		{
			try
			{
				if (log.idportal == 8)
				{
					AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[3]", _chrome);
					_Service(_chrome, log);
				}
				else
				{
					AjustClick.ClickByXPath("/html/body/main/section/div/div[3]/button[3]", _chrome);
					_Service(_chrome, log);
				}
			}
			catch (Exception)
			{
				if (log.idportal == 8)
				{
					AjustClick.ClickByXPath("/html/body/main/section/div/div[4]/button[3]", _chrome);
					_Service(_chrome, log);
				}
			}
		}
		public void _Service(ChromeDriver _chrome, log_navegacao log)
		{
			if (pagebase.ExpectedUrl(_chrome, "contact"))
			{
				AjustClick.ClickById("btn_contato_chat", _chrome);
				if (pagebase.ExpectedUrl(_chrome, "virtual/chat/"))
				{
					pagebase.GaveGood(_chrome, pessoa, log);
					_chrome.Navigate().Back();
				}
				else
					pagebase.Fail();
				AjustClick.ClickById("btn_contato_whatsapp", _chrome);
				Thread.Sleep(1000);
				socialNetworks.WhatsApp(_chrome, log);
				if (log.idportal == 8)
				{
					IList<IWebElement> button = _chrome.FindElementsByCssSelector("body > main > section > div > div:nth-child(2) > div");
					foreach (var item in button)
					{
						if (item.Text.Equals("SMS"))
						{
							item.Click();
							Thread.Sleep(2000);
							if (_chrome.Url.Contains("sms"))
							{
								Phone(_chrome, log);
								break;
							}
						}
					}
					IList<IWebElement> button2 = _chrome.FindElementsByCssSelector("body > main > section > div > div:nth-child(2) > div");
					foreach (var item in button2)
					{
						if (item.Text.Equals("Telegram"))
						{
							item.Click();
							Thread.Sleep(2000);
							Telegram(_chrome, log);
							break;
						}
					}
				}
				IList<IWebElement> button3 = _chrome.FindElementsByCssSelector("body > main > section > div > div:nth-child(2) > div");
				foreach (var item in button3)
				{
					if (item.Text.Equals("Facebook"))
					{
						item.Click();
						Thread.Sleep(2000);
						if (_chrome.Url.Contains("facebook"))
						{
							pagebase.GaveGood(_chrome, pessoa, log);
							_chrome.Navigate().Back();
							break;
						}
					}
				}

				AjustClick.ClickById("btn_contato_ligacao", _chrome);
				Thread.Sleep(1000);
				Phone(_chrome, log);
				AjustClick.ClickById("btn_contato_email", _chrome);
				if (_chrome.Url.Contains("email"))
				{
					AjustSendKeys.SendKeysById("nome", Name, _chrome);
					AjustSendKeys.SendKeysById("email", pessoa.Email, _chrome);
					pagebase.GaveGood(_chrome, pessoa, log);
					AjustClick.ClickByXPath("//*[@id=\"form-contato\"]/div[3]/button", _chrome);
					AjustClick.ClickByXPath("/html/body/main/section/div/div[2]/button", _chrome);
				}
				else
					pagebase.Fail();
			}
			else
				pagebase.Fail();
			AjustClick.ClickByXPath("/html/body/main/section/div/div[4]/button[1]", _chrome);
			Thread.Sleep(3000);
		}
		public void Phone(ChromeDriver _chrome, log_navegacao log)
		{
			AjustSendKeys.SendKeysById("nome", Name, _chrome);
			AjustSendKeys.SendKeysById("phone", pessoa.Phone, _chrome);
			pagebase.GaveGood(_chrome, pessoa, log);
			AjustClick.ClickByXPath("//*[@id=\"form-contato\"]/div[3]/button", _chrome);
			AjustClick.ClickByXPath("/html/body/main/section/div/div[2]/button", _chrome);
		}
		public void Telegram(ChromeDriver _chrome, log_navegacao log)
		{
			if (_chrome.Url.Contains("telegram"))
			{
				pagebase.GaveGood(_chrome, pessoa, log);
				for (int i = 0; i < 20; i++)
				{
					_chrome.Navigate().Back();
					if (_chrome.Url.Contains("/contact"))
					{
						break;
					}
				}
			}
			else
				pagebase.Fail();
		}

	}
}
