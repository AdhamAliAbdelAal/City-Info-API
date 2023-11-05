using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfoApi.Models;

public class PointOfInterestDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { set; get; }
    
    [Required]
    [MaxLength(50)]
    public string Name { set; get; }
    
    [MaxLength(200)]
    public string Description { set; get; }
    
    public CityDbModel City { set; get; }
    
    public int CityId { set; get; }
    
    
}