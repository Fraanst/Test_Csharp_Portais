using System;

public class TestService
{
	public TestService GetQueryResult()
	{


	}

	public TestService GettCpf()
	{
		IList<string> ListCpf = new List<String>() { };
		Random rand = new Random(DateTime.Now.Millisecond);
		string result = ListCpf[rand.Next(ListCpf.Count)];
	}
}
