using PD_CW5.Services;
using System.Data.SqlClient;
using System.Data;

namespace wykl6.Services;

public class DbService : IDbService
{

    public string connectionString = "Data Source=db-mssql;Initial Catalog=s17943;Integrated Security=True";

    public bool doesTheProductExist(int idProduct)
    {
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand();
        com.Connection = con;
        con.Open();
        com.CommandText = "SELECT COUNT(idProduct) FROM Product WHERE idProduct =@idProduct";
        com.Parameters.AddWithValue("@idProduct", SqlDbType.Int).Value = idProduct;
        
        int exists = (int)com.ExecuteScalar();
        if (exists == 0) return false; else return true;

    }

    public bool doesTheWarehouseExist(int idWarehouse)
    {
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand();
        com.Connection = con;
        con.Open();
        com.CommandText = "SELECT COUNT(idWarehouse) FROM WAREHOUSE WHERE idWAREHOUSE =@idWarehouse";
        com.Parameters.AddWithValue("@idWarehouse", SqlDbType.Int).Value = idWarehouse;
        
        int exists = (int)com.ExecuteScalar();
        if (exists == 0) return false; else return true;
    }

    public bool checkOrder(int IdProduct, int Amount)
    {
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand();
        com.Connection = con;
        con.Open();
        com.CommandText = "SELECT COUNT(idOrder) FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount";
        com.Parameters.AddWithValue("@IdProduct", SqlDbType.Int).Value = IdProduct;
        com.Parameters.AddWithValue("@Amount", SqlDbType.Int).Value = Amount;
        
        int exists = (int)com.ExecuteScalar();
        if (exists == 0) return false; else return true;
    }

    public bool verifyDate(DateTime date, int IdProduct, int Amount)
    {
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand();
        com.Connection = con;
        con.Open();
        com.CommandText = "SELECT CreatedAt FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount";
        com.Parameters.AddWithValue("@IdProduct", SqlDbType.Int).Value = IdProduct;
        com.Parameters.AddWithValue("@Amount", SqlDbType.Int).Value = Amount;
        if ((DateTime)com.ExecuteScalar() < date) return false; else return true;
    }

    public bool checkIfOrderHasBeenCompleted(int IdProduct, int Amount)
    {
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand();
        com.Connection = con;
        con.Open();
        com.CommandText =
            "SELECT count(IdProductWarehouse) from Product_Warehouse Where IdOrder = (SELECT IdOrder FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount)";
        com.Parameters.AddWithValue("@idProduct", SqlDbType.Int).Value = IdProduct;
        com.Parameters.AddWithValue("@Amount", SqlDbType.Int).Value = Amount;
        
        int exists = (int)com.ExecuteScalar();
        if (exists == 0) return true; else return false;
    }

    public void updateFulfillmentDate(int IdProduct, int Amount)
    {
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand();
        com.Connection = con;
        con.Open();
        com.CommandText = "UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdOrder = (SELECT idOrder FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount)";
        com.Parameters.AddWithValue("@IdProduct", SqlDbType.Int).Value = IdProduct;
        com.Parameters.AddWithValue("@Amount", SqlDbType.Int).Value = Amount;
        com.ExecuteNonQuery();
    }

    public int createProductWarehouse(int IdProduct, int IdWarehouse, int Amount)
    {
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand();
        com.Connection = con;
        con.Open();
        com.CommandText = "SET IDENTITY_INSERT Product_Warehouse ON;INSERT INTO Product_Warehouse (IdProductWarehouse, IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)VALUES (((SELECT ISNULL(Max(IdProductWarehouse),0) FROM Product_Warehouse) + 1), @IdWarehouse, @IdProduct,(SELECT idOrder FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount),@Amount, ((SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount), GETDATE());SET IDENTITY_INSERT Product_Warehouse OFF";
        com.Parameters.AddWithValue("@IdProduct", SqlDbType.Int).Value = IdProduct;
        com.Parameters.AddWithValue("@Amount", SqlDbType.Int).Value = Amount;
        com.Parameters.AddWithValue("@IdWarehouse", SqlDbType.Int).Value = IdWarehouse;
        com.ExecuteNonQuery();
        com.CommandText = "SELECT IdProductWarehouse from Product_Warehouse Where IdProduct = @IdProduct and IdWarehouse = @IdWarehouse and Amount = @Amount and Price = ((SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount)";
        
        int id = (int)com.ExecuteScalar();
        return id;
    }

    public void createProductWarehouseWithProcedure(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt)
    {
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand("AddProductToWarehouse", con);
        com.Connection = con;
        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@IdProduct", SqlDbType.Int).Value = IdProduct;
        com.Parameters.AddWithValue("@Amount", SqlDbType.Int).Value = Amount;
        com.Parameters.AddWithValue("@IdWarehouse", SqlDbType.Int).Value = IdWarehouse;
        com.Parameters.AddWithValue("@CreatedAt", SqlDbType.Date).Value = CreatedAt;
        con.Open();
        com.ExecuteNonQuery();
    }

    public int getProductWarehouseID(int IdProduct, int IdWarehouse, int Amount)
    {
        
        using SqlConnection con = new SqlConnection(connectionString);
        SqlCommand com = new SqlCommand();
        com.Connection = con;
        con.Open();
        com.CommandText =
            "SELECT IdProductWarehouse from Product_Warehouse Where IdProduct = @IdProduct and IdWarehouse = @IdWarehouse and Amount = @Amount and Price = ((SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount)";
        com.Parameters.AddWithValue("@IdProduct", SqlDbType.Int).Value = IdProduct;
        com.Parameters.AddWithValue("@Amount", SqlDbType.Int).Value = Amount;
        com.Parameters.AddWithValue("@IdWarehouse", SqlDbType.Int).Value = IdWarehouse;
        int id = (int)com.ExecuteScalar();
        return id;
    }
    private int ReadSingleRow(IDataRecord dataRecord)
    {
        return (int)dataRecord[0];
    }
    private DateTime ReadSingleTimeRow(IDataRecord dataRecord)
    {
        return (DateTime)dataRecord[0];
    }
}

