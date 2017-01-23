using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlServerCe;

namespace RapidSync  
{
    public class DBhandler
    {
        public static String rdaConnection()
        {
            string newString = @"Provider=SQLOLEDB;" +
              "Data Source=WEBINVSERV\\MSSQLSERVER;" +
              "Initial Catalog=RAPIDINVENTORY;" +
              "User Id = sa;" +
              "Password = LocalRposAdmin!;";
            return newString;
        }
        
        public void createDB()  
        {
            if (!File.Exists("RAPID_COUNT.sdf"))
            {
                using (SqlCeEngine sqlEngine = new SqlCeEngine())
                {
                sqlEngine.LocalConnectionString = "Data Source=RAPID_COUNT.sdf";
                sqlEngine.CreateDatabase();
                }
            }
        }
        public void deleteData(String tableName)
        {
            if (File.Exists("RAPID_COUNT.sdf"))
            {
                File.Delete("RAPID_COUNT.sdf");
            }
            createDB();
        }
    }

    public class RDAsync   
    {
        public void DropTable(string tableName, string connectionString) 
        {
            using (SqlCeConnection cn = new SqlCeConnection(connectionString))
            {
                SqlCeCommand cmd = cn.CreateCommand();
                cmd.CommandText=String.Format("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = ‘{0}’", tableName);
                cn.Open();

                if((int)cmd.ExecuteScalar() == 1)
                {
                    cmd.CommandText = String.Format("DROP TABLE {0}", tableName);
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
                cmd.Dispose();
            }
        }

        public void RefreshLogins()  
        {
            using (SqlCeRemoteDataAccess rda = new SqlCeRemoteDataAccess())
            {
                String connection = DBhandler.rdaConnection(); 
                rda.InternetUrl = "https://rapidphysicalcount.com:3441/rda/sqlcesa35.dll";
                rda.LocalConnectionString = "Data Source=RAPID_COUNT.sdf";

                //Drop Table
                DropTable("RAPID_USR_ACCT", rda.LocalConnectionString);

                //Pull Table
                rda.Pull("RAPID_USR_ACCT",
                    "SELECT * FROM RAPID_USR_ACCT",
                    connection, 
                    RdaTrackOption.TrackingOnWithIndexes,
                    "UserErrorTable");

                //Check ErrorTable for entried to roll back
                using (SqlCeConnection cn = new SqlCeConnection(rda.LocalConnectionString))
                {
                    SqlCeCommand cmd = cn.CreateCommand();
                    cmd.CommandText=String.Format("SELECT COUNT(*) from UserErrorTable", cn);
                }
            }
        }

        public void RefreshItemData()
        {
            using (SqlCeRemoteDataAccess rda = new SqlCeRemoteDataAccess())
            {
                using (SqlCeConnection cn = new SqlCeConnection(rda.LocalConnectionString))
                {
                    string connection = DBhandler.rdaConnection(); 
                    rda.InternetUrl = "https://rapidphysicalcount.com:3441/rda/sqlcesa35.dll";
                    rda.LocalConnectionString = "Data Source=RAPID_COUNT.sdf";

                    //Drop Table
                    DropTable("RAPID_IM_BARCOD", rda.LocalConnectionString);

                    //Pull Table
                    rda.Pull("RAPID_IM_BARCOD",
                        "SELECT * FROM RAPID_IM_BARCOD WHERE COMPANY_NAM = '' AND LOC_ID = ''",
                        connection,
                        RdaTrackOption.TrackingOnWithIndexes,
                        "BarcodErrorTable");

                    //Check ErrorTable for entried to roll back
                    SqlCeCommand cmd = cn.CreateCommand();
                    cmd.CommandText = String.Format("SELECT COUNT(*) from BarcodErrorTable", cn);

                    //Drop Table
                    DropTable("RAPID_IM_GRID_DIMS", rda.LocalConnectionString);

                    //Pull Table
                    rda.Pull("RAPID_IM_GRID_DIMS",
                        "SELECT * FROM RAPID_IM_GRID_DIMS WHERE COMPANY_NAM = '' AND LOC_ID = ''",
                        connection,
                        RdaTrackOption.TrackingOnWithIndexes,
                        "GridErrorTable");

                    //Check ErrorTable for entried to roll back
                    cmd.CommandText = String.Format("SELECT COUNT(*) from GridErrorTable", cn);

                    //Drop Table
                    DropTable("RAPID_IM_INV", rda.LocalConnectionString);

                    //Pull Table
                    rda.Pull("RAPID_IM_INV",
                        "SELECT * FROM RAPID_IM_INV WHERE COMPANY_NAM = '' AND LOC_ID = ''",
                        connection,
                        RdaTrackOption.TrackingOnWithIndexes,
                        "InvErrorTable");

                    //Check ErrorTable for entried to roll back
                    cmd.CommandText = String.Format("SELECT COUNT(*) from InvErrorTable", cn);

                    //Drop Table
                    DropTable("RAPID_IM_INV_CELL", rda.LocalConnectionString);

                    //Pull Table
                    rda.Pull("RAPID_IM_INV_CELL",
                        "SELECT * FROM RAPID_IM_INV_CELL WHERE COMPANY_NAM = '' AND LOC_ID = ''",
                        connection,
                        RdaTrackOption.TrackingOnWithIndexes,
                        "InvCellErrorTable");

                    //Check ErrorTable for entried to roll back
                    cmd.CommandText = String.Format("SELECT COUNT(*) from InvCellErrorTable", cn);

                    //Drop Table
                    DropTable("RAPID_IM_ITEM", rda.LocalConnectionString);

                    //Pull Table
                    rda.Pull("RAPID_IM_ITEM",
                        "SELECT * FROM RAPID_IM_ITEM WHERE COMPANY_NAM = '' AND LOC_ID = ''",
                        connection,
                        RdaTrackOption.TrackingOnWithIndexes,
                        "ItemErrorTable");

                    //Check ErrorTable for entried to roll back
                    cmd.CommandText = String.Format("SELECT COUNT(*) from ItemErrorTable", cn);
                }    
            }
        }

        public void uploadCount()
        {
            SqlCeRemoteDataAccess rda = new SqlCeRemoteDataAccess();
            String connection = DBhandler.rdaConnection(); 
            rda.InternetUrl = "https://rapidphysicalcount.com:3441/rda/sqlcesa35.dll";
            rda.LocalConnectionString = "Data Source=RAPID_COUNT.sdf";
            rda.Push("RAPID_RAW_SCAN_DATA", connection, RdaBatchOption.BatchingOn);
        }
    }
}