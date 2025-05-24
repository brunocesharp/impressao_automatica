using impressao_automatica.Enumeradores;
using System.Drawing.Printing;
using System.Management;

namespace impressao_automatica
{
    public class Impressora
    {
        public string Nome { get; set; }

        public Impressora(string nome)
        {
            Nome = nome;
        }

        public Impressora()
        {
            Nome = ImpressoraPadrao();
        }

        public static IEnumerable<string> Listar()
        {
            var impressoras = new List<string>();
            foreach (var impressora in PrinterSettings.InstalledPrinters)
            {
                impressoras.Add(item: impressora.ToString());
            }
            return impressoras;
        }

        public string ImpressoraPadrao()
        {
            var impressora = new PrinterSettings();
            return impressora.PrinterName;
        }

        public void Imprimir(string pedido, string impressora)
        {
            // Lógica para imprimir o pedido
            // Isso pode incluir a formatação do pedido e o envio para a impressora selecionada
        }

        public SituacaoImpressoraEnum ObterStatusImpressora(string nomeImpressora)
        {
            var status = SituacaoImpressoraEnum.Desconhecido;

            try
            {
                string query = $"SELECT * FROM Win32_Printer WHERE Name = '{nomeImpressora.Replace("\\", "\\\\")}'";

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject printer in searcher.Get())
                    {
                        var printerStatus = printer["PrinterStatus"];

                        // Pode usar PrinterStatus ou ExtendedPrinterStatus para mais detalhes
                        int statusCodigo = Convert.ToInt32(printer["PrinterStatus"]);

                        
                        status = (SituacaoImpressoraEnum)statusCodigo;
                    }
                }
            }
            catch (Exception ex)
            {
                status = SituacaoImpressoraEnum.Desconhecido;
            }

            return status;
        }
    }
}
