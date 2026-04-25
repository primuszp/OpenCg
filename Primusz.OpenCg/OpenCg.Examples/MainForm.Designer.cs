namespace OpenCg.Examples
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnRun = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(12, 420);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(260, 33);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.BtnRunClick);
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Items.AddRange(new object[] {
            "[01] Vertex Program",
            "[02] Fragment Program",
            "[03] Uniform Parameter",
            "[04] Varying Parameter",
            "[05] Texture Sampling",
            "[06] Vertex Twisting",
            "[07] Two Texture Accesses",
            "[08] Vertex Transform",
            "[09] Vertex Lighting",
            "[10] Fragment Lighting",
            "[11] Two Lights with Structs",
            "[12] Light Attenuation",
            "[13] Spotlight",
            "[14] Bulge",
            "[15] Particle System",
            "[16] Bump Mapping",
            "[17] Projective Texturing",
            "[18] Cube Map Reflection",
            "[19] Cube Map Refraction",
            "[20] Chromatic Dispersion",
            "[21] Specular Bump Map",
            "[22] Bump Map Floor",
            "[23] Bump Map Torus",
            "[24] Uniform Fog",
            "[25] Toon Shading",
            "[26] Phong Model"});
            this.listBox.Location = new System.Drawing.Point(12, 12);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(260, 400);
            this.listBox.TabIndex = 1;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.ListBoxSelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 465);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Cg Examples";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.ListBox listBox;
    }
}

