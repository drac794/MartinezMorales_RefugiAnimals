using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RefugiAnimals.Models;

[Index(nameof(Nom), IsUnique = true)]
public class Voluntari
{
    public int VoluntariID { get; set; }

    [Required]
    [StringLength(50)]
    public string Nom { get; set; }

    [Required]
    public Torn Torn { get; set; }

    public int MinutsPasseig { get; set; }

    public virtual ICollection<Animal> Animals { get; set; }

    public Voluntari()
    {
        MinutsPasseig = 0;
        Animals = new HashSet<Animal>();
    }

    public Voluntari(string nom, Torn torn)
    {
        Nom = nom;
        Torn = torn;
        MinutsPasseig = 0;
        Animals = new HashSet<Animal>();
    }

    public override string ToString()
    {
        return $"VoluntariID: {VoluntariID}, Nom: {Nom}, Torn: {Torn}, " +
               $"MinutsPasseig: {MinutsPasseig}, Animals: {Animals.Count}";
    }
}