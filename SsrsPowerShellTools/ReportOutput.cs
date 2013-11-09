using System;
using System.Collections.Generic;
using ReportExecution2005;

namespace SsrsPowerShellTools
{
    public class ReportOutput
    {
        /// <summary>
        /// An object containing information about the report server.
        /// </summary>
        public ServerInfoHeader ServerInfoHeader { get; set; }

        /// <summary>
        /// An object containing information for the executed report.
        /// </summary>
        public ExecutionInfo ExecutionInfo { get; set; }

        /// <summary>
        /// A Byte[] array of the report in the specified format.
        /// </summary>
        public byte[] Result { get; set; }

        /// <summary>
        /// The file extension corresponding to the output stream.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// The MIME type of the rendered report.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// The encoding used when report server renders the contents of the report.
        /// </summary>
        public string Encoding { get; set; }

        /// <summary>
        /// An array of Warning objects that describes any warnings that occurred during report processing.
        /// </summary>
        public IEnumerable<Warning> Warnings { get; set; }

        /// <summary>
        /// The stream identifiers. These IDs are passed to the RenderStream method. You can use them to render the external resources (images, etc.) that are associated with a given report.If the IMAGE rendering extension is used, the method outputs an empty array in StreamIds.
        /// </summary>
        public IEnumerable<string> StreamIds { get; set; }
    }
}
