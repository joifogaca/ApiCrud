namespace ApiCrud.Estudantes
{
    public class Estudante
    {
        public Estudante() { }
        public Estudante(string name)
        {
                Nome = name;
                Id = Guid.NewGuid();
                Ativo = true;

        }
        public Guid Id { get; init; } // init => depois de atribuido o valor não poderemos alterar o valor
        public string Nome { get; private set; }

        public bool Ativo { get; private set; }

        public void AtualizarNome(string nome) {
            Nome = nome;
        }

        public void Desativar() {
        Ativo=false; // Não terá como ativar
        }

    }
}
