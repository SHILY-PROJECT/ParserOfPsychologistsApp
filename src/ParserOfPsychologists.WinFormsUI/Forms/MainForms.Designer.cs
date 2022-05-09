namespace ParserOfPsychologists.WinFormsUI;

partial class MainForms
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.citiesBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.parsePageToBox = new System.Windows.Forms.ComboBox();
            this.parsePageFromBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // citiesBox
            // 
            this.citiesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.citiesBox.FormattingEnabled = true;
            this.citiesBox.Location = new System.Drawing.Point(6, 37);
            this.citiesBox.Name = "citiesBox";
            this.citiesBox.Size = new System.Drawing.Size(256, 23);
            this.citiesBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.parsePageToBox);
            this.groupBox1.Controls.Add(this.parsePageFromBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.citiesBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 355);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Поиск";
            // 
            // parsePageToBox
            // 
            this.parsePageToBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parsePageToBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parsePageToBox.FormattingEnabled = true;
            this.parsePageToBox.Location = new System.Drawing.Point(152, 81);
            this.parsePageToBox.Name = "parsePageToBox";
            this.parsePageToBox.Size = new System.Drawing.Size(110, 23);
            this.parsePageToBox.TabIndex = 3;
            // 
            // parsePageFromBox
            // 
            this.parsePageFromBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parsePageFromBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parsePageFromBox.FormattingEnabled = true;
            this.parsePageFromBox.Location = new System.Drawing.Point(6, 81);
            this.parsePageFromBox.Name = "parsePageFromBox";
            this.parsePageFromBox.Size = new System.Drawing.Size(110, 23);
            this.parsePageFromBox.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(126, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "—";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Номера страниц парсить (с - по)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Город";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(6, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(256, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Парсить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(613, 370);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[WWW.B17.RU] PARSER OF PSYCHOLOGISTS";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

    }

    private ComboBox citiesBox;
    private GroupBox groupBox1;
    private Button button1;
    private ComboBox parsePageFromBox;
    private Label label2;
    private Label label1;
    private ComboBox parsePageToBox;
    private Label label5;
}