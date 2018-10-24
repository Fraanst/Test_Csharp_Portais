using System.Collections.Generic;
using X9.Domain;
using System;
using X9.Domain.Helpers;
using X9.Domain.Models;

namespace X9.DataInfrastructure
{
    public class QueryLog
    {
        GenericDapperRepository _repository = new GenericDapperRepository();

        public IEnumerable<Pessoa> SaveLog(log_navegacao log)
        {
            string query = "INSERT INTO  log_navegacao (idportal, pagina, idstatus, data_ini, data_fim, cpf, Exception) VALUES (" + log.idportal + ",'" + log.pagina + "'," + log.idstatus + ",'" + log.data_ini + "','" + log.data_fim + "','" + log.cpf + "','" + log.Exception + "')";
            return _repository.GetDapperResult<Pessoa>(query, new Constants().X9_cns);
        }
        public IEnumerable<log_navegacao> SelectId(log_navegacao log)
        {
            string query = "SELECT ID FROM log_navegacao WHERE IDPORTAL = " + log.idportal + " AND PAGINA = '" + log.pagina + "' AND IDSTATUS = " + log.idstatus + " AND DATA_INI = '" + log.data_ini + "' AND DATA_FIM = '" + log.data_fim + "'";
            return _repository.GetDapperResult<log_navegacao>(query, new Constants().X9_cns);
        }
        public IEnumerable<Pessoa> DeleteCpf(Pessoa pessoa, int portal)
        {
            try
            {
                string query = "UPDATE PESSOAS SET isdeleted= 1 WHERE CPF ='"+pessoa.CPF+"' and idportal ="+portal;
                return _repository.GetDapperResult<Pessoa>(query, new Constants().X9_cns);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }

}
