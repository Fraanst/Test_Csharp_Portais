using ApplicationServices;
using Handlenium;
using OpenQA.Selenium.Chrome;
using System;
using X9.Domain;
using X9.Domain.Enums;
using X9.Domain.Helpers;
using X9.Domain.Models;
using X9.Test.Pages;

namespace X9.Test.Workflow
{
	public class SkyAdesao
	{
		PageBase pag = new PageBase();
		SocialNetWorks social = new SocialNetWorks();
		log_navegacao log = new log_navegacao();
		Pessoa pessoa = new Pessoa();
		TestPortaisServices service = new TestPortaisServices();

		public void FullFlow(ChromeDriver _chrome)
		{
			try
			{
				log.data_ini = pag.InserTimeIn();
				_chrome.Navigate().GoToUrl(new Constants().SkyAdesao_url);
				if (_chrome.Url.Equals(new Constants().SkyAdesao_url))
				{
					FillinData(_chrome);
					log.idportal = Convert.ToInt32(Portal.SkyAdesao);
					if (!pag.ValidateNotFound(_chrome, log))
						return;
					Register(_chrome);
					pag.ExpectedUrl(_chrome, "contact");
					if (_chrome.Url.Contains("contact/"))
					{
						AjustClick.ClickById("contact_whatsapp", _chrome);
						social.WhatsApp(_chrome, log);
						AjustClick.ClickById("contact_ligacao", _chrome);
						social.Call(_chrome, log, pessoa);
						AjustClick.ClickById("contact_sms", _chrome);
						social.Sms(_chrome, pessoa, log);
						AjustClick.ClickById("contact_chat", _chrome);
						_chrome.Navigate().Back();
					} 
					else
						pag.Fail();

					pag.GaveGood(_chrome, pessoa, log);
				}
				else
					pag.Fail();
			}
			catch (System.Exception)
			{
				pag.Error(_chrome, pessoa, log);
				throw;
			}
		}
		public void Register(ChromeDriver _chrome)
		{
			AjustSendKeys.SendKeysById("name", pessoa.Name, _chrome);
			AjustSendKeys.SendKeysById("phone", pessoa.Phone, _chrome);
			AjustSendKeys.SendKeysById("cpf", pessoa.CPF, _chrome);
			AjustSendKeys.SendKeysById("email", pessoa.Email, _chrome);
			AjustClick.ClickById("form-send", _chrome);
		}
		public void FillinData(ChromeDriver _chrome)
		{
			pessoa.Name = "Test Sky_Adesao";
			pessoa.Phone = pag.InsertPhone();
			pessoa.CPF = service.GerarCpf();
			pessoa.Email = "skyadesao@mailinator.com";
		}

		

	}
}
