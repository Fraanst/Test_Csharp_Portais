using OpenQA.Selenium.Chrome;
using System;
using X9.Domain.Models;
using X9.Test.Pages;
using X9.Domain.Enums;
using Handlenium;
using System.Threading;
using X9.Domain.Helpers;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using OpenQA.Selenium;
using X9.Domain;

namespace X9.Test.Workflow
{
	public class SkyPre
	{
		PageBase pagbase = new PageBase();
		Pessoa pessoa = new Pessoa();
		log_navegacao log = new log_navegacao();
		public void FullFlow(ChromeDriver _chrome)
		{
			try
			{
				_chrome.Navigate().GoToUrl(new Constants().SlyPrePago_url);
				log.data_ini = pagbase.InserTimeIn();
				log.idportal = Convert.ToInt32(Portal.skyprepago);
				if (!pagbase.ValidateNotFound(_chrome, log))
					return;
				if (pagbase.ExpectedUrl(_chrome, new Constants().SlyPrePago_url))
				{
					FillinData(_chrome);
					Register(_chrome);
					if (pagbase.ExpectedElement(_chrome, "//*[@id=\"myModal\"]/div/div/div[2]", tipo.XPath))
					{
						IList<IWebElement> validate = _chrome.FindElementsByCssSelector("#myModal > div > div > div.modal-body > p");
						foreach (var item in validate)
						{
							string text = item.Text;
							
							text = Regex.Replace(text, "PARABENS!\r\nAgora voce faz parte desse grande time..", "PARABENS!");
							if (text.Equals("PARABENS!"))
							{
								pagbase.GaveGood(_chrome, pessoa, log);
								break;
							}
						}
					}
					else
						pagbase.Fail();
				}
				else
					pagbase.Fail();
			}
			catch (Exception e)
			{
				pagbase.Error(_chrome, pessoa, log);
			}
		}

		public void Register(ChromeDriver _chrome)
		{
			AjustSendKeys.SendKeysById("nome", pessoa.Name, _chrome);
			AjustSendKeys.SendKeysById("email", pessoa.Email, _chrome);
			AjustSendKeys.SendKeysById("empresa", pessoa.Empresa, _chrome);
			AjustSendKeys.SendKeysById("CNPJ", pessoa.Phone, _chrome);
			AjustSendKeys.SendKeysById("telefone", pessoa.CNPJ, _chrome);
			AjustSendKeys.SendKeysById("cidade", pessoa.Cidade, _chrome);
			AjustSendKeys.SendKeysById("estado", pessoa.Estado, _chrome);
			Thread.Sleep(2000);
			AjustClick.ClickById("resposta", _chrome);
			AjustClick.ClickById("btn-salvar", _chrome);

		}
		public void FillinData(ChromeDriver _chrome)
		{
			pessoa.Name = "Sky Pré-pago";
			pessoa.Email = "sky_prepago@mailinator.com";
			pessoa.Empresa = "Sky Pré-pago";
			pessoa.Phone = "4190000-0000";
			pessoa.CNPJ = "34952832000120";
			pessoa.Cidade = "Sky_teste";
			pessoa.Estado = "Sky_Teste";
		}
	}
}
