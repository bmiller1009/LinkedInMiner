using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace LinkedInMiner
{
	public class Data
	{	
		private Data ()
		{
		}
		
		public static int ExecuteScalar(string sqlText)
		{  
		   try
		   {	
			   long retVal;
			   	
		       using(var dbconn = new MySqlConnection(Global.ConnectionString))
			   {
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
	}
}