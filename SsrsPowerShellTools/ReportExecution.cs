using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Principal;

namespace SsrsPowerShellTools
{
    public class ReportExecution
    {
        #region Constructors

        public ReportExecution()
            : base()
        {
            this.Parameters = new Dictionary<string, string>();
            this.DeviceInfo = "<DeviceInfo></DeviceInfo>";
            this.Credential = System.Net.CredentialCache.DefaultNetworkCredentials;
        }

        #endregion Constructors

        #region Public Properties
        /// <summary>
        /// The URL of the ReportExecution2005 service.
        /// </summary>
        public string ReportServerUrl { get; set; }

        /// <summary>
        /// The full name of the report.
        /// </summary>
        public string Report { get; set; }

        /// <summary>
        /// The format in which to render the report. This argument maps to a rendering extension. Supported extensions include XML, NULL, CSV, IMAGE, PDF, HTML4.0, HTML3.2, MHTML, EXCEL, and Word. A list of supported extensions may be obtained by calling the ListRenderingExtensions method.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// The parameters for the report run.
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// An XML string that contains the device-specific content that is required by the rendering extension specified in the Format parameter. DeviceInfo settings must be passed as internal elements of a DeviceInfo XML element. For more information about device information settings for specific output formats, see <see cref="http://technet.microsoft.com/en-us/library/ms155397.aspx">Passing Device Information Settings to Rendering Extensions</see>.
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// The history ID of the snapshot.
        /// </summary>
        public string HistoryId { get; set; }

        /// <summary>
        /// The credentials to use when rendering the report.
        /// </summary>
        public NetworkCredential Credential { get; set; }

        #endregion Public Properties

        #region Private Properties

        ReportExecutionServiceSoapClient _client;

        #endregion Private Properties

        #region Public Methods

        public ReportOutput Execute()
        {
            ExecutionInfo executionInfo;
            List<ParameterValue> parameterValues = new List<ParameterValue>();
            TrustedUserHeader trustedUserHeader = null;
            ServerInfoHeader serverInfoHeader;
            ExecutionHeader executionHeader;

            byte[] result;
            string extension;
            string mimeType;
            string encoding;
            Warning[] warnings = { };
            string[] streamIds;

            // Build client
            _client = new ReportExecutionServiceSoapClient(new BasicHttpBinding(), new EndpointAddress(this.ReportServerUrl));

            // Set credentials
            _client.ClientCredentials.Windows.ClientCredential = this.Credential;
            _client.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            // Load the report
            executionHeader = _client.LoadReport(trustedUserHeader, this.Report, this.HistoryId, out serverInfoHeader, out executionInfo);

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
                serverInfoHeader = _client.SetExecutionParameters(executionHeader, trustedUserHeader, parameterValues.ToArray(), null, out executionInfo);

            // Render the report
            serverInfoHeader = _client.Render(executionHeader, trustedUserHeader, this.Format, this.DeviceInfo, out result, out extension, out mimeType, out encoding, out warnings, out streamIds);

            // Build output object
            return new ReportOutput()
            {
                ExecutionInfo = executionInfo,
                Result = result,
                Extension = extension,
                MimeType = mimeType,
                Encoding = encoding,
                Warnings = warnings,
                StreamIds = streamIds
            };
        }

        public void Abort()
        {
            _client.Abort();
        }

        public void Close()
        {
            _client.Close();
        }

        #endregion Public Methods
    }
}
