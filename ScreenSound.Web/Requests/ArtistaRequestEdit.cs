using System.ComponentModel.DataAnnotations;

namespace ScreenSound.Web.Requests;

public record ArtistaRequestEdit([Required] string Nome, string Bio, string FotoPerfil, [Required] int Id);
