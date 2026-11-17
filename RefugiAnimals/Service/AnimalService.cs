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

    
    public List<Animal> GetTots()
    {
        return ctx.Animals
            .ToList();
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
      public bool CanviarMare(string codiXipCria, string? codiXipMare)
    {
        Animal? cria = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXipCria);

        if (cria == null)
        {
            Console.WriteLine("La cria no existeix.");
            return false;
        }
        

        Animal? mare = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXipMare);

        if (mare == null)
        {
            Console.WriteLine("La mare no existeix.");
            return false;
        }

        if (!mare.EsFemella)
        {
            Console.WriteLine("es mascle no pot ser la mare");
            return false;
        }

        if (cria.MareID == mare.AnimalID)
        {
            Console.WriteLine("ja es la mare de");
            return false;
        }

        cria.Mare = mare;
        ctx.SaveChanges();

        Console.WriteLine($"{mare.Nom} es ara la mare de {cria.Nom}.");
        return true;
    }

    public bool RegistrarCria(string codiXipMare, Animal cria)
    {
        Animal? mare = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXipMare);

        if (mare == null)
        {
            Console.WriteLine("La mare no existeix.");
            return false;
        }

        if (!mare.EsFemella)
        {
            Console.WriteLine("es mascle no pot ser la mare.");
            return false;
        }

        if (cria.Especie != mare.Especie)
        {
            Console.WriteLine("La cria ha de ser de la mateixa especie que la mare.");
            return false;
        }

        if (cria.DataNaixement <= mare.DataNaixement)
        {
            Console.WriteLine("La cria ha de haver nascut despres de la mare.");
            return false;
        }
        

        mare.Cries.Add(cria);
        ctx.SaveChanges();

        Console.WriteLine($"{cria.Nom} es ara cria de {mare.Nom}.");
        return true;
    }
    
    public List<Animal> Descendents(string codiXip)
    {
        List<Animal> llista = new List<Animal>();

        Animal? animal = ctx.Animals.FirstOrDefault(a => a.Xip.Codi == codiXip);

        if (animal == null)
        {
            Console.WriteLine("L'animal no existeix.");
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

    public void CalcularDespesaMes()
    {
        decimal total = 0;
        decimal max = 0;

        Animal animalmescar = null;

        foreach (Animal a in ctx.Animals.ToList())
        {
            decimal previ = a.DespesaAcumulada;
            decimal afegides = 0;
            

            if (!a.EstaAdoptat)
            {
         
                if (a.Especie == Especie.GOS)
                {
                    afegides = 60;     
                }else if (a.Especie == Especie.GAT)
                {
                    afegides = 40;     

                } else if (a.Especie == Especie.CONILL)
                {
                    afegides = 15;     

                } else if (a.Especie == Especie.AU)
                {
                    afegides = 10;     

                }
                else
                {
                    afegides = 20;     

                }
            }
            bool un = false;
            bool dos = false;
            bool tres= false;
            
            if (a.Voluntaris.Count >= 1)
            {
                afegides = afegides * 0.9m;
                un = true;
            }
            if (DateTime.Today.Year - a.DataNaixement.Year >= 8)
               
            {
                afegides = afegides * 1.3m;
                dos = true;
            }

            int criesnoadoptades = 0;
            foreach (Animal c in a.Cries.ToList())
            {
                
                if (!c.EstaAdoptat)
                {
                    criesnoadoptades++;
                    tres = true;

                }
                
            }
            afegides = afegides * (1+0.1m*criesnoadoptades);
            
            a.DespesaAcumulada = a.DespesaAcumulada + afegides;
            if (afegides > max)
            {
                max = afegides;
                animalmescar = a;
            }
            
            total = total + afegides;
            if (un)
            {
                Console.WriteLine($"{a.Nom} tenia una despesa de {previ} aquest mes ha gastat {afegides} i el total es de {a.DespesaAcumulada} el modificador de voluntaris ha sigut aplicat ");

                
            }    else if(dos)

            {
                Console.WriteLine($"{a.Nom} tenia una despesa de {previ} aquest mes ha gastat {afegides} i el total es de {a.DespesaAcumulada} el modificador de any ha sigut aplicat ");
            } else if(tres)

            {
                Console.WriteLine($"{a.Nom} tenia una despesa de {previ} aquest mes ha gastat {afegides} i el total es de {a.DespesaAcumulada} el modificador de cries ha sigut aplicat ");
            }  else if(un && dos )

            {
                Console.WriteLine($"{a.Nom} tenia una despesa de {previ} aquest mes ha gastat {afegides} i el total es de {a.DespesaAcumulada} el modificador de voluntaris i senior han sigut aplicats ");
            } else if(un && tres)

            {
                Console.WriteLine($"{a.Nom} tenia una despesa de {previ} aquest mes ha gastat {afegides} i el total es de {a.DespesaAcumulada} el modificador de voluntaris i cries han sigut aplicats ");
            } else if(dos && tres)

            {
                Console.WriteLine($"{a.Nom} tenia una despesa de {previ} aquest mes ha gastat {afegides} i el total es de {a.DespesaAcumulada} el modificador de cries i any han sigut aplicats ");
            }
            else
            {
                Console.WriteLine($"{a.Nom} tenia una despesa de {previ} aquest mes ha gastat {afegides} i el total es de {a.DespesaAcumulada}");
            }

            
        }
        Console.WriteLine($"{animalmescar.Nom}  es el animal mes car d'aquest mes amb un total de {max}");
        Console.WriteLine($"{total}  es el que ha costat en total aquest mes");

        ctx.SaveChanges();    

        
    }

    

}