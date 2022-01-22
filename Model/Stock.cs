using System.Text.Json;

public class Stock
{
    public int Id { get; set; }
    public Produto Prod { get; set; }
    public int Quantidade { get; set; }

    //verificar se existem produtos que não existem no Stock
    public static void Init()
    {
        var listaItemStock = ObterListaItemStock();
        foreach (var item in Produto.ObterListaProdutos())
        {
            if (!listaItemStock.Any(x => x.Prod.Id == item.Id))
            {
                listaItemStock.Add(new Stock() { Id = ObterProximoId(listaItemStock), Prod = item, Quantidade = 0 });
            }
        }

        SobrescreverListaStock(listaItemStock);
    }

    public static void AlterarQuantidadeStock(int idProduto, int quantidade)
    {
        var listaItemStock = ObterListaItemStock();
        var itemStock = listaItemStock.Single(x => x.Prod.Id == idProduto);
        listaItemStock.Remove(itemStock);
        itemStock.Quantidade = quantidade;
        listaItemStock.Add(itemStock);

        SobrescreverListaStock(listaItemStock);

    }


    public static void AbaterQuantidadeStock(int idProduto, int quantidade)
    {
        var listaItemStock = ObterListaItemStock();
        var itemStock = listaItemStock.Single(x => x.Prod.Id == idProduto);
        listaItemStock.Remove(itemStock);
        itemStock.Quantidade = itemStock.Quantidade - quantidade;
        listaItemStock.Add(itemStock);

        SobrescreverListaStock(listaItemStock);

    }

    private static string ObterCaminhoArquivo()
    {
        string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var directory = System.IO.Path.GetDirectoryName(path);
        return directory + @"\BaseDados\Stock.txt";
    }

    //Método que grava o json no arquivo
    private static void GravarListaStock(string curFile, string json)
    {
        File.Delete(curFile);
        using (StreamWriter sw = File.AppendText(curFile))
        {
            sw.Write(json);
        }
    }

    public static void CriarItemStock(Stock ItemStock)
    {
        var listaItemStock = ObterListaItemStock();
        var idMax = ObterProximoId(listaItemStock);
        ItemStock.Id = idMax;
        listaItemStock.Add(ItemStock);
        var json = JsonSerializer.Serialize(listaItemStock);
        GravarListaStock(ObterCaminhoArquivo(), json);
    }

    //Método que obtem o maior ID a ser inserido
    private static int ObterProximoId(List<Stock> listaItemStock)
    {
        var idMax = 0;

        foreach (var item in listaItemStock)
        {
            idMax = item.Id > idMax ? item.Id : idMax;
        }

        return idMax + 1;
    }

    //Método para obter lista de itens do stock
    public static List<Stock> ObterListaItemStock()
    {
        //Verificar se existe arquivo de dados dos funcionários
        //caso não exista, ciar arquivo e carregar usuário master

        string curFile = ObterCaminhoArquivo();

        var listaStock = new List<Stock>();

        if (File.Exists(curFile))
        {

            using (StreamReader sr = new StreamReader(curFile))
            {
                string line = sr.ReadToEnd();

                listaStock = JsonSerializer.Deserialize<List<Stock>>(line);
            }

        }

        return listaStock;
    }


    //Método de criação de funcionário
    protected static void SobrescreverListaStock(List<Stock> listaStock)
    {
        var json = JsonSerializer.Serialize(listaStock);
        GravarListaStock(ObterCaminhoArquivo(), json);
    }

    //Método de sobrecarga do ToString()
    public override string ToString()
    {
        return $"|Stock - {Id}|Produto - {Prod.Id}|Nome - {Prod.Nome}|Categoria - {Prod.Categoria}|Preço - {decimal.Round(Prod.Preco, 2)}|Quantidade {Quantidade}|";
    }



}