using impressao_automatica.Enumeradores;
using impressao_automatica.Model;
using System.Drawing.Printing;

namespace impressao_automatica
{
    public class Sistema
    {
        // Propriedades
        public SituacaoSistemaEnum Situacao { get; set; }
        private Impressora Impressora { get; set; } = new Impressora();
        public PedidoApiService _pedidoApiService { get; set; }

        public Sistema()
        {
            _pedidoApiService = new PedidoApiService();
            Situacao = SituacaoSistemaEnum.Parado;
        }

        //Iniciar ou parar 
        public void IniciarOuParar()
        {
            if (!EstaAtivo())
            {
                this.Situacao = SituacaoSistemaEnum.Iniciado;
            }
            else
            {
                this.Situacao = SituacaoSistemaEnum.Parado;
            }
        }

        //verifica se esta iniciado
        public bool EstaAtivo()
        {
            return this.Situacao == SituacaoSistemaEnum.Iniciado;
        }


        //Definir impressora
        public void DefinirImpressora(string nomeImpressora)
        {
            if (string.IsNullOrEmpty(nomeImpressora))
            {
                throw new ArgumentException("O nome da impressora não pode ser nulo ou vazio.", nameof(nomeImpressora));
            }

            this.Impressora.Nome = nomeImpressora;
        }
        public Impressora ObterImpressora()
        {
            return this.Impressora;
        }

        public async Task<string> ImprimirProximoPedidoAsync()
        {
            Impressora.Situacao = SituacaoImpressoraEnum.Imprimindo;

            var pedidos = await _pedidoApiService.ObterPedidosPendentesAsync();
            var proximoPedido = pedidos.OrderByDescending(p => p.Data).FirstOrDefault();

            if (proximoPedido == null)
            {
                Impressora.Situacao = SituacaoImpressoraEnum.Aguardando;
                return "Aguardado pedido...";
            };

            await Impressora.Imprimir(proximoPedido);

            await _pedidoApiService.AlterarStatusPedidoAsync(proximoPedido.Codigo.ToString());

            Impressora.Situacao = SituacaoImpressoraEnum.Aguardando;

            return proximoPedido.ToString();

        }

        public async Task<string> Imprimir(string codigo)
        {
            //get pedido by codigo
            var pedido = await _pedidoApiService.ObterPedidoPorCodigoAsync(codigo);
            if (pedido == null)
            {
                MessageBox.Show($"Pedido com código {codigo} não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return $"Pedido com código {codigo} não encontrado.";
            }
            await Impressora.Imprimir(pedido!);

            return pedido.ToString();
        }

        public async Task Imprimir(Pedido pedido)
        {
            if (pedido == null)
            {
                MessageBox.Show("Ocorreu um erro ao imprimir o pedido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            await Impressora.Imprimir(pedido!);

        }
    }
}
