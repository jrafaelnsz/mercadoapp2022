public class Menu
{
    Funcionario funcionarioLogado;

    public void MainMenu()
    {
        System.Console.WriteLine("--Sistema de vendas Mercado APP--");
        System.Console.WriteLine("1 - Entrar com login");
        System.Console.WriteLine("2 - Cadastrar novo funcionário");
        System.Console.WriteLine("Qualquer outra tecla para finalizar");
        var opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                Logar();
                break;
            case "2":
                NovoFuncionario();
                break;
            default:
                Environment.Exit(0);
                break;
        }

    }

    void Logar()
    {
        var credenciais = ObterCredenciais(false);

        funcionarioLogado = Funcionario.ValidarAcesso(credenciais.login, credenciais.senha);

        if (funcionarioLogado != null)
        {
            switch (funcionarioLogado.Cargo)
            {
                case Perfil.Gerente:
                    MostrarMenuGerente();
                    break;
                case Perfil.Caixa:
                    MostrarMenuCaixa();
                    break;
                case Perfil.Repositor:
                    MostrarMenuRepositor();
                    break;
                default:
                    Environment.Exit(0);
                    break;
            }
        }
        else
        {
            System.Console.WriteLine("Acesso negado");
        }
    }

    void MostrarMenuRepositor()
    {
        //Forçar atualização do stock para novos produtos
        Stock.Init();

        Console.Clear();

        var opcao = "-1";

        while (opcao != "0")
        {
            System.Console.WriteLine("------------MENU REPOSITOR-----------");
            System.Console.WriteLine("1 - Exibir lista de produtos");
            System.Console.WriteLine("2 - Adicionar produto");
            System.Console.WriteLine("3 - Excluir produto");
            System.Console.WriteLine("4 - Maipular stock");
            System.Console.WriteLine("0 = Para finalizar");
            opcao = Console.ReadLine();

            System.Console.WriteLine(opcao);

            switch (opcao)
            {
                case "1":
                    ExibirListaProdutos();
                    break;
                case "2":
                    MostrarMenuAdicionarProduto();
                    break;
                case "3":
                    MostrarMenuRemoverProduto();
                    break;
                case "4":
                    MostrarMenuAlterarStock();
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }
    }

    private void MostrarMenuAlterarStock()
    {
        System.Console.WriteLine("------------MENU REPOSITOR>MANIPULAR STOCK-----------");
        System.Console.WriteLine("1 - Exibir stock");
        System.Console.WriteLine("2 - Alterar quantidade de produto no stock");
        System.Console.WriteLine("0 = Para finalizar");
        var opcao = Console.ReadLine();

        System.Console.WriteLine(opcao);

        switch (opcao)
        {
            case "1":
                ExibirListaItemStock();
                break;
            case "2":
                AlterarStock();
                break;
            default:
                Console.Clear();
                break;
        }
    }

    void MostrarMenuCaixa()
    {
        Console.Clear();

        var opcao = "-1";

        while (opcao != "0")
        {
            System.Console.WriteLine("------------MENU CAIXA-----------");
            System.Console.WriteLine("1 - Exibir lista de vendas");
            System.Console.WriteLine("2 - Realizar venda");
            System.Console.WriteLine("0 = Para finalizar");
            opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    ExibirListaVendas();
                    break;
                case "2":
                    MostrarMenuRealizarVenda();
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }
    }

    private void MostrarMenuRealizarVenda()
    {
        Console.Clear();

        var opcao = "-1";

        while (opcao != "0")
        {
            System.Console.WriteLine("------------MENU CAIXA>REALIZAR VENDA-----------");
            System.Console.WriteLine("1 - Exibir stock");
            System.Console.WriteLine("2 - Realizar venda");
            System.Console.WriteLine("0 = Para finalizar");
            opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    ExibirListaItemStock();
                    break;
                case "2":
                    RealizarVenda();
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }
    }

    private void RealizarVenda()
    {
        var opcao = "-1";
        var venda = new Venda();
        venda.ListaProdutos = new List<Produto>();
        venda.NomeFuncionario = funcionarioLogado.Login;

        System.Console.WriteLine("Digite o nome do cliente");
        venda.NomeCliente = Console.ReadLine();

        while (opcao != "0")
        {
            System.Console.WriteLine("Insira o ID do produto");
            var idProduto = Console.ReadLine();

            System.Console.WriteLine("Insira a quantidade");
            var quantidade = Console.ReadLine();

            var lista = IncrementarVenda(Convert.ToInt32(idProduto), Convert.ToInt32(quantidade));
            foreach (var item in lista)
            {
                venda.ListaProdutos.Add(item);
            }

            System.Console.WriteLine("0 Para finalizar - Qualquer tecla para continuar");
            opcao = Console.ReadLine();

            if (opcao == "0")
            {
                FinalizarVenda(venda);
            }
        }
    }

    private void FinalizarVenda(Venda venda)
    {

        if (venda.ListaProdutos != null && venda.ListaProdutos.Count > 0)
        {
            Venda.CriarVenda(venda);
        }
    }

    private List<Produto> IncrementarVenda(int idProduto, int quantidade)
    {
        var listaProduto = new List<Produto>();
        var ListaGenerica = Produto.ObterListaProdutos();
        if (ValidarEstoque(idProduto, quantidade))
        {

            for (int i = 0; i < quantidade; i++)
            {
                listaProduto.Add(ListaGenerica.Single(x => x.Id == idProduto));
            }
        }
        else
        {
            System.Console.WriteLine("Produto inválido ou quantidade insuficiente");
        }

        foreach (var item in listaProduto)
        {
            System.Console.WriteLine(item.ToString());
        }

        return listaProduto;
    }

    private bool ValidarEstoque(int idProduto, int quantidade)
    {
        var listaStock = Stock.ObterListaItemStock();
        return listaStock.Any(x => x.Prod.Id == idProduto && x.Quantidade >= quantidade);
    }

    private void ExibirListaVendas()
    {
        foreach (var item in Venda.ObterListaVenda())
        {
            System.Console.WriteLine(item.ToString());
        }

    }

    void MostrarMenuGerente()
    {
        Console.Clear();

        var opcao = "-1";

        while (opcao != "0")
        {
            System.Console.WriteLine("------------MENU GERENTE-----------");
            System.Console.WriteLine("1 - Exibir lista de funcionário");
            System.Console.WriteLine("2 - Excluir funcionário");
            System.Console.WriteLine("3 - Exibir opções de venda");
            System.Console.WriteLine("0 = Para finalizar");
            opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    ExibirListaFuncionarios();
                    break;
                case "2":
                    MostrarMenuRemoverFuncionario();
                    break;
                case "3":
                    MostrarMenuCaixa();
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }
    }

    void MostrarMenuRemoverFuncionario()
    {
        System.Console.WriteLine("Digite o ID do funcioário a ser removido");
        var idFuncionario = Console.ReadLine();
        var retorno = Gerente.RemoverFuncionario(Convert.ToInt32(idFuncionario));

        if (retorno)
        {
            System.Console.WriteLine("usuário removido com sucesso!");
        }
        else
        {
            System.Console.WriteLine("Não foi possível remover o usuário");
        }

    }

    void MostrarMenuRemoverProduto()
    {
        System.Console.WriteLine("Digite o ID do produto a ser removido");
        var idProduto = Console.ReadLine();
        var retorno = Repositor.RemoverProduto(Convert.ToInt32(idProduto));

        if (retorno)
        {
            System.Console.WriteLine("produto removido com sucesso!");
        }
        else
        {
            System.Console.WriteLine("Não foi possível remover o produto");
        }
    }

    void MostrarMenuAdicionarProduto()
    {
        System.Console.WriteLine("Digite o nome do produto");
        var nome = Console.ReadLine();

        System.Console.WriteLine("Digite o preço do produto");
        var preco = Console.ReadLine();

        System.Console.WriteLine("Digite a categoria do produto");
        System.Console.WriteLine("1 - Congelado");
        System.Console.WriteLine("2 - Pateleira");
        System.Console.WriteLine("3 - Enlatado");
        var categoria = Console.ReadLine();


        var produto = new Produto() { Nome = nome, Preco = decimal.Parse(preco) };

        switch (categoria)
        {
            case "1":
                produto.Categoria = Categoria.Congelado;
                break;
            case "2":
                produto.Categoria = Categoria.Patreleira;
                break;
            case "3":
                produto.Categoria = Categoria.Enlatado;
                break;
            default:
                break;
        }

        Repositor.CriarProduto(produto);
        Stock.Init();
    }

    void AlterarStock()
    {
        System.Console.WriteLine("Digite o ID do produto qpara alterar a quantidade");
        var idProduto = Convert.ToInt32(Console.ReadLine());

        System.Console.WriteLine("Digite a quantidade");
        var quantidade = Convert.ToInt32(Console.ReadLine());

        Repositor.AlterarQuantidadeStock(idProduto, quantidade);
    }

    void ExibirListaFuncionarios()
    {
        foreach (var item in Funcionario.ObterListaFuncionarios())
        {
            System.Console.WriteLine(item.ToString());
        }
    }

    void ExibirListaProdutos()
    {
        foreach (var item in Produto.ObterListaProdutos())
        {
            System.Console.WriteLine(item.ToString());
        }
    }

    void ExibirListaItemStock()
    {
        foreach (var item in Stock.ObterListaItemStock())
        {
            System.Console.WriteLine(item.ToString());
        }
    }

    void NovoFuncionario()
    {
        var credenciais = ObterCredenciais(true);
        Funcionario func;

        switch (credenciais.cargo)
        {
            case Perfil.Gerente:
                func = new Gerente() { Login = credenciais.login, Nome = credenciais.login, Senha = credenciais.senha, Cargo = credenciais.cargo };
                break;
            case Perfil.Caixa:
                func = new Caixa() { Login = credenciais.login, Nome = credenciais.login, Senha = credenciais.senha, Cargo = credenciais.cargo };
                break;
            case Perfil.Repositor:
                func = new Repositor() { Login = credenciais.login, Nome = credenciais.login, Senha = credenciais.senha, Cargo = credenciais.cargo };
                break;
            default:
                func = new Funcionario() { Login = credenciais.login, Nome = credenciais.login, Senha = credenciais.senha, Cargo = credenciais.cargo };
                break;
        }

        Funcionario.CriarFuncionario(func);
    }

    Credenciais ObterCredenciais(bool ehCadastro)
    {
        System.Console.WriteLine("Insira o login");
        var _login = Console.ReadLine();

        System.Console.WriteLine("Insira a senha");
        var _senha = Console.ReadLine();

        var credencial = new Credenciais() { login = _login, senha = _senha };

        if (ehCadastro)
        {
            System.Console.WriteLine("Insira o perfil do funcionário");
            System.Console.WriteLine("1 - GERENTE");
            System.Console.WriteLine("2 - CAIXA");
            System.Console.WriteLine("3 - REPOSITOR");
            var perfil = Console.ReadLine();

            switch (perfil)
            {
                case "1":
                    credencial.cargo = Perfil.Gerente;
                    break;

                case "2":
                    credencial.cargo = Perfil.Caixa;
                    break;

                case "3":
                    credencial.cargo = Perfil.Repositor;
                    break;

                default:
                    break;
            }
        }

        return credencial;
    }

    struct Credenciais
    {
        public string login;
        public string senha;
        public Perfil cargo;
    }

}