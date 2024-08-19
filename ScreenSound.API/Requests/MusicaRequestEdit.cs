using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record MusicaRequestEdit([Required] string Nome, [Required] int AnoLancamento, [Required] int ArtistaId, int Id);