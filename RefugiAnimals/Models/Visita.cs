using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace RefugiAnimals.Models;

public class Visita
{
    [Required]
    public int VisitaID { get; set; }
    [Required]
    public DateTime Data { get; set; }

    [Required]
    public Motiu Motiu { get; set; }
    
    [Required]
    public int Cost  { get; set; }
    [Required]
    public virtual Animal Animal { get; set; }
    
    public Visita()
    {
     
    }
    
       
    public Visita(Motiu motiu, int cost, Animal animal)
    {
        Motiu = motiu;
        Cost = cost;
        Animal = animal;
        Data = DateTime.Now;
    }
    public override string ToString()
    {
        return $"VisitaID: {VisitaID}, Data: {Data},  Motiu: {Motiu}, Cost: {Cost}, " +
               $"Animal: {Animal}";
    } 
    
    

}