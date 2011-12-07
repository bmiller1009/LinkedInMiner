using System;
using System.Data;
using System.Data.OleDb;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Globals;

namespace LinkedInMiner.Data
{
	internal class ConnectionFactory
	{
		public static IDbConnection GetConnection ()
		{
			if(Global.ConnectionString.Contains("text"))
				return new OleDbConnection(Global.ConnectionString);
			else
				return new MySqlConnection(Global.ConnectionString);
		}
	}
}

