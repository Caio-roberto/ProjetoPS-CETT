namespace ProjetoPSCett.Models;

public class PublicacaoViewModel
{
    public List<Publicacao> Publicacoes = new();

    public string? StringPesquisa { get; set; }

    public string? Comentario { get; set; }
}
