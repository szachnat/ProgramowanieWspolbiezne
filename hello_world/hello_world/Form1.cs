namespace hello_world
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void OnBtnClick(object sender, EventArgs e)
        {
            this.Text = "Hello World";
        }
    }
}
