
using Microsoft.EntityFrameworkCore;
using RefugiAnimals.Data;
using RefugiAnimals.Models;

namespace RefugiAnimals;

internal class Program
{
    static void Main(string[] args)
    {
        using (var ctx = new RefugiContext())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();

            var toby = new Animal("Toby", Especie.GOS, false,
                new DateTime(2021, 3, 15), 12.5);
            toby.DataEntrada = new DateTime(2026, 9, 1);
            toby.Xip = new Xip("123456789012345");

            var luna = new Animal("Luna", Especie.GAT, true,
                new DateTime(2022, 7, 10), 4.2);
            luna.DataEntrada = new DateTime(2026, 9, 5);
            luna.Xip = new Xip("223456789012345");

            var rocky = new Animal("Rocky", Especie.GOS, false,
                new DateTime(2020, 1, 20), 25.0);
            rocky.DataEntrada = new DateTime(2026, 9, 10);
            rocky.Xip = new Xip("323456789012345");

            var mia = new Animal("Mia", Especie.GAT, true,
                new DateTime(2023, 5, 8), 3.5);
            mia.DataEntrada = new DateTime(2026, 9, 12);
            mia.Xip = new Xip("423456789012345");

            var max = new Animal("Max", Especie.GOS, false,
                new DateTime(2019, 11, 2), 30.0);
            max.DataEntrada = new DateTime(2026, 9, 15);
            max.Xip = new Xip("523456789012345");

            var nala = new Animal("Nala", Especie.GAT, true,
                new DateTime(2021, 9, 14), 4.8);
            nala.DataEntrada = new DateTime(2026, 9, 18);
            nala.Xip = new Xip("623456789012345");

            var bobby = new Animal("Bobby", Especie.GOS, false,
                new DateTime(2022, 2, 18), 15.3);
            bobby.DataEntrada = new DateTime(2026, 9, 20);
            bobby.Xip = new Xip("723456789012345");

            var nube = new Animal("Nube", Especie.CONILL, true,
                new DateTime(2024, 4, 12), 2.1);
            nube.DataEntrada = new DateTime(2026, 9, 23);
            nube.Xip = new Xip("823456789012345");

            var kiwi = new Animal("Kiwi", Especie.AU, false,
                new DateTime(2023, 8, 25), 0.5);
            kiwi.DataEntrada = new DateTime(2026, 9, 25);
            kiwi.Xip = new Xip("923456789012345");

            ctx.Animals.Add(toby);
            ctx.Animals.Add(luna);
            ctx.Animals.Add(rocky);
            ctx.Animals.Add(mia);
            ctx.Animals.Add(max);
            ctx.Animals.Add(nala);
            ctx.Animals.Add(bobby);
            ctx.Animals.Add(nube);
            ctx.Animals.Add(kiwi);

            ctx.SaveChanges();

            Console.WriteLine("DADES INSERIDES");

            List<Animal> llista = ctx.Animals.Include( a => a.Xip).ToList();

            foreach (Animal a in llista)
            {
                Console.WriteLine(a);
                Console.WriteLine("Codi xip: " + a.Xip.Codi);
            }
        }
    }
}
