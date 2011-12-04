using System;
using System.Data;
using MySql.Data.MySqlClient;
using Globals;

namespace LinkedInMiner
{	
	/// <summary>
	/// Small wrapper class for persisting new records to MySQL
	/// </summary>
	internal class Data
	{	
		#region Constructors
		private Data (){}
		#endregion
		
		#region Public Methods
		/// <summary>
		/// Executes a sql string
		/// </summary>
		/// <returns>
		/// Primary key of the new record.
		/// </returns>
		/// <param name='sqlText'>
		/// SQL string to run
		/// </param>
		public static int ExecuteScalar(string sqlText)
		{  
		   try
		   {	
			   long retVal;
			   	
		       using(var dbconn = new MySqlConnection(Global.ConnectionString))
			   {	
					//TODO:  Should change this to a transaction
					dbconn.Open();
					
			        using (var dbcmd = dbconn.CreateCommand())
					{
			      		dbcmd.CommandText = sqlText;
						dbcmd.ExecuteNonQuery();
						
						dbcmd.CommandText = "SELECT LAST_INSERT_ID()";
						retVal = (long)dbcmd.ExecuteScalar();
					}
					
					dbconn.Close();
				}
				
				return Convert.ToInt32(retVal);
			}
			catch
			{
				throw;
			}
		}
		#endregion
	}
}