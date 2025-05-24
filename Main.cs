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
            this.Name = "Serviço de Automatização";

            var impressoras = Impressora.Listar();
            foreach (var impressora in impressoras)
            {
                comboBoxImpressoras.Items.Add(impressora);
            }

            comboBoxImpressoras.SelectedItem = _sistema.Impressora.Nome;
        }
    }
}
