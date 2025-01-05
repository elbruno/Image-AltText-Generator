namespace AltTextImageGeneratorWinForm
{
    public partial class formSettings : Form
    {
        public formSettings()
        {
            InitializeComponent();
        }

        private void formSettings_Load(object sender, EventArgs e)
        {
            chkUseOllama.Checked = Properties.Settings.Default.UseOllama;
            txtOllamaUrl.Text = Properties.Settings.Default.OllamaUrl;
            txtOllamaModel.Text = Properties.Settings.Default.OllamaModelId;

            chkUseOpenAI.Checked = Properties.Settings.Default.UseOpenAI;
            txtOpenAIKey.Text = Properties.Settings.Default.OpenAIKey;
            txtOpenAIModel.Text = Properties.Settings.Default.OpenAIModel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseOllama = chkUseOllama.Checked;
            Properties.Settings.Default.OllamaUrl = txtOllamaUrl.Text;
            Properties.Settings.Default.OllamaModelId = txtOllamaModel.Text;
            Properties.Settings.Default.UseOpenAI = chkUseOpenAI.Checked;
            Properties.Settings.Default.OpenAIKey = txtOpenAIKey.Text;
            Properties.Settings.Default.OpenAIModel = txtOpenAIModel.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Settings saved.");
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void chkUseOllama_CheckedChanged(object sender, EventArgs e)
        {
            chkUseOpenAI.Checked = !chkUseOllama.Checked;
        }

        private void chkUseOpenAI_CheckedChanged(object sender, EventArgs e)
        {
            chkUseOllama.Checked = !chkUseOpenAI.Checked;
        }
    }
}
