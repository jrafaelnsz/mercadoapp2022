using System.Text.Json;

public class Venda
{
    public int Id { get; set; }
    public List<Produto> ListaProdutos { get; set; }
    public string NomeFuncionario { get; set; }
    public string NomeCliente { get; set; }

    decimal CalcularFaturaVenda()
    {
        decimal fatura = 0.00m;
        foreach (var item in ListaProdutos)
        {
            fatura += item.Preco;
        }

        return fatura;
    }

    private static string ObterCaminhoArquivo()
    {
        string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var directory = System.IO.Path.GetDirectoryName(path);
        return directory + @"\BaseDados\Venda.txt";
    }

    //Método que grava o json no arquivo
    private static void GravarListaVenda(string curFile, string json)
    {
        File.Delete(curFile);
        using (StreamWriter sw = File.AppendText(curFile))
        {
            sw.Write(json);
        }
    }

    public static void CriarVenda(Venda venda)
    {
        var listaVenda = ObterListaVenda();
        var idMax = ObterProximoId(listaVenda);
        venda.Id = idMax;
        listaVenda.Add(venda);
        var json = JsonSerializer.Serialize(listaVenda);
        GravarListaVenda(ObterCaminhoArquivo(), json);

        foreach (var item in venda.ListaProdutos)
        {
            Stock.AbaterQuantidadeStock(item.Id, 1);
        }
    }

    //Método que obtem o maior ID a ser inserido
    private static int ObterProximoId(List<Venda> listaVenda)
    {
        var idMax = 0;

        foreach (var item in listaVenda)
        {
            idMax = item.Id > idMax ? item.Id : idMax;
        }

        return idMax + 1;
    }


    public static List<Venda> ObterListaVenda()
    {
        string curFile = ObterCaminhoArquivo();

        var listaVenda = new List<Venda>();

        if (File.Exists(curFile))
        {

            using (StreamReader sr = new StreamReader(curFile))
            {
                string line = sr.ReadToEnd();

                listaVenda = JsonSerializer.Deserialize<List<Venda>>(line);
            }

        }

        return listaVenda;
    }

    protected static void SobrescreverListaVendas(List<Venda> listaVenda)
    {
        var json = JsonSerializer.Serialize(listaVenda);
        GravarListaVenda(ObterCaminhoArquivo(), json);
    }

    //Método de sobrecarga do ToString()
    public override string ToString()
    {
        var nota = $"------------------------NOTA DE VENDA: {Id}------------------------\r\n";
        foreach (var item in ListaProdutos)
        {
            nota += $"|Produto - {item.Id}|Nome - {item.Nome}|Categoria - {item.Categoria}|Preço - {decimal.Round(item.Preco, 2)}|\r\n";
        }
        nota += "----------------------------------------------------------\r\n";
        nota += $"TOTAL ------------------------> {decimal.Round(CalcularFaturaVenda(), 2)}<------------------------";
        return nota;
    }

}