namespace Lab2_CSharp
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            ucLogIn1.Hide();
            ucRegister1.Show();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            ucRegister1.Hide();
            ucLogIn1.Show();
        }
    }
}