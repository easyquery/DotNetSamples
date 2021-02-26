using System;
using System.Threading;
using System.Windows.Forms;

namespace EqDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs ev)
        {
            button1.Invoke((MethodInvoker)delegate {
                button1.Text = "Loading...";
                button1.Enabled = false;
            });

            var eqForm = new EasyQueryForm();
            eqForm.FormClosed += (s, e) => Application.Exit();
            Hide();
            eqForm.Show();
        }
    }
}
