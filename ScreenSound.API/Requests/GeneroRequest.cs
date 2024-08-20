using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests;

public record GeneroRequest(string Nome, string Descricao);