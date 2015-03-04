using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caros.CI.Publisher
{
    public partial class FormMain : Form
    {
        DateTime _startTime;

        public FormMain()
        {
            InitializeComponent();
        }

        void publisher_OnUpdateProgress(float percentage)
        {
            progressAll.Value = (int)percentage;
        }

        void publisher_OnSuccess(string message)
        {
            Post(message, Publisher.EventTypes.Success);
        }

        void publisher_OnFinishedAll(string message)
        {
            publisher_OnSuccess(message);
            publisher_OnUpdateProgress(100);
        }

        void publisher_OnFailure(string message)
        {
            Post(message, Publisher.EventTypes.Failure);
        }

        void publisher_OnInfo(string message)
        {
            Post(message, Publisher.EventTypes.Info);
        }

        private void Post(string message, Publisher.EventTypes result)
        {
            var timeInfo = (DateTime.Now - _startTime).ToString(@"mm\:ss\.fff");
            this.Invoke((MethodInvoker)delegate { listEvents.Items.Add(message, GetImageKeyFromEventType(result)).SubItems.Add(timeInfo); });
        }

        private string GetImageKeyFromEventType(Publisher.EventTypes result)
        {
            switch (result)
            {
                case Publisher.EventTypes.Success:
                    return "good";
                case Publisher.EventTypes.Failure:
                    return "bad";
                case Publisher.EventTypes.Info:
                    return "info";
                default:
                    throw new InvalidOperationException("Unexpected EventType in GetImageKeyFromEventType '" + result.ToString() + "'");
            }
        }

        bool _formActivated = false;
        private void FormMain_Activated(object sender, EventArgs e)
        {
            if (_formActivated)
                return;

            _formActivated = true;

            var solutionPath = Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();

            if (solutionPath == null)
            {
                MessageBox.Show("Please give first command as location to Caros4 solution", "Cannot publish", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                this.Close();
                return;
            }

            _startTime = DateTime.Now;

            var publisher = new Publisher(solutionPath);

            publisher.OnInfo += publisher_OnInfo;
            publisher.OnFailure += publisher_OnFailure;
            publisher.OnFinishedAll += publisher_OnFinishedAll;
            publisher.OnSuccess += publisher_OnSuccess;
            publisher.OnUpdateProgress += publisher_OnUpdateProgress;

            publisher.Start();
        }
    }
}
