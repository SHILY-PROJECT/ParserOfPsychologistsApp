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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForms));
            this.cityBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.parsePageToBox = new System.Windows.Forms.ComboBox();
            this.parsePageFromBox = new System.Windows.Forms.ComboBox();
            this.openResultsButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.clearToCityBoxButton = new System.Windows.Forms.Button();
            this.startParsingButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timeoutsBox = new System.Windows.Forms.MaskedTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.passInput = new System.Windows.Forms.TextBox();
            this.loginInput = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // citiesBox
            // 
            this.cityBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cityBox.ForeColor = System.Drawing.Color.Black;
            this.cityBox.FormattingEnabled = true;
            this.cityBox.Location = new System.Drawing.Point(6, 37);
            this.cityBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cityBox.Name = "citiesBox";
            this.cityBox.Size = new System.Drawing.Size(259, 23);
            this.cityBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.parsePageToBox);
            this.groupBox1.Controls.Add(this.parsePageFromBox);
            this.groupBox1.Controls.Add(this.openResultsButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.clearToCityBoxButton);
            this.groupBox1.Controls.Add(this.startParsingButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cityBox);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semilight", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox1.Location = new System.Drawing.Point(7, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(298, 188);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройки парсера";
            // 
            // parsePageToBox
            // 
            this.parsePageToBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parsePageToBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.parsePageToBox.FormattingEnabled = true;
            this.parsePageToBox.Location = new System.Drawing.Point(219, 70);
            this.parsePageToBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.parsePageToBox.Name = "parsePageToBox";
            this.parsePageToBox.Size = new System.Drawing.Size(75, 23);
            this.parsePageToBox.TabIndex = 3;
            // 
            // parsePageFromBox
            // 
            this.parsePageFromBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parsePageFromBox.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.parsePageFromBox.FormattingEnabled = true;
            this.parsePageFromBox.Location = new System.Drawing.Point(115, 70);
            this.parsePageFromBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.parsePageFromBox.Name = "parsePageFromBox";
            this.parsePageFromBox.Size = new System.Drawing.Size(75, 23);
            this.parsePageFromBox.TabIndex = 3;
            // 
            // openResultsButton
            // 
            this.openResultsButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("openResultsButton.BackgroundImage")));
            this.openResultsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.openResultsButton.ForeColor = System.Drawing.Color.Black;
            this.openResultsButton.Location = new System.Drawing.Point(249, 141);
            this.openResultsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.openResultsButton.Name = "openResultsButton";
            this.openResultsButton.Size = new System.Drawing.Size(45, 41);
            this.openResultsButton.TabIndex = 1;
            this.openResultsButton.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label5.Location = new System.Drawing.Point(194, 70);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 19);
            this.label5.TabIndex = 2;
            this.label5.Text = "—";
            // 
            // clearToCityBoxButton
            // 
            this.clearToCityBoxButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("clearToCityBoxButton.BackgroundImage")));
            this.clearToCityBoxButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.clearToCityBoxButton.Font = new System.Drawing.Font("Segoe UI Semilight", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.clearToCityBoxButton.ForeColor = System.Drawing.Color.Black;
            this.clearToCityBoxButton.Location = new System.Drawing.Point(268, 36);
            this.clearToCityBoxButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.clearToCityBoxButton.Name = "clearToCityBoxButton";
            this.clearToCityBoxButton.Size = new System.Drawing.Size(26, 25);
            this.clearToCityBoxButton.TabIndex = 1;
            this.clearToCityBoxButton.UseVisualStyleBackColor = true;
            // 
            // startParsingButton
            // 
            this.startParsingButton.Font = new System.Drawing.Font("Segoe UI Semilight", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.startParsingButton.ForeColor = System.Drawing.Color.Black;
            this.startParsingButton.Location = new System.Drawing.Point(6, 141);
            this.startParsingButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.startParsingButton.Name = "startParsingButton";
            this.startParsingButton.Size = new System.Drawing.Size(240, 41);
            this.startParsingButton.TabIndex = 1;
            this.startParsingButton.Text = "Начать парсинг";
            this.startParsingButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(6, 74);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Страницы парсить";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Город";
            // 
            // timeoutsBox
            // 
            this.timeoutsBox.Location = new System.Drawing.Point(6, 28);
            this.timeoutsBox.Mask = "000.0 сек - 000.0 сек - 000.0 сек";
            this.timeoutsBox.Name = "timeoutsBox";
            this.timeoutsBox.Size = new System.Drawing.Size(266, 25);
            this.timeoutsBox.TabIndex = 2;
            this.timeoutsBox.Text = "005000300020";
            this.timeoutsBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.timeoutsBox.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.passInput);
            this.groupBox2.Controls.Add(this.loginInput);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semilight", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox2.Location = new System.Drawing.Point(313, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(279, 108);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Аккаунт";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.checkBox1.Location = new System.Drawing.Point(59, 79);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(161, 23);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Подключить аккаунт";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // passInput
            // 
            this.passInput.ForeColor = System.Drawing.Color.Black;
            this.passInput.Location = new System.Drawing.Point(6, 51);
            this.passInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.passInput.Name = "passInput";
            this.passInput.PasswordChar = '*';
            this.passInput.PlaceholderText = "Введите пароль...";
            this.passInput.Size = new System.Drawing.Size(270, 25);
            this.passInput.TabIndex = 0;
            this.passInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // loginInput
            // 
            this.loginInput.ForeColor = System.Drawing.Color.Black;
            this.loginInput.Location = new System.Drawing.Point(6, 22);
            this.loginInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.loginInput.Name = "loginInput";
            this.loginInput.PlaceholderText = "Введите логин...";
            this.loginInput.Size = new System.Drawing.Size(270, 25);
            this.loginInput.TabIndex = 0;
            this.loginInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.timeoutsBox);
            this.groupBox4.Font = new System.Drawing.Font("Segoe UI Semilight", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.groupBox4.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox4.Location = new System.Drawing.Point(313, 119);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Size = new System.Drawing.Size(279, 74);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Настройки таймаутов между запросами";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(6, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(270, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "Главные страницы -> Пользователи -> Контакты";
            // 
            // MainForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(605, 201);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Segoe UI Semilight", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "MainForms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[WWW.B17.RU] PARSER OF PSYCHOLOGISTS by SHILY";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

    }

    private ComboBox cityBox;
    private GroupBox groupBox1;
    private Button startParsingButton;
    private ComboBox parsePageFromBox;
    private Label label2;
    private Label label1;
    private ComboBox parsePageToBox;
    private Label label5;
    private Button openResultsButton;
    private GroupBox groupBox2;
    private TextBox loginInput;
    private TextBox passInput;
    private CheckBox checkBox1;
    private GroupBox groupBox4;
    private Label label7;
    private MaskedTextBox timeoutsBox;
    private Button clearToCityBoxButton;
}