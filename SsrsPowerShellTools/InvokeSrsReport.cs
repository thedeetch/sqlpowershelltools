using System;
using System.Collections.Generic;
using System.Collections;
using System.Management.Automation;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Principal;
using System.Linq;

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
            this.Parameters = new Hashtable();
        }

        #region Private Properties

        ReportExecution _execution;

        #endregion Private Properties

        #region Public Properties
        /// <summary>
        /// The URL of the ReportExecution2005 service.
        /// </summary>
        [Parameter(Position = 0,
            Mandatory = true,
            HelpMessage = "The URL of the ReportExecution2005 service.")]
        [ValidatePattern(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?")]
        public string ReportServerUrl
        {
            get { return _execution.ReportServerUrl; }
            set { _execution.ReportServerUrl = value; }
        }

        /// <summary>
        /// The full name of the report.
        /// </summary>
        [Parameter(Position = 1,
            Mandatory = true,
            HelpMessage = "The full name and path of the report.")]
        [ValidatePattern(@"^/.+")]
        public string Report
        {
            get { return _execution.Report; }
            set { _execution.Report = value; }
        }

        /// <summary>
        /// The format in which to render the report. This argument maps to a rendering extension. Supported extensions include XML, NULL, CSV, IMAGE, PDF, HTML4.0, HTML3.2, MHTML, EXCEL, and Word. A list of supported extensions may be obtained by calling the ListRenderingExtensions method.
        /// </summary>
        [Parameter(Position = 2,
            Mandatory = true,
            HelpMessage = "The format in which to render the report. This argument maps to a rendering extension. Supported extensions include XML, NULL, CSV, IMAGE, PDF, HTML4.0, HTML3.2, MHTML, EXCEL, and Word. A list of supported extensions may be obtained by calling the ListRenderingExtensions method.")]
        [ValidateNotNullOrEmpty]
        public string Format
        {
            get { return _execution.Format; }
            set { _execution.Format = value; }
        }

        /// <summary>
        /// The parameters for the report run.
        /// </summary>
        [Parameter(Position = 3,
            ValueFromRemainingArguments = true,
            HelpMessage = "The parameters for the report run.")]
        public Hashtable Parameters { get; set; }

        /// <summary>
        /// An XML string that contains the device-specific content that is required by the rendering extension specified in the Format parameter. DeviceInfo settings must be passed as internal elements of a DeviceInfo XML element. For more information about device information settings for specific output formats, see <see cref="http://technet.microsoft.com/en-us/library/ms155397.aspx">Passing Device Information Settings to Rendering Extensions</see>.
        /// </summary>
        [Parameter(HelpMessage = "An XML string that contains the device-specific content that is required by the rendering extension specified in the Format parameter. DeviceInfo settings must be passed as internal elements of a DeviceInfo XML element. For more information about device information settings for specific output formats, see Passing Device Information Settings to Rendering Extensions.")]
        public string DeviceInfo
        {
            get { return _execution.DeviceInfo; }
            set { _execution.DeviceInfo = value; }
        }

        /// <summary>
        /// The history ID of the snapshot.
        /// </summary>
        [Parameter(HelpMessage = "The history ID of the snapshot.")]
        public string HistoryId
        {
            get { return _execution.HistoryId; }
            set { _execution.HistoryId = value; }
        }

        /// <summary>
        /// The credentials to use when rendering the report.
        /// </summary>
        [Parameter(HelpMessage = "The credentials to use when rendering the report.")]
        [ValidateNotNullOrEmpty]
        public NetworkCredential Credential
        {
            get { return _execution.Credential; }
            set { _execution.Credential = value; }
        }

        #endregion Public Properties

        #region Pipeline Methods

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            _execution = new ReportExecution();
        }

        /// <summary>
        /// Provides a record-by-record processing functionality for the cmdlet.
        /// </summary>
        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            ReportOutput output;

            // Map parameters to dictionary
            _execution.Parameters = this.Parameters
                .Cast<DictionaryEntry>()
                .ToDictionary(kvp => (string)kvp.Key, kvp => (string)kvp.Value);

            // Execute report
            output = _execution.Execute();

            // Print warnings
            foreach (Warning warning in output.Warnings ?? new Warning[0])
                WriteWarning(warning.Message);

            // Write output to pipeline
            WriteObject(output);
        }

        /// <summary>
        /// Provides a one-time, post-processing functionality for the cmdlet.
        /// </summary>
        protected override void EndProcessing()
        {
            base.EndProcessing();

            _execution.Close();
        }

        /// <summary>
        /// Stops processing records when the user stops the cmdlet asynchronously.
        /// </summary>
        protected override void StopProcessing()
        {
            base.StopProcessing();

            _execution.Abort();
        }

        #endregion Pipeline Methods
    }
}
