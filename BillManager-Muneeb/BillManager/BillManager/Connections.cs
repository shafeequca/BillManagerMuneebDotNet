using System;
using System.Data;
using System.Data.OleDb;


namespace BillManager
{
    public sealed class Connections
    {
        private static Connections instance = null;

        string path = Environment.CurrentDirectory + "\\DB\\DB.mdb;";

        string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source = "+ Environment.CurrentDirectory + "\\DB\\DB.mdb;Persist Security Info=False;";
        string App_Type = System.Configuration.ConfigurationSettings.AppSettings["App_Type"];

        //string ConnectionString = @"Data Source=tcp:192.168.100.12;Initial Catalog=Inventory_GST;User ID=sa;Password=INS";

        //string ConnectionString = @"Data Source=USER-PC;Initial Catalog=Inventory;Integrated Security=True;";



        public OleDbConnection con;

        public static Connections Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Connections();
                }
                return instance;
            }
        }
        public void OpenConection()
        {

            con = new OleDbConnection(ConnectionString);
            con.Open();
        }


        public void CloseConnection()
        {
            con.Close();
        }


        public void ExecuteQueries(string Query_)
        {
            //SqlCommand cmd = new SqlCommand(Query_, con);

            OpenConection();
            OleDbCommand cmd = new OleDbCommand(Query_, con);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public Int32 ExuecuteQueryWithReturn(string Query_)
        {
            //using (SqlCommand cmd = new SqlCommand(Query_, con))
            //{
            //    int modified = (int)cmd.ExecuteScalar();
            //    return modified;
            //}
            OpenConection();
            using (OleDbCommand cmd = new OleDbCommand(Query_, con))
            {
                int modified = (int)cmd.ExecuteScalar();
                CloseConnection();
                return modified;
            }
            CloseConnection();
        }

        public OleDbDataReader DataReader(string Query_)
        {
            //SqlCommand cmd = new SqlCommand(Query_, con);
            //SqlDataReader dr = cmd.ExecuteReader();
            //return dr;
            OpenConection();
            OleDbCommand cmd = new OleDbCommand(Query_, con);
            OleDbDataReader dr = cmd.ExecuteReader();
            CloseConnection();
            return dr;
        }


        public object ShowDataInGridView(string Query_)
        {
            OpenConection();
            OleDbDataAdapter dr = new OleDbDataAdapter(Query_, ConnectionString);
            DataSet ds = new DataSet();
            dr.Fill(ds);
            dr.Dispose();
            object dataum = ds.Tables[0];
            CloseConnection();
            ds.Dispose();
            return dataum;
        }
    }
}
