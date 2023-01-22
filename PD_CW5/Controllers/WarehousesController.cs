using Microsoft.AspNetCore.Mvc;
using PD_CW5.Model;
using PD_CW5.Services;
namespace PD_CW5.Controllers;


[Route("api/[controller]")]
[ApiController]
public class WarehousesController : ControllerBase
{
    
    private IDbService _dbService;

    public WarehousesController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost]
    public IActionResult createProduct_Warehouse(Warehouse warehouse)
    {
        if (_dbService.doesTheProductExist(warehouse.IdProduct)
            && _dbService.doesTheWarehouseExist(warehouse.IdWarehouse)
            && warehouse.Amount > 0)
        {
            if (_dbService.checkOrder(warehouse.IdProduct, warehouse.Amount)
                && _dbService.verifyDate(warehouse.CreatedAt, warehouse.IdProduct, warehouse.Amount))
            {
                if (_dbService.checkIfOrderHasBeenCompleted(warehouse.IdProduct, warehouse.Amount))
                {
                    _dbService.updateFulfillmentDate(warehouse.IdProduct, warehouse.Amount);
                    return Created("","New entry created = " + _dbService.createProductWarehouse(warehouse.IdProduct, warehouse.IdWarehouse, warehouse.Amount));
                }
                else return BadRequest("It appears that the order with these parameters has been already fulfilled");
            } else return NotFound("There is no such order in the database");
        } else return NotFound("There is no such product or warehouse");
    }
       
}