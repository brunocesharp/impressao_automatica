using impressao_automatica.Enumeradores;
using impressao_automatica.Model;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace impressao_automatica
{
    public class PedidoApiService
    {
        private readonly HttpClient _httpClient;
        private long _codigoFila = 0;

        public PedidoApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.envianozap.com.br/api/v1/commercial/sale?branch=1&search=pending&page=1&size=1");
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosPendentesAsync()
        {
            try
            {
                return await ListarPedidoFake();
                var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return Converter(json);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Enumerable.Empty<Pedido>();
        }

        private async Task<IEnumerable<Pedido>> ListarPedidoFake()
        {
            var listaFake = new List<Pedido>();
            for (int i = 0; i < 10; i++)
            {
                _codigoFila++;
                listaFake.Add(new Pedido
                {
                    CodigoFila = _codigoFila,
                    Numero = 123456789 + i,
                    Situacao = SituacaoPedidoEnum.Pendente,
                    Data = DateTime.Now,
                    Enfileirado = false
                });
            }

            return listaFake;
            var json = JsonSerializer.Serialize(listaFake);
            return Converter(json);
        }

        private IEnumerable<Pedido> Converter(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            foreach (JsonElement pedido in doc.RootElement.EnumerateArray())
            {
                _codigoFila++;
                string codigoFila = pedido.GetProperty("CodigoFila").GetString();
                string numero = pedido.GetProperty("Numero").GetString();
                string situacao = pedido.GetProperty("Situacao").GetString();
                DateTime data = pedido.GetProperty("Data").GetDateTime();
                bool enfileirado = pedido.GetProperty("Enfileirado").GetBoolean();

                yield return new Pedido
                {
                    CodigoFila = _codigoFila,
                    Numero = long.Parse(numero),
                    Situacao = (SituacaoPedidoEnum)Enum.Parse(typeof(SituacaoPedidoEnum), situacao),
                    Data = data,
                    Enfileirado = false
                };
            }
        }
    }
}
