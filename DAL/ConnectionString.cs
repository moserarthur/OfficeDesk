using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class ConnectionString
    {
        //public static SqlConnection Connection { get; set; } = new SqlConnection("Data Source=vetontracksqlserver.ce7vxurcnd8m.sa-east-1.rds.amazonaws.com;Initial Catalog=BDVetOnTrack;User ID=admin;Password=12345678");
        //public static SqlConnection Connection { get; set; } = new SqlConnection("Data Source=DESKTOP-A4R4I53;Initial Catalog=BDVetOnTrack;Integrated Security=True");
        public static SqlConnection Connection { get; set; } = new SqlConnection("Data Source=DESKTOP-A4R4I53;Initial Catalog=BDOfficeSoft;Integrated Security=True");
    }
}

