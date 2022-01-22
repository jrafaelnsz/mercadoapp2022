using System.Text.Json;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public Categoria Categoria { get; set; }
    public decimal Preco { get; set; }

    private static string ObterCaminhoArquivo()
    {
        string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var directory = System.IO.Path.GetDirectoryName(path);
        return directory + @"\BaseDados\Produto.txt";
    }

    //Método que grava o json no arquivo
    private static void GravarListaProdutos(string curFile, string json)
    {
        File.Delete(curFile);
        using (StreamWriter sw = File.AppendText(curFile))
        {
            sw.Write(json);
        }
    }

    //Método de criação de funcionário
    public static void CriarProduto(Produto prod)
    {
        var listaProdutos = ObterListaProdutos();
        var idMax = ObterProximoId(listaProdutos);
        prod.Id = idMax;
        listaProdutos.Add(prod);
        var json = JsonSerializer.Serialize(listaProdutos);
        GravarListaProdutos(ObterCaminhoArquivo(), json);
    }

    //Método que obtem o maior ID a ser inserido
    private static int ObterProximoId(List<Produto> listaProduto)
    {
        var idMax = 0;

        foreach (var item in listaProduto)
        {
            idMax = item.Id > idMax ? item.Id : idMax;
        }

        return idMax + 1;
    }

    //Método para obter lista de funcionários
    public static List<Produto> ObterListaProdutos()
    {
        //Verificar se existe arquivo de dados dos funcionários
        //caso não exista, ciar arquivo e carregar usuário master

        string curFile = ObterCaminhoArquivo();

        var listaProduto = new List<Produto>();

        if (File.Exists(curFile))
        {

            using (StreamReader sr = new StreamReader(curFile))
            {
                string line = sr.ReadToEnd();

                listaProduto = JsonSerializer.Deserialize<List<Produto>>(line);
            }

        }

        return listaProduto;
    }

    public static bool RemoverProduto(int id)
    {
        try
        {
            var listaProduto = ObterListaProdutos();
            listaProduto.RemoveAll(x => x.Id == id);

            SobrescreverListaProdutos(listaProduto);

            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    //Método de criação de funcionário
    protected static void SobrescreverListaProdutos(List<Produto> listaProduto)
    {
        var json = JsonSerializer.Serialize(listaProduto);
        GravarListaProdutos(ObterCaminhoArquivo(), json);
    }

    //Método de sobrecarga do ToString()
    public override string ToString()
    {
        return $"|Produto - {Id}|Nome - {Nome}|Categoria - {Categoria}|Preço - {decimal.Round(Preco, 2)}|";
    }
}