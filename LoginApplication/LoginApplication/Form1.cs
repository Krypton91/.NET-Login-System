using LoginApplication.classes;
using System;
using System.Windows.Forms;

namespace LoginApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxUsername.Text)) 
            {
                MessageBox.Show("Error Username is empty!");
            }

            else if (string.IsNullOrEmpty(textBoxPassword.Text)) 
            {
                MessageBox.Show("Error Password is empty!");
            }

            else 
            {
                ProcessLogin(textBoxUsername.Text, textBoxPassword.Text);
            }
        }

        private void ProcessLogin(string Username, string Password) 
        {
            string error = "";
            if(!Webhelper.GetInstance().RequestServerToLogin(Username, Password, out error)) 
            {
                MessageBox.Show($"{error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else 
            {
                MessageBox.Show($"Welcome: {Username}", "Sucesfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
    }
}
