namespace DrioSync {
	partial class Form1 {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.linkListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// linkListBox
			// 
			this.linkListBox.AllowDrop = true;
			this.linkListBox.FormattingEnabled = true;
			this.linkListBox.ItemHeight = 15;
			this.linkListBox.Location = new System.Drawing.Point(13, 25);
			this.linkListBox.Margin = new System.Windows.Forms.Padding(4);
			this.linkListBox.Name = "linkListBox";
			this.linkListBox.Size = new System.Drawing.Size(347, 289);
			this.linkListBox.TabIndex = 0;
			this.linkListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.linkListBox_DragDrop);
			this.linkListBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.linkListBox_DragEnter);
			this.linkListBox.DoubleClick += new System.EventHandler(this.linkListBox_DoubleClick);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(373, 326);
			this.Controls.Add(this.linkListBox);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox linkListBox;
	}
}

