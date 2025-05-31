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
                    e.Graphics.DrawString(Pedido.TextoImpressao, new Font("Arial", 12), Brushes.Black, new PointF(100, 100));
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
                    //MessageBox de erro com a mensagem "Ocorreu um erro ao imprimir: {ex.Message}"
                    MessageBox.Show($"Ocorreu um erro ao imprimir: {ex.Message}", "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
