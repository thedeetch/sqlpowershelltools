using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SqlPowerShellTools.Dbms
{
    public class SqlExecution
    {
        #region Private Properties

        SqlConnectionStringBuilder _builder;
        SqlCommand _command;

        #endregion Private Properties

        #region Constructors

        public SqlExecution()
            : base()
        {
            _builder = new SqlConnectionStringBuilder();
            this.CommandTimeout = 0;
            this.Parameters = new Dictionary<string, object>();
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the name or network address of the instance of SQL Server to connect to.
        /// </summary>
        public string ServerInstance
        {
            get { return _builder.DataSource; }
            set { _builder.DataSource = value; }
        }

        /// <summary>
        /// Gets or sets the name of the database associated with the connection.
        /// </summary>
        public string Database
        {
            get { return _builder.InitialCatalog; }
            set { _builder.InitialCatalog = value; }
        }

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the user ID to be used when connecting to SQL Server.
        /// </summary>
        public string Username
        {
            get { return _builder.UserID; }
            set { _builder.UserID = value; }
        }

        /// <summary>
        /// Gets or sets the password for the SQL Server account.
        /// </summary>
        public string Password
        {
            set { _builder.Password = value; }
        }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// Gets or sets the length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.
        /// </summary>
        public int ConnectionTimeout
        {
            get { return _builder.ConnectTimeout; }
            set { _builder.ConnectTimeout = value; }
        }

        /// <summary>
        /// Identifies a file that contains a batch of SQL statements or stored procedures.
        /// </summary>
        public string InputFile { get; set; }

        /// <summary>
        /// Declares the application workload type when connecting to a database in an SQL Server Availability Group. 
        /// </summary>
        public ApplicationIntent ApplicationIntent
        {
            get { return _builder.ApplicationIntent; }
            set { _builder.ApplicationIntent = value; }
        }

        /// <summary>
        /// Occurs when SQL Server returns a warning or informational message.
        /// </summary>
        public SqlInfoMessageEventHandler InfoMessageEventHandler { get; set; }

        /// <summary>
        /// Gets or sets the name or address of the partner server to connect to if the primary server is down.
        /// </summary>
        public string FailoverPartner
        {
            get { return _builder.FailoverPartner; }
            set { _builder.FailoverPartner = value; }
        }

        /// <summary>
        /// If your application is connecting to an AlwaysOn availability group (AG) on different subnets, setting MultiSubnetFailover=true provides faster detection of and connection to the (currently) active server. 
        /// </summary>
        public bool MultiSubnetFailover
        {
            get { return _builder.MultiSubnetFailover; }
            set { _builder.MultiSubnetFailover = value; }
        }

        /// <summary>
        /// The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.
        /// </summary>
        public IDictionary<string, object> Parameters { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Tries to cancel execution
        /// </summary>
        public void Cancel()
        {
            // If we're not null, assume we're executing
            if (_command != null)
                _command.Cancel();
        }

        /// <summary>
        /// Executes the specified query
        /// </summary>
        /// <returns>A DataSet containing the results</returns>
        public DataSet Execute()
        {
            string queryText;
            DataSet dataSet = new DataSet();

            // Set integrated security option
            if (this.Username == null)
                _builder.IntegratedSecurity = true;

            // Build query
            if (this.InputFile == null)
                queryText = System.IO.File.ReadAllText(this.InputFile);
            else
                queryText = this.Query;

            // Create connection
            using (SqlConnection connection = new SqlConnection(_builder.ConnectionString))
            using (_command = new SqlCommand(queryText, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(_command))
            {
                // Add print statement handler
                connection.FireInfoMessageEventOnUserErrors = true;
                connection.InfoMessage += this.InfoMessageEventHandler;

                // Set command properties
                _command.CommandTimeout = this.CommandTimeout;
                _command.CommandType = CommandType.Text;

                // Add parameters
                foreach (KeyValuePair<string, object> parameter in this.Parameters)
                    _command.Parameters.AddWithValue(parameter.Key, parameter.Value);

                // Open connection
                connection.Open();

                // Execute statement and fill dataset
                adapter.Fill(dataSet);

                // Close the connection
                connection.Close();
            }

            // Lets set it to null so we know it's been disposed
            _command = null;

            return dataSet;
        }

        #endregion Public Methods
    }
}
