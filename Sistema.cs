using impressao_automatica.Model;
using System.Drawing.Printing;

namespace impressao_automatica
{
    public class Sistema
    {
        // Propriedades
        public bool Ativo { get; set; } = false;
        public PedidoApiService _pedidoApiService { get; set; }
        public IList<Pedido> _filaImpressao { get; set; } = new List<Pedido>();
        public Impressora Impressora { get; set; } = new Impressora();
        public DateTime Intervalo { get; set; }

        public Sistema()
        {
            _pedidoApiService = new PedidoApiService();
        }

        public async Task Iniciar()
        {
            this.Ativo = !this.Ativo;
        }

        public void Atulizar()
        {
            if (this.Ativo)
            {
                ListarPedidos();
            }
            else
            {
                // Lógica para parar o sistema
                // Isso pode incluir a limpeza de recursos, parada de timers, etc.
                LimparFila();
            }
        }

        public void Imprimir(Pedido pedido)
        {
            // Lógica para imprimir o pedido
            // Isso pode incluir a formatação do pedido e o envio para a impressora selecionada
            PrintDocument pd = new PrintDocument();

        }

        public void LimparFila()
        {
            // Lógica para limpar a fila de impressão
            // Isso pode incluir a remoção de todos os pedidos da fila ou a limpeza de um pedido específico
            _filaImpressao.Clear();
        }

        public async Task ListarPedidos()
        {
            var pedidos = await _pedidoApiService.ObterPedidosPendentesAsync();
            foreach (var pedido in pedidos.OrderBy(d => d.CodigoFila))
            {
                _filaImpressao.Add(pedido);
            }
        }

        private void AdicionarPedidoFila(Pedido pedido)
        {
            _filaImpressao.Add(pedido);
            //var json = JsonSerializer.Serialize(pedido);
            //File.AppendAllText(Application.StartupPath, json);
        }
    }
}
