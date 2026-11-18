using Microsoft.EntityFrameworkCore;
using RefugiAnimals.Data;
using RefugiAnimals.Models;

namespace RefugiAnimals.Services;

public class VisitaService
{
    Random rnd = new Random();

    public readonly RefugiContext ctx;

    private List<Animal> animals = new List<Animal>();

    public VisitaService(RefugiContext context)
    {
        ctx = context;
    }

    public void GenerarVisites(int quantes)
    {
        animals = ctx.Animals.ToList();

        if (animals.Count == 0)
        {
            Console.WriteLine("No hi ha animals al refugi.");
            return;
        }

        Motiu[] motius = Enum.GetValues<Motiu>();

        for (int i = 0; i < quantes; i++)
        {
            Motiu motiu = motius[rnd.Next(motius.Length)];

            Animal animal = animals[rnd.Next(animals.Count)];

            DateTime dataMaxima = DateTime.Today;

            if (animal.DataAdopcio != null)
            {
                dataMaxima = animal.DataAdopcio.Value;
            }

            int dies = (dataMaxima.Date - animal.DataEntrada.Date).Days;

            DateTime data = animal.DataEntrada.Date.AddDays(
                rnd.Next(dies + 1)
            );

            int cost = motiu switch
            {
                Motiu.REVISIO => rnd.Next(30, 61),
                Motiu.VACUNA => rnd.Next(25, 51),
                Motiu.FERIDA => rnd.Next(60, 151),
                Motiu.CIRURGIA => rnd.Next(250, 601),
                _ => 0
            };

            Visita visita = new Visita()
            {
                Motiu = motiu,
                Data = data,
                Cost = cost,
                Animal = animal
            };

            Console.WriteLine($"{animal.Nom} {data} {motiu} {cost} €");

            ctx.Add(visita);
        }

        ctx.SaveChanges();
    }
    public void EstadistiquesPerMotiu()
    {
        var estadistiques = ctx.Visitants
            .GroupBy(v => v.Motiu)
            .Select(g => new
            {
                Motiu = g.Key,
                Total = g.Count(),
                CostTotal = g.Sum(v => v.Cost),
                CostMitja = g.Average(v => v.Cost)
            })
            .OrderByDescending(x => x.CostTotal)
            .ToList();

        foreach (var esta in estadistiques)
        {
            Console.WriteLine($"Motiu: {esta.Motiu} " +
                              $"Total visites: {esta.Total} " +
                              $"Cost total: {esta.CostTotal} € " +
                              $"Cost mitjà: {esta.CostMitja} €");
        }
    }


 
    
}