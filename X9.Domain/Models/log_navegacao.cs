using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X9.Domain
{
	public class log_navegacao
	{
		public string id { get; set; }
		public int idportal { get; set; }
		public int idstatus { get; set; }
		public string pagina { get; set; }
		public string data_ini { get; set; }
		public string data_fim { get; set; }
		public string cpf { get; set; }
		public string Exception { get; set; }
	}
}
