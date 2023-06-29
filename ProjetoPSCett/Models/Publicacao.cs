using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjetoPSCett.Models;

public class Publicacao : IEntidade
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Autor não pode ser vazio.")]
    public string Autor { get; set; }

    [Required(ErrorMessage = "Titulo não pode ser vazio.")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "Conteudo não pode ser vazio.")]
    public string Conteudo { get; set; }

    [Display(Name = "Data de publicação")]
    [DataType(DataType.Date)]
    public DateTime DataPublicacao { get; set; }

    [Required(ErrorMessage = "Por favor selecione uma opção válida.")]
    public EnumeradorEsporte Esporte { get; set; }

    public List<string> Comentários = new();

    public Publicacao(int id, string autor, string titulo, string conteudo, DateTime horarioPublicacao, EnumeradorEsporte esporte)
    {
        Id = id;
        Autor = autor;
        Titulo = titulo;
        Conteudo = conteudo;
        DataPublicacao = horarioPublicacao;
        Esporte = esporte;
    }
}
