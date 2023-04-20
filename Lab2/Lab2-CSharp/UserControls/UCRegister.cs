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
    public partial class UCRegister : UserControl
    {
        public UCRegister()
        {
            InitializeComponent();
        }

        private void WriteFileText()
        {
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter("Test.txt");
                sw.WriteLine("Name\tPhone\tEmail\tPass");
                sw.WriteLine(tbUsername.Text + "\t" + tbPhone.Text + "\t" + tbEmail.Text + "\t" + tbPass.Text);
                //Close the file
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        private void btnRegisterN_Click(object sender, EventArgs e)
        {
            WriteFileText();
        }
    }
}
