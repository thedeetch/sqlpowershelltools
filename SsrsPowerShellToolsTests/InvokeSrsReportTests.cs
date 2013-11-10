using System;
using System.Collections.Generic;
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
            //Microsoft.SqlServer.ReportingServices.Fakes.ShimReportExecutionService.AllInstances.RenderStringStringStringOutStringOutStringOutWarningArrayOutStringArrayOut =
            //    (format, deviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIds, out result) => 
            //    {


            //    };
            // }
            IEnumerator result;
            ReportOutput actual;


            InvokeSrsReport target = new InvokeSrsReport()
            {
                Format = "PDF",
                Parameters = new Dictionary<string, string> 
                    { 
                        { "schoolyear", "2010-2011" } ,
                        { "testname", "NAEP Math Grade 4"}, // 1
                        { "breakdown", "4" }, //1
                        { "aggroup", "12"}
                    },
                Report = "/Public/NAEP Report",
                ReportServerUrl = "http://edw.vermont.gov/REPORTSERVER/ReportExecution2005.asmx"
            };

            result = target.Invoke().GetEnumerator();

            Assert.IsTrue(result.MoveNext());
            Assert.IsTrue(result.Current is ReportOutput);

            actual = (ReportOutput)result.Current;

            Assert.IsNotNull(actual.Result);
        }
    }
}

