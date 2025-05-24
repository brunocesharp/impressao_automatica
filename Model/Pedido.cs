using impressao_automatica.Enumeradores;

namespace impressao_automatica.Model
{
    public class Pedido
    {
        public long CodigoFila { get; set; }
        public long Numero { get; set; }
        public SituacaoPedidoEnum Situacao { get; set; }
        public DateTime Data { get; set; }
        public bool Enfileirado { get; set; } = false;

        public override string ToString()
        {
            return $"Pedido {Numero}    |   Situação: {Situacao.ToString()} |   Data: {Data.ToLongDateString()}";
        }
    }
}
