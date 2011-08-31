using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace LinkedInMiner
{
	public class Data
	{	
		public Data ()
		{
		}
		
		public int ExecuteScalar(string sqlText)
		{  
		   try
		   {	
			   long retVal;
			   	
		       using(var dbcon = new MySqlConnection(Global.ConnectionString))
			   {
					dbcon.Open();
			        using (var dbcmd = dbcon.CreateCommand())
					{
			      		dbcmd.CommandText = sqlText;
						dbcmd.ExecuteNonQuery();
						
						dbcmd.CommandText = "SELECT LAST_INSERT_ID()";
						retVal = (long)dbcmd.ExecuteScalar();
					}
					
					dbcon.Close();
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

