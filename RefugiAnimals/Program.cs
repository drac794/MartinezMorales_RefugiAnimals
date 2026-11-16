
using RefugiAnimals.Data;
using RefugiAnimals.Models;
using RefugiAnimals.Services;

namespace RefugiAnimals;

internal class Program
{
    static void Main(string[] args)
    {
        using var ctx = new RefugiContext();
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();

        // Animals (P1)
        var toby = new Animal("Toby", Especie.GOS, false, new DateTime(2021, 3, 15), 12.5) { Xip = new Xip("123456789012345") };
        var luna = new Animal("Luna", Especie.GAT, true, new DateTime(2022, 7, 10), 4.2) { Xip = new Xip("223456789012345") };
        var rocky = new Animal("Rocky", Especie.GOS, false, new DateTime(2020, 1, 20), 25) { Xip = new Xip("323456789012345") };
        var mia = new Animal("Mia", Especie.GAT, true, new DateTime(2023, 5, 8), 3.5) { Xip = new Xip("423456789012345") };
        var max = new Animal("Max", Especie.GOS, false, new DateTime(2019, 11, 2), 30) { Xip = new Xip("523456789012345") };
        var nala = new Animal("Nala", Especie.GAT, true, new DateTime(2021, 9, 14), 4.8) { Xip = new Xip("623456789012345") };
        var bobby = new Animal("Bobby", Especie.GOS, false, new DateTime(2022, 2, 18), 15.3) { Xip = new Xip("723456789012345") };
        var nube = new Animal("Nube", Especie.CONILL, true, new DateTime(2024, 4, 12), 2.1) { Xip = new Xip("823456789012345") };
        var kiwi = new Animal("Kiwi", Especie.AU, false, new DateTime(2023, 8, 25), 0.5) { Xip = new Xip("923456789012345") };

        // Adoptants (P2)
        var marta = new Adoptant { DNI = "12345678Z", Nom = "Marta", Cognoms = "Puig Soler", TeJardi = true };
        var pau = new Adoptant { DNI = "87654321X", Nom = "Pau", Cognoms = "Ferrer Vidal", TeJardi = false };
        toby.Adoptant = marta;
        toby.EstaAdoptat = true;
        toby.DataAdopcio = DateTime.Now;

        // Voluntaris (P3)
        var aina = new Voluntari("Aina", Torn.MATI);
        aina.Animals.Add(rocky);
        aina.Animals.Add(bobby);
        var joan = new Voluntari("Joan", Torn.TARDA);
        joan.Animals.Add(nala);
        var nuria = new Voluntari("Núria", Torn.CAP_DE_SETMANA);

        ctx.Animals.AddRange(toby, luna, rocky, mia, max, nala, bobby, nube, kiwi);
        ctx.Adoptants.AddRange(marta, pau);
        ctx.Voluntaris.AddRange(aina, joan, nuria);
        ctx.SaveChanges();

        var animals = new AnimalService(ctx);
        var adoptants = new AdoptantService(ctx);
        var voluntaris = new VoluntariService(ctx);

        // P1
        Console.WriteLine(animals.GetDisponibles(Especie.GOS).Count);
        Console.WriteLine(animals.CercaPerXip("123456789012345")?.Nom);
        Console.WriteLine(animals.CercaPerXip("123"));
        Console.WriteLine(animals.CercaPerNom("LU").Count);

        // P2
        Console.WriteLine(adoptants.Adoptar("12345678Z", "223456789012345"));  // ok
        Console.WriteLine(adoptants.Adoptar("00000000A", "523456789012345"));  // adoptant no existeix
        Console.WriteLine(adoptants.Adoptar("12345678Z", "000000000000000"));  // animal no existeix
        Console.WriteLine(adoptants.Adoptar("87654321X", "223456789012345"));  // ja adoptat
        Console.WriteLine(adoptants.Adoptar("87654321X", "523456789012345"));  // gos sense jardi
        Console.WriteLine(adoptants.Adoptar("12345678Z", "423456789012345"));  // 3a, ok
        Console.WriteLine(adoptants.Adoptar("12345678Z", "623456789012345"));  // 4a, falla
        Console.WriteLine(adoptants.Adoptar("87654321X", "823456789012345"));  // Pau - Nube
        Console.WriteLine(adoptants.Retornar("223456789012345"));              // ok
        Console.WriteLine(adoptants.Retornar("223456789012345"));              // no adoptat
        adoptants.LlistatAdoptants(1);
        adoptants.LlistatAdoptants(2);

        // P3 nivell 1
        Console.WriteLine(voluntaris.AssignarAnimal("Aina", "523456789012345"));   // ok
        Console.WriteLine(voluntaris.AssignarAnimal("Pere", "523456789012345"));   // voluntari no existeix
        Console.WriteLine(voluntaris.AssignarAnimal("Aina", "000000000000000"));   // animal no existeix
        Console.WriteLine(voluntaris.AssignarAnimal("Núria", "123456789012345"));  // adoptat
        Console.WriteLine(voluntaris.AssignarAnimal("Aina", "323456789012345"));   // ja assignat
        Console.WriteLine(voluntaris.AssignarAnimal("Aina", "923456789012345"));   // 4t, ok
        Console.WriteLine(voluntaris.AssignarAnimal("Aina", "223456789012345"));   // 5e, falla

        // P3 nivell 2
        Console.WriteLine(voluntaris.DesassignarAnimal("Aina", "923456789012345")); // ok
        Console.WriteLine(voluntaris.DesassignarAnimal("Aina", "923456789012345")); // no el tenia
        Console.WriteLine(adoptants.Adoptar("12345678Z", "323456789012345"));       // Marta adopta Rocky
        Console.WriteLine(aina.Animals.Count);                                      // Aina: 2 (Bobby i Max)

        // P3 nivell 3
        Console.WriteLine(voluntaris.RegistrarPasseig("Aina", "723456789012345", 30));
        Console.WriteLine(voluntaris.RegistrarPasseig("Aina", "723456789012345", 45));
        Console.WriteLine(voluntaris.RegistrarPasseig("Joan", "623456789012345", 20)); // gat, falla
    }
}