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
            //using (ShimsContext.Create())
            //{
            //    Microsoft.SqlServer.ReportingServices.Fakes.ShimReportExecutionService.AllInstances.RenderStringStringStringOutStringOutStringOutWarningArrayOutStringArrayOut =
            //        (format, deviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIds, out result) => 
            //        {
                    
                    
            //        };
            //     }
                IEnumerator result;

                InvokeSrsReport target = new InvokeSrsReport()
                {
                    Format = "PDF",
                    //Parameters = { { "Year", "2013" } },
                    Report = "/Public/NAEP Report",
                    ReportServerUrl = "http://localhost/REPORTSERVER/ReportExecution2005.asmx"
                };

                result = target.Invoke().GetEnumerator();

                Assert.IsTrue(result.MoveNext());
                //Assert.IsTrue(result.Current is Assembly);
           
        }
    }
}
