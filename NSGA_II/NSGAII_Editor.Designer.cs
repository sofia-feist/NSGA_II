using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NSGA_II
{
    partial class NSGAII_Editor
    {
        /// Required designer variable.
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
            this.PopulationSizeLabel = new System.Windows.Forms.Label();
            this.PopSizeInputField = new System.Windows.Forms.NumericUpDown();
            this.OkButton = new System.Windows.Forms.Button();
            this.NGenerationsInputField = new System.Windows.Forms.NumericUpDown();
            this.GenerationsCheckBox = new System.Windows.Forms.CheckBox();
            this.TimeCheckBox = new System.Windows.Forms.CheckBox();
            this.StopConditionG = new System.Windows.Forms.GroupBox();
            this.H_MinsLabel = new System.Windows.Forms.Label();
            this.DurationLabel = new System.Windows.Forms.Label();
            this.NGenerationsLabel = new System.Windows.Forms.Label();
            this.DurationTimeHours = new System.Windows.Forms.NumericUpDown();
            this.DurationTimeMinutes = new System.Windows.Forms.NumericUpDown();
            this.RunOptimizationButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PopSizeInputField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NGenerationsInputField)).BeginInit();
            this.StopConditionG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DurationTimeHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationTimeMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // PopulationSizeLabel
            // 
            this.PopulationSizeLabel.AutoSize = true;
            this.PopulationSizeLabel.Font = new System.Drawing.Font("Source Sans Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PopulationSizeLabel.Location = new System.Drawing.Point(11, 34);
            this.PopulationSizeLabel.Name = "PopulationSizeLabel";
            this.PopulationSizeLabel.Size = new System.Drawing.Size(107, 19);
            this.PopulationSizeLabel.TabIndex = 3;
            this.PopulationSizeLabel.Text = "Population Size";
            // 
            // PopSizeInputField
            // 
            this.PopSizeInputField.Font = new System.Drawing.Font("Source Sans Pro", 9F);
            this.PopSizeInputField.Location = new System.Drawing.Point(210, 31);
            this.PopSizeInputField.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PopSizeInputField.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.PopSizeInputField.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PopSizeInputField.Name = "PopSizeInputField";
            this.PopSizeInputField.Size = new System.Drawing.Size(80, 26);
            this.PopSizeInputField.TabIndex = 2;
            this.PopSizeInputField.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.PopSizeInputField.TextChanged += new System.EventHandler(this.PopSizeInputField_ValueChanged);
            this.PopSizeInputField.ValueChanged += new System.EventHandler(this.PopSizeInputField_ValueChanged);
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.Font = new System.Drawing.Font("Source Sans Pro", 10F);
            this.OkButton.Location = new System.Drawing.Point(1160, 635);
            this.OkButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(132, 35);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // NGenerationsInputField
            // 
            this.NGenerationsInputField.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NGenerationsInputField.Font = new System.Drawing.Font("Source Sans Pro", 9F);
            this.NGenerationsInputField.Location = new System.Drawing.Point(184, 68);
            this.NGenerationsInputField.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NGenerationsInputField.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NGenerationsInputField.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NGenerationsInputField.Name = "NGenerationsInputField";
            this.NGenerationsInputField.Size = new System.Drawing.Size(80, 26);
            this.NGenerationsInputField.TabIndex = 5;
            this.NGenerationsInputField.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NGenerationsInputField.ValueChanged += new System.EventHandler(this.NGenerationsInputField_ValueChanged);
            // 
            // GenerationsCheckBox
            // 
            this.GenerationsCheckBox.AutoSize = true;
            this.GenerationsCheckBox.Checked = true;
            this.GenerationsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GenerationsCheckBox.Font = new System.Drawing.Font("Source Sans Pro", 9F);
            this.GenerationsCheckBox.Location = new System.Drawing.Point(10, 32);
            this.GenerationsCheckBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GenerationsCheckBox.Name = "GenerationsCheckBox";
            this.GenerationsCheckBox.Size = new System.Drawing.Size(180, 23);
            this.GenerationsCheckBox.TabIndex = 6;
            this.GenerationsCheckBox.Text = "  Number of Generations";
            this.GenerationsCheckBox.UseVisualStyleBackColor = false;
            this.GenerationsCheckBox.CheckedChanged += new System.EventHandler(this.GenerationCheckBox_CheckedChanged);
            // 
            // TimeCheckBox
            // 
            this.TimeCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TimeCheckBox.AutoSize = true;
            this.TimeCheckBox.Font = new System.Drawing.Font("Source Sans Pro", 9F);
            this.TimeCheckBox.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.TimeCheckBox.Location = new System.Drawing.Point(10, 127);
            this.TimeCheckBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TimeCheckBox.Name = "TimeCheckBox";
            this.TimeCheckBox.Size = new System.Drawing.Size(125, 23);
            this.TimeCheckBox.TabIndex = 8;
            this.TimeCheckBox.Text = "  Runtime Limit";
            this.TimeCheckBox.UseVisualStyleBackColor = true;
            this.TimeCheckBox.CheckedChanged += new System.EventHandler(this.TimeCheckBox_CheckedChanged);
            // 
            // StopConditionG
            // 
            this.StopConditionG.Controls.Add(this.H_MinsLabel);
            this.StopConditionG.Controls.Add(this.DurationLabel);
            this.StopConditionG.Controls.Add(this.NGenerationsLabel);
            this.StopConditionG.Controls.Add(this.NGenerationsInputField);
            this.StopConditionG.Controls.Add(this.GenerationsCheckBox);
            this.StopConditionG.Controls.Add(this.DurationTimeHours);
            this.StopConditionG.Controls.Add(this.TimeCheckBox);
            this.StopConditionG.Controls.Add(this.DurationTimeMinutes);
            this.StopConditionG.Location = new System.Drawing.Point(16, 69);
            this.StopConditionG.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StopConditionG.Name = "StopConditionG";
            this.StopConditionG.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StopConditionG.Size = new System.Drawing.Size(274, 199);
            this.StopConditionG.TabIndex = 9;
            this.StopConditionG.TabStop = false;
            this.StopConditionG.Text = "Stop Condition";
            // 
            // H_MinsLabel
            // 
            this.H_MinsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.H_MinsLabel.AutoSize = true;
            this.H_MinsLabel.Font = new System.Drawing.Font("Source Sans Pro", 8F);
            this.H_MinsLabel.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.H_MinsLabel.Location = new System.Drawing.Point(162, 138);
            this.H_MinsLabel.Name = "H_MinsLabel";
            this.H_MinsLabel.Size = new System.Drawing.Size(92, 18);
            this.H_MinsLabel.TabIndex = 13;
            this.H_MinsLabel.Text = "H          :     Mins";
            this.H_MinsLabel.UseMnemonic = false;
            // 
            // DurationLabel
            // 
            this.DurationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DurationLabel.AutoSize = true;
            this.DurationLabel.Font = new System.Drawing.Font("Source Sans Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DurationLabel.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.DurationLabel.Location = new System.Drawing.Point(6, 164);
            this.DurationLabel.Name = "DurationLabel";
            this.DurationLabel.Size = new System.Drawing.Size(97, 19);
            this.DurationLabel.TabIndex = 12;
            this.DurationLabel.Text = "Max. Duration";
            this.DurationLabel.UseMnemonic = false;
            // 
            // NGenerationsLabel
            // 
            this.NGenerationsLabel.AutoSize = true;
            this.NGenerationsLabel.Font = new System.Drawing.Font("Source Sans Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NGenerationsLabel.Location = new System.Drawing.Point(6, 70);
            this.NGenerationsLabel.Name = "NGenerationsLabel";
            this.NGenerationsLabel.Size = new System.Drawing.Size(117, 19);
            this.NGenerationsLabel.TabIndex = 11;
            this.NGenerationsLabel.Text = "Max. Generations";
            // 
            // DurationTimeHours
            // 
            this.DurationTimeHours.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DurationTimeHours.Enabled = false;
            this.DurationTimeHours.Font = new System.Drawing.Font("Source Sans Pro", 9F);
            this.DurationTimeHours.Location = new System.Drawing.Point(149, 161);
            this.DurationTimeHours.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DurationTimeHours.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.DurationTimeHours.Name = "DurationTimeHours";
            this.DurationTimeHours.Size = new System.Drawing.Size(49, 26);
            this.DurationTimeHours.TabIndex = 6;
            this.DurationTimeHours.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DurationTimeHours.ValueChanged += new System.EventHandler(this.DurationHours_ValueChanged);
            // 
            // DurationTimeMinutes
            // 
            this.DurationTimeMinutes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DurationTimeMinutes.Enabled = false;
            this.DurationTimeMinutes.Font = new System.Drawing.Font("Source Sans Pro", 9F);
            this.DurationTimeMinutes.Location = new System.Drawing.Point(215, 161);
            this.DurationTimeMinutes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DurationTimeMinutes.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.DurationTimeMinutes.Name = "DurationTimeMinutes";
            this.DurationTimeMinutes.Size = new System.Drawing.Size(49, 26);
            this.DurationTimeMinutes.TabIndex = 5;
            this.DurationTimeMinutes.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.DurationTimeMinutes.ValueChanged += new System.EventHandler(this.DurationMinutes_ValueChanged);
            // 
            // RunOptimizationButton
            // 
            this.RunOptimizationButton.Location = new System.Drawing.Point(15, 286);
            this.RunOptimizationButton.Name = "RunOptimizationButton";
            this.RunOptimizationButton.Size = new System.Drawing.Size(159, 33);
            this.RunOptimizationButton.TabIndex = 11;
            this.RunOptimizationButton.Text = "Run Optimization";
            this.RunOptimizationButton.UseVisualStyleBackColor = true;
            this.RunOptimizationButton.Click += new System.EventHandler(this.RunOptimizationButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(210, 286);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(80, 33);
            this.resetButton.TabIndex = 12;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // NSGAII_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1318, 693);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.RunOptimizationButton);
            this.Controls.Add(this.StopConditionG);
            this.Controls.Add(this.PopulationSizeLabel);
            this.Controls.Add(this.PopSizeInputField);
            this.Controls.Add(this.OkButton);
            this.Font = new System.Drawing.Font("Source Sans Pro", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "NSGAII_Editor";
            this.Text = "NSGA-II Editor";
            this.Load += new System.EventHandler(this.NSGAII_EditorWindow);
            ((System.ComponentModel.ISupportInitialize)(this.PopSizeInputField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NGenerationsInputField)).EndInit();
            this.StopConditionG.ResumeLayout(false);
            this.StopConditionG.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DurationTimeHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DurationTimeMinutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button OkButton;
        private NumericUpDown PopSizeInputField;
        private Label PopulationSizeLabel;
        private CheckBox GenerationsCheckBox;
        private CheckBox TimeCheckBox;
        private GroupBox StopConditionG;
        private NumericUpDown NGenerationsInputField;
        private NumericUpDown DurationTimeMinutes;
        private NumericUpDown DurationTimeHours;
        private Label NGenerationsLabel;
        private Label DurationLabel;
        private Button RunOptimizationButton;
        private Label H_MinsLabel;
        private Button resetButton;
    }
}

