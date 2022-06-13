using System;

namespace GestUser.Models
{
  public class InfoMsg
  {
    public DateTime Date { get; set; }
    public string Message { get; set; }

    public InfoMsg(DateTime Data, String Message)
    {
      this.Date = Date;
      this.Message = Message;
    }
  }
}
