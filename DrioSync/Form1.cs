﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrioSync {
	public partial class Form1 : Form {

		private SQLDataBase db;
		public Form1() {

			//データベースと実際の齟齬の確認（データベースのものが実際あるかどうかの確認（islinkが必要？））isFileの活用

			InitializeComponent();

			db = new SQLDataBase("sync.db");
			ReloadWindow();
		}
		private void ReloadWindow() {
			linkListBox.Items.Clear();
			linkListBox.Items.AddRange(db.GetColumnFromDB("originalDirectory", "files").ToArray());
		}
		private void linkListBox_DragDrop(object sender, DragEventArgs e) {
			string[] fileNames =
			(string[])e.Data.GetData(DataFormats.FileDrop, false);
			foreach (string fileName in fileNames) {
				Program.linkToBackup(fileName);
			}
			ReloadWindow();
		}

		private void linkListBox_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}


		private void linkListBox_DoubleClick(object sender, EventArgs e) {
			MouseEventArgs mouseE = (MouseEventArgs)e;
			int index = ((ListBox)sender).IndexFromPoint(new Point(mouseE.X, mouseE.Y));
			if (index == -1) return;
			string originalDirectory = (string)linkListBox.SelectedItem;
			string linkDirectory = db.GetColumnFromDB("linkDirectory", "files", "originalDirectory", originalDirectory)[0];
			WsnTools.FileOperator.Delete(linkDirectory);
			db.DeleteFromDB("files", "originalDirectory", originalDirectory);
			ReloadWindow();
		}
	}
}
