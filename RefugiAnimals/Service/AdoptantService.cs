using Microsoft.EntityFrameworkCore;
using RefugiAnimals.Data;
using RefugiAnimals.Models;

namespace RefugiAnimals.Services;

public class AdoptantService
{
    private const int MaxAnimalsPerAdoptant = 3;

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
            Console.WriteLine("L'animal ja esta adoptat.");
            return false;
        }

        if (animal.Especie == Especie.GOS && !adoptant.TeJardi)
        {
            Console.WriteLine("Per adoptar un gos cal tenir jardi.");
            return false;
        }

        int totalAdoptats = ctx.Animals.Count(a => a.AdoptantID == adoptant.AdoptantID);
        if (totalAdoptats >= MaxAnimalsPerAdoptant)
        {
            Console.WriteLine($"{adoptant.Nom} ja te {MaxAnimalsPerAdoptant} animals adoptats (maxim permes).");
            return false;
        }

        animal.AdoptantID = adoptant.AdoptantID;
        animal.EstaAdoptat = true;
        animal.DataAdopcio = DateTime.Now;
        animal.Voluntaris.Clear();   // un animal adoptat deixa de tenir voluntaris

        ctx.SaveChanges();

        Console.WriteLine($"{adoptant.Nom} ha adoptat {animal.Nom}.");
        return true;
    }

    public bool Retornar(string codiXip)
    {
        var animal = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("L'animal no existeix.");
            return false;
        }

        if (!animal.EstaAdoptat || animal.AdoptantID == null)
        {
            Console.WriteLine($"{animal.Nom} no esta adoptat.");
            return false;
        }

        animal.Adoptant = null;
        animal.AdoptantID = null;
        animal.EstaAdoptat = false;
        animal.DataAdopcio = null;
        animal.DataEntrada = DateTime.Now;

        ctx.SaveChanges();

        Console.WriteLine($"{animal.Nom} ha tornat al refugi.");
        return true;
    } 

    public void LlistatAdoptants(int minAnimals)
    {
        var adoptants = ctx.Adoptants
            .Where(a => a.Animals.Count >= minAnimals)
            .OrderByDescending(a => a.Animals.Count)
            .ThenBy(a => a.Cognoms)
            .Select(a => new
            {
                NomComplet = a.Nom + " " + a.Cognoms,
                NombreAnimals = a.Animals.Count
            })
            .ToList();

        Console.WriteLine($"ADOPTANTS AMB ALMENYS {minAnimals} ANIMAL(S):");

        if (adoptants.Count == 0)
        {
            Console.WriteLine("  (cap)");
            return;
        }

        foreach (var a in adoptants)
        {
            Console.WriteLine($"  {a.NomComplet}: {a.NombreAnimals} animal(s)");
        }
    }
}