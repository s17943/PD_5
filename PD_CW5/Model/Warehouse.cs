using System.ComponentModel.DataAnnotations;

namespace PD_CW5.Model;

public class Warehouse
{
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}