using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for db_connection
/// </summary>
public class db_connection
{
    public MySqlConnection connection;
string connectionString;
	public db_connection()
	{
        DB_configuration();
	}

    //Initialize values
    private void DB_configuration()
    {
        
        connectionString = "server=127.0.0.1; user id=root;password=630716;persistsecurityinfo=True;database=fypdb";
        connection = new MySqlConnection(connectionString);
    }

    public string getConn(){
        return connectionString;
    }

    //open connection to database
    public void open()
    {
        connection.Open();
    }

    //Close connection
    public void close()
    {
        connection.Close();
    }


}