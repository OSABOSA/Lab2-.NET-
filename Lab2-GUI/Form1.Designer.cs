namespace Lab2_GUI
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
            baseCurrencyComboBox = new ComboBox();
            label1 = new Label();
            currenciesLoadedLabel = new Label();
            chosenCurrency = new Label();
            comboBoxNewCurrency = new ComboBox();
            newCurrency = new Label();
            baseAmount = new NumericUpDown();
            newAmount = new TextBox();
            allExchangeRatesTextBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)baseAmount).BeginInit();
            SuspendLayout();
            // 
            // baseCurrencyComboBox
            // 
            baseCurrencyComboBox.FormattingEnabled = true;
            baseCurrencyComboBox.Location = new Point(60, 52);
            baseCurrencyComboBox.Name = "baseCurrencyComboBox";
            baseCurrencyComboBox.Size = new Size(207, 23);
            baseCurrencyComboBox.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 24);
            label1.Name = "label1";
            label1.Size = new Size(119, 15);
            label1.TabIndex = 5;
            label1.Text = "Chose base currency:";
            // 
            // currenciesLoadedLabel
            // 
            currenciesLoadedLabel.AutoSize = true;
            currenciesLoadedLabel.Location = new Point(193, 24);
            currenciesLoadedLabel.Name = "currenciesLoadedLabel";
            currenciesLoadedLabel.Size = new Size(0, 15);
            currenciesLoadedLabel.TabIndex = 6;
            // 
            // chosenCurrency
            // 
            chosenCurrency.AutoSize = true;
            chosenCurrency.Location = new Point(273, 104);
            chosenCurrency.Name = "chosenCurrency";
            chosenCurrency.Size = new Size(93, 15);
            chosenCurrency.TabIndex = 8;
            chosenCurrency.Text = "chosenCurrency";
            // 
            // comboBoxNewCurrency
            // 
            comboBoxNewCurrency.FormattingEnabled = true;
            comboBoxNewCurrency.Location = new Point(60, 133);
            comboBoxNewCurrency.Name = "comboBoxNewCurrency";
            comboBoxNewCurrency.Size = new Size(207, 23);
            comboBoxNewCurrency.TabIndex = 9;
            // 
            // newCurrency
            // 
            newCurrency.AutoSize = true;
            newCurrency.Location = new Point(60, 104);
            newCurrency.Name = "newCurrency";
            newCurrency.Size = new Size(117, 15);
            newCurrency.TabIndex = 10;
            newCurrency.Text = "Chose new currency:";
            // 
            // baseAmount
            // 
            baseAmount.Location = new Point(273, 52);
            baseAmount.Name = "baseAmount";
            baseAmount.Size = new Size(120, 23);
            baseAmount.TabIndex = 13;
            // 
            // newAmount
            // 
            newAmount.Location = new Point(273, 133);
            newAmount.Name = "newAmount";
            newAmount.ReadOnly = true;
            newAmount.Size = new Size(120, 23);
            newAmount.TabIndex = 15;
            // 
            // allExchangeRatesTextBox
            // 
            allExchangeRatesTextBox.Location = new Point(399, 35);
            allExchangeRatesTextBox.Multiline = true;
            allExchangeRatesTextBox.Name = "allExchangeRatesTextBox";
            allExchangeRatesTextBox.Size = new Size(192, 247);
            allExchangeRatesTextBox.TabIndex = 16;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(618, 339);
            Controls.Add(allExchangeRatesTextBox);
            Controls.Add(newAmount);
            Controls.Add(baseAmount);
            Controls.Add(newCurrency);
            Controls.Add(comboBoxNewCurrency);
            Controls.Add(chosenCurrency);
            Controls.Add(currenciesLoadedLabel);
            Controls.Add(label1);
            Controls.Add(baseCurrencyComboBox);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)baseAmount).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ComboBox baseCurrencyComboBox;
        private Label label1;
        private Label currenciesLoadedLabel;
        private Label chosenCurrency;
        private ComboBox comboBoxNewCurrency;
        private Label newCurrency;
        private NumericUpDown baseAmount;
        private TextBox newAmount;
        private TextBox allExchangeRatesTextBox;
    }
}
