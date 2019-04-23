namespace SimpleExample
{
    partial class simpleexample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(simpleexample));
            this.CMB_comport = new System.Windows.Forms.ComboBox();
            this.cmb_baudrate = new System.Windows.Forms.ComboBox();
            this.but_connect = new System.Windows.Forms.Button();
            this.but_armdisarm = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btn_setAHRS_GPS_USE = new System.Windows.Forms.Button();
            this.tb_GPS_USE_val = new System.Windows.Forms.TextBox();
            this.btn_getPara = new System.Windows.Forms.Button();
            this.btn_getAHRS_GPS_USE = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_heartbeat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CMB_comport
            // 
            this.CMB_comport.FormattingEnabled = true;
            this.CMB_comport.Location = new System.Drawing.Point(53, 50);
            this.CMB_comport.Margin = new System.Windows.Forms.Padding(4);
            this.CMB_comport.Name = "CMB_comport";
            this.CMB_comport.Size = new System.Drawing.Size(160, 23);
            this.CMB_comport.TabIndex = 0;
            this.CMB_comport.Click += new System.EventHandler(this.CMB_comport_Click);
            // 
            // cmb_baudrate
            // 
            this.cmb_baudrate.FormattingEnabled = true;
            this.cmb_baudrate.Items.AddRange(new object[] {
            "9600",
            "14400",
            "19200",
            "28800",
            "38400",
            "57600",
            "115200"});
            this.cmb_baudrate.Location = new System.Drawing.Point(53, 102);
            this.cmb_baudrate.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_baudrate.Name = "cmb_baudrate";
            this.cmb_baudrate.Size = new System.Drawing.Size(160, 23);
            this.cmb_baudrate.TabIndex = 1;
            this.cmb_baudrate.Text = "115200";
            // 
            // but_connect
            // 
            this.but_connect.Location = new System.Drawing.Point(221, 53);
            this.but_connect.Margin = new System.Windows.Forms.Padding(4);
            this.but_connect.Name = "but_connect";
            this.but_connect.Size = new System.Drawing.Size(148, 72);
            this.but_connect.TabIndex = 2;
            this.but_connect.Text = "点击连接";
            this.but_connect.UseVisualStyleBackColor = true;
            this.but_connect.Click += new System.EventHandler(this.but_connect_Click);
            // 
            // but_armdisarm
            // 
            this.but_armdisarm.Location = new System.Drawing.Point(53, 219);
            this.but_armdisarm.Margin = new System.Windows.Forms.Padding(4);
            this.but_armdisarm.Name = "but_armdisarm";
            this.but_armdisarm.Size = new System.Drawing.Size(155, 47);
            this.but_armdisarm.TabIndex = 3;
            this.but_armdisarm.Text = "解锁/锁定";
            this.but_armdisarm.UseVisualStyleBackColor = true;
            this.but_armdisarm.Click += new System.EventHandler(this.but_armdisarm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // btn_setAHRS_GPS_USE
            // 
            this.btn_setAHRS_GPS_USE.Location = new System.Drawing.Point(53, 274);
            this.btn_setAHRS_GPS_USE.Margin = new System.Windows.Forms.Padding(4);
            this.btn_setAHRS_GPS_USE.Name = "btn_setAHRS_GPS_USE";
            this.btn_setAHRS_GPS_USE.Size = new System.Drawing.Size(155, 47);
            this.btn_setAHRS_GPS_USE.TabIndex = 5;
            this.btn_setAHRS_GPS_USE.Text = "修改AHRS_GPS_USE";
            this.btn_setAHRS_GPS_USE.UseVisualStyleBackColor = true;
            this.btn_setAHRS_GPS_USE.Click += new System.EventHandler(this.btn_setAHRS_GPS_USE_Click);
            // 
            // tb_GPS_USE_val
            // 
            this.tb_GPS_USE_val.Location = new System.Drawing.Point(215, 287);
            this.tb_GPS_USE_val.Name = "tb_GPS_USE_val";
            this.tb_GPS_USE_val.Size = new System.Drawing.Size(39, 25);
            this.tb_GPS_USE_val.TabIndex = 6;
            this.tb_GPS_USE_val.Text = "0";
            this.tb_GPS_USE_val.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btn_getPara
            // 
            this.btn_getPara.Location = new System.Drawing.Point(526, 53);
            this.btn_getPara.Margin = new System.Windows.Forms.Padding(4);
            this.btn_getPara.Name = "btn_getPara";
            this.btn_getPara.Size = new System.Drawing.Size(155, 47);
            this.btn_getPara.TabIndex = 5;
            this.btn_getPara.Text = "获取参数";
            this.btn_getPara.UseVisualStyleBackColor = true;
            this.btn_getPara.Click += new System.EventHandler(this.btn_getPara_Click);
            // 
            // btn_getAHRS_GPS_USE
            // 
            this.btn_getAHRS_GPS_USE.Location = new System.Drawing.Point(53, 329);
            this.btn_getAHRS_GPS_USE.Margin = new System.Windows.Forms.Padding(4);
            this.btn_getAHRS_GPS_USE.Name = "btn_getAHRS_GPS_USE";
            this.btn_getAHRS_GPS_USE.Size = new System.Drawing.Size(155, 47);
            this.btn_getAHRS_GPS_USE.TabIndex = 5;
            this.btn_getAHRS_GPS_USE.Text = "读取AHRS_GPS_USE";
            this.btn_getAHRS_GPS_USE.UseVisualStyleBackColor = true;
            this.btn_getAHRS_GPS_USE.Click += new System.EventHandler(this.btn_getAHRS_GPS_USE_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(218, 345);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "null";
            // 
            // btn_heartbeat
            // 
            this.btn_heartbeat.Location = new System.Drawing.Point(53, 164);
            this.btn_heartbeat.Margin = new System.Windows.Forms.Padding(4);
            this.btn_heartbeat.Name = "btn_heartbeat";
            this.btn_heartbeat.Size = new System.Drawing.Size(155, 47);
            this.btn_heartbeat.TabIndex = 3;
            this.btn_heartbeat.Text = "启动心跳";
            this.btn_heartbeat.UseVisualStyleBackColor = true;
            this.btn_heartbeat.Click += new System.EventHandler(this.btn_heartbeat_Click);
            // 
            // simpleexample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 535);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_GPS_USE_val);
            this.Controls.Add(this.btn_getPara);
            this.Controls.Add(this.btn_getAHRS_GPS_USE);
            this.Controls.Add(this.btn_setAHRS_GPS_USE);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_heartbeat);
            this.Controls.Add(this.but_armdisarm);
            this.Controls.Add(this.but_connect);
            this.Controls.Add(this.cmb_baudrate);
            this.Controls.Add(this.CMB_comport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "simpleexample";
            this.Text = "MissionLite";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.simpleexample_FormClosing);
            this.Load += new System.EventHandler(this.simpleexample_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CMB_comport;
        private System.Windows.Forms.ComboBox cmb_baudrate;
        private System.Windows.Forms.Button but_connect;
        private System.Windows.Forms.Button but_armdisarm;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_setAHRS_GPS_USE;
        private System.Windows.Forms.TextBox tb_GPS_USE_val;
        private System.Windows.Forms.Button btn_getPara;
        private System.Windows.Forms.Button btn_getAHRS_GPS_USE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_heartbeat;
    }
}

