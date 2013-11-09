using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SsrsPowerShellTools;

namespace SsrsPowerShellToolsTests
{
    [TestClass]
    public class InvokeSrsReportTests
    {
        [TestMethod]
        public void ProcessRecordTest()
        {
            IEnumerator result;

            InvokeSrsReport target = new InvokeSrsReport()
            {
                //Credential = System.Net.CredentialCache.DefaultNetworkCredentials,
                Format = "PDF",
                //Parameters = {{"Year", "2013"}},
                Report = "/Public/State Report",
                ReportServerUrl = "http://edw.vermont.gov/REPORTSERVER/ReportService2005.asmx"
            };

            result = target.Invoke().GetEnumerator();

            Assert.IsTrue(result.MoveNext());
            //Assert.IsTrue(result.Current is Assembly);
        }
    }
}
