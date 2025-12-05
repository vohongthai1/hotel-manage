using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DAO
{
    public class DataProvider
    {
        private static DataProvider instance;

        // CHUỖI KẾT NỐI ĐÚNG THEO MÁY BẠN (TỪ SSMS)
        private string connectionStr =
            @"Data Source=VOHONGTHAI\SQLEXPRESS;Initial Catalog=HotelManagement;Integrated Security=True;TrustServerCertificate=True;";

        // Các dòng cũ (đã comment để tránh nhầm)
        // private string connectionStr = @"Data Source=DESKTOP-83IOFVC;Initial Catalog=Hotel_ManagentBK;Integrated Security=True";
        // private string connectionStr = @"Data Source=THIEN-AI\THIENAI;Initial Catalog=HotelManagement;Integrated Security=True";
        // private string connectionStr = @"Data Source=.\sqlexpress;Initial Catalog=HotelManagement;Integrated Security=True";

        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    AddParameter(query, parameter, command);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(data);
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi kết nối CSDL: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return data;
        }

        public int ExecuteNoneQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    AddParameter(query, parameter, command);
                    data = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi thực thi lệnh: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return data;
        }

        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = null;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    AddParameter(query, parameter, command);
                    data = command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi thực thi Scalar: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return data;
        }

        private void AddParameter(string query, object[] parameter, SqlCommand command)
        {
            if (parameter != null)
            {
                string[] listParameter = query.Split(' ');
                int i = 0;
                foreach (string item in listParameter)
                {
                    if (item.Contains("@") && i < parameter.Length)
                    {
                        command.Parameters.AddWithValue(item, parameter[i++]);
                    }
                }
            }
        }

        // Singleton Pattern
        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return instance; }
            private set => instance = value;
        }

        private DataProvider() { }
    }
}