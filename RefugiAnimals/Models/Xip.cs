using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RefugiAnimals.Models;

[Index(nameof(Codi), IsUnique =true)]
public class Xip
{
    public int XipID { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 15)]
    public string Codi { get; set; }

    [Required]
    public DateTime DataImplantacio { get; set; }

    [Required]
    public virtual Animal Animal { get; set; }

    public Xip()
    {
        DataImplantacio = DateTime.Now;
    }

    public Xip(string codi)
    {
        Codi = codi;
        DataImplantacio = DateTime.Now;
    }
    public override string ToString()
    {
        return $"XipID: {XipID}, Codi: {Codi}, DataImplantacio: {DataImplantacio}";
    }
}