using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectPublisher
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Add("Started", 10);
        }

        private void Add(string message, float progress, bool success = true)
        {
            listEvents.Items.Add(message, success ? "good" : "bad").SubItems.Add(DateTime.Now.TimeOfDay.ToString());
            progressAll.Value = (int)progress;
        }
    }
}
