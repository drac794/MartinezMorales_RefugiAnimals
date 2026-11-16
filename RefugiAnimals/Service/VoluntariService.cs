using RefugiAnimals.Data;
using RefugiAnimals.Models;

namespace RefugiAnimals.Services;

public class VoluntariService
{
    private const int MaxAnimalsPerVoluntari = 4;

    private readonly RefugiContext ctx;

    public VoluntariService(RefugiContext context)
    {
        ctx = context;
    }

    public bool AssignarAnimal(string nomVoluntari, string codiXip)
    {
        var voluntari = ctx.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);

        if (voluntari == null)
        {
            Console.WriteLine("El voluntari no existeix.");
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
            Console.WriteLine($"{animal.Nom} esta adoptat i no pot tenir voluntaris.");
            return false;
        }

        if (voluntari.Animals.Contains(animal))
        {
            Console.WriteLine($"{voluntari.Nom} ja te cura de {animal.Nom}.");
            return false;
        }

        if (voluntari.Animals.Count >= MaxAnimalsPerVoluntari)
        {
            Console.WriteLine($"{voluntari.Nom} ja te cura de {MaxAnimalsPerVoluntari} animals (maxim permes).");
            return false;
        }

        voluntari.Animals.Add(animal);
        ctx.SaveChanges();

        Console.WriteLine($"{voluntari.Nom} ara te cura de {animal.Nom}.");
        return true;
    }

    public bool DesassignarAnimal(string nomVoluntari, string codiXip)
    {
        var voluntari = ctx.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);

        if (voluntari == null)
        {
            Console.WriteLine("El voluntari no existeix.");
            return false;
        }

        var animal = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("L'animal no existeix.");
            return false;
        }

        if (!voluntari.Animals.Contains(animal))
        {
            Console.WriteLine($"{voluntari.Nom} no tenia assignat a {animal.Nom}.");
            return false;
        }

        voluntari.Animals.Remove(animal); // esborra la fila de Cura: l'animal no s'esborra
        ctx.SaveChanges();

        Console.WriteLine($"{voluntari.Nom} ja no te cura de {animal.Nom}.");
        return true;
    }

    public bool RegistrarPasseig(string nomVoluntari, string codiXip, int minuts)
    {
        var voluntari = ctx.Voluntaris.FirstOrDefault(v => v.Nom == nomVoluntari);

        if (voluntari == null)
        {
            Console.WriteLine("El voluntari no existeix.");
            return false;
        }

        var animal = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("L'animal no existeix.");
            return false;
        }

        if (!voluntari.Animals.Contains(animal))
        {
            Console.WriteLine($"{voluntari.Nom} no te assignat a {animal.Nom}.");
            return false;
        }

        if (animal.Especie != Especie.GOS)
        {
            Console.WriteLine($"{animal.Nom} no es un gos: no es pot passejar.");
            return false;
        }

        if (minuts <= 0)
        {
            Console.WriteLine("Els minuts han de ser positius.");
            return false;
        }

        int previs = voluntari.MinutsPasseig;
        voluntari.MinutsPasseig += minuts;
        ctx.SaveChanges();

        Console.WriteLine(
            $"{voluntari.Nom} passeja {animal.Nom} {minuts} min. Minuts previs: {previs}, actuals: {voluntari.MinutsPasseig}.");
        return true;
    }
    public List<Animal> Descendents(string codiXip)
    {
        List<Animal> llista = new List<Animal>();

        Animal? animal = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("L'animal no existeix");
            return llista;
        }

        AfegirDescendents(animal, llista);
        return llista;
    }

    private void AfegirDescendents(Animal animal, List<Animal> llista)
    {
        foreach (Animal cria in animal.Cries.OrderBy(c => c.AnimalID))
        {
            llista.Add(cria);
            AfegirDescendents(cria, llista); 
        }
    }

}