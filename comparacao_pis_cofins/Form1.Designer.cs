namespace comparacao_pis_cofins
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label2 = new Label();
            txtCaminhoSped = new TextBox();
            button1 = new Button();
            btnComparar = new Button();
            txtCaminhoPlanilha = new TextBox();
            button2 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(36, 24);
            label2.Name = "label2";
            label2.Size = new Size(143, 20);
            label2.TabIndex = 2;
            label2.Text = "Caminho dos sped : ";
            // 
            // txtCaminhoSped
            // 
            txtCaminhoSped.BorderStyle = BorderStyle.FixedSingle;
            txtCaminhoSped.Location = new Point(185, 24);
            txtCaminhoSped.Name = "txtCaminhoSped";
            txtCaminhoSped.Size = new Size(425, 27);
            txtCaminhoSped.TabIndex = 3;
            // 
            // button1
            // 
            button1.Location = new Point(635, 22);
            button1.Name = "button1";
            button1.Size = new Size(40, 29);
            button1.TabIndex = 4;
            button1.Text = ". . .";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnComparar
            // 
            btnComparar.Location = new Point(212, 148);
            btnComparar.Name = "btnComparar";
            btnComparar.Size = new Size(302, 90);
            btnComparar.TabIndex = 5;
            btnComparar.Text = "Comparar";
            btnComparar.UseVisualStyleBackColor = true;
            btnComparar.Click += btnComparar_Click;
            // 
            // txtCaminhoPlanilha
            // 
            txtCaminhoPlanilha.BorderStyle = BorderStyle.FixedSingle;
            txtCaminhoPlanilha.Location = new Point(185, 81);
            txtCaminhoPlanilha.Name = "txtCaminhoPlanilha";
            txtCaminhoPlanilha.Size = new Size(425, 27);
            txtCaminhoPlanilha.TabIndex = 6;
            // 
            // button2
            // 
            button2.Location = new Point(635, 79);
            button2.Name = "button2";
            button2.Size = new Size(40, 29);
            button2.TabIndex = 7;
            button2.Text = ". . .";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 83);
            label1.Name = "label1";
            label1.Size = new Size(159, 20);
            label1.TabIndex = 8;
            label1.Text = "Salvar planilha como : ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(725, 264);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(txtCaminhoPlanilha);
            Controls.Add(btnComparar);
            Controls.Add(button1);
            Controls.Add(txtCaminhoSped);
            Controls.Add(label2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Comparação de Pis e Cofins";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private TextBox txtCaminhoSped;
        private Button button1;
        private Button btnComparar;
        private TextBox txtCaminhoPlanilha;
        private Button button2;
        private Label label1;
    }
}
