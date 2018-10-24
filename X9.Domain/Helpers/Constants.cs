using System.Configuration;

namespace X9.Domain.Helpers
{
	public class Constants
	{
		public Constants()
		{
			X9_cns = ConfigurationManager.ConnectionStrings["X9_connectionString"]?.ConnectionString;
			Sly_url = ConfigurationManager.AppSettings["urlSky"]?.ToString();
			Date = ConfigurationManager.AppSettings["FindDate"]?.ToString();
			Atacadao_url = ConfigurationManager.AppSettings["urlAtacadao"].ToString();
			FiquenClaro_url = ConfigurationManager.AppSettings["urlFiqueClaro"]?.ToString();
			Claro_url = ConfigurationManager.AppSettings["urlClaro"]?.ToString();
			Humana_url = ConfigurationManager.AppSettings["urlRecoveryHumana"]?.ToString();
			Digital_url = ConfigurationManager.AppSettings["urlRecoveryDigital"]?.ToString();
			Net_url = ConfigurationManager.AppSettings["Net_url"]?.ToString();
			Vivo_url = ConfigurationManager.AppSettings["urlVivo"]?.ToString();
			SlyPrePago_url = ConfigurationManager.AppSettings["urlPreSky"]?.ToString();
			SinalSky_url = ConfigurationManager.AppSettings["urlSinalSky"]?.ToString();
			SkyAdesao_url = ConfigurationManager.AppSettings["urlSkyAdesao"]?.ToString();
			ComboNet_url = ConfigurationManager.AppSettings["urlNetCombo"]?.ToString();
		}

		public string X9_cns { get; set; }
		public string Atacadao_url { get; set; }
		public string Sly_url { get; set; }
		public string Date { get; set; }
		public string FiquenClaro_url { get; set; }
		public string Claro_url { get; set; }
		public string Net_url { get; set; }
		public string Humana_url { get; set; }
		public string Vivo_url { get; set; }
		public string Digital_url { get; set; }
		public string SlyPrePago_url { get; set; }
		public string SkyAdesao_url { get; set; }
		public string SinalSky_url { get; set; }
		public string ComboNet_url { get; set; }
	}
}
