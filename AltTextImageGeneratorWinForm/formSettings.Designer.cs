namespace AltTextImageGeneratorWinForm
{
    partial class formSettings
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            chkUseOllama = new CheckBox();
            lblOllamaUrl = new Label();
            txtOllamaUrl = new TextBox();
            panelOllama = new Panel();
            txtOllamaModel = new TextBox();
            label1 = new Label();
            panel1 = new Panel();
            txtOpenAIModel = new TextBox();
            lblOpenAIModel = new Label();
            txtOpenAIKey = new TextBox();
            lblOpenAIKey = new Label();
            chkUseOpenAI = new CheckBox();
            btnSave = new Button();
            btnClose = new Button();
            panel2 = new Panel();
            btnSelectOnnxFolder = new Button();
            txtPhi4ModelPath = new TextBox();
            lblPhi4ModelPath = new Label();
            chkLocalOnnxModel = new CheckBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            panelOllama.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // chkUseOllama
            // 
            chkUseOllama.AutoSize = true;
            chkUseOllama.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkUseOllama.Location = new Point(26, 12);
            chkUseOllama.Name = "chkUseOllama";
            chkUseOllama.Size = new Size(110, 24);
            chkUseOllama.TabIndex = 0;
            chkUseOllama.Text = "Use Ollama";
            chkUseOllama.UseVisualStyleBackColor = true;
            // 
            // lblOllamaUrl
            // 
            lblOllamaUrl.AutoSize = true;
            lblOllamaUrl.Location = new Point(13, 17);
            lblOllamaUrl.Name = "lblOllamaUrl";
            lblOllamaUrl.Size = new Size(87, 20);
            lblOllamaUrl.TabIndex = 1;
            lblOllamaUrl.Text = "Ollama URL";
            // 
            // txtOllamaUrl
            // 
            txtOllamaUrl.Location = new Point(130, 14);
            txtOllamaUrl.Name = "txtOllamaUrl";
            txtOllamaUrl.Size = new Size(623, 27);
            txtOllamaUrl.TabIndex = 2;
            // 
            // panelOllama
            // 
            panelOllama.Controls.Add(txtOllamaModel);
            panelOllama.Controls.Add(label1);
            panelOllama.Controls.Add(txtOllamaUrl);
            panelOllama.Controls.Add(lblOllamaUrl);
            panelOllama.Location = new Point(12, 42);
            panelOllama.Name = "panelOllama";
            panelOllama.Size = new Size(770, 104);
            panelOllama.TabIndex = 3;
            // 
            // txtOllamaModel
            // 
            txtOllamaModel.Location = new Point(131, 57);
            txtOllamaModel.Name = "txtOllamaModel";
            txtOllamaModel.Size = new Size(623, 27);
            txtOllamaModel.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 60);
            label1.Name = "label1";
            label1.Size = new Size(104, 20);
            label1.TabIndex = 3;
            label1.Text = "Ollama Model";
            // 
            // panel1
            // 
            panel1.Controls.Add(txtOpenAIModel);
            panel1.Controls.Add(lblOpenAIModel);
            panel1.Controls.Add(txtOpenAIKey);
            panel1.Controls.Add(lblOpenAIKey);
            panel1.Location = new Point(11, 193);
            panel1.Name = "panel1";
            panel1.Size = new Size(770, 104);
            panel1.TabIndex = 5;
            // 
            // txtOpenAIModel
            // 
            txtOpenAIModel.Location = new Point(131, 57);
            txtOpenAIModel.Name = "txtOpenAIModel";
            txtOpenAIModel.Size = new Size(623, 27);
            txtOpenAIModel.TabIndex = 4;
            // 
            // lblOpenAIModel
            // 
            lblOpenAIModel.AutoSize = true;
            lblOpenAIModel.Location = new Point(14, 60);
            lblOpenAIModel.Name = "lblOpenAIModel";
            lblOpenAIModel.Size = new Size(106, 20);
            lblOpenAIModel.TabIndex = 3;
            lblOpenAIModel.Text = "OpenAI Model";
            // 
            // txtOpenAIKey
            // 
            txtOpenAIKey.Location = new Point(130, 14);
            txtOpenAIKey.Name = "txtOpenAIKey";
            txtOpenAIKey.PasswordChar = '*';
            txtOpenAIKey.Size = new Size(623, 27);
            txtOpenAIKey.TabIndex = 2;
            // 
            // lblOpenAIKey
            // 
            lblOpenAIKey.AutoSize = true;
            lblOpenAIKey.Location = new Point(13, 17);
            lblOpenAIKey.Name = "lblOpenAIKey";
            lblOpenAIKey.Size = new Size(87, 20);
            lblOpenAIKey.TabIndex = 1;
            lblOpenAIKey.Text = "OpenAI Key";
            // 
            // chkUseOpenAI
            // 
            chkUseOpenAI.AutoSize = true;
            chkUseOpenAI.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkUseOpenAI.Location = new Point(25, 163);
            chkUseOpenAI.Name = "chkUseOpenAI";
            chkUseOpenAI.Size = new Size(150, 24);
            chkUseOpenAI.TabIndex = 4;
            chkUseOpenAI.Text = "Use OpenAI APIs";
            chkUseOpenAI.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(575, 462);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 6;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(685, 462);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(94, 29);
            btnClose.TabIndex = 7;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnSelectOnnxFolder);
            panel2.Controls.Add(txtPhi4ModelPath);
            panel2.Controls.Add(lblPhi4ModelPath);
            panel2.Location = new Point(12, 342);
            panel2.Name = "panel2";
            panel2.Size = new Size(770, 104);
            panel2.TabIndex = 9;
            // 
            // btnSelectOnnxFolder
            // 
            btnSelectOnnxFolder.Location = new Point(658, 49);
            btnSelectOnnxFolder.Name = "btnSelectOnnxFolder";
            btnSelectOnnxFolder.Size = new Size(94, 29);
            btnSelectOnnxFolder.TabIndex = 7;
            btnSelectOnnxFolder.Text = "...";
            btnSelectOnnxFolder.UseVisualStyleBackColor = true;
            btnSelectOnnxFolder.Click += btnSelectOnnxFolder_Click;
            // 
            // txtPhi4ModelPath
            // 
            txtPhi4ModelPath.Location = new Point(14, 49);
            txtPhi4ModelPath.Name = "txtPhi4ModelPath";
            txtPhi4ModelPath.Size = new Size(638, 27);
            txtPhi4ModelPath.TabIndex = 2;
            // 
            // lblPhi4ModelPath
            // 
            lblPhi4ModelPath.AutoSize = true;
            lblPhi4ModelPath.Location = new Point(13, 17);
            lblPhi4ModelPath.Name = "lblPhi4ModelPath";
            lblPhi4ModelPath.Size = new Size(249, 20);
            lblPhi4ModelPath.TabIndex = 1;
            lblPhi4ModelPath.Text = "Phi-4 Multimodal model folder path";
            // 
            // chkLocalOnnxModel
            // 
            chkLocalOnnxModel.AutoSize = true;
            chkLocalOnnxModel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            chkLocalOnnxModel.Location = new Point(26, 312);
            chkLocalOnnxModel.Name = "chkLocalOnnxModel";
            chkLocalOnnxModel.Size = new Size(139, 24);
            chkLocalOnnxModel.TabIndex = 8;
            chkLocalOnnxModel.Text = "Use Phi-4 Onnx";
            chkLocalOnnxModel.UseVisualStyleBackColor = true;
            // 
            // formSettings
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 503);
            Controls.Add(panel2);
            Controls.Add(chkLocalOnnxModel);
            Controls.Add(btnClose);
            Controls.Add(btnSave);
            Controls.Add(panel1);
            Controls.Add(chkUseOpenAI);
            Controls.Add(panelOllama);
            Controls.Add(chkUseOllama);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "formSettings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Alt Text Generator Settings";
            Load += formSettings_Load;
            panelOllama.ResumeLayout(false);
            panelOllama.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox chkUseOllama;
        private Label lblOllamaUrl;
        private TextBox txtOllamaUrl;
        private Panel panelOllama;
        private TextBox txtOllamaModel;
        private Label label1;
        private Panel panel1;
        private TextBox txtOpenAIModel;
        private Label lblOpenAIModel;
        private TextBox txtOpenAIKey;
        private Label lblOpenAIKey;
        private CheckBox chkUseOpenAI;
        private Button btnSave;
        private Button btnClose;
        private Panel panel2;
        private TextBox txtPhi4ModelPath;
        private Label lblPhi4ModelPath;
        private CheckBox chkLocalOnnxModel;
        private Button btnSelectOnnxFolder;
        private FolderBrowserDialog folderBrowserDialog1;
    }
}
