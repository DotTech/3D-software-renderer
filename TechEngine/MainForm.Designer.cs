namespace TechEngine
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
            this.components = new System.ComponentModel.Container();
            this.timerFPS = new System.Windows.Forms.Timer(this.components);
            this.timerDraw = new System.Windows.Forms.Timer(this.components);
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.game = new TechEngine.Engine.Game();
            this.SuspendLayout();
            // 
            // timerFPS
            // 
            this.timerFPS.Enabled = true;
            // 
            // timerDraw
            // 
            this.timerDraw.Interval = 20;
            // 
            // game
            // 
            this.game.BackColor = System.Drawing.Color.Black;
            this.game.Location = new System.Drawing.Point(0, 0);
            this.game.Name = "game";
            this.game.Size = new System.Drawing.Size(640, 480);
            this.game.TabIndex = 2;
            this.game.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.Controls.Add(this.game);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "TechEngine";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerFPS;
        private System.Windows.Forms.Timer timerDraw;
        private System.Windows.Forms.Timer timerUpdate;
        private Engine.Game game;
    }
}