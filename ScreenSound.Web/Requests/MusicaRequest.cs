using System.ComponentModel.DataAnnotations;

namespace ScreenSound.Web.Requests;

public record MusicaRequest([Required] string Nome, [Required] int ArtistaID, int AnoLancamento, ICollection<GeneroRequest> Generos = null);