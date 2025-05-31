namespace impressao_automatica
{
    public partial class Main : Form
    {
        //propriedades
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Name = "Servi�o de Automatiza��o";

            var impressoras = Impressora.Listar();
            comboBoxImpressoras.Items.AddRange(impressoras.ToArray());

            this._sistema = new Sistema();

            comboBoxImpressoras.SelectedItem = _sistema.ObterImpressora()?.Nome;
        }
    }
}
