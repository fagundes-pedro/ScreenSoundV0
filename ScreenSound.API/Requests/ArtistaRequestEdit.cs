using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record ArtistaRequestEdit([Required] string Nome, string Bio, string FotoPerfil, [Required] int Id);
