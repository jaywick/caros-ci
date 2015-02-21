using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caros.Publisher
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        DateTime _startTime;

        private void FormMain_Load(object sender, EventArgs e)
        {
            _startTime = DateTime.Now;

            var publisher = new Publisher(@"C:\Users\Jay\Dropbox\Developer\Workspace\ProjectPublisher\ProjectPublisher");

            publisher.OnFailure += publisher_OnFailure;
            publisher.OnFinishedAll += publisher_OnFinishedAll;
            publisher.OnSuccess += publisher_OnSuccess;
            publisher.OnUpdateProgress += publisher_OnUpdateProgress;

            publisher.Start();
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

        private void Post(string message, Publisher.EventTypes result)
        {
            var timeInfo = (DateTime.Now - _startTime).ToString(@"mm\:ss\.fff");
            listEvents.Items.Add(message, GetImageKeyFromEventType(result)).SubItems.Add(timeInfo);
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
    }
}
