using Microsoft.EntityFrameworkCore;
using RefugiAnimals.Models;
using RefugiAnimals.Data;
using RefugiAnimals.Models;


namespace RefugiAnimals.Services;

public class AdoptantService
{
    private readonly RefugiContext ctx;

    public AdoptantService(RefugiContext context)
    {
        ctx = context;
    }

    public bool Adoptar(string dni, string codiXip)
    {
        var adoptant = ctx.Adoptants.FirstOrDefault(a => a.DNI == dni);

        if (adoptant == null)
        {
            Console.WriteLine("L'adoptant no existeix.");
            return false;
        }

        var animal = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("L'animal no existeix.");
            return false;
        }

        if (animal.EstaAdoptat)
        {
            Console.WriteLine("Ja esta adoptat.");
            return false;
        }

        if (animal.Especie.Equals("GOS") && !adoptant.TeJardi)
        {
            Console.WriteLine("Necesites jardi");
            return false;
        }

        animal.AdoptantID = adoptant.AdoptantID;
        animal.EstaAdoptat = true;
        animal.DataAdopcio = DateTime.Now;

        ctx.SaveChanges();

        Console.WriteLine("Adoptat");

        return true;
    }

    public bool Retornar(string codiXip)
    {
        var adoptat = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);

        
        
        return true;
    }

}