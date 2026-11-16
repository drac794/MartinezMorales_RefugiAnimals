
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
        AnimalService animalService = new AnimalService(ctx);

        
        
        // ============ Nivell 1 · Dades ============
    // La família es munta en memòria amb les col·leccions de cries i s'afegeix NOMÉS la Nala:
    // EF segueix les navegacions i insereix els 4 animals, els 4 xips i les MareID.
    // Pes obligatori: amb el CHECK de la BD, un pes de 0 fa petar el SaveChanges
    Animal nala = new Animal() { Nom = "Nala", Especie = Especie.GOS, EsFemella = true, Pes = 25,
    DataNaixement = DateTime.Today.AddYears(-9), Xip = new Xip() { Codi = "724098100000011" } };
    Animal coco = new Animal() { Nom = "Coco", Especie = Especie.GOS, EsFemella = true, Pes = 21,
    DataNaixement = DateTime.Today.AddYears(-5), Xip = new Xip() { Codi = "724098100000012" } };
    Animal rocky = new Animal() { Nom = "Rocky", Especie = Especie.GOS, EsFemella = false, Pes = 6.5,
    DataNaixement = DateTime.Today.AddMonths(-4), Xip = new Xip() { Codi = "724098100000013" } };
    Animal bimba = new Animal() { Nom = "Bimba", Especie = Especie.GOS, EsFemella = true, Pes = 5.8,
    DataNaixement = DateTime.Today.AddMonths(-4), Xip = new Xip() { Codi = "724098100000014" } };

    coco.Cries.Add(rocky);
    coco.Cries.Add(bimba);
    nala.Cries.Add(coco);

    ctx.Animals.Add(nala);
    ctx.SaveChanges();

    void MostrarFamilies()
    {
    // Ho llegim de la BD amb el servei. a.Mare i a.Cries els carrega el lazy loading
    foreach (Animal an in animalService.GetTots())
    {
        string mare;
        if (an.Mare != null)
        {
            mare = an.Mare.Nom;     // an.Mare.Nom sense comprovar el null peta amb els que no tenen mare
        }
        else
        {
            mare = "sense mare";
        }
        Console.WriteLine($"{an.Nom} ({an.Especie}) · mare: {mare} · {an.Cries.Count} cries");
    }
    }   

    Console.WriteLine("\n--- Animals amb la seva mare i les seves cries ---");
    MostrarFamilies();      // Nala: sense mare, 1 cria · Coco: mare Nala, 2 cries · Rocky i Bimba: mare Coco, 0 cries

    // ============ Nivell 1 · CanviarMare ============
    // La Kira (gossa, nascuda el 2023) no té mare. La Luna (gossa, 2020) i el Thor (gos, mascle)
    Console.WriteLine("\n--- Canviar la mare ---");
    animalService.CanviarMare("724098100000003", "724098100000001");    // bé: la Kira ara és cria de la Luna
    animalService.CanviarMare("724098100000003", "724098100000001");    // false: ja té la Luna com a mare
    animalService.CanviarMare("724098100000003", null);                 // bé: la Kira es queda sense mare
    animalService.CanviarMare("724098100000003", "724098100000002");    // false: el Thor és mascle
    animalService.CanviarMare("724098100000999", "724098100000001");    // false: no hi ha cap cria amb aquest xip
    animalService.CanviarMare("724098100000003", "724098100000999");    // false: no hi ha cap mare amb aquest xip
    animalService.CanviarMare("724098100000003", null);                 // false: la Kira ja no té mare

    // ============ Nivell 2 · RegistrarCria ============
    // Cada prova ha de fallar NOMÉS per la condició que vols provar: per això cada cria dolenta
    // és correcta en tot menys en una cosa
    Console.WriteLine("\n--- Registrar cries ---");
    Animal toby = new Animal() { Nom = "Toby", Especie = Especie.GOS, EsFemella = false, Pes = 2.1,
    DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000015" } };
    animalService.RegistrarCria("724098100000012", toby);               // bé: el Toby és cria de la Coco

    Animal criaThor = new Animal() { Nom = "Max", Especie = Especie.GOS, EsFemella = false, Pes = 2,
    DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000016" } };
    animalService.RegistrarCria("724098100000002", criaThor);           // false: el Thor és mascle

    Animal criaGat = new Animal() { Nom = "Misha", Especie = Especie.GAT, EsFemella = true, Pes = 0.8,
    DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000017" } };
    animalService.RegistrarCria("724098100000012", criaGat);            // false: la Coco és un gos i la Misha un gat

    Animal criaXip = new Animal() { Nom = "Duc", Especie = Especie.GOS, EsFemella = false, Pes = 2,
    DataNaixement = DateTime.Today.AddMonths(-1), Xip = new Xip() { Codi = "724098100000011" } };
    animalService.RegistrarCria("724098100000012",bimba);            // false: el xip ja és de la Nala

   
    Console.WriteLine("\n--- Famílies després de registrar (la Coco té 3 cries, cap animal nou dels dolents) ---");
    MostrarFamilies();

    }
}