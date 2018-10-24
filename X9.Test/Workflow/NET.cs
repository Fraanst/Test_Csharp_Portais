using Handlenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using X9.Domain;
using X9.Domain.Enums;
using X9.Domain.Helpers;
using X9.Domain.Models;
using X9.Test.Pages;

namespace X9.Test.Workflow
{
	public class NET
	{
		PageBase pagebase = new PageBase();
		SocialNetWorks _social = new SocialNetWorks();
		Pessoa pessoa = new Pessoa();
		log_navegacao log = new log_navegacao();
		List<string> _returnXpath = new List<string>();
		public void FullFlow(ChromeDriver _chrome)
		{
			try
			{
				log.data_ini = pagebase.InserTimeIn();
				log.idportal = Convert.ToInt32(Portal.Net);
				_chrome.Navigate().GoToUrl(new Constants().ComboNet_url);
				if (!pagebase.ValidateNotFound(_chrome, log))
					return;
				if (_chrome.Url.Equals(new Constants().ComboNet_url))
				{
					//Net HDTV
					Xpath(_chrome, Net.HDTV, Domain.Enums.Plans.FacilHD);
					Xpath(_chrome, Net.HDTV, Domain.Enums.Plans.MixHD);
					Xpath(_chrome, Net.HDTV, Domain.Enums.Plans.TopHD);

					//NET Virtua
					Xpath(_chrome, Net.VIRTUA, Domain.Enums.Plans.Virtua5);
					Xpath(_chrome, Net.VIRTUA, Domain.Enums.Plans.Virtua35);
					Xpath(_chrome, Net.VIRTUA, Domain.Enums.Plans.Virtua60);
					Xpath(_chrome, Net.VIRTUA, Domain.Enums.Plans.Virtua120);
					Xpath(_chrome, Net.VIRTUA, Domain.Enums.Plans.Virtua240);

					//Net Fone
					Xpath(_chrome, Net.FONE, Domain.Enums.Plans.Local);
					Xpath(_chrome, Net.FONE, Domain.Enums.Plans.Brasi21);
					Xpath(_chrome, Net.FONE, Domain.Enums.Plans.BrasilTotal21);
					Xpath(_chrome, Net.FONE, Domain.Enums.Plans.MundoTotal21);

					//Net Combo
					AjustClick.ClickByXPath("/html/body/main/nav/div/div/div/div/ul[1]/li[4]/a", _chrome);
					Combos(_chrome, log);

					//Now
					AjustClick.ClickByXPath("/html/body/main/nav/div/div/div/div/ul[1]/li[5]/a", _chrome);

					//Celular
					//Xpath(_chrome, Net.Celular, Domain.Enums.Plans.GB6);
					//Xpath(_chrome, Net.Celular, Domain.Enums.Plans.GB8);
					//Xpath(_chrome, Net.Celular, Domain.Enums.Plans.GB10);
					//Xpath(_chrome, Net.Celular, Domain.Enums.Plans.GB15);
					//Xpath(_chrome, Net.Celular, Domain.Enums.Plans.GB30);
					//Xpath(_chrome, Net.Celular, Domain.Enums.Plans.GB50);
					//Xpath(_chrome, Net.Celular, Domain.Enums.Plans.Controle);
					Controle(_chrome, _returnXpath);

				}
				pagebase.GaveGood(_chrome, pessoa, log);
			}
			catch (Exception e)
			{
				log.Exception = e.Message.ToString();
				pagebase.Error(_chrome, pessoa, log);
				throw;
			}
		}
		/// <summary>
		/// Plano Celular Controle
		/// </summary>
		/// <param name="_chrome"></param>
		/// <param name="_returnXpath"></param>
		public void Controle(ChromeDriver _chrome, List<string> _returnXpath)
		{
			IList<IWebElement> menu = _chrome.FindElementsByCssSelector("body > main > section > div > div:nth-child(3) > div.col l4 s12 animated");
			//2GB + 100
			foreach (var item in menu)
			{
				item.Click();
			}
			//3GB + Ilimitadas
			foreach (var item in menu)
			{
				item.Click();
			}
			//4GB + Ilimitadas
			foreach (var item in menu)
			{
				item.Click();
			}
		}
		public void Xpath(ChromeDriver _chrome, Net Net, Plans Plans)
		{
			_returnXpath.Clear();
			var element = String.Empty;
			var combo = String.Empty;
			switch (Net)
			{
				case Net.HDTV:
					_returnXpath.Add("/html/body/main/nav/div/div/div/div/ul[1]/li[1]/a");
					combo = "/html/body/main/section[1]/div/div[3]/div[2]/div[2]/a/button";
					break;
				case Net.VIRTUA:
					_returnXpath.Add("/html/body/main/nav/div/div/div/div/ul[1]/li[2]/a");
					combo = "/html/body/main/section[1]/div/div[4]/div[2]/div[2]/a/button";
					break;
				case Net.FONE:
					_returnXpath.Add("/html/body/main/nav/div/div/div/div/ul[1]/li[3]/a");
					combo = "/html/body/main/section[1]/div/div[3]/div[2]/div[2]/a/button";
					break;
				case Net.Celular:
					_returnXpath.Add("/html/body/main/nav/div/div/div/div/ul[1]/li[6]/a");
					combo = "/html/body/main/section[2]/div/div[3]/div[2]/div[2]/a/button";
					break;
			}
			var NumberDrop = 0;
			var NumberLine = 0;
			switch (Plans)
			{
				case Plans.FacilHD:
					NumberDrop = 2;
					NumberLine = 2;
					element = "assine_chat_facil_hd";
					break;
				case Plans.MixHD:
					NumberDrop = 2;
					NumberLine = 3;
					element = "assine_chat_mix_hd";
					break;
				case Plans.TopHD:
					NumberDrop = 2;
					NumberLine = 4;
					element = "assine_chat_top_hd";
					break;
				case Plans.Virtua5:
					NumberDrop = 1;
					NumberLine = 2;
					element = "assine_chat_5_mega";
					break;
				case Plans.Virtua35:
					NumberDrop = 1;
					NumberLine = 3;
					element = "assine_chat_35_mega";
					break;
				case Plans.Virtua60:
					NumberDrop = 1;
					NumberLine = 4;
					element = "assine_chat_60_mega";
					break;
				case Plans.Virtua120:
					NumberDrop = 1;
					NumberLine = 5;
					element = "assine_chat_120_mega";
					break;
				case Plans.Virtua240:
					NumberDrop = 1;
					NumberLine = 6;
					element = "assine_chat_240_mega";
					break;
				case Plans.Local:
					NumberDrop = 3;
					NumberLine = 2;
					element = "assine_chat_local";
					break;
				case Plans.Brasi21:
					NumberDrop = 3;
					NumberLine = 3;
					element = "assine_chat_brasil_21";
					break;
				case Plans.BrasilTotal21:
					NumberDrop = 3;
					NumberLine = 4;
					element = "assine_chat_brasil_total_21";
					break;
				case Plans.MundoTotal21:
					NumberDrop = 3;
					NumberLine = 5;
					element = "assine_chat_mundo_total_21";
					break;
				case Plans.GB6:
					NumberDrop = 4;
					NumberLine = 2;
					element = "assine_chat_celular_6gb";
					break;
				case Plans.GB8:
					NumberDrop = 4;
					NumberLine = 3;
					element = "assine_chat_celular_8gb";
					break;
				case Plans.GB10:
					NumberDrop = 4;
					NumberLine = 4;
					element = "assine_chat_celular_10gb";
					break;
				case Plans.GB15:
					NumberDrop = 4;
					NumberLine = 5;
					element = "assine_chat_celular_15gb";
					break;
				case Plans.GB30:
					NumberDrop = 4;
					NumberLine = 6;
					element = "assine_chat_celular_30gb";
					break;
				case Plans.GB50:
					NumberDrop = 4;
					NumberLine = 7;
					element = "assine_chat_celular_50gb";
					break;
				case Plans.Controle:
					NumberDrop = 4;
					NumberLine = 8;
					element = "//*[@id=\"sessenta\"]/div/p/a";
					break;
			}
			_returnXpath.Add("//*[@id=\"dropdown" + NumberDrop.ToString() + "\"]/li[" + NumberLine.ToString() + "]/a");
			_returnXpath.Add(element);
			_returnXpath.Add(combo);
			if (Plans != Plans.Controle)
			{
				MenuPlans(_chrome, _returnXpath);
			}
		}

		public void MenuPlans(ChromeDriver _chrome, List<string> List)
		{
			AjustClick.ClickByXPath(List[0], _chrome);
			Plans(_chrome, List);
			_chrome.Navigate().Back();
		}

		public void Plans(ChromeDriver _chrome, List<string> List)
		{
			AjustClick.ClickByXPath(List[1], _chrome);
			Thread.Sleep(200);
			//Chat
			SaveEx(_chrome, log);
			OpenChat(_chrome, List);
			for (int i = 0; i < 30; i++)
			{
				Thread.Sleep(2000);
				if (_chrome.Url.Contains("srvcloo2.callflex"))
				{
					_social.Chat(_chrome, log);
					break;
				}
				else
					OpenChat(_chrome, List);
			}
			//Combos
			AjustClick.ClickByXPath(List[3], _chrome);
		}
		public void OpenChat(ChromeDriver _chrome, List<string> List)
		{
			AjustClick.ClickById(List[2], _chrome);
		}
		public void SaveEx(ChromeDriver _chrome, log_navegacao log)
		{
			var url = _chrome.Url.ToString();
			url = Regex.Replace(url, ".+combonetvendas.com.br/", "");
			var ex = ("Tela :" + url + "- OK");
			log.Exception = ex;
		}
		public void ValidateChat(ChromeDriver _chrome, string element, log_navegacao log)
		{
			Ajust(_chrome, element);
			SaveEx(_chrome, log);
			for (int i = 0; i < 30; i++)
			{
				Thread.Sleep(2000);
				if (_chrome.Url.Contains("srvcloo2.callflex"))
				{
					_social.Chat(_chrome, log);
					break;
				}
				else
					Ajust(_chrome, element);
			}
		}

		/// <summary>
		/// Assinar Todos os Combos Pelo Chat
		/// </summary>
		/// <param name="_chrome"></param>
		public void Combos(ChromeDriver _chrome, log_navegacao log)
		{
			ValidateChat(_chrome, "//*[@id=\"assine_chat_combo_1\"]/button", log);
			ValidateChat(_chrome, "//*[@id=\"assine_chat_combo_2\"]/button", log);
			ValidateChat(_chrome, "//*[@id=\"assine_chat_combo_3\"]/button", log);
			ValidateChat(_chrome, "//*[@id=\"assine_chat_combo_4\"]/button", log);
			_chrome.Navigate().Back();
		}
		public void Ajust(ChromeDriver _chrome, string element)
		{
			AjustClick.ClickByXPath(element, _chrome);
		}
	}

}
