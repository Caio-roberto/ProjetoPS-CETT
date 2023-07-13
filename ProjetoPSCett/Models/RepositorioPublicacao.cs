using FirebirdSql.Data.FirebirdClient;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace ProjetoPSCett.Models;

public class RepositorioPublicacao : RepositorioAbstrato<Publicacao>
{
    public string caminhoConexao = @"DataSource=localhost; Port=3050; Database=C:\Users\caior\Documents\Caio\Projeto PS\PROJETOCETT.FDB; username= SYSDBA; password = masterkey";
    public FbConnection? conn;

    public override void Add(Publicacao publicacao)
    {
        conn = new(caminhoConexao);
        conn.Open();
        FbCommand comandoAdd = new($"INSERT INTO TBPUBLICACAO (PUBID, PUBAUTOR, PUBTITULO, PUBCONTEUDO, PUBDATA, PUBESPORTE) VALUES ({publicacao.Id}, '{publicacao.Autor}', '{publicacao.Titulo}', '{publicacao.Conteudo}', '{publicacao.DataPublicacao:dd.MM.yyyy}', {(int)publicacao.Esporte});", conn);
        comandoAdd.ExecuteNonQuery();
        conn.Close();
    }

    public override void Remove(Publicacao publicacao)
    {
        conn = new(caminhoConexao);
        conn.Open();
        RemoveComentarios(publicacao.Id, conn);
        FbCommand comandoRemove = new($"DELETE FROM TBPUBLICACAO WHERE PUBID = {publicacao.Id};", conn);
        comandoRemove.ExecuteNonQuery();
        conn.Close();
    }

    private static void RemoveComentarios(int id, FbConnection conn)
    {
        FbCommand comandoRemoveComentarios = new($"DELETE FROM TBCOMENTARIOS WHERE COMID = {id};", conn);
        comandoRemoveComentarios.ExecuteNonQuery();
    }

    public override void Update(Publicacao publicacao)
    {
        conn = new(caminhoConexao);
        conn.Open();
        FbCommand comandoUpdate = new($"UPDATE TBPUBLICACAO SET PUBID = {publicacao.Id}," +
            $"PUBAUTOR = '{publicacao.Autor}'," +
            $"PUBTITULO = '{publicacao.Titulo}'," +
            $"PUBCONTEUDO = '{publicacao.Conteudo}'," +
            $"PUBESPORTE = {(int)publicacao.Esporte}," +
            $"PUBDATA = '{publicacao.DataPublicacao:dd.MM.yyyy}' " +
            $"WHERE PUBID = {publicacao.Id};", conn);
        comandoUpdate.ExecuteNonQuery();
        conn.Close();
    }

    public override IEnumerable<Publicacao> GetAll()
    {
        conn = new(caminhoConexao);
        conn.Open();
        FbCommand comandoGetAll = new($"SELECT * FROM TBPUBLICACAO", conn);
        FbDataReader reader = comandoGetAll.ExecuteReader();
        List<Publicacao> publicacoes = new();
        while (reader.Read())
        {
            Publicacao publicacao = new(
                Convert.ToInt32(reader["PUBID"]),
                reader["PUBAUTOR"].ToString() ?? "",
                reader["PUBTITULO"].ToString() ?? "",
                reader["PUBCONTEUDO"].ToString() ?? "",
                Convert.ToDateTime(reader["PUBDATA"]),
                (EnumeradorEsporte)reader["PUBESPORTE"]
            );
            publicacao.Comentários = GetComentarios(publicacao.Id);
            publicacoes.Add(publicacao);
        }
        reader.Close();
        conn.Close();
        return publicacoes.OrderBy(publicacao => publicacao.DataPublicacao);
    }

    private List<string> GetComentarios(int id)
    {
        conn = new(caminhoConexao);
        conn.Open();
        FbCommand comandoGetAll = new($"SELECT * FROM TBCOMENTARIOS WHERE COMID = {id}", conn);
        FbDataReader reader = comandoGetAll.ExecuteReader();
        List<string> comentarios = new();
        while (reader.Read())
        {
            string comentario = reader["COMCONTEUDO"].ToString() ?? "";
            comentarios.Add(comentario);
        }
        return comentarios;
    }

    public void AddComentario(int id, string comentario)
    {
        conn = new(caminhoConexao);
        conn.Open();
        FbCommand comandoAdd = new($"INSERT INTO TBCOMENTARIOS (COMID, COMCONTEUDO) VALUES ({id}, '{comentario}');", conn);
        comandoAdd.ExecuteNonQuery();
        conn.Close();
    }

    public override IEnumerable<Publicacao> Get(Expression<Func<Publicacao, bool>> predicate)
    {
        return GetAll().Where(predicate.Compile());
    }

    public Publicacao GetById(int id)
    {
        return Get(publicacao => publicacao.Id == id).First();
    }

    public IEnumerable<Publicacao> GetByAutor(string parteDoNome)
    {
        return Get(publicacao => publicacao.Autor.ToLower().Contains(parteDoNome)).ToList();
    }

    public IEnumerable<Publicacao> GetByTitulo(string parteDoNome)
    {
        return Get(publicacao => publicacao.Titulo.ToString().Contains(parteDoNome)).ToList();
    }
}
