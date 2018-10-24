using System;
using System.Collections.Generic;
using System.Linq;
using X9.DataInfrastructure;
using X9.Domain;
using X9.Domain.Enums;
using X9.Domain.Models;

namespace ApplicationServices
{
	public class TestPortaisServices
	{
		Pessoa Pessoa = new Pessoa();
		log_navegacao log = new log_navegacao();
		Query _Sql = new Query();
		Portal portal = new Portal();
		QueryLog Sql = new QueryLog();
		List<Pessoa> ListCpf;
		public string InserLog(log_navegacao log)
		{
			Sql.SaveLog(log);
			List<log_navegacao> Listid = Sql.SelectId(log).ToList();
			return Listid.FirstOrDefault().id;
		}
		public void InsertLog(log_navegacao log)
		{
			Sql.SaveLog(log);
		}
		public  String GerarCpf()
		{
			int soma = 0, resto = 0;
			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			Random rnd = new Random();
			string semente = rnd.Next(100000000, 999999999).ToString();

			for (int i = 0; i < 9; i++)
				soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			semente = semente + resto;
			soma = 0;

			for (int i = 0; i < 10; i++)
				soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

			resto = soma % 11;

			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			semente = semente + resto;
			return semente;
		}
		public string Getdate(Pessoa pessoa, Portal portal)
		{
			var _portal = Convert.ToInt32(portal);
			pessoa = _Sql.Getdate(pessoa, _portal).FirstOrDefault();
			var date = pessoa.DT_Nasc;
			if (date.Length != 10)
				return null;
			else
				return date;
		}
		public string GetCPF(log_navegacao log)
		{
			ListCpf = _Sql.GetCPF(log).ToList();
			return Rand(ListCpf, 1, ListCpf.Count);

		}

		public string Rand(List<Pessoa> List, int ini, int fim)
		{
			Random rand = new Random();
			string result = List[rand.Next(ini, fim)].CPF;
			return result;
		}
		public void DeleteCPf(Pessoa pessoa, Portal portal)
		{
			var _portal = Convert.ToInt32(portal);
			Sql.DeleteCpf(pessoa, _portal);
		}

	}

}
