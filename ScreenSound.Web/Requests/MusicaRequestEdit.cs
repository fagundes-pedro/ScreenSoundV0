using System.ComponentModel.DataAnnotations;

namespace ScreenSound.Web.Requests;

public record MusicaRequestEdit([Required] string Nome, [Required] int AnoLancamento, [Required] int ArtistaId, int Id);