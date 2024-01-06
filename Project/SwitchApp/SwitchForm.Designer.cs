namespace SwitchApp
{
    partial class SwitchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOn = new System.Windows.Forms.Button();
            this.buttonOff = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxContainers = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDataName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonOn
            // 
            this.buttonOn.ForeColor = System.Drawing.Color.LimeGreen;
            this.buttonOn.Location = new System.Drawing.Point(12, 74);
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.Size = new System.Drawing.Size(87, 80);
            this.buttonOn.TabIndex = 4;
            this.buttonOn.Text = "Send \"ON\"";
            this.buttonOn.UseVisualStyleBackColor = true;
            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
            // 
            // buttonOff
            // 
            this.buttonOff.ForeColor = System.Drawing.Color.Red;
            this.buttonOff.Location = new System.Drawing.Point(105, 74);
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.Size = new System.Drawing.Size(87, 80);
            this.buttonOff.TabIndex = 5;
            this.buttonOff.Text = "Send \"OFF\"";
            this.buttonOff.UseVisualStyleBackColor = true;
            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Container";
            // 
            // comboBoxContainers
            // 
            this.comboBoxContainers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxContainers.FormattingEnabled = true;
            this.comboBoxContainers.Location = new System.Drawing.Point(70, 12);
            this.comboBoxContainers.Name = "comboBoxContainers";
            this.comboBoxContainers.Size = new System.Drawing.Size(122, 21);
            this.comboBoxContainers.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Data name";
            // 
            // textBoxDataName
            // 
            this.textBoxDataName.Location = new System.Drawing.Point(77, 43);
            this.textBoxDataName.Name = "textBoxDataName";
            this.textBoxDataName.Size = new System.Drawing.Size(115, 20);
            this.textBoxDataName.TabIndex = 9;
            this.textBoxDataName.Text = "Status";
            // 
            // SwitchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 166);
            this.Controls.Add(this.textBoxDataName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxContainers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOff);
            this.Controls.Add(this.buttonOn);
            this.Name = "SwitchForm";
            this.Text = "Switch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonOn;
        private System.Windows.Forms.Button buttonOff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxContainers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDataName;
    }
}

