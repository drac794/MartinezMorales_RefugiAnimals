using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RefugiAnimals.Models;
[Index(nameof(DNI), IsUnique = true)] 
public class Adoptant
{
    public int AdoptantID { get; set; }
    
    [Required]
    [StringLength(9)]
    public string DNI { get; set; }

    [Required]
    [StringLength(50)]
    public string Nom { get; set; }
    [Required]
    [StringLength(100)]
    public string Cognoms { get; set; }
    
    [Required]
    public bool TeJardi { get; set; }
    
    [Required]
    public DateTime DataRegistre { get; set; }
    
    public virtual ICollection<Animal> Animals { get; set; }
    
    public Adoptant()
    {
        DataRegistre = DateTime.Now;
        Animals = new HashSet<Animal>();

    }
    public Adoptant(string nom,string cognoms  , bool teJardi,
        DateTime dataRegistre, double pes)
    {
        Nom = nom;
        Cognoms = cognoms;
        TeJardi = teJardi;
        DataRegistre = dataRegistre;
        Animals = new HashSet<Animal>();
    }
    
    public override string ToString()
    {
        return $"AdoptantID: {AdoptantID}, Nom: {Nom}, Cognoms: {Cognoms}, " +
               $"Animals: {Animals}, " +
               $"TeJardi: {TeJardi}, DataRegistre: {DataRegistre}, ";
    }
    
    
}