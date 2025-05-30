using impressao_automatica.Enumeradores;
using impressao_automatica.Model;
using System.Drawing.Printing;
using System.Management;

namespace impressao_automatica
{
    public class Impressora
    {
        public string Nome { get; set; }
        public SituacaoImpressoraEnum Situacao { get; set; }
        public Pedido? Pedido { get; private set; }

        public Impressora(string nome)
        {
            Nome = nome;
            Situacao = SituacaoImpressoraEnum.Desconhecido;
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

        public void Imprimir()
        {
            //se pedido for nulo, não imprime
            if (Pedido == null) return;

            //se pedido nao for nulo, imprime
            using (PrintDocument printDocument = new PrintDocument())
            {
                printDocument.PrinterSettings.PrinterName = Nome;

                printDocument.PrintPage += (sender, e) =>
                {
                    e.Graphics.DrawString(Pedido.ToString(), new Font("Arial", 12), Brushes.Black, new PointF(100, 100));
                };

                try
                {
                    printDocument.Print();
                }
                catch (Exception ex)
                {
                    // Tratar exceção de impressão
                    Console.WriteLine($"Erro ao imprimir: {ex.Message}");
                }
            }
        }

        public async Task Imprimir(Pedido pedido)
        {
            //se pedido for nulo, não imprime
            if (pedido == null) return;

            //se pedido nao for nulo, imprime
            using (PrintDocument printDocument = new PrintDocument())
            {
                printDocument.PrinterSettings.PrinterName = Nome;

                printDocument.PrintPage += (sender, e) =>
                {
                    e.Graphics.DrawString(pedido.TextoImpressao, new Font("Arial", 12), Brushes.Black, new PointF(100, 100));
                };

                try
                {
                    printDocument.Print();
                }
                catch (Exception ex)
                {
                    // Tratar exceção de impressão
                    Console.WriteLine($"Erro ao imprimir: {ex.Message}");
                }
            }
        }

        public void AtualizarSituacao()
        {
            var status = SituacaoImpressoraEnum.Desconhecido;

            try
            {
                string query = $"SELECT * FROM Win32_Printer WHERE Name = '{Nome.Replace("\\", "\\\\")}'";

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject printer in searcher.Get())
                    {
                        var printerStatus = printer["PrinterStatus"];

                        int statusCodigo = Convert.ToInt32(printer["PrinterStatus"]);


                        Situacao = (SituacaoImpressoraEnum)statusCodigo;
                    }
                }
            }
            catch (Exception ex)
            {
                status = SituacaoImpressoraEnum.Desconhecido;
            }
        }

        public void AdicionarPedido(Pedido pedido)
        {
            if (pedido == null) throw new ArgumentNullException(nameof(pedido), "O pedido não pode ser nulo.");
            Pedido = pedido;
            Situacao = SituacaoImpressoraEnum.Imprimindo;
        }

        //remover pedido   
        public void RemoverPedido()
        {
            Pedido = null;
            Situacao = SituacaoImpressoraEnum.Aguardando;
        }

        public void AlterarNomeImpressora(string novoNome)
        {
            if (string.IsNullOrEmpty(novoNome))
            {
                throw new ArgumentException("O nome da impressora não pode ser nulo ou vazio.", nameof(novoNome));
            }

            Nome = novoNome;
        }

        // Verifica se a impressora está pronta para imprimir
        public bool EstaProntaParaImprimir()
        {
            return Situacao == SituacaoImpressoraEnum.Aguardando;
        }

        // Verifica se a impressora está imprimindo
        public bool EstaImprimindo()
        {
            return Situacao == SituacaoImpressoraEnum.Imprimindo;
        }
    }
}
