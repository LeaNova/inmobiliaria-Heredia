using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_Heredia.Models;

public class CambiarAvatar {

    [Display(Name = "Avatar"), Required]
    public IFormFile avatarFile { get; set; }
}