using impressao_automatica.Enumeradores;
using impressao_automatica.Model;
using System.Drawing.Printing;

namespace impressao_automatica
{
    public class Sistema
    {
        // Propriedades
        public SituacaoSistemaEnum Situacao { get; set; }
        public Impressora Impressora { get; set; } = new Impressora();
        public bool Ativo { get; set; } = false;
        public PedidoApiService _pedidoApiService { get; set; }
        public IList<Pedido> _filaImpressao { get; set; } = new List<Pedido>();
        public IList<Pedido> listaRetornoAPIFake { get; set; } = new List<Pedido>();

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
                LimparFila();
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

        //Imprimir proximo pedido
        public async Task ImprimirProximoPedidoAsync()
        {
            if (_filaImpressao.Any())
            {
                var pedido = _filaImpressao.OrderByDescending(d => d.Codigo).FirstOrDefault();
                if (pedido != null)
                {
                    Impressora.AdicionarPedido(pedido);
                    var atualizado = await _pedidoApiService.AlterarStatusPedidoAsync(pedido.Codigo.ToString());
                    if (atualizado)
                        _filaImpressao.Remove(pedido);

                }
            }
        }

        public async Task ListarPedidos()
        {
            var pedidos = await _pedidoApiService.ObterPedidosPendentesAsync();
            //filtrar os pedidos com os pedidos na _filaImpressao
            pedidos = pedidos.Where(p => !_filaImpressao.Any(f => f.Codigo == p.Codigo)).ToList();

            if (!pedidos.Any()) return;

            foreach (var pedido in pedidos.OrderBy(d => d.CodigoFila))
            {
                _filaImpressao.Add(pedido);
            }
        }

        public async Task Imprimir(string codigo)
        {
            //get pedido by codigo
            var pedido = await _pedidoApiService.ObterPedidoPorCodigoAsync(codigo);
            await Impressora.Imprimir(pedido);

        }

        public void LimparFila()
        {
            // Lógica para limpar a fila de impressão
            // Isso pode incluir a remoção de todos os pedidos da fila ou a limpeza de um pedido específico
            _filaImpressao.Clear();
        }

        private void AdicionarPedidoFila(Pedido pedido)
        {
            _filaImpressao.Add(pedido);
            //var json = JsonSerializer.Serialize(pedido);
            //File.AppendAllText(Application.StartupPath, json);
        }


    }
}
