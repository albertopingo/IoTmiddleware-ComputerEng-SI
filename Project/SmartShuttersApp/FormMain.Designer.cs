namespace SmartShuttersApp
{
    partial class FormMain
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
            this.buttonCreateContainer = new System.Windows.Forms.Button();
            this.buttonCreateSubscription = new System.Windows.Forms.Button();
            this.buttonSubscribe = new System.Windows.Forms.Button();
            this.textBoxContainerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxEndPoint = new System.Windows.Forms.TextBox();
            this.comboBoxEvent = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSubscriptionName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonCreateContainer
            // 
            this.buttonCreateContainer.Location = new System.Drawing.Point(768, 42);
            this.buttonCreateContainer.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCreateContainer.Name = "buttonCreateContainer";
            this.buttonCreateContainer.Size = new System.Drawing.Size(251, 39);
            this.buttonCreateContainer.TabIndex = 1;
            this.buttonCreateContainer.Text = "Create Container";
            this.buttonCreateContainer.UseVisualStyleBackColor = true;
            this.buttonCreateContainer.Click += new System.EventHandler(this.buttonCreateContainer_Click);
            // 
            // buttonCreateSubscription
            // 
            this.buttonCreateSubscription.Location = new System.Drawing.Point(768, 266);
            this.buttonCreateSubscription.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCreateSubscription.Name = "buttonCreateSubscription";
            this.buttonCreateSubscription.Size = new System.Drawing.Size(251, 39);
            this.buttonCreateSubscription.TabIndex = 2;
            this.buttonCreateSubscription.Text = "Create Subscription";
            this.buttonCreateSubscription.UseVisualStyleBackColor = true;
            this.buttonCreateSubscription.Click += new System.EventHandler(this.buttonCreateSubscription_Click);
            // 
            // buttonSubscribe
            // 
            this.buttonSubscribe.Location = new System.Drawing.Point(768, 364);
            this.buttonSubscribe.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSubscribe.Name = "buttonSubscribe";
            this.buttonSubscribe.Size = new System.Drawing.Size(251, 39);
            this.buttonSubscribe.TabIndex = 3;
            this.buttonSubscribe.Text = "Subscribe";
            this.buttonSubscribe.UseVisualStyleBackColor = true;
            // 
            // textBoxContainerName
            // 
            this.textBoxContainerName.Location = new System.Drawing.Point(456, 54);
            this.textBoxContainerName.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxContainerName.Name = "textBoxContainerName";
            this.textBoxContainerName.Size = new System.Drawing.Size(266, 29);
            this.textBoxContainerName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(236, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nome do Container";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(559, 114);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "EndPoint";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(860, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Tipo de evento";
            // 
            // textBoxEndPoint
            // 
            this.textBoxEndPoint.Location = new System.Drawing.Point(490, 172);
            this.textBoxEndPoint.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEndPoint.Name = "textBoxEndPoint";
            this.textBoxEndPoint.Size = new System.Drawing.Size(233, 29);
            this.textBoxEndPoint.TabIndex = 8;
            // 
            // comboBoxEvent
            // 
            this.comboBoxEvent.FormattingEnabled = true;
            this.comboBoxEvent.Items.AddRange(new object[] {
            "1",
            "2"});
            this.comboBoxEvent.Location = new System.Drawing.Point(825, 170);
            this.comboBoxEvent.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxEvent.Name = "comboBoxEvent";
            this.comboBoxEvent.Size = new System.Drawing.Size(209, 32);
            this.comboBoxEvent.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(223, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(204, 25);
            this.label4.TabIndex = 10;
            this.label4.Text = "Nome da Subscription";
            // 
            // textBoxSubscriptionName
            // 
            this.textBoxSubscriptionName.Location = new System.Drawing.Point(203, 170);
            this.textBoxSubscriptionName.Name = "textBoxSubscriptionName";
            this.textBoxSubscriptionName.Size = new System.Drawing.Size(233, 29);
            this.textBoxSubscriptionName.TabIndex = 11;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 550);
            this.Controls.Add(this.textBoxSubscriptionName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxEvent);
            this.Controls.Add(this.textBoxEndPoint);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxContainerName);
            this.Controls.Add(this.buttonSubscribe);
            this.Controls.Add(this.buttonCreateSubscription);
            this.Controls.Add(this.buttonCreateContainer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonCreateContainer;
        private System.Windows.Forms.Button buttonCreateSubscription;
        private System.Windows.Forms.Button buttonSubscribe;
        private System.Windows.Forms.TextBox textBoxContainerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxEndPoint;
        private System.Windows.Forms.ComboBox comboBoxEvent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSubscriptionName;
    }
}