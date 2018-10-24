using Microsoft.VisualStudio.TestTools.UnitTesting;
using X9.Domain.Helpers;
using X9.Test.Workflow;

namespace X9.Test
{
	[TestClass]
    public class TestePortais : TestBase
    {
        [TestMethod]
        public void Atacadao()
        {
            var workflow = new Atacadao();

            workflow.FullFlow(chromeDriver);
        }
		[TestMethod]
		public void SkyAdesao()
		{
			var workflow = new SkyAdesao();

			workflow.FullFlow(chromeDriver);
		}
		[TestMethod]
        public void Claro()
        {
            var workflow = new Claro();
            workflow.FullFlow(chromeDriver);
        }
		[TestMethod]
		public void SinalSky()
		{
			var workflow = new SinalSky();
			workflow.FullFlow(chromeDriver);
		}
		[TestMethod]
		public void SkyPre()
		{
			var workflow = new SkyPre();
			workflow.FullFlow(chromeDriver);
		}

        [TestMethod]
        public void FiqueNaClaro()
        {
            var workflow = new FiqueNaClaro();

            workflow.FullFlow(chromeDriver);
        }

        [TestMethod]
        public void NET()
        {
            var workflow = new NET();
            workflow.FullFlow(chromeDriver);
        }
		[TestMethod]
		public void RecoveryDigital()
		{
			var workflow = new Recovery();
			chromeDriver.Navigate().GoToUrl(new Constants().Digital_url);
			workflow.FullFlow(chromeDriver);
		}
        [TestMethod]
        public void RecoveryHumana()
        {
            var workflow = new Recovery();
			chromeDriver.Navigate().GoToUrl(new Constants().Humana_url);
			workflow.FullFlow(chromeDriver);
        }
        [TestMethod]
        public void SkyPagueFacil()
        {
            var workflow = new SkyPagueFacil();
            workflow.FullFlow(chromeDriver);
        }
        [TestMethod]
        public void Vivo()
        {
            var workflow = new Vivo();
            workflow.FullFlow(chromeDriver);
        }
    }
}
