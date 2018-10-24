using System.Collections.Generic;
using X9.Domain;
using X9.Domain.Enums;
using X9.Domain.Helpers;
using X9.Domain.Models;

namespace X9.DataInfrastructure
{
    public class Query
    {
        GenericDapperRepository _repository = new GenericDapperRepository();
        string _conn = new Constants().X9_cns;

        public IEnumerable<Pessoa> GetCPF(log_navegacao log)
        {
            string query = "select cpf from pessoas where idportal = "+log.idportal+" and isdeleted = 0";
            return _repository.GetDapperResult<Pessoa>(query, new Constants().X9_cns);
        }
        public IEnumerable<Pessoa> Getdate(Pessoa pessoa, int portal)
        {
            string query = "SELECT DT_NASC FROM PESSOAS WHERE CPF = '"+pessoa.CPF+"' AND IDPORTAL ="+portal;
            return _repository.GetDapperResult<Pessoa>(query, new Constants().X9_cns);
        }
    }
}


