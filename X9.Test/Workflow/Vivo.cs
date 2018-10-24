using Handlenium;
using OpenQA.Selenium;
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
	public class Vivo
    {
		PageBase pagebase = new PageBase();
		Pessoa pessoa = new Pessoa();
		log_navegacao log = new log_navegacao();

		//Validando se o site está no ar
		//Apenas Tela inicial
		public void FullFlow(ChromeDriver _chrome)
        {
            try
            {
				log.data_ini = pagebase.InserTimeIn();
				log.idportal = Convert.ToInt32(Portal.Vivo);
				_chrome.Navigate().GoToUrl( new Constants().Vivo_url);
				if (!pagebase.ValidateNotFound(_chrome, log))
					return;
				IWebElement text = _chrome.FindElementByXPath("/html/body/div/div[2]/p[1]");
				if (text.Text.Equals("Seja bem-vindo ao portal Vivo Regulariza Fácil!"))
				{
					text = _chrome.FindElementByXPath("/html/body/div/div[2]/p[2]");
					if (text.Text.Equals("Negocie e regularize seus débitos Vivo com rapidez e segurança ou solicite um atendimento personalizado, através dos canais disponíveis: Chat, WhatsApp, SMS, Telegram, ligação ou e-mail"))
					{
						text = _chrome.FindElementByXPath("/html/body/div/div[2]/h1");
						if (text.Text.Equals("Aproveite esta oportunidade inovadora para a regularização de débitos e planejamento pessoal."))
						{
							pagebase.GaveGood(_chrome, pessoa, log);
						}
					}
				}
				else
				{
					log.Exception = "Pagina Fora do Ar:" + _chrome.Url.ToString();
					pagebase.Error404(_chrome, pessoa, log);
				}

			}
            catch (Exception e)
            {
				log.Exception = e.ToString();
				pagebase.Error404(_chrome, pessoa, log); 
            }
        }
    }
}
