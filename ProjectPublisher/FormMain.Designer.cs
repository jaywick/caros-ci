namespace ProjectPublisher
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.progressAll = new System.Windows.Forms.ProgressBar();
            this.listEvents = new System.Windows.Forms.ListView();
            this.columnInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "info");
            this.imageList1.Images.SetKeyName(1, "good");
            this.imageList1.Images.SetKeyName(2, "bad");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.progressAll, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listEvents, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.97591F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.024096F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(469, 480);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // progressAll
            // 
            this.progressAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressAll.Location = new System.Drawing.Point(3, 454);
            this.progressAll.Name = "progressAll";
            this.progressAll.Size = new System.Drawing.Size(463, 23);
            this.progressAll.TabIndex = 7;
            // 
            // listEvents
            // 
            this.listEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnInfo,
            this.columnTime});
            this.listEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEvents.FullRowSelect = true;
            this.listEvents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listEvents.Location = new System.Drawing.Point(3, 4);
            this.listEvents.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listEvents.Name = "listEvents";
            this.listEvents.Size = new System.Drawing.Size(463, 443);
            this.listEvents.SmallImageList = this.imageList1;
            this.listEvents.TabIndex = 6;
            this.listEvents.UseCompatibleStateImageBehavior = false;
            this.listEvents.View = System.Windows.Forms.View.Details;
            // 
            // columnInfo
            // 
            this.columnInfo.Text = "Info";
            this.columnInfo.Width = 300;
            // 
            // columnTime
            // 
            this.columnTime.Text = "Time";
            this.columnTime.Width = 150;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 480);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMain";
            this.Text = "Publisher";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ProgressBar progressAll;
        private System.Windows.Forms.ListView listEvents;
        private System.Windows.Forms.ColumnHeader columnInfo;
        private System.Windows.Forms.ColumnHeader columnTime;
    }
}

