using Handlenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using X9.Domain.Models;
using X9.Domain.Enums;
using X9.Domain;

namespace X9.Test.Pages
{
	public class SocialNetWorks
	{
		PageBase pagebase = new PageBase();
		Pessoa pessoa = new Pessoa();
		log_navegacao log = new log_navegacao();


		public void WhatsApp(ChromeDriver _chrome, log_navegacao log)
		{
			try
			{
				if (_chrome.Url.Contains("/contact") || _chrome.Url.Contains("whatsapp"))
				{
					pessoa.CPF = log.cpf;
					if (pagebase.ExpectedUrl(_chrome, "send?phone="))
					{
						Thread.Sleep(1000);
						_chrome.Url = Regex.Replace(_chrome.Url, "=.+", "=5541900000000");
						Thread.Sleep(1000);
						if (pagebase.ExpectedUrl(_chrome, "send?phone")) { } //deu boa
						else
							pagebase.Fail();
						AjustClick.ClickById("action-button-container", _chrome);
						Thread.Sleep(100);
						if (pagebase.ExpectedUrl(_chrome, "web.whatsapp.com/")) { } //Deu boa
						else
							pagebase.Fail();
					}
					else
						pagebase.Fail();

					pagebase.GaveGood(_chrome, pessoa, log);
					for (int i = 0; i < 20; i++)
					{
						_chrome.Navigate().Back();
						if (_chrome.Url.Contains("/contact") || _chrome.Url.Contains("fiquenaclaro"))
						{
							break;
						}
					}
				}
			}
			catch (Exception e)
			{
				pagebase.Error(_chrome, pessoa, log);
				return;
			}
		}

		public void Chat(ChromeDriver _chrome, log_navegacao log)
		{
			try
			{
				if (log.idportal == Convert.ToInt32(Portal.SkyAdesao) || log.idportal == Convert.ToInt32(Portal.Net)){}
				else
				{
					pessoa.CPF = log.cpf;
					AjustClick.ClickById("contact-chat", _chrome);
				}
				if (pagebase.ExpectedUrl(_chrome, "callflex.com.b"))
				{
					pagebase.GaveGood(_chrome, pessoa, log);
					_chrome.Navigate().Back();
				}
				else
					pagebase.Fail();
			}
			catch (Exception)
			{
				pagebase.Error(_chrome, pessoa, log);
			}
		}

		public void Sms(ChromeDriver _chrome, Pessoa pessoa, log_navegacao log)
		{
			try
			{
				if (log.idportal == Convert.ToInt32(Portal.SkyAdesao))
				{
					AjustSendKeys.SendKeysById("name", pessoa.Name, _chrome);
					AjustSendKeys.SendKeysById("phone", pessoa.Phone, _chrome);
					AjustClick.ClickById("next-step", _chrome);
					pagebase.GaveGood(_chrome, pessoa, log);
					_chrome.Navigate().Back();
					_chrome.Navigate().Back();
				}
				else
				{
					AjustClick.ClickById("contact-sms", _chrome);
					AjustSendKeys.SendKeysById("sms_name", pessoa.Name.ToString(), _chrome);
					AjustSendKeys.SendKeysById("sms_phone", pessoa.Phone.ToString(), _chrome);
					AjustClick.ClickById("contact-sms-send", _chrome);
					AjustClick.ClickByXPath("//*[@id=\"confirmation\"]/div/div/div/input", _chrome);
					pagebase.GaveGood(_chrome, pessoa, log);
				}
			}

			catch (Exception)
			{
				pagebase.Error(_chrome, pessoa, log);
			}
		}
		public void Facebook(ChromeDriver _chrome, log_navegacao log)
		{
			try
			{
				pessoa.CPF = log.cpf;
				AjustClick.ClickById("contact-facebook", _chrome);
				if (pagebase.ExpectedUrl(_chrome, "facebook"))
				{
					pagebase.GaveGood(_chrome, pessoa, log);
					_chrome.Navigate().Back();
				}
			}
			catch (Exception)
			{
				pagebase.Error(_chrome, pessoa, log);
			}
		}
		public void Call(ChromeDriver _chrome, log_navegacao log, Pessoa pessoa)
		{
			try
			{
				if (log.idportal == Convert.ToInt32(Portal.SkyAdesao))
				{
					AjustSendKeys.SendKeysById("name", pessoa.Name, _chrome);
					AjustSendKeys.SendKeysById("phone", pessoa.Phone, _chrome);
					AjustClick.ClickById("next-step", _chrome);
					pagebase.GaveGood(_chrome, pessoa, log);
					_chrome.Navigate().Back();
				}
				else
				{
					pessoa.CPF = log.cpf;
					AjustClick.ClickById("contact-ligacao", _chrome);
					AjustSendKeys.SendKeysById("call_name", pessoa.Name.ToString(), _chrome);
					AjustSendKeys.SendKeysById("call_phone", pessoa.Phone.ToString(), _chrome);
					AjustClick.ClickById("contact-ligacao-send", _chrome);
					pagebase.GaveGood(_chrome, pessoa, log);
					AjustClick.ClickByXPath("//*[@id=\"confirmation\"]/div/div/div/input", _chrome);
				}

			}
			catch (Exception)
			{
				pagebase.Error(_chrome, pessoa, log);
			}
		}
	}
}
