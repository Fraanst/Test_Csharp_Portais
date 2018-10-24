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
	public class SinalSky
	{
		PageBase pb = new PageBase();
		SocialNetWorks sn = new SocialNetWorks();
		log_navegacao log = new log_navegacao();
		Pessoa pessoa = new Pessoa();
		public void FullFlow(ChromeDriver _chrome)
		{
			try
			{
				log.data_ini = pb.InserTimeIn();
				log.idportal = Convert.ToInt32(Portal.SinalSky);
				_chrome.Navigate().GoToUrl(new Constants().SinalSky_url);
				if (!pb.ValidateNotFound(_chrome, log))
					return;
			}
			catch (Exception e)
			{
				pb.Error(_chrome, pessoa, log);
				throw;
			}
		}
	}
}
