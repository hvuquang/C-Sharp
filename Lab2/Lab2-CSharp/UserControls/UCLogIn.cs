using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2_CSharp.UserControls
{
    public partial class UCLogIn : UserControl
    {
        string username;
        string password;
        public UCLogIn()
        {
            InitializeComponent();
        }

        void ReadFileText()
        {
            string text = File.ReadAllText("Test.txt");
            string[] acc = text.Split('\n');
            string[] accountProp;
            for (int i = 1; i < acc.Length; i++)
            {
                accountProp = acc[i].Split('\t');
                username = accountProp[0];
                password = accountProp[3];
                if (checkAccount(username, password))
                {
                    Form form = new FormHome();
                    form.Show();
                    break;
                }
                else
                {
                    MessageBox.Show("Invalid");
                    break;
                }
            }
        }

        bool checkAccount(string username, string password)
        {
            if (tbUsername.Text == username)
            {
                if (tbPass.Text == password)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadFileText();
        }
    }
}
