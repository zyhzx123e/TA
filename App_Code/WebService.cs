using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MySql.Data.MySqlClient;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for WebService
/// </summary>
/// 

[WebService(Namespace = "http://tempuri.org/")]

[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {
 //   pages_TA_roundingDashboard roundingDashboard = new pages_TA_roundingDashboard();
    public WebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
       
    }

    [WebMethod]


    public static string[] GetPC(string prefix)
    {
        List<string> PCS = new List<string>();
        db_connection db_obj1 = new db_connection();

        using (db_obj1.connection)
        {
            string sql = "select fypdb.problem.pc from fypdb.problem where fypdb.problem.pc like '" + prefix + "%'";
            // conn.ConnectionString = ConfigurationManager.ConnectionStrings[db_obj1.getConn()].ConnectionString;
            using (MySqlCommand cmd = new MySqlCommand())
            {
                sql = "select fypdb.problem.pc from fypdb.problem where fypdb.problem.pc like '" + prefix + "%'";
                cmd.CommandText = sql;

                cmd.Connection = db_obj1.connection;
                db_obj1.open();
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        PCS.Add(string.Format("{0}-{1}", sdr["pc"]));
                    }
                }
                db_obj1.close();
            }
        }
        return PCS.ToArray();
    }
   
   


  

    
}
