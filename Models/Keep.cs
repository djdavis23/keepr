using System.ComponentModel.DataAnnotations;

namespace keepr.Models
{
  public class Keep
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string Description { get; set; }

    [MaxLength(255)]
    public string Img { get; set; }

    public int IsPrivate { get; set; }

    [Required]
    public string UserId { get; set; }

    public int Views { get; set; }

    public int Shares { get; set; }

    public int Keeps { get; set; }

  }

  public class KeepForm
  {
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }

    [MaxLength(255)]
    public string Description { get; set; }

    [Required]
    public string UserId { get; set; }

    [MaxLength(255)]
    public string Img { get; set; }
    public int IsPrivate { get; set; }

  }

  public class KeepDiscard
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public int IsPrivate { get; set; }

  }

}