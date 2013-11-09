using System;
using System.Management.Automation;
using System.ComponentModel;

namespace SsrsPowerShellTools
{

    #region PowerShell snap-in
    /// <summary>
    /// Create the Windows PowerShell snap-in for this sample.
    /// </summary>
    [RunInstaller(true)]
    public class SsrsPowerShellToolsSnapIn : PSSnapIn
    {
        /// <summary>
        /// Initializes a new instance of the GetProcPSSnapIn01 class.
        /// </summary>
        public SsrsPowerShellToolsSnapIn()
            : base()
        {
        }

        /// <summary>
        /// Get a name for the snap-in. This name is used to register
        /// the snap-in.
        /// </summary>
        public override string Name
        {
            get
            {
                return "SsrsPowerShellToolsSnapIn";
            }
        }

        /// <summary>
        /// Get the name of the vendor of the snap-in.
        /// </summary>
        public override string Vendor
        {
            get
            {
                return "thedeetch";
            }
        }

        /// <summary>
        /// Get the resource information for vendor. This is a string of format: 
        /// resourceBaseName,resourceName. 
        /// </summary>
        public override string VendorResource
        {
            get
            {
                return "SsrsPowerShellToolsSnapIn,thedeetch";
            }
        }

        /// <summary>
        /// Get a description the snap-in.
        /// </summary>
        public override string Description
        {
            get
            {
                return "This is a PowerShell snap-in for making SSRS intractions simpler.";
            }
        }
    }
    #endregion PowerShell snap-in
}
