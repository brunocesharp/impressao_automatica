namespace impressao_automatica
{
    public partial class PrintModal : Form
    {
        public long NumeroPedido { get; set; }
        public PrintModal()
        {
            InitializeComponent();
        }

        private void btnConfirma_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNumeroPedido.Text))
            {
                MessageBox.Show("Informe o número do pedido.");
            }
            else if (!long.TryParse(txtNumeroPedido.Text, out long numeroPedido))
            {
                MessageBox.Show("Número do pedido inválido.");
            }
            else if (numeroPedido <= 0)
            {
                MessageBox.Show("Número do pedido deve ser maior que zero.");
            }
            else
            {
                NumeroPedido = Convert.ToInt64(txtNumeroPedido.Text);
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
