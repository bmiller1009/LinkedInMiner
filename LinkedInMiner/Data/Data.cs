using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;
using Globals;
using Logging;

namespace LinkedInMiner.Data
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
		public static int ExecuteScalar(string sqlText, Logger logger)
		{  
		   long retVal = 0;	
			
		   try
		   {	
		       using(var dbconn = ConnectionFactory.GetConnection())
			   {	
					dbconn.Open();
					
					using(var transaction = dbconn.BeginTransaction())
					{	
						try
						{
					        using (var dbcmd = dbconn.CreateCommand())
							{
					      		dbcmd.CommandText = sqlText;
								dbcmd.Transaction = transaction;
								
								dbcmd.ExecuteNonQuery();
								
								dbcmd.CommandText = "SELECT LAST_INSERT_ID()";
								retVal = (long)dbcmd.ExecuteScalar();
							}
						}
						catch(Exception ex)
						{
							logger.AddLogMessage(ex);
							
							try
							{
								transaction.Rollback();
							}
							catch(Exception rollbackException)
							{
								logger.AddLogMessage(rollbackException);
							}
							
							throw;
						}
						
						transaction.Commit();
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