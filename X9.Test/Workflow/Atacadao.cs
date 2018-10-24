using ApplicationServices;
using Handlenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;
using X9.Domain.Helpers;
using X9.Domain.Enums;
using X9.Domain;
using X9.Test.Pages;
using X9.Domain.Models;
using System.Text.RegularExpressions;

namespace X9.Test.Workflow
{
	public class Atacadao
	{
		TestPortaisServices service = new TestPortaisServices();
		string url = new Constants().Atacadao_url;
		PageBase PagBase = new PageBase();
		Pessoa pessoa = new Pessoa();
		log_navegacao log = new log_navegacao();
		public void FullFlow(ChromeDriver _chrome)
		{
			try
			{
				log.idportal = Convert.ToInt32(Portal.atacadao);
				pessoa.Email = "atacadao@mailinator.com";
				pessoa.Phone = PagBase.InsertPhone();
				log.data_ini = PagBase.InserTimeIn();
				pessoa.CPF = service.GetCPF(log);
				pessoa.DT_Nasc = service.Getdate(pessoa, Portal.atacadao);
				_chrome.Navigate().GoToUrl(url);
				if (!PagBase.ValidateNotFound(_chrome, log))
					return;
				if (_chrome.Url.Equals(url))
				{

					Thread.Sleep(1000);
					foreach (var item in pessoa.DT_Nasc)
					{
						AjustSendKeys.SendKeysById("dt-nasc", item.ToString(), _chrome);
					}
					foreach (var item in pessoa.CPF)
					{
						AjustSendKeys.SendKeysById("numb-cpf", item.ToString(), _chrome);
					}
					foreach (var item in pessoa.Phone)
					{
						AjustSendKeys.SendKeysById("numb-tel", item.ToString(), _chrome);
					}
					Thread.Sleep(1000);
					AjustClick.ClickById("form-send", _chrome);
					//Solicitar Fatura
					Thread.Sleep(3000);
					if (_chrome.Url.Contains("/enter/menu"))
					{
						try
						{
							PagBase.ExpectedElement(_chrome, "/html/body/div[2]/div[4]/p", tipo.XPath);
							service.DeleteCPf(pessoa, Portal.atacadao);
							_chrome.Url = Regex.Replace(_chrome.Url, "com.br/", "com.br/CPF_Encontrado_Invalido.");
							PagBase.Error(_chrome, pessoa, log);
							FullFlow(_chrome);
						}
						catch (Exception)
						{
							if ((PagBase.ExpectedUrl(_chrome, "/enter/menu")))
							{
								Email(_chrome);
							}
							else
								PagBase.Fail();
							Thread.Sleep(1000);
							AjustClick.ClickById("btnNegociacao", _chrome);
							Scheduling(_chrome);
							Payment(_chrome);
							SemPrevisao(_chrome);
							_chrome.Navigate().GoToUrl(url + "/enter/menu");
							AjustClick.ClickById("btnAtendimento", _chrome);
							Atendimento(_chrome);
							AjustClick.ClickByXPath("/html/body/header/div/div/div[1]/a/i", _chrome);
							Thread.Sleep(1000);
							_chrome.FindElementByCssSelector("#slide-out > li:nth-child(4) > a").Click();
							PagBase.GaveGood(_chrome, pessoa, log);
						}
					}
					else
						Validate(_chrome, pessoa);

				}

			}
			catch (Exception e)
			{
				PagBase.Error(_chrome, pessoa, log);
				throw;
			}
		}
		public void Validate(ChromeDriver chromeDriver, Pessoa pessoa)
		{
			Thread.Sleep(3000);
			log.data_ini = PagBase.InserTimeIn();
			Thread.Sleep(1000);
			service.DeleteCPf(pessoa, Portal.atacadao);
			chromeDriver.Url = Regex.Replace(chromeDriver.Url, "com.br/", "com.br/CPF_Encontrado_Invalido.");
			PagBase.Error(chromeDriver, pessoa, log);
			Thread.Sleep(1000);
			FullFlow(chromeDriver);
		}
		public void SemPrevisao(ChromeDriver _chrome)
		{
			try
			{
				AjustClick.ClickById("btnSemPrevisao", _chrome);
				if (PagBase.ExpectedUrl(_chrome, "pay-info/no-date"))
				{
					AjustClick.ClickById("financialProblems", _chrome);
					if (PagBase.ExpectedUrl(_chrome, "atendimento"))
					{
						PagBase.GaveGood(_chrome, pessoa, log);
						_chrome.Navigate().Back();
					}
				}
				else
					PagBase.Fail();
				if (PagBase.ExpectedUrl(_chrome, "pay-info/no-date"))
				{
					AjustClick.ClickById("valueDisagree", _chrome);
					if (PagBase.ExpectedUrl(_chrome, "atendimento"))
					{
						PagBase.GaveGood(_chrome, pessoa, log);
						_chrome.Navigate().Back();
					}
				}
				else
					PagBase.Fail();
				if (PagBase.ExpectedUrl(_chrome, "pay-info/no-date"))
				{
					AjustClick.ClickById("anawareInvoice", _chrome);
					if (PagBase.ExpectedUrl(_chrome, "atendimento"))
					{
						PagBase.GaveGood(_chrome, pessoa, log);
						_chrome.Navigate().Back();
					}
				}
				else
					PagBase.Fail();
				if (PagBase.ExpectedUrl(_chrome, "pay-info/no-date"))
				{
					AjustClick.ClickByXPath("/html/body/div[3]/div[3]/div/div/button[4]", _chrome);
					PagBase.GaveGood(_chrome, pessoa, log);
					AjustClick.ClickByXPath("/html/body/div[2]/a/i", _chrome);
				}
				else
					PagBase.Fail();
				if (PagBase.ExpectedUrl(_chrome, "pay-info/no-date"))
				{
					AjustClick.ClickById("otherMotive", _chrome);
					if (PagBase.ExpectedUrl(_chrome, "atendimento"))
					{
						PagBase.GaveGood(_chrome, pessoa, log);
						_chrome.Navigate().Back();
					}
				}
				else
					PagBase.Fail();
			}
			catch (Exception)
			{
				PagBase.Error(_chrome, pessoa, log);
			}
		}
		public void Atendimento(ChromeDriver _chrome)
		{
			try
			{
				if (PagBase.ExpectedUrl(_chrome, "atendimento"))
				{
					AjustClick.ClickById("btnChat", _chrome);
					if (PagBase.ExpectedUrl(_chrome, "srvcloo2.callflex.com.br"))
					{
						PagBase.GaveGood(_chrome, pessoa, log);
						_chrome.Navigate().Back();
					}
					else
						PagBase.Fail();
					AjustClick.ClickById("btnWhatsApp", _chrome);
					if (PagBase.ExpectedUrl(_chrome, "api.whatsapp.com"))
					{
						PagBase.GaveGood(_chrome, pessoa, log);
						_chrome.Navigate().Back();
					}
					else
						PagBase.Fail();
					AjustClick.ClickById("btnEmail1", _chrome);
					AjustSendKeys.SendKeysById("email-formr", pessoa.Email, _chrome);
					AjustClick.ClickById("btnAtendimentoEmail", _chrome);
					AjustClick.ClickByXPath("/html/body/div[3]/div/div/div[2]/button", _chrome);
					AjustClick.ClickById("btnAtendimento", _chrome);
					AjustClick.ClickById("btnLigacao", _chrome);
					AjustSendKeys.SendKeysById("phone-formr", pessoa.Phone, _chrome);
					AjustClick.ClickById("btnTelefone", _chrome);
					AjustClick.ClickByXPath("/html/body/div[3]/div/div/div[2]/button", _chrome);
					PagBase.GaveGood(_chrome, pessoa, log);
				}
			}
			catch (Exception)
			{
				PagBase.Error(_chrome, pessoa, log);
			}
		}
		public void Email(ChromeDriver _chrome)
		{
			try
			{
				AjustClick.ClickById("btnEmail", _chrome);
				AjustClick.ClickById("btnOutroEmail", _chrome);
				AjustSendKeys.SendKeysById("fatura-formr", pessoa.Email, _chrome);
				AjustClick.ClickById("btnFatura1", _chrome);
				Thread.Sleep(1000);
				_chrome.FindElementByCssSelector("body > div.container > div > div > div.col.l8.offset-l2.s12.m10.offset-m1 > button").Click();
				PagBase.GaveGood(_chrome, pessoa, log);
			}
			catch (Exception)
			{
				PagBase.Error(_chrome, pessoa, log);
			}
		}

		public void Scheduling(ChromeDriver _chrome)
		{
			try
			{
				try
				{
					PagBase.ExpectedElement(_chrome, "invoice-receive", tipo.Id);
					{
						AjustClick.ClickById("invoice-receive", _chrome);
						IList<IWebElement> div = _chrome.FindElementsByCssSelector("body > div.container > div:nth-child(3) > div > div > button");
						foreach (var item in div)
						{
							if (item.Text.Equals("SMS"))
							{
								item.Click();
							}
						}
						Thread.Sleep(1000);
						AjustSendKeys.SendKeysById("phone-sms", pessoa.Phone, _chrome);
						AjustClick.ClickById("send-sms", _chrome);
						AjustClick.ClickByXPath("/html/body/div[3]/div/div/div[2]/button", _chrome);
						AjustClick.ClickByXPath("/html/body/div[2]/a/i", _chrome);
						PagBase.GaveGood(_chrome, pessoa, log);
					}
				}
				catch (Exception)
				{
					AjustClick.ClickById("btnSendValue1", _chrome);
					AjustClick.ClickByXPath("/html/body/div[3]/div[2]/div/div/div[2]/div[2]/div[2]/div[1]/span[1]/img", _chrome);
					AjustClick.ClickById("send-date", _chrome);
					try
					{
						PagBase.ExpectedElement(_chrome, "modal-dia-util", tipo.Id);
						AjustClick.ClickByXPath("//*[@id=\"modal-dia-util\"]/div/div/div/div/div/button", _chrome);
						AuxScheduling(_chrome);
					}
					catch (Exception)
					{
						AuxScheduling(_chrome);
					}
				}
			}
			catch (Exception e)
			{
				PagBase.Error(_chrome, pessoa, log);
			}
		}
		public void AuxScheduling(ChromeDriver _chrome)
		{
			_chrome.FindElementByCssSelector("body > div.container > div:nth-child(3) > div > div > button:nth-child(2)").Click();
			AjustSendKeys.SendKeysById("phone-sms", pessoa.Phone, _chrome);
			Thread.Sleep(1000);
			AjustClick.ClickById("send-sms", _chrome);
			AjustClick.ClickByXPath("/html/body/div[3]/div/div/div[2]/button", _chrome);
			AjustClick.ClickByXPath("/html/body/div[2]/a/i", _chrome);
			PagBase.GaveGood(_chrome, pessoa, log);
		}


		public void Payment(ChromeDriver _chrome)
		{
			try
			{
				AjustClick.ClickById("btnAlegaPagamento", _chrome);
				_chrome.FindElementByCssSelector("body > div.container > div:nth-child(2) > div > div > div.date-desktop > div:nth-child(2) > div.col.s11.m9.l6.center-align > div:nth-child(1) > span.down.down-day > img").Click();
				AjustClick.ClickById("send-date", _chrome);
				PagBase.GaveGood(_chrome, pessoa, log);
				AjustClick.ClickByXPath("/html/body/div[3]/div/div/div[2]/button", _chrome);
				Thread.Sleep(1000);
				AjustClick.ClickByXPath("/html/body/div[2]/a/i", _chrome);
			}
			catch (Exception e)
			{
				PagBase.Error(_chrome, pessoa, log);
			}
		}
	}
}
