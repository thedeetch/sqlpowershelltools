using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using SsrsPowerShellTools;

namespace SsrsPowerShellToolsTests
{
    [TestClass]
    public class InvokeSrsReportTests
    {
        [TestMethod]
        public void ProcessRecordTest()
        {
            using (ShimsContext.Create())
            {
                Microsoft.SqlServer.ReportingServices.Fakes.ShimReportExecutionService.AllInstances.RenderStringStringStringOutStringOutStringOutWarningArrayOutStringArrayOut =
                    (format, deviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIds, out result) => 
                    {
                    
                    
                    };
                
                IEnumerator result;

                InvokeSrsReport target = new InvokeSrsReport()
                {
                    Credential = System.Net.CredentialCache.DefaultNetworkCredentials,
                    Format = "PDF",
                    Parameters = { { "Year", "2013" } },
                    Report = "",
                    ReportServerUrl = ""
                };

                result = target.Invoke().GetEnumerator();

                Assert.IsTrue(result.MoveNext());
                //Assert.IsTrue(result.Current is Assembly);
            }
        }
    }
}
