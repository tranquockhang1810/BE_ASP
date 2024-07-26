namespace BE_ASP.Models
{
  public class UserTask
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int Priority { get; set; }
    public int Status { get; set; }
  }
}