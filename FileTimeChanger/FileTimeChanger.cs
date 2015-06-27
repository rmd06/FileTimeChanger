﻿using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FileTimeChanger
{
    public partial class FileTimeChanger : Form
    {
        public FileTimeChanger()
        {
            InitializeComponent();
        }

        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            int count = ((string[])e.Data.GetData(DataFormats.FileDrop)).GetLength(0);

            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = count;
            progressBar.Step = 1;

            
            foreach (string fileName in ((string[])e.Data.GetData(DataFormats.FileDrop)))
            {
                if (File.GetAttributes(fileName).ToString().StartsWith("Archive") == true)
                {
                    FileInfo fi = new FileInfo(fileName);
                    ListViewItem item = new ListViewItem(new string[] { fi.FullName, fi.Name, fi.CreationTime.ToString(), fi.LastWriteTime.ToString(), fi.LastAccessTime.ToString() });
                    item.Name = fi.FullName;

                    if (!listView.Items.ContainsKey(fi.FullName))
                    {
                        listView.Items.Add(item);
                        progressBar.PerformStep();
                    }
                }
            }

            progressBar.Value = progressBar.Maximum;
        }

        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            
            string FullFilePath = string.Empty;
            //string FileName = string.Empty;
            string Created = string.Empty;
            string Modified = string.Empty;
            string Accessed = string.Empty;

            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = listView.Items.Count;
            progressBar.Step = 1;

            foreach (ListViewItem item in listView.Items)
            {
                FullFilePath = item.SubItems[0].Text;
                //FileName = item.SubItems[1].Text;
                Created = item.SubItems[2].Text;
                Modified = item.SubItems[3].Text;
                Accessed = item.SubItems[4].Text;

                FileInfo fi = new FileInfo(FullFilePath);

                if (checkTime.Checked)
                {
                    fi.CreationTime = new DateTime(dateTimePicker.Value.Year, dateTimePicker.Value.Month, dateTimePicker.Value.Day, fi.CreationTime.Hour, fi.CreationTime.Minute, fi.CreationTime.Second);
                    fi.LastWriteTime = new DateTime(dateTimePicker.Value.Year, dateTimePicker.Value.Month, dateTimePicker.Value.Day, fi.LastWriteTime.Hour, fi.LastWriteTime.Minute, fi.LastWriteTime.Second);
                    fi.LastAccessTime = new DateTime(dateTimePicker.Value.Year, dateTimePicker.Value.Month, dateTimePicker.Value.Day, fi.LastAccessTime.Hour, fi.LastAccessTime.Minute, fi.LastAccessTime.Second);
                }
                else
                {
                    fi.CreationTime = dateTimePicker.Value;
                    fi.LastWriteTime = dateTimePicker.Value;
                    fi.LastAccessTime = dateTimePicker.Value;
                }

                item.SubItems[2].Text = fi.CreationTime.ToString();
                item.SubItems[3].Text = fi.LastWriteTime.ToString();
                item.SubItems[4].Text = fi.LastAccessTime.ToString();

                progressBar.PerformStep();
            }
            progressBar.Value = progressBar.Maximum;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            listView.Items.Clear();
            dateTimePicker.Value = DateTime.Now;
            progressBar.Value = 0;
            checkTime.Checked = false;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            ListViewItemsRemove();
        }

        private void ListViewItemsRemove()
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                listView.Items.Remove(item);
            }
        }

        private void FileTimeChanger_Load(object sender, EventArgs e)
        {
            this.Size = new Size(660, 350);
            this.MaximumSize = new Size(660, Screen.PrimaryScreen.WorkingArea.Size.Height);
            this.MinimumSize = new Size(660, 350);

            listView.Size = new Size(625, 230);
            listView.Location = new Point(10, 10);
        }
    }
}