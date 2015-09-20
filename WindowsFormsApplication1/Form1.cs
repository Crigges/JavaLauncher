using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string jar;

        public Form1(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("please enter the jar you want to start as parameter");
                Environment.Exit(0);
            }
            else
            {
                jar = args[0];
                string key = getKey();
                if (key == null)
                { 
                    if (checkStandardJava())
                    {
                        setKey("standard");
                        startJarWithStandardJava(args[0]);
                    }
                    else
                    {
                        InitializeComponent();
                    }
                }
                else if (key.Equals("standard"))
                {
                    startJarWithStandardJava(args[0]);
                }
                else
                {
                    startJarWithJRE(key, args[0]);
                }
            }
        }


        private bool checkJRE(string jre)
        {
            string name = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllBytes(name, WindowsFormsApplication1.Properties.Resources.versiontest);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c \"" + jre + "\" -jar " + name;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();
            Console.WriteLine(output);
            if (output.Length < 4)
            {
                return false;
            }
            return output.Substring(0, 4).Equals("succ");
        }

        private void startJarWithJRE(string jre, string path)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c \"" + jre + "\" -jar " + path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (err != null && err.Length > 1)
            {
                MessageBox.Show(err, "Wurst Launcher",
                      MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            Environment.Exit(0);
        }

        private string getKey() {
            Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey key2 = rkey.OpenSubKey("Software\\Wurst");
            if(key2 == null)
            {
                return null;
            }
            object o = key2.GetValue("jrehome");
            if(o == null)
            {
                return null;
            }
            string s = (string) o;
            key2.Close();
            return s;
        }

        private void setKey(string key){
            Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser;
            Microsoft.Win32.RegistryKey rkey3 = rkey.CreateSubKey("Software\\Wurst");
            rkey3.SetValue("jrehome", key);
        }

        private void startJarWithStandardJava(string path){
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c java -jar " + path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (err!= null && err.Length > 1)
            {
                MessageBox.Show(err, "Wurst Launcher",
                      MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            Environment.Exit(0);
        }

        private bool checkStandardJava(){
            string name = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllBytes(name, WindowsFormsApplication1.Properties.Resources.versiontest);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c java -jar \"" + name + "\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (output.Length < 4)
            {
                return false;
            }
            return output.Substring(0, 4).Equals("succ");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(textBox1.Text + "\\bin\\java.exe"))
            {
                MessageBox.Show("No JRE was found", "Wurst Launcher",
                    MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                if (checkJRE(textBox1.Text + "\\bin\\java.exe"))
                {
                    setKey(textBox1.Text + "\\bin\\java.exe");
                    startJarWithJRE(textBox1.Text + "\\bin\\java.exe", jar);
                }
                else
                {
                    MessageBox.Show("JRE was found but is outdated pls choose a JRE 8", "Wurst Launcher",
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //download java
            System.Diagnostics.Process.Start("http://www.oracle.com/technetwork/java/javase/downloads/jre8-downloads-2133155.html");
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //exit launcher
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFileDialog1 = new FolderBrowserDialog();

            // Set filter options and filter index.


            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOK = openFileDialog1.ShowDialog();

            textBox1.Text = openFileDialog1.SelectedPath;
            textBox1.Update();

        }

    }
}
