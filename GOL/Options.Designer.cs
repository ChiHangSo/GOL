
namespace GOL
{
    partial class Options
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownMili = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWidCells = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownHeiCells = new System.Windows.Forms.NumericUpDown();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMili)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidCells)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeiCells)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Interval in Miliseconds";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Width of Universe in Cells";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Height of Universe in Cells";
            // 
            // numericUpDownMili
            // 
            this.numericUpDownMili.Location = new System.Drawing.Point(234, 47);
            this.numericUpDownMili.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownMili.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMili.Name = "numericUpDownMili";
            this.numericUpDownMili.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownMili.TabIndex = 3;
            this.numericUpDownMili.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMili.ValueChanged += new System.EventHandler(this.numericUpDownMili_ValueChanged);
            // 
            // numericUpDownWidCells
            // 
            this.numericUpDownWidCells.Location = new System.Drawing.Point(234, 84);
            this.numericUpDownWidCells.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownWidCells.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownWidCells.Name = "numericUpDownWidCells";
            this.numericUpDownWidCells.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownWidCells.TabIndex = 4;
            this.numericUpDownWidCells.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownWidCells.ValueChanged += new System.EventHandler(this.numericUpDownWidCells_ValueChanged);
            // 
            // numericUpDownHeiCells
            // 
            this.numericUpDownHeiCells.Location = new System.Drawing.Point(234, 121);
            this.numericUpDownHeiCells.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownHeiCells.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownHeiCells.Name = "numericUpDownHeiCells";
            this.numericUpDownHeiCells.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownHeiCells.TabIndex = 5;
            this.numericUpDownHeiCells.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // OkButton
            // 
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(109, 161);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(234, 161);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButton;
            this.ClientSize = new System.Drawing.Size(439, 209);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.numericUpDownHeiCells);
            this.Controls.Add(this.numericUpDownWidCells);
            this.Controls.Add(this.numericUpDownMili);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMili)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidCells)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeiCells)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownMili;
        private System.Windows.Forms.NumericUpDown numericUpDownWidCells;
        private System.Windows.Forms.NumericUpDown numericUpDownHeiCells;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
    }
}