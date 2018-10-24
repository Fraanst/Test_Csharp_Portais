using Handlenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using X9.Domain.Helpers;
using X9.Domain.Models;
using X9.Test.Pages;
using X9.Domain.Enums;
using X9.Domain;

namespace X9.Test.Workflow
{
	public class FiqueNaClaro
	{
		PageBase pagbase = new PageBase();
		Pessoa pessoa = new Pessoa();
		SocialNetWorks socialNetworks = new SocialNetWorks();
		log_navegacao log = new log_navegacao();
		public void FullFlow(ChromeDriver _chrome)
		{
			try
			{
				log.idportal = Convert.ToInt32(Portal.FiqueClaro);
				pessoa.Name = "Test_FiquenaClaro";
				log.data_ini = pagbase.InserTimeIn();
				pessoa.Phone = pagbase.InsertPhone();
				_chrome.Navigate().GoToUrl(new Constants().FiquenClaro_url);
				if (pagbase.ExpectedUrl(_chrome, new Constants().FiquenClaro_url))
				{
					if (!pagbase.ValidateNotFound(_chrome, log))
						return;
					AjustClick.ClickById("contact-whatsapp", _chrome);
					socialNetworks.WhatsApp(_chrome, log);
					socialNetworks.Chat(_chrome, log);
					socialNetworks.Sms(_chrome, pessoa, log);
					socialNetworks.Facebook(_chrome, log);
					socialNetworks.Call(_chrome, log, pessoa);
					pagbase.GaveGood(_chrome, pessoa, log);
				}
				else
					log.Exception = "Pagina Fora do Ar:" + _chrome.Url.ToString();
					pagbase.Error404(_chrome, pessoa, log);
			}
			catch (Exception e)
			{
				pagbase.Error(_chrome, pessoa, log);
				throw;
			}
		}

	}
}
