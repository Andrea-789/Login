using System;
using System.ComponentModel.DataAnnotations;

namespace GestUser.Models
{
  public class Users
  {
    [Key]
    public string Id { get; set; }

    [StringLength(100, ErrorMessage = "The Name must be max 100 characters long")]
    public string Name { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "The Surname must be max 100 characters long")]
    public string Surname { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "The address must be max 100 characters long")]
    public string Address { get; set; } = string.Empty;

    [StringLength(5, ErrorMessage = "The ZIP must be 5 characters long")]
    public string ZIP { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "The City must be max 100 characters long")]
    public string City { get; set; } = string.Empty;

    [StringLength(2, ErrorMessage = "The State must be 2 characters long")]
    public string State { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "The Telephone must be max 30 characters long")]
    public string Telephone { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "The Company Name must be max 100 characters long")]
    public string CompanyName { get; set; } = string.Empty;

    public string STN { get; set; } = string.Empty;

    public string SSN { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Email must be max 50 characters long")]
    [Required(ErrorMessage = "Insert a valid Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Insert a Password")]
    public string Password { get; set; } = string.Empty;

    public byte Enabled { get; set; } = 0;

    public DateTime Date { get; set; } = DateTime.Today;
    public string Role { get; set; } = string.Empty;

  }
}
