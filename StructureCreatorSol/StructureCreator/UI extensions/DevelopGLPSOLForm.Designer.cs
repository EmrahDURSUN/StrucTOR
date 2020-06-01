namespace StructureCreator.UI_extensions
{
    partial class DevelopGLPSOLForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.glpsolOptimizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.glpsoloptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.glpsolOptimizerToolStripMenuItem,
            this.glpsoloptionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(848, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // glpsolOptimizerToolStripMenuItem
            // 
            this.glpsolOptimizerToolStripMenuItem.Name = "glpsolOptimizerToolStripMenuItem";
            this.glpsolOptimizerToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.glpsolOptimizerToolStripMenuItem.Text = "GLPK Optimization";
            this.glpsolOptimizerToolStripMenuItem.Click += new System.EventHandler(this.glpsolOptimizerToolStripMenuItem_Click);
            // 
            // glpsoloptionsToolStripMenuItem
            // 
            this.glpsoloptionsToolStripMenuItem.Name = "glpsoloptionsToolStripMenuItem";
            this.glpsoloptionsToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.glpsoloptionsToolStripMenuItem.Text = "GLPK Options";
            this.glpsoloptionsToolStripMenuItem.Click += new System.EventHandler(this.glpsoloptionsToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(7, 22);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 461);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Location = new System.Drawing.Point(70, 330);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(349, 62);
            this.panel3.TabIndex = 33;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 23);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Status:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(75, 23);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Start process";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(341, 271);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "lp file created";
            this.label5.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(70, 251);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(211, 54);
            this.button3.TabIndex = 7;
            this.button3.Text = "Optimize / Convert";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 67);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Choose folder!";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(70, 55);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 36);
            this.button2.TabIndex = 5;
            this.button2.Text = "Choose project folder";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(187, 154);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(438, 60);
            this.textBox1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(187, 111);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "Choose .mod file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(70, 154);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Command:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(70, 113);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = ".mod file path: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "GLPK Optimization";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBox2);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.checkedListBox1);
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(7, 22);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(835, 460);
            this.panel2.TabIndex = 9;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(271, 386);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(488, 58);
            this.textBox2.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(203, 409);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Command:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(71, 397);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(104, 36);
            this.button4.TabIndex = 3;
            this.button4.Text = "Create Command";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "--mps \t \t read LP/MIP problem in fixed MPS format",
            "--freemps \t \t read LP/MIP problem in free MPS format (default)",
            "--lp \t \t  \t read LP/MIP problem in CPLEX LP format",
            "--glp \t \t  \t read LP/MIP problem in GLPK format",
            "--math \t \t  read LP/MIP model written in GNU MathProg modeling language",
            "--seed value \t  \t initialize pseudo-random number generator used in MathProg mode" +
                "l ",
            "--mincost \t  \t read min-cost flow problem in DIMACS format",
            "--maxflow \t  \t read maximum flow problem in DIMACS format",
            "--simplex \t  \t use simplex method (default)",
            "--interior \t  \t use interior point method (LP only)",
            "--min \t \t \t minimization",
            "--max \t \t maximization",
            "--scale \t \t scale problem (default)",
            "--noscale \t  \t do not scale problem",
            "--tmlim nnn \t  \t limit solution time to nnn seconds",
            "--memlim nnn \t  \t limit available memory to nnn megabytes",
            "--check \t \t do not solve problem, check input data only",
            "--name probname \t change problem name to probname",
            "--wmps filename \t write problem to filename in fixed MPS format",
            "--wfreemps filename \t write problem to filename in free MPS format",
            "--wlp filename \t \t write problem to filename in CPLEX LP format",
            "--wglp filename \t  write problem to filename in GLPK format",
            "--log filename \t \t write copy of terminal output to filename",
            "--luf \t \t \t LU + Forrest-Tomlin update",
            "--cbg \t \t \t LU + Schur complement + Bartels-Golub update",
            "--cgr \t \t \t LU + Schur complement + Givens rotation update",
            "--primal \t \t use primal simplex (default)",
            "--dual \t \t use dual simplex",
            "--std \t \t \t use standard initial basis of all slacks",
            "--adv \t \t \t use advanced initial basis (default)",
            "--bib \t \t \t use Bixby\'s initial basis",
            "--ini filename \t \t use as initial basis previously saved with -w",
            "--steep \t \t use steepest edge technique (default)",
            "--nosteep \t \t use standard \"textbook\" pricing",
            "--relax \t \t use Harris\' two-pass ratio test (default)",
            "--norelax \t \t use standard \"textbook\" ratio test",
            "--presol \t \t use presolver (default; assumes --scale and --adv)",
            "--nopresol \t \t do not use presolver",
            "--exact \t \t use simplex method based on exact arithmetic",
            "--xcheck \t \t check final basis using exact arithmetic",
            "--nord \t \t use natural (original) ordering",
            "--qmd \t \t use quotient minimum degree ordering",
            "--amd \t \t use approximate minimum degree ordering (default)",
            "--symamd \t \t use approximate minimum degree ordering",
            "--nomip \t \t consider all integer variables as continuous",
            "--first \t \t \t branch on first integer variable",
            "--last \t \t \t branch on last integer variable",
            "--mostf \t \t branch on most fractional variable",
            "--drtom \t \t branch using heuristic by Driebeck and Tomlin",
            "--pcost \t \t branch using hybrid pseudocost heuristic ",
            "--dfs \t \t \t backtrack using depth first search",
            "--bfs \t \t \t backtrack using breadth first search",
            "--bestp \t \t backtrack using the best projection heuristic",
            "--bestb \t \t backtrack using node with best local bound",
            "--intopt \t \t use MIP presolver (default)",
            "--nointopt \t \t do not use MIP presolver",
            "--binarize \t \t replace general integer variables by binary ones",
            "--fpump \t \t apply feasibility pump heuristic",
            "--gomory \t \t generate Gomory\'s mixed integer cuts",
            "--mir \t \t \t generate MIR (mixed integer rounding) cuts",
            "--cover \t \t generate mixed cover cuts",
            "--clique \t \t generate clique cuts",
            "--cuts \t \t \t generate all cuts above",
            "--mipgap tol \t \t set relative mip gap tolerance to tol"});
            this.checkedListBox1.Location = new System.Drawing.Point(78, 65);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(681, 289);
            this.checkedListBox1.TabIndex = 2;
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(704, 16);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(79, 13);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Documentation";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(279, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(258, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Choose the options glpsol will be using";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DevelopGLPSOLForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 489);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DevelopGLPSOLForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GLPK Optimization";
            this.TopMost = true;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem glpsolOptimizerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem glpsoloptionsToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label8;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label9;
    }
}