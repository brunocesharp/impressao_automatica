using impressao_automatica.Enumeradores;
using impressao_automatica.Model;
using System.Text.Json;

namespace impressao_automatica
{
    public class PedidoApiService
    {
        private readonly HttpClient _httpClient;

        public PedidoApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.envianozap.com.br/api/");
            //POST api/v1/commercial/sale/status?sale={id}&status=working
            _httpClient.DefaultRequestHeaders.Add("branch", "1");
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosPendentesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("v2/commercial/sale/print/internal/available");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return ConverterLista(json);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Enumerable.Empty<Pedido>();
        }

        public async Task<bool> AlterarStatusPedidoAsync(string idPedido)
        {
            //available/{id}/status?status=working
            try
            {
                var response = await _httpClient.PostAsync($"v1/commercial/sale/status?sale={idPedido}&status=working", null);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                    // Sucesso ao alterar o status do pedido
                }
                else
                {
                    return false;
                    // Tratar erro ao alterar o status do pedido
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //obter pedido por codigo async
        public async Task<Pedido?> ObterPedidoPorCodigoAsync(string codigo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"v2/commercial/sale/print/internal/{codigo}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    //converter para pedido
                    var objetoResponse = JsonSerializer.Deserialize<object>(json);
                    //converter object to Pedido
                    if (objetoResponse is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
                    {
                        var pedido = new Pedido
                        {
                            Codigo = long.Parse(jsonElement.GetProperty("id").ToString()!),
                            TextoImpressao = jsonElement.GetProperty("text").ToString()!,
                            Data = jsonElement.GetProperty("operationDate").GetDateTime(),
                        };

                        return pedido;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        private IEnumerable<Pedido> ConverterLista(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            foreach (JsonElement pedido in doc.RootElement.EnumerateArray())
            {
                var id = pedido.GetProperty("id").ToString();
                var texto = pedido.GetProperty("text").ToString();
                var operationDate = pedido.GetProperty("operationDate").GetDateTime();


                yield return new Pedido
                {
                    Codigo = long.Parse(id!),
                    TextoImpressao = texto!,
                    Data = operationDate,
                };
            }
        }
    }
}
