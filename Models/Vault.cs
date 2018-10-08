using System.ComponentModel.DataAnnotations;

namespace keepr.Models
{
  public class Vault
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string Description { get; set; }

    [Required]
    public string UserId { get; set; }
  }

  public class VaultForm
  {
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string Description { get; set; }

    [Required]
    public string UserId { get; set; }
  }

}