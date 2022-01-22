public class Gerente : Funcionario
{
    public static bool RemoverFuncionario(int id)
    {
        try
        {
            var listaFuncionario = ObterListaFuncionarios();
            listaFuncionario.RemoveAll(x => x.Id == id);

            SobrescreverListaFuncionario(listaFuncionario);

            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public void VenderProduto()
    {
        throw new NotImplementedException();
    }
}