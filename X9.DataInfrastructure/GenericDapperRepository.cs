using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace X9.DataInfrastructure
{
	public class GenericDapperRepository
    {

		public IEnumerable<dynamic> GetDapperResult(string query, string connString)
		{
			SqlConnection conn = new SqlConnection(connString);

			conn.Open();
			var result = conn.Query(query);
			conn.Close();
            
			return result;
		}
		public IEnumerable<T> GetDapperResult<T>(string query, string connString)
		{
			try
			{
				SqlConnection conn = new SqlConnection(connString);

				conn.Open();
				var result = conn.Query<T>(query);
				conn.Close();
				return result;
			}
			catch (System.Exception e)
			{
				throw;
			}
		}
	}
}
