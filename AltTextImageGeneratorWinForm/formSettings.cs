namespace AltTextImageGeneratorWinForm
{
    public partial class formSettings : Form
    {
        public formSettings()
        {
            InitializeComponent();

            // Add event handlers for the checkboxes
            chkUseOllama.CheckedChanged += chkUseOllama_CheckedChanged;
            chkUseOpenAI.CheckedChanged += chkUseOpenAI_CheckedChanged;
            chkLocalOnnxModel.CheckedChanged += chkLocalOnnxModel_CheckedChanged;
        }

        private void formSettings_Load(object sender, EventArgs e)
        {
            chkUseOllama.Checked = Properties.Settings.Default.UseOllama;
            txtOllamaUrl.Text = Properties.Settings.Default.OllamaUrl;
            txtOllamaModel.Text = Properties.Settings.Default.OllamaModelId;

            chkUseOpenAI.Checked = Properties.Settings.Default.UseOpenAI;
            txtOpenAIKey.Text = Properties.Settings.Default.OpenAIKey;
            txtOpenAIModel.Text = Properties.Settings.Default.OpenAIModel;

            chkLocalOnnxModel.Checked = Properties.Settings.Default.UseLocalOnnxModel;
            txtPhi4ModelPath.Text = Properties.Settings.Default.LocalOnnxModelPath;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseOllama = chkUseOllama.Checked;
            Properties.Settings.Default.OllamaUrl = txtOllamaUrl.Text;
            Properties.Settings.Default.OllamaModelId = txtOllamaModel.Text;
            Properties.Settings.Default.UseOpenAI = chkUseOpenAI.Checked;
            Properties.Settings.Default.OpenAIKey = txtOpenAIKey.Text;
            Properties.Settings.Default.OpenAIModel = txtOpenAIModel.Text;
            Properties.Settings.Default.UseLocalOnnxModel = chkLocalOnnxModel.Checked;
            Properties.Settings.Default.LocalOnnxModelPath = txtPhi4ModelPath.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Settings saved.");
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void btnSelectOnnxFolder_Click(object sender, EventArgs e)
        {
            //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    txtPhi4ModelPath.Text = folderBrowserDialog1.SelectedPath;
            //}
        }

        private void chkUseOllama_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseOllama.Checked)
            {
                chkUseOpenAI.Checked = false;
                chkLocalOnnxModel.Checked = false;
            }
        }

        private void chkUseOpenAI_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseOpenAI.Checked)
            {
                chkUseOllama.Checked = false;
                chkLocalOnnxModel.Checked = false;
            }
        }

        private void chkLocalOnnxModel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLocalOnnxModel.Checked)
            {
                chkUseOllama.Checked = false;
                chkUseOpenAI.Checked = false;
            }
        }

    }
}
