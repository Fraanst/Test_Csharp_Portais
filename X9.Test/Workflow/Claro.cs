using Handlenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using X9.Domain;
using X9.Domain.Enums;
using X9.Domain.Helpers;
using X9.Domain.Models;
using X9.Test.Pages;

namespace X9.Test.Workflow
{
	public class Claro
    {
		PageBase pagbase = new PageBase();
		Pessoa pessoa = new Pessoa();
		log_navegacao log = new log_navegacao();
        public void FullFlow(ChromeDriver _chrome)
        {
            try
            {
				log.data_ini = pagbase.InserTimeIn();
				log.idportal = Convert.ToInt32(Portal.Claro);
				pagbase.InsertPhone();
				_chrome.Navigate().GoToUrl(new Constants().Claro_url);
				if (!pagbase.ValidateNotFound(_chrome, log))
					return;
				AjustClick.ClickByXPath("/html/body/div[4]/div[3]/button", _chrome);
                Thread.Sleep(1000);
				pagbase.GaveGood(_chrome, pessoa, log);
			}
            catch (Exception e)
            {
				pagbase.Error(_chrome, pessoa, log);
				throw;
            }
        }
    }
}
