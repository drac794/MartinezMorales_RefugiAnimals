using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RefugiAnimals.Models;

public class Animal
{
    public int AnimalID { get; set; }
    
    

    [Required]
    [StringLength(50)]
    public string Nom { get; set; }

    [Required]
    public Especie Especie { get; set; }

    [Required]
    public bool EsFemella { get; set; }

    [Required]
    public DateTime DataNaixement { get; set; }

    [Required]
    public DateTime DataEntrada { get; set; }

    [Required]
    [Range(0.1, 100)]
    public double Pes { get; set; }

    [Required]
    public bool EstaAdoptat { get; set; }

    public DateTime? DataAdopcio { get; set; }

    [Required]
    [Precision(8, 2)]
    public decimal QuotaAdopcio { get; set; }

    [Required]
    public int XipID { get; set; }

    public virtual Xip Xip { get; set; }


    public int? AdoptantID { get; set; }

    public virtual Adoptant? Adoptant { get; set; }

    public Animal()
    {
        DataEntrada = DateTime.Now;
        EstaAdoptat = false;
        DataAdopcio = null;
        QuotaAdopcio = 0;
    }

    public Animal(string nom, Especie especie, bool esFemella,
        DateTime dataNaixement, double pes)
    {
        Nom = nom;
        Especie = especie;
        EsFemella = esFemella;
        DataNaixement = dataNaixement;
        Pes = pes;

        DataEntrada = DateTime.Now;
        EstaAdoptat = false;
        DataAdopcio = null;
        QuotaAdopcio = 0;
    }
    
    public override string ToString()
    {
        return $"AnimalID: {AnimalID}, Nom: {Nom}, Especie: {Especie}, " +
               $"EsFemella: {EsFemella}, DataNaixement: {DataNaixement}, " +
               $"DataEntrada: {DataEntrada}, Pes: {Pes}, " +
               $"EstaAdoptat: {EstaAdoptat}, DataAdopcio: {DataAdopcio}, " +
               $"Adoptant: {Adoptant}, AdoptantID: {AdoptantID}, " +
               $"QuotaAdopcio: {QuotaAdopcio}, XipID: {XipID} " ;
    }
}