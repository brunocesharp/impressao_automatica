using impressao_automatica.Enumeradores;

namespace impressao_automatica.Model
{
    public class Pedido
    {
        public long Codigo { get; set; }

        public string? TextoImpressao { get; set; }
        public DateTime Data { get; set; }

        public override string ToString()
        {
            return $"Pedido {Codigo}    |   Data: {Data.ToString("dd/MM/yyyy HH:mm")} |   Impresso";
        }
    }
}
