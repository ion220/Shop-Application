using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Diagnostics;
using System.Xml.Linq;
using static System.Configuration.ConfigurationManager;

namespace Shop
{
    public class API
    {
        private static SqlConnection CreateNewConnection()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = AppSettings.Get("DataSource");
                builder.UserID = AppSettings.Get("UserID");
                builder.Password = AppSettings.Get("Password");

                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                return connection;
                
            }
            catch (SqlException e)
            {
                ShowError("Ошибка подключения к БД. Проверьте конфигурацию данных для входа.");
                return null;
            }
        }
        
        #region Product
        public static bool InsertIntoProduct(string name, string description, int measureUnitId, int groupId, float price)
        {
            String query = "INSERT INTO dbo.Product (name,description,measure_unit_id,group_id,price) " +
                           "VALUES (@name,@description,@measure_unit_id,@group_id,@price)";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@measure_unit_id", measureUnitId);
                command.Parameters.AddWithValue("@group_id", groupId);
                command.Parameters.AddWithValue("@price", price);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка вставки данных.");
                    return false;
                }
                return true;
            }
        }

        public static bool UpdateProduct(int id, string name, string description, int measureUnitId, int groupId, float price)
        {
            String query = "UPDATE dbo.Product " +
                           "SET name=@name,description=@description,measure_unit_id=@measure_unit_id,group_id=@group_id,price=@price " +
                           "WHERE id = @id";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@measure_unit_id", measureUnitId);
                command.Parameters.AddWithValue("@group_id", groupId);
                command.Parameters.AddWithValue("@price", price);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка изменения данных.");

                    return false;
                }
                return true;
            }
        }
        #endregion

        #region Group
        public static bool InsertIntoGroup(string name, int parentGroupId)
        {
            String query = "INSERT INTO dbo.Product_group (name,parent_group_id) " +
                           "VALUES (@name,@parent_group_id)";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@parent_group_id", parentGroupId);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка вставки данных.");
                    return false;
                }
                return true;
            }
        }

        public static bool InsertIntoGroup(string name)
        {
            String query = "INSERT INTO dbo.Product_group (name) " +
                           "VALUES (@name)";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка вставки данных.");
                    return false;
                }
                return true;
            }
        }

        public static bool UpdateGroup(int id, string name, int parentGroupId)
        {
            String query = "UPDATE dbo.Product_group " +
                           "SET name=@name,parent_group_id=@parent_group_id " +
                           "WHERE id = @id";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@parent_group_id", parentGroupId);

                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка изменения данных.");
                    return false;
                }
                return true;
            }
        }

        public static bool UpdateGroup(int id, string name)
        {
            String query = "UPDATE dbo.Product_group " +
                           "SET name=@name,parent_group_id=NULL " +
                           "WHERE id = @id";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);

                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка изменения данных.");
                    return false;
                }
                return true;
            }
        }

        public static void DeleteGroup(int id)
        {
            foreach (DataRow group in SelectGroupByParentId(id).Rows)
            {
                DeleteGroup((int)group["id"]);
            }

            DeleteFromTable(id, "Product_group");
        }

        public static DataTable SelectGroupByParentId(int parentGroupId)
        {
            DataTable dataTable = new DataTable();

            string query = $"SELECT * FROM dbo.Product_group WHERE parent_group_id={parentGroupId}";

            using (var connection = CreateNewConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                    connection.Close();

                    return dataTable;
                }
            }
        }

        #endregion

        #region Measure Unit

        public static bool InsertIntoMeasureUnit(string name, string shortName)
        {
            String query = "INSERT INTO dbo.Measure_unit (name,short_name) " +
                           "VALUES (@name,@short_name)";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@short_name", shortName);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка вставки данных.");
                    return false;
                }
                return true;
            }
        }

        public static bool UpdateMeasureUnit(int id, string name, string shortName)
        {
            String query = "UPDATE dbo.Measure_unit " +
                           "SET name=@name,short_name=@short_name " +
                           "WHERE id = @id";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@short_name", shortName);

                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка изменения данных.");
                    return false;
                }
                return true;
            }
        }
        #endregion

        #region Order

        public static bool InsertIntoOrder(string number, DateTime creationDatetime, DateTime deliveryDatetime, string clientAddress, string clientPhone, int statusId, string cardNumber, string comment)
        {
            String query = "INSERT INTO dbo.[Order] (number,creation_datetime,delivery_datetime,client_address,client_phone,status_id,card_number,comment) " +
                           "VALUES (@number,@creation_datetime,@delivery_datetime,@client_address,@client_phone,@status_id,@card_number,@comment)";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@number", number);
                command.Parameters.AddWithValue("@creation_datetime", creationDatetime);
                command.Parameters.AddWithValue("@delivery_datetime", deliveryDatetime);
                command.Parameters.AddWithValue("@client_address", clientAddress);
                command.Parameters.AddWithValue("@client_phone", clientPhone);
                command.Parameters.AddWithValue("@status_id", statusId);
                command.Parameters.AddWithValue("@card_number", cardNumber);
                command.Parameters.AddWithValue("@comment", comment);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка вставки данных.");
                    return false;
                }
                return true;
            }
        }

        public static bool InsertIntoOrderProduct(int orderId, int productId, int amount)
        {
            String query = "INSERT INTO dbo.Order_Product (id_order,id_product,amount) " +
                           "VALUES (@id_order,@id_product,@amount)";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_order", orderId);
                command.Parameters.AddWithValue("@id_product", productId);
                command.Parameters.AddWithValue("@amount", amount);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка вставки данных.");
                    return false;
                }
                return true;
            }
        }

        public static bool UpdateOrder(int id, string number, DateTime creationDatetime, DateTime deliveryDatetime, string clientAddress, string clientPhone, int statusId, string cardNumber, string comment)
        {
            String query = "UPDATE dbo.[Order] " +
                           "SET number=@number,creation_datetime=@creation_datetime,delivery_datetime=@delivery_datetime,client_address=@client_address," +
                           "client_phone=@client_phone,status_id=@status_id,card_number=@card_number,comment=@comment " +
                           "WHERE id = @id";

            using (var connection = CreateNewConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@number", number);
                command.Parameters.AddWithValue("@creation_datetime", creationDatetime);
                command.Parameters.AddWithValue("@delivery_datetime", deliveryDatetime);
                command.Parameters.AddWithValue("@client_address", clientAddress);
                command.Parameters.AddWithValue("@client_phone", clientPhone);
                command.Parameters.AddWithValue("@status_id", statusId);
                command.Parameters.AddWithValue("@card_number", cardNumber);
                command.Parameters.AddWithValue("@comment", comment);
                connection.Open();
                int result = command.ExecuteNonQuery();

                if (result < 0)
                {
                    ShowError("Ошибка изменения данных.");

                    return false;
                }
                return true;
            }
        }

        public static DataTable GetProductsByOrderId(int id)
        {
            DataTable dataTable = new DataTable();

            string query = $"SELECT * FROM dbo.Product JOIN Order_Product ON Order_Product.id_product = Product.id " +
                           $"WHERE dbo.Product.id IN (SELECT id_product FROM dbo.Order_Product WHERE dbo.Order_Product.id_order={id}) ";

            using (var connection = CreateNewConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                    connection.Close();

                    return dataTable;
                }
            }
        }

        public static DataTable GetOrderStatus(int id)
        {
            DataTable dataTable = new DataTable();

            string query = $"SELECT * FROM dbo.Order_status WHERE id = (SELECT status_id FROM dbo.[Order] WHERE dbo.[Order].id={id})";

            using (var connection = CreateNewConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                    connection.Close();

                    return dataTable;
                }
            }
        }

        public static int? GetLastOrderNumber()
        {
            DataTable dataTable = new DataTable();

            string query = "SELECT MAX(number) as id FROM dbo.[Order]";

            using (var connection = CreateNewConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                    connection.Close();

                    return dataTable.Rows[0]["id"] as int?;
                    
                }
            }
        }

        public static int GetLastOrderId()
        {
            DataTable dataTable = new DataTable();

            string query = "SELECT MAX(id) as id FROM dbo.[Order]";

            using (var connection = CreateNewConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                    connection.Close();

                    int? id = (int)dataTable.Rows[0]["id"];

                    id = id is null ? 1 : id;

                    return Int32.Parse(id.ToString());
                }
            }
        }

        public static bool DeleteOrderProductByOrderId(int id)
        {
            try
            {
                using (var connection = CreateNewConnection())
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM dbo.Order_Product WHERE id_order = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                ShowError("Ошибка удаления данных.");
            }
            return true;
        }


        #endregion

        public static DataTable SelectAllFromTable(string tableName)
        {
            DataTable dataTable = new DataTable();

            string query = $"SELECT * FROM dbo.{tableName}";

            using (var connection = CreateNewConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                    connection.Close();

                    return dataTable;
                }
            }
        }

        public static DataRow SelectRowFromTable(int id, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);

            string query = $"SELECT * FROM dbo.{tableName} WHERE id={id}";

            using (var connection = CreateNewConnection())
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(dataTable);
                    connection.Close();

                    return dataTable.Rows[0];
                }
            }
        }

        public static bool DeleteFromTable(int id, string tableName)
        {
            try
            {
                using (var connection = CreateNewConnection())
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM dbo.{tableName} WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                ShowError("Ошибка удаления данных.");
            }
            return true;
        }

        private static void ShowError(string textError)
        {
            MessageBox.Show(textError,
                "Ошибка при взаимодействии с БД",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
