using Microsoft.AspNetCore.Mvc;
using PD_CW5.Model;
using PD_CW5.Controllers;
using PD_CW5.Services;

namespace PD_CW5.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WarehouseController2 : ControllerBase
{
    private IDbService _dbService;

    public WarehouseController2(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpPost]
    public IActionResult CreateProductWarehouse(Warehouse warehouse)
    {
        _dbService.createProductWarehouseWithProcedure(warehouse.IdProduct, warehouse.IdWarehouse, warehouse.Amount, warehouse.CreatedAt);
        return Created("", "Entry with id: " + _dbService.getProductWarehouseID(warehouse.IdProduct, warehouse.IdWarehouse, warehouse.Amount)+  " has been inserted into the table.");
    }
}