using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestUser.Models
{
  public class Profiles
  {
    [Key]
    public int Id { get; set; }
    public string CodFidelity { get; set; }
    public string Type { get; set; }

    public virtual Users User { get; set; }
  }
}
