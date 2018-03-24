using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for problem
/// </summary>
public class problem
{

    public int id { get; set; }
    public string venue { get; set; }
    public string pc { get; set; }
    public string description { get; set; }
    public int location_id { get; set; }
    public string shift { get; set; }
    public string date{ get; set; }
    public char status { get; set; }
    public char type { get; set; }
    public int added_by { get; set; }

    public problem()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}