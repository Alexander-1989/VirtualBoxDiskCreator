using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using VirtualBoxDiskCreator.Drive;
using VirtualBoxDiskCreator.Utility;

namespace VirtualBoxDiskCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeDiskInfo();
        }

        private readonly string fullPath = Service.GetVirtualBoxDirectory();

        private void InitializeDiskInfo()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(DiskDrive.GetDriveInfo());

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fullPath == string.Empty)
            {
                MsgBox message = new MsgBox("VirtualBox is not installed.");
                message.Show(this);
                return;
            }

            DiskDrive drive = comboBox1.SelectedItem as DiskDrive;
            saveFileDialog1.FileName = drive.Model.Replace(' ', '_');

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = saveFileDialog1.FileName;
                    using (Process process = new Process())
                    {
                        process.StartInfo = new ProcessStartInfo()
                        {
                            FileName = fullPath,
                            Arguments = $"internalcommands createrawvmdk -filename \"{fileName}\" -rawdisk {drive.Name}",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        process.Start();
                        process.WaitForExit();
                    }

                    if (File.Exists(fileName))
                    {
                        MsgBox message = new MsgBox($"VMDK \"{drive.Model}\" has been created!");
                        message.Show(this);
                    }
                    else
                    {
                        MsgBox message = new MsgBox("Something went wrong!");
                        message.Show(this);
                    }
                }
                catch (Exception exc)
                {
                    MsgBox message = new MsgBox(exc.Message);
                    message.Show(this);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitializeDiskInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}