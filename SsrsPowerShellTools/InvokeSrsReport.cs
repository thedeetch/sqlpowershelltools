using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net;
using Microsoft.SqlServer.ReportingServices;
using System.Security.Principal;

namespace SsrsPowerShellTools
{
    /// <summary>
    /// Used to run SSRS reports
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "SrsReport")]
    public class InvokeSrsReport : Cmdlet
    {
        public InvokeSrsReport()
            : base()
        {
            this.Parameters = new Dictionary<string, string>();
            this.DeviceInfo = "<DeviceInfo></DeviceInfo>";
            this.Format = "PDF";
            this.Credential = System.Net.CredentialCache.DefaultNetworkCredentials;
        }

        #region Private Properties

        ReportExecutionService _client;

        #endregion Private Properties

        #region Public Properties
        /// <summary>
        /// The URL of the ReportExecution2005 service.
        /// </summary>
        [Parameter(Position = 0,
            Mandatory = true,
            HelpMessage = "The URL of the ReportExecution2005 service.")]
        public string ReportServerUrl { get; set; }

        /// <summary>
        /// The full name of the report.
        /// </summary>
        [Parameter(Position = 1,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The full name and path of the report.")]
        public string Report { get; set; }

        /// <summary>
        /// The format in which to render the report. This argument maps to a rendering extension. Supported extensions include XML, NULL, CSV, IMAGE, PDF, HTML4.0, HTML3.2, MHTML, EXCEL, and Word. A list of supported extensions may be obtained by calling the ListRenderingExtensions method.
        /// </summary>
        [Parameter(Position = 2,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The format in which to render the report. This argument maps to a rendering extension. Supported extensions include XML, NULL, CSV, IMAGE, PDF, HTML4.0, HTML3.2, MHTML, EXCEL, and Word. A list of supported extensions may be obtained by calling the ListRenderingExtensions method.")]
        [ValidateNotNullOrEmpty]
        [PSDefaultValue(Help = "", Value = "PDF")]
        public string Format { get; set; }

        /// <summary>
        /// The parameters for the report run.
        /// </summary>
        [Parameter(Position = 3,
            ValueFromRemainingArguments = true,
            HelpMessage = "The parameters for the report run.")]
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// An XML string that contains the device-specific content that is required by the rendering extension specified in the Format parameter. DeviceInfo settings must be passed as internal elements of a DeviceInfo XML element. For more information about device information settings for specific output formats, see <see cref="http://technet.microsoft.com/en-us/library/ms155397.aspx">Passing Device Information Settings to Rendering Extensions</see>.
        /// </summary>
        [Parameter(HelpMessage = "An XML string that contains the device-specific content that is required by the rendering extension specified in the Format parameter. DeviceInfo settings must be passed as internal elements of a DeviceInfo XML element. For more information about device information settings for specific output formats, see Passing Device Information Settings to Rendering Extensions.")]
        [ValidateNotNullOrEmpty]
        public string DeviceInfo { get; set; }

        /// <summary>
        /// The history ID of the snapshot.
        /// </summary>
        [Parameter(HelpMessage = "The history ID of the snapshot.")]
        [ValidateNotNullOrEmpty]
        public string HistoryId { get; set; }

        /// <summary>
        /// The credentials to use when rendering the report.
        /// </summary>
        [Parameter(HelpMessage = "The credentials to use when rendering the report.")]
        [ValidateNotNullOrEmpty]
        public ICredentials Credential { get; set; }

        #endregion Public Properties

        #region Pipeline Methods
        /// <summary>
        /// Provides a record-by-record processing functionality for the cmdlet.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            ExecutionInfo executionInfo;

            List<ParameterValue> parameterValues = new List<ParameterValue>();

            byte[] result;
            string extension;
            string mimeType;
            string encoding;
            Warning[] warnings = {};
            string[] streamIds;

            ReportOutput output;

            // Load the report
            executionInfo = _client.LoadReport(this.Report, this.HistoryId);

            // Map the parameters
            foreach (KeyValuePair<string, string> parameter in this.Parameters)
            {
                parameterValues.Add(new ParameterValue()
                    {
                        Name = parameter.Key,
                        Value = parameter.Value
                    });
            }

            // Set the execution parameters
            if (parameterValues.Count > 0)
                executionInfo = _client.SetExecutionParameters(parameterValues.ToArray(), null);

            // Render the report
            result = _client.Render(this.Format, this.DeviceInfo, out extension, out mimeType, out encoding, out warnings, out streamIds);

            // Write out any warnings we received
            foreach (Warning warning in warnings ?? new Warning[0])
            {
                WriteWarning(string.Format("{0} [Code: {1}, Severity: {2}, ObjectName: {3}, ObjectType: {4}]", warning.Message, warning.Code, warning.Severity, warning.ObjectName, warning.ObjectType));
            }

            // Build output object
            output = new ReportOutput()
            {
                ExecutionInfo = executionInfo,
                Result = result,
                Extension = extension,
                MimeType = mimeType,
                Encoding = encoding,
                Warnings = warnings,
                StreamIds = streamIds
            };

            // Write output to pipeline
            WriteObject(output);
        }

        /// <summary>
        /// Provides a one-time, preprocessing functionality for the cmdlet.
        /// </summary>
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            // Build client
            _client = new ReportExecutionService(this.ReportServerUrl);
            _client.Credentials = this.Credential;
        }

        /// <summary>
        /// Provides a one-time, post-processing functionality for the cmdlet.
        /// </summary>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            _client.Dispose();
        }

        /// <summary>
        /// Stops processing records when the user stops the cmdlet asynchronously.
        /// </summary>
        protected override void StopProcessing()
        {
            base.StopProcessing();

            _client.Abort();
        }

        #endregion Pipeline Methods
    }
}
