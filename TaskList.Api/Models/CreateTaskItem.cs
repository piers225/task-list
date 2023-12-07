
using System.ComponentModel.DataAnnotations;

namespace TaskList.Api.Models;

public record CreateTaskItem 
{
    [Required]
    public string Name { get; set; }
}