using System;
using System.Windows.Forms;

namespace ZeroKLobby.Notifications
{
    public partial class NotifyBarContainer: UserControl
    {
        public INotifyBar BarContent { get; protected set; }
        public NotifyBarContainer() {}

        public NotifyBarContainer(INotifyBar barContent)
        {
            BarContent = barContent;
            InitializeComponent();
            btnDetail.Click += detail_Click;
            btnStop.Click += stop_Click;
            btnStop.Image = Resources.Remove;
            var control = barContent.GetControl();
            Height = control.Height; //inherit height from barContent
            tableLayoutPanel1.Controls.Add(control, 1, 0);
            control.Dock = DockStyle.Fill;
            Dock = DockStyle.Top;
            BarContent.AddedToContainer(this);
        }


        protected virtual void detail_Click(object sender, EventArgs e)
        {
            BarContent.DetailClicked(this);
        }


        protected virtual void stop_Click(object sender, EventArgs e)
        {
            BarContent.CloseClicked(this);
        }
    }
}