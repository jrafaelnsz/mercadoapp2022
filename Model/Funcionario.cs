using System.Text.Json;

public class Funcionario
{
    //Atributos
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
    public Perfil Cargo { get; set; }

    //Método construtor Padrão
    public Funcionario()
    {

    }

    //Método construtor customizado
    public Funcionario(int id, string nome, string login, string senha, Perfil cargo)
    {
        Id = id;
        Nome = nome;
        Login = login;
        Senha = senha;
        Cargo = cargo;
    }

    //Método para obter lista de funcionários
    public static List<Funcionario> ObterListaFuncionarios()
    {
        //Verificar se existe arquivo de dados dos funcionários
        //caso não exista, ciar arquivo e carregar usuário master

        string curFile = ObterCaminhoArquivo();

        var listaFuncionario = new List<Funcionario>();

        if (File.Exists(curFile))
        {

            using (StreamReader sr = new StreamReader(curFile))
            {
                string line = sr.ReadToEnd();

                listaFuncionario = JsonSerializer.Deserialize<List<Funcionario>>(line);
            }

        }

        return listaFuncionario;
    }

    //Método para obter caminho padrão do arquivo de dados
    private static string ObterCaminhoArquivo()
    {
        string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var directory = System.IO.Path.GetDirectoryName(path);
        return directory + @"\BaseDados\Funcionario.txt";
    }

    //Método que grava o json no arquivo
    private static void GravarListaFuncionarios(string curFile, string json)
    {
        File.Delete(curFile);
        using (StreamWriter sw = File.AppendText(curFile))
        {
            sw.Write(json);
        }
    }

    //Método de criação de funcionário
    public static void CriarFuncionario(Funcionario func)
    {
        var listafuncionarios = ObterListaFuncionarios();
        var idMax = ObterProximoId(listafuncionarios);
        func.Id = idMax;
        listafuncionarios.Add(func);
        var json = JsonSerializer.Serialize(listafuncionarios);
        GravarListaFuncionarios(ObterCaminhoArquivo(), json);
    }

    //Método de criação de funcionário
    protected static void SobrescreverListaFuncionario(List<Funcionario> listafuncionarios)
    {
        var json = JsonSerializer.Serialize(listafuncionarios);
        GravarListaFuncionarios(ObterCaminhoArquivo(), json);
    }

    //Método que obtem o maior ID a ser inserido
    private static int ObterProximoId(List<Funcionario> listafuncionarios)
    {
        var idMax = 0;

        foreach (var item in listafuncionarios)
        {
            idMax = item.Id > idMax ? item.Id : idMax;
        }

        return idMax + 1;
    }

    //Método de validação de acesso
    public static Funcionario ValidarAcesso(string login, string senha)
    {
        foreach (var item in ObterListaFuncionarios())
        {
            if (item.Login == login && item.Senha == senha)
            {
                return item;
            }
        }
        return null;
    }

    //Método de sobrecarga do ToString()
    public override string ToString()
    {
        return $"|Funcionario - {Id}|Nome - {Nome}|Login - {Login}| Cargo {Cargo}|";
    }
}