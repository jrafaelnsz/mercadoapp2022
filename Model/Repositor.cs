public class Repositor : Funcionario
{
    public static void CriarProduto(Produto produto)
    {
        Produto.CriarProduto(produto);
    }

    public static void AlterarQuantidadeStock(int idProduto, int quantidade)
    {
        Stock.AlterarQuantidadeStock(idProduto, quantidade);
    }

    public static bool RemoverProduto(int idProduto)
    {
        return Produto.RemoverProduto(idProduto);
    }
}