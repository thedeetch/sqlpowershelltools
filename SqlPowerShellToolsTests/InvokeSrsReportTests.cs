using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using SsrsPowerShellTools;

namespace SsrsPowerShellTools.Tests
{
    [TestClass]
    public class InvokeSrsReportTests
    {
        [TestMethod]
        public void ProcessRecordTest()
        {
            IEnumerator result;
            ReportOutput actual;

            InvokeSrsReport target = new InvokeSrsReport()
            {
                Format = "PDF",
                Parameters = new Hashtable 
                    { 
                        { "schoolyear", "2010-2011" } ,
                        { "testname", "NAEP Math Grade 4"}, // 1
                        { "breakdown", "4" }, //1
                        { "aggroup", "12"}
                    },
                Report = "/Public/NAEP Report",
                ReportServerUrl = "http://contoso.com/REPORTSERVER/ReportExecution2005.asmx"
            };

            result = target.Invoke().GetEnumerator();

            Assert.IsTrue(result.MoveNext());
            Assert.IsTrue(result.Current is ReportOutput);

            actual = (ReportOutput)result.Current;

            Assert.IsNotNull(actual.Result);
        }
    }
}

