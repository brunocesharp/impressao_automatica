using impressao_automatica.Enumeradores;

namespace impressao_automatica.Model
{
    public class Pedido
    {
        public long Codigo { get; set; }

        public string TextoImpressao { get; set; }
        public DateTime Data { get; set; }
        public SituacaoPedidoEnum Situacao { get; set; }
        public bool Enfileirado { get; set; } = false;


        public long CodigoFila { get; set; }
        public long Numero { get; set; }

        public override string ToString()
        {
            return $"Pedido {Codigo}    |   Data: {Data.ToLongDateString()} |   Impresso";
        }

        //metodo para verificar se esta pendente
        public bool EstaPendente()
        {
            return this.Situacao == SituacaoPedidoEnum.Pendente;
        }
    }
}
