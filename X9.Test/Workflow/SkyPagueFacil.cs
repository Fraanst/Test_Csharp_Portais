using ApplicationServices;
using Handlenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using X9.Test.Pages;
using X9.Domain.Helpers;
using X9.Domain.Enums;
using X9.Domain.Models;
using X9.Domain;

namespace X9.Test.Workflow
{
	public class SkyPagueFacil
	{
		PageBase PaginaBase = new PageBase();
		SocialNetWorks socialNetworks = new SocialNetWorks();
		log_navegacao log = new log_navegacao();
		Pessoa pessoa = new Pessoa();
		TestPortaisServices service = new TestPortaisServices();
		tipo tipo = new tipo();


		public int Discount { get; set; }

		public void FullFlow(ChromeDriver chromeDriver)
		{
			try
			{
				log.idportal = Convert.ToInt32(Portal.sky);
				log.data_ini = PaginaBase.InserTimeIn();
				log.pagina = new Constants().Sly_url;
				if(!PaginaBase.ValidateNotFound(chromeDriver, log))
					return;
				AuxFluxo(chromeDriver);
				Thread.Sleep(1000);
				WithDiscount(chromeDriver);
				EfetuarPagamento(chromeDriver);
				PaymentMade(chromeDriver);
				PrevisãoPagamento(chromeDriver);
				Atendimento(chromeDriver);
				AjustClick.ClickByXPath("//*[@id=\"index-banner\"]/div[1]/div/div[1]/a/i", chromeDriver);
				PaginaBase.GaveGood(chromeDriver, pessoa, log);
				AjustClick.ClickByXPath("//*[@id=\"slide-out\"]/li[6]/a", chromeDriver);
			}
			catch (Exception e)
			{
				if (e.Message.Equals("Close")) { }
				else
					PaginaBase.Error(chromeDriver, pessoa, log);
				throw;
			}
		}
		public void AuxFluxo(ChromeDriver chromeDriver)
		{
			try
			{
				pessoa.CPF = service.GetCPF(log);
				chromeDriver.Navigate().GoToUrl(new Constants().Sly_url);
				Thread.Sleep(1000);
				AjustSendKeys.SendKeysById("cpf", pessoa.CPF, chromeDriver);
				AjustClick.ClickById("form-send", chromeDriver);
				Validate(chromeDriver, pessoa);
			}
			catch (Exception e)
			{
			}
		}

		public void Validate(ChromeDriver chromeDriver, Pessoa pessoa)
		{
			PaginaBase.ExpectedUrl(chromeDriver, "sky");
			Thread.Sleep(3000);
			if (chromeDriver.Url.Contains("enter/options"))
			{
				log.data_ini = PaginaBase.InserTimeIn();
			}
			else 
			{
				Thread.Sleep(1000);
				service.DeleteCPf(pessoa, Portal.sky);
				chromeDriver.Url = Regex.Replace(chromeDriver.Url, "com.br/", "com.br/CPF_Encontrado_Invalido.");
				PaginaBase.Error(chromeDriver, pessoa, log);
				Thread.Sleep(1000);
				AuxFluxo(chromeDriver);
			}

		}
		public void WithDiscount(ChromeDriver chromeDriver)
		{
			IWebElement table = chromeDriver.FindElementByCssSelector("#fade-1 > div:nth-child(1) > div > div > table > tbody > tr:nth-child(2) > td:nth-child(2) > b");
			if (table.Text.Equals("R$ 1,00") || table.Text.Equals("R$ 10,00"))
			{
				Discount = 0;
			}
			else
				Discount = 1;
		}
		public void EfetuarPagamento(ChromeDriver _chrome)
		{
			try
			{
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(2)").Click();
				PaymentMethods(_chrome);
				_chrome.Navigate().Back();
			}
			catch (Exception e)
			{
				PaginaBase.Error(_chrome, pessoa, log);
				throw;
			}
		}
		public void CreditCard(ChromeDriver _chrome)
		{
			AjustClick.ClickById("credit-card", _chrome);
			Thread.Sleep(1000);
			if (PaginaBase.ExpectedUrl(_chrome, "virtual/chat/"))
				PaginaBase.GaveGood(_chrome, pessoa, log);
			else
				PaginaBase.Fail();
			_chrome.Navigate().Back();
		}
		public void lotterypayment(ChromeDriver _chrome, string xpath)
		{
			Thread.Sleep(1000);
			_chrome.FindElementByCssSelector(xpath).Click();
			try
			{
				PaginaBase.ExpectedElement(_chrome, "//*[@id=\"modal-10611\"]/div/h4", tipo.XPath);
				AjustClick.ClickByXPath("//*[@id=\"modal-10611\"]/div/a/i", _chrome);
				//add ex informando que esta n pagina receber boleto
				PaginaBase.GaveGood(_chrome, pessoa, log);
				_chrome.Navigate().Back();
			}
			catch (Exception)
			{
				Thread.Sleep(1000);
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(1)").Click();
				AjustClick.ClickByXPath("//*[@id=\"informar-dados\"]/div[2]/div[1]/div[1]/label[1]", _chrome);
				if (PaginaBase.ExpectedElement(_chrome, "email", tipo.Id))
				{
					_chrome.FindElementByCssSelector("#informar-dados > div.modal-content > div:nth-child(2) > div.col.l12.m12.s12.left-align > label:nth-child(2)").Click();
					AjustSendKeys.SendKeysById("email", "a@a.com", _chrome);
					AjustSendKeys.SendKeysById("telefone", "41999999999", _chrome);
					PaginaBase.GaveGood(_chrome, pessoa, log);
					_chrome.FindElementByCssSelector("#informar-dados > div.modal-content > div:nth-child(3) > div > button").Click();
					if (PaginaBase.ExpectedUrl(_chrome, "return-payment")) { }
					else
						PaginaBase.Fail();
				}
				else
					PaginaBase.Fail();

				for (int i = 0; i < 2; i++)
				{
					_chrome.Navigate().Back();
				}
			}
		}

		public void Boleto(ChromeDriver _chrome, string xpath)
		{
			_chrome.FindElementByCssSelector(xpath).Click();
			if (_chrome.Url.Contains("contact/"))
			{
				Thread.Sleep(1000);
				_chrome.Navigate().Back();
			}
			else
				_chrome.Navigate().Back();
		}

		public void Installment(ChromeDriver _chrome)
		{
			IList<IWebElement> line = _chrome.FindElementsByCssSelector("#fade-1 > div.row > div > a");
			foreach (var a in line)
			{
				if (a.Text.Equals("PAGAMENTO PARCELADO"))
				{
					a.Click();
					break;
				}
			}
			//Adicionar excpetion informando que está na pagina do pag parcelamento 
			PaginaBase.GaveGood(_chrome, pessoa, log);
			_chrome.Navigate().Back();
		}
		public void PaymentMethods(ChromeDriver _chrome)
		{
			try
			{
				Thread.Sleep(1000);
				if (Discount == 1)
				{
					CreditCard(_chrome);
					Installment(_chrome);
					lotterypayment(_chrome, "#fade-1 > div.row > div > a:nth-child(4)");
				}
				else
				{
					CreditCard(_chrome);
					lotterypayment(_chrome, "#fade-1 > div.row > div > a:nth-child(3)");
					Boleto(_chrome, "#fade-1 > div.row > div > a:nth-child(4)");
				}
				PaginaBase.GaveGood(_chrome, pessoa, log);
			}
			catch (Exception e)
			{
				PaginaBase.Error(_chrome, pessoa, log);
				throw;
			}

		}
		public void PaymentMade(ChromeDriver _chrome)
		{
			try
			{
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(3)").Click();
				_chrome.FindElementByCssSelector("#fade-1 > div.row.data-pagamento > div > div:nth-child(2) > img.arrow.down.down-day").Click();
				AjustClick.ClickById("send-date", _chrome);
				Thread.Sleep(1000);
				_chrome.FindElementByCssSelector("#action-modal").Click();
				if (PaginaBase.ExpectedUrl(_chrome, "/return-register-date"))
					PaginaBase.GaveGood(_chrome, pessoa, log);
				else
					PaginaBase.Fail();
				for (int i = 0; i < 2; i++)
				{
					_chrome.Navigate().Back();
				}
			}
			catch (Exception)
			{
				PaginaBase.Error(_chrome, pessoa, log);
				throw;
			}
		}
		public void PrevisãoPagamento(ChromeDriver _chrome)
		{
			try
			{
				if (PaginaBase.ExpectedUrl(_chrome, "enter/options"))
				{
					_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(4)").Click();
				}
				#region Pagamento Em Outra Data
				else
					PaginaBase.Fail();
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(2)").Click();
				PaginaBase.GaveGood(_chrome, pessoa, log);
				AjustClick.ClickById("send-another-date", _chrome);
				Thread.Sleep(2000);
				if (PaginaBase.ExpectedUrl(_chrome, "info/methods")) { }
				else
					PaginaBase.Fail();
				Thread.Sleep(1000);
				for (int i = 0; i < 2; i++)
				{
					_chrome.Navigate().Back();
				}
				#endregion

				#region Problemas Técnicos
				//Escolher Outro Seletor
				IList<IWebElement> Link = _chrome.FindElements(By.CssSelector("#fade-1 > div:nth-child(2) > div > a"));
				foreach (var i in Link)
				{
					if (i.Text.Equals("PROBLEMAS TÉCNICOS"))
					{
						i.Click();
						break;
					}
				}
				#endregion

				#region Problemas Financeiros
				_chrome.Navigate().GoToUrl(new Constants().Sly_url + "pay-info/no-date");
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(4)").Click();
				#endregion

				#region Discorda dos Valores da Fatura
				_chrome.Navigate().GoToUrl(new Constants().Sly_url + "pay-info/no-date");
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(5)").Click();
				#endregion

				#region Assinatura Cancelada
				_chrome.Navigate().GoToUrl(new Constants().Sly_url + "pay-info/no-date");
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(6)").Click();
				#endregion

				#region Não Reconhce Assinatura 
				_chrome.Navigate().GoToUrl(new Constants().Sly_url + "pay-info/no-date");
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(7)").Click();
				if (PaginaBase.ExpectedUrl(_chrome, "nao-reconhece-assinatura")) { }
				else
					PaginaBase.Fail();
				#endregion

				#region Titular Falecido 
				_chrome.Navigate().GoToUrl(new Constants().Sly_url + "pay-info/no-date");
				PaginaBase.GaveGood(_chrome, pessoa, log);
				_chrome.FindElementByCssSelector("#fade-1 > div:nth-child(2) > div > a:nth-child(8)").Click();
				if (PaginaBase.ExpectedUrl(_chrome, "titular-falecido"))
				{
					PaginaBase.GaveGood(_chrome, pessoa, log);
				}
				else
					PaginaBase.Fail();
				#endregion
				_chrome.Navigate().GoToUrl(new Constants().Sly_url + "enter/options");
				Thread.Sleep(2000);
			}
			catch (Exception e)
			{
				PaginaBase.Error(_chrome, pessoa, log);
				throw;
			}
		}
		public void Atendimento(ChromeDriver _chrome)
		{
			try
			{
				AjustClick.ClickByXPath("//*[@id=\"fade-1\"]/div[2]/div/a[4]", _chrome);
				AjustClick.ClickById("contact_chat", _chrome);
				if (_chrome.Url.Contains("omne/virtual/"))
				{
					PaginaBase.GaveGood(_chrome, pessoa, log);
					_chrome.Navigate().Back();
				}
				log.cpf = pessoa.CPF;

				for (int i = 0; i < 20; i++)
				{
					_chrome.Navigate().Back();
					if (_chrome.Url.Contains("enter/options"))
					{
						break;
					}
				}
			}
			catch (Exception e)
			{
				PaginaBase.Error(_chrome, pessoa, log);
				throw;
			}
		}

	}

}

