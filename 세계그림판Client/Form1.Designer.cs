namespace 세계그림판Client
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.lineImgList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnToolDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuHand = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPencil = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLine = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCircle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRect = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLineDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnuLine1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLine2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLine3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLine4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLine5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtFill = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLineCld = new System.Windows.Forms.ToolStripButton();
            this.btnShapeCld = new System.Windows.Forms.ToolStripButton();
            this.cld = new System.Windows.Forms.ColorDialog();
            this.panel1 = new 세계그림판Client.DoubleBufferPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "hand.png");
            this.imgList.Images.SetKeyName(1, "pencil.jpg");
            this.imgList.Images.SetKeyName(2, "line.jpg");
            this.imgList.Images.SetKeyName(3, "ellipse.jpg");
            this.imgList.Images.SetKeyName(4, "rectangle.jpg");
            // 
            // lineImgList
            // 
            this.lineImgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("lineImgList.ImageStream")));
            this.lineImgList.TransparentColor = System.Drawing.Color.Transparent;
            this.lineImgList.Images.SetKeyName(0, "line1Button.JPG");
            this.lineImgList.Images.SetKeyName(1, "line2Button.JPG");
            this.lineImgList.Images.SetKeyName(2, "line3Button.JPG");
            this.lineImgList.Images.SetKeyName(3, "line4Button.JPG");
            this.lineImgList.Images.SetKeyName(4, "line5Button.JPG");
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnToolDropDown,
            this.btnLineDropDown,
            this.toolStripSeparator1,
            this.txtFill,
            this.toolStripSeparator2,
            this.btnLineCld,
            this.btnShapeCld});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.ShowItemToolTips = false;
            this.toolStrip1.Size = new System.Drawing.Size(800, 41);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnToolDropDown
            // 
            this.btnToolDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnToolDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHand,
            this.mnuPencil,
            this.mnuLine,
            this.mnuCircle,
            this.mnuRect});
            this.btnToolDropDown.Image = global::세계그림판Client.Properties.Resources.pencil;
            this.btnToolDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToolDropDown.Name = "btnToolDropDown";
            this.btnToolDropDown.Size = new System.Drawing.Size(43, 38);
            this.btnToolDropDown.Text = "toolStripDropDownButton1";
            // 
            // mnuHand
            // 
            this.mnuHand.Image = global::세계그림판Client.Properties.Resources.hand;
            this.mnuHand.Name = "mnuHand";
            this.mnuHand.Size = new System.Drawing.Size(124, 36);
            this.mnuHand.Text = "Hand";
            this.mnuHand.Click += new System.EventHandler(this.changeShape);
            // 
            // mnuPencil
            // 
            this.mnuPencil.Image = global::세계그림판Client.Properties.Resources.pencil;
            this.mnuPencil.Name = "mnuPencil";
            this.mnuPencil.Size = new System.Drawing.Size(124, 36);
            this.mnuPencil.Text = "Pencil";
            this.mnuPencil.Click += new System.EventHandler(this.changeShape);
            // 
            // mnuLine
            // 
            this.mnuLine.Image = global::세계그림판Client.Properties.Resources.line;
            this.mnuLine.Name = "mnuLine";
            this.mnuLine.Size = new System.Drawing.Size(124, 36);
            this.mnuLine.Text = "Line";
            this.mnuLine.Click += new System.EventHandler(this.changeShape);
            // 
            // mnuCircle
            // 
            this.mnuCircle.Image = global::세계그림판Client.Properties.Resources.ellipse;
            this.mnuCircle.Name = "mnuCircle";
            this.mnuCircle.Size = new System.Drawing.Size(124, 36);
            this.mnuCircle.Text = "Circle";
            this.mnuCircle.Click += new System.EventHandler(this.changeShape);
            // 
            // mnuRect
            // 
            this.mnuRect.Image = global::세계그림판Client.Properties.Resources.rectangle;
            this.mnuRect.Name = "mnuRect";
            this.mnuRect.Size = new System.Drawing.Size(124, 36);
            this.mnuRect.Text = "Rect";
            this.mnuRect.Click += new System.EventHandler(this.changeShape);
            // 
            // btnLineDropDown
            // 
            this.btnLineDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnLineDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLine1,
            this.mnuLine2,
            this.mnuLine3,
            this.mnuLine4,
            this.mnuLine5});
            this.btnLineDropDown.Image = global::세계그림판Client.Properties.Resources.line1Button;
            this.btnLineDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLineDropDown.Name = "btnLineDropDown";
            this.btnLineDropDown.Size = new System.Drawing.Size(43, 38);
            this.btnLineDropDown.Text = "toolStripDropDownButton2";
            // 
            // mnuLine1
            // 
            this.mnuLine1.Image = global::세계그림판Client.Properties.Resources.line1Button;
            this.mnuLine1.Name = "mnuLine1";
            this.mnuLine1.Size = new System.Drawing.Size(97, 36);
            this.mnuLine1.Text = "1";
            this.mnuLine1.Click += new System.EventHandler(this.changeLineThick);
            // 
            // mnuLine2
            // 
            this.mnuLine2.Image = global::세계그림판Client.Properties.Resources.line2Button;
            this.mnuLine2.Name = "mnuLine2";
            this.mnuLine2.Size = new System.Drawing.Size(97, 36);
            this.mnuLine2.Text = "2";
            this.mnuLine2.Click += new System.EventHandler(this.changeLineThick);
            // 
            // mnuLine3
            // 
            this.mnuLine3.Image = global::세계그림판Client.Properties.Resources.line3Button;
            this.mnuLine3.Name = "mnuLine3";
            this.mnuLine3.Size = new System.Drawing.Size(97, 36);
            this.mnuLine3.Text = "3";
            this.mnuLine3.Click += new System.EventHandler(this.changeLineThick);
            // 
            // mnuLine4
            // 
            this.mnuLine4.Image = global::세계그림판Client.Properties.Resources.line4Button;
            this.mnuLine4.Name = "mnuLine4";
            this.mnuLine4.Size = new System.Drawing.Size(97, 36);
            this.mnuLine4.Text = "4";
            this.mnuLine4.Click += new System.EventHandler(this.changeLineThick);
            // 
            // mnuLine5
            // 
            this.mnuLine5.Image = global::세계그림판Client.Properties.Resources.line5Button;
            this.mnuLine5.Name = "mnuLine5";
            this.mnuLine5.Size = new System.Drawing.Size(97, 36);
            this.mnuLine5.Text = "5";
            this.mnuLine5.Click += new System.EventHandler(this.changeLineThick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 41);
            // 
            // txtFill
            // 
            this.txtFill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.txtFill.Name = "txtFill";
            this.txtFill.Size = new System.Drawing.Size(47, 38);
            this.txtFill.Text = "채우기";
            this.txtFill.Click += new System.EventHandler(this.txtFill_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 41);
            // 
            // btnLineCld
            // 
            this.btnLineCld.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLineCld.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.btnLineCld.Image = global::세계그림판Client.Properties.Resources.rectangleColored;
            this.btnLineCld.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLineCld.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnLineCld.Name = "btnLineCld";
            this.btnLineCld.Size = new System.Drawing.Size(23, 25);
            this.btnLineCld.Click += new System.EventHandler(this.btnLineCld_Click);
            // 
            // btnShapeCld
            // 
            this.btnShapeCld.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnShapeCld.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.btnShapeCld.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnShapeCld.Image = ((System.Drawing.Image)(resources.GetObject("btnShapeCld.Image")));
            this.btnShapeCld.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShapeCld.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnShapeCld.Name = "btnShapeCld";
            this.btnShapeCld.Size = new System.Drawing.Size(23, 25);
            this.btnShapeCld.Text = "toolStripButton1";
            this.btnShapeCld.Click += new System.EventHandler(this.btnShapeCld_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 411);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 455);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 149);
            this.panel2.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtChat);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(800, 125);
            this.panel4.TabIndex = 1;
            // 
            // txtChat
            // 
            this.txtChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChat.Location = new System.Drawing.Point(0, 0);
            this.txtChat.Multiline = true;
            this.txtChat.Name = "txtChat";
            this.txtChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChat.Size = new System.Drawing.Size(800, 125);
            this.txtChat.TabIndex = 0;
            this.txtChat.WordWrap = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtSend);
            this.panel3.Controls.Add(this.btnSend);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 125);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 24);
            this.panel3.TabIndex = 0;
            // 
            // txtSend
            // 
            this.txtSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSend.Location = new System.Drawing.Point(0, 0);
            this.txtSend.Name = "txtSend";
            this.txtSend.Size = new System.Drawing.Size(725, 21);
            this.txtSend.TabIndex = 1;
            this.txtSend.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSend_KeyUp);
            // 
            // btnSend
            // 
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSend.Location = new System.Drawing.Point(725, 0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 24);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Say";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 604);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "세계그림판";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.ImageList lineImgList;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton btnToolDropDown;
        private System.Windows.Forms.ToolStripMenuItem mnuHand;
        private System.Windows.Forms.ToolStripMenuItem mnuPencil;
        private System.Windows.Forms.ToolStripMenuItem mnuLine;
        private System.Windows.Forms.ToolStripMenuItem mnuCircle;
        private System.Windows.Forms.ToolStripMenuItem mnuRect;
        private System.Windows.Forms.ToolStripDropDownButton btnLineDropDown;
        private System.Windows.Forms.ToolStripMenuItem mnuLine1;
        private System.Windows.Forms.ToolStripMenuItem mnuLine2;
        private System.Windows.Forms.ToolStripMenuItem mnuLine3;
        private System.Windows.Forms.ToolStripMenuItem mnuLine4;
        private System.Windows.Forms.ToolStripMenuItem mnuLine5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel txtFill;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnLineCld;
        private System.Windows.Forms.ColorDialog cld;
        private System.Windows.Forms.ToolStripButton btnShapeCld;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.Button btnSend;
        private DoubleBufferPanel panel1;
    }
}

