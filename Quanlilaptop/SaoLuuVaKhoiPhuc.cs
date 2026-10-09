using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;


namespace Quanlilaptop
{
    public partial class SaoLuuVaKhoiPhuc : Form
    {
        string mysqlPath = @"D:\xampp\mysql\bin\";
        string database = "quanlimaytinh";
        string user = "root";
        string password = "";
        private void EncryptFile(string inputFile, string outputFile, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32));
            byte[] ivBytes = Encoding.UTF8.GetBytes("1234567890123456");

            using (FileStream fsInput = new FileStream(inputFile, FileMode.Open))
            using (FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create))
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                using (CryptoStream cs = new CryptoStream(fsEncrypted, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    fsInput.CopyTo(cs);
                }
            }
        }
        private void DecryptFile(string inputFile, string outputFile, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.PadRight(32));
            byte[] ivBytes = Encoding.UTF8.GetBytes("1234567890123456");
            using (FileStream fsEncrypted = new FileStream(inputFile, FileMode.Open))
            using (FileStream fsDecrypted = new FileStream(outputFile, FileMode.Create))
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (CryptoStream cs = new CryptoStream(fsEncrypted, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.CopyTo(fsDecrypted);
                }
            }
        }


        public SaoLuuVaKhoiPhuc()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Chọn nơi lưu file sao lưu";
                sfd.Filter = "SQL files (*.sql)|*.sql";
                sfd.FileName = "backup.sql";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    txtDuongDan.Text = sfd.FileName;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDuongDan.Text))
            {
                string dumpExe = Path.Combine(mysqlPath, "mysqldump.exe");

                if (!File.Exists(dumpExe))
                {
                    MessageBox.Show("Không tìm thấy mysqldump.exe! Vui lòng kiểm tra lại đường dẫn.");
                    return;
                }

                string tempSqlFile = txtDuongDan.Text;
                string encryptedFile = Path.ChangeExtension(tempSqlFile, ".sql.enc");

                string arguments = $"--user={user} {(string.IsNullOrEmpty(password) ? "" : $"--password=\"{password}\"")} --databases {database} --result-file=\"{tempSqlFile}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = dumpExe,
                    Arguments = arguments,
                    RedirectStandardOutput = false,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                try
                {
                    Process p = Process.Start(psi);
                    p.WaitForExit();

                    string encryptionKey = "mysecurepassword";
                    EncryptFile(tempSqlFile, encryptedFile, encryptionKey);
                    File.Delete(tempSqlFile);

                    MessageBox.Show("Sao lưu và mã hóa thành công!\nFile: " + encryptedFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sao lưu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nơi lưu file sao lưu!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chọn file sao lưu đã mã hóa (.sql.enc)";
                ofd.Filter = "Encrypted SQL files (*.sql.enc)|*.sql.enc";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtDuongDan.Text = ofd.FileName;

                    if (!File.Exists(ofd.FileName))
                    {
                        MessageBox.Show("File .sql.enc không tồn tại!");
                        return;
                    }

                    string mysqlExe = Path.Combine(mysqlPath, "mysql.exe");

                    if (!File.Exists(mysqlExe))
                    {
                        MessageBox.Show("Không tìm thấy mysql.exe! Vui lòng kiểm tra lại đường dẫn.");
                        return;
                    }

                    string decryptedSql = Path.ChangeExtension(ofd.FileName, ".tmp.sql");
                    string encryptionKey = "mysecurepassword"; 

                    try
                    {
                        DecryptFile(ofd.FileName, decryptedSql, encryptionKey);

                        string restoreCommand = $"\"{mysqlExe}\" --user={user} {(string.IsNullOrEmpty(password) ? "" : $"--password=\"{password}\"")} {database} < \"{decryptedSql}\"";

                        ProcessStartInfo psi = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c {restoreCommand}",
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };

                        Process p = Process.Start(psi);
                        p.WaitForExit();

                        MessageBox.Show("Khôi phục thành công!");

                        File.Delete(decryptedSql);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi khôi phục: " + ex.Message);
                    }
                }
            }
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
