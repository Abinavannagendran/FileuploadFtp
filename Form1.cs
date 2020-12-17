using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.IO;

namespace howto_ftp_upload_file
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Start with the executable file.
        private void Form1_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Done";
            txtFile.Text = Application.ExecutablePath;
        }

        // Let the user pick a file.
        private void btnPickFile_Click(object sender, EventArgs e)
        {
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = ofdFile.FileName;
            }
        }

        // Upload the selected file.
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                lblStatus.Text = "Working...";
                Application.DoEvents();

                FtpUploadFile(txtFile.Text, txtUri.Text,
                    txtUsername.Text, txtPassword.Text);

                lblStatus.Text = "Done";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error";
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // Use FTP to upload a file.
        private void FtpUploadFile(string filename, string to_uri, string user_name, string password)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(to_uri);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // Get network credentials.
            request.Credentials = new NetworkCredential(user_name.Normalize(), password.Normalize());
            

            // Read the file's contents into a byte array.
            byte[] bytes = System.IO.File.ReadAllBytes(filename);

            // Write the bytes into the request stream.
            request.ContentLength = bytes.Length;
            using (Stream request_stream = request.GetRequestStream())
            {
                request_stream.Write(bytes, 0, bytes.Length);
                request_stream.Close();
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
