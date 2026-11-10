using RefugiAnimals.Data;
using RefugiAnimals.Models;

namespace RefugiAnimals.Services;

public class AnimalService
{
    private RefugiContext ctx;

    public AnimalService(RefugiContext context)
    {
        ctx = context;
    }

    public List<Animal> GetDisponibles(Especie especie)
    {
        return ctx.Animals
            .Where(a => a.Especie == especie && !a.EstaAdoptat)
            .OrderBy(a => a.DataEntrada)
            .ToList();
    }

    public Animal? CercaPerXip(string codiXip)
    {
        if (codiXip.Length != 15)
        {
            Console.WriteLine("El codi xip ha de tenir 15 digits.");
            return null;
        }

        foreach (char c in codiXip)
        {
            if (c < '0' || c > '9')
            {
                Console.WriteLine("El codi xip nomes pot contenir digits.");
                return null;
            }
        }

        Animal? animal = ctx.Animals
            .FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("No existeix cap animal amb aquest xip.");
        }

        return animal;
    }

    public List<Animal> CercaPerNom(string text)
    {
        return ctx.Animals
            .Where(a => a.Nom.ToLower().Contains(text.ToLower()))
            .ToList();
    }
}