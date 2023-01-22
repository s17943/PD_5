namespace PD_CW5.Services;

public interface IDbService
{
    public bool doesTheProductExist(int idProduct);
    public bool doesTheWarehouseExist(int idWarehouse);
    public bool checkOrder(int IdProduct, int Amount);
    public bool verifyDate(DateTime date, int IdProduct, int Amount);
    public bool checkIfOrderHasBeenCompleted(int IdProduct, int Amount);
    public void updateFulfillmentDate(int IdProduct, int Amount);
    public int createProductWarehouse(int IdProduct, int IdWarehouse, int Amount);
    public void createProductWarehouseWithProcedure(int IdProduct, int IdWarehouse, int Amount, DateTime CreatedAt);
    public int getProductWarehouseID(int IdProduct, int IdWarehouse, int Amount);

}