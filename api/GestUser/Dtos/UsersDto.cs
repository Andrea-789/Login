using System;

namespace GestUser.Dtos
{
  public class UsersDto
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    string Surname { get; set; } = string.Empty;
    string Address { get; set; } = string.Empty;
    public string ZIP { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string Telephone { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string STN { get; set; } = string.Empty;

    public string SSN { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Int16 Enabled { get; set; } = 0;

    public string Role { get; set; } = string.Empty;

  }

}
