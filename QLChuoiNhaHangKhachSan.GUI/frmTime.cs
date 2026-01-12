using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public partial class frmTime : Form
    {
        private readonly DateTime _baseDate;
        private int _hour;
        private int _minute;
        private bool _isPm;

        public DateTime SelectedDateTime { get; private set; }

        public frmTime(DateTime baseDate)
        {
            _baseDate = baseDate;
            _hour = baseDate.Hour;
            _minute = baseDate.Minute - (baseDate.Minute % 5);
            _isPm = _hour >= 12;
            if (_hour == 0) _hour = 12;
            else if (_hour > 12) _hour -= 12;

            InitializeComponent();

            this.Shown += (s, e) =>
            {
                BuildHourButtons();
                BuildMinuteButtons();
                UpdateDisplay();
            };

            _btnOk.Click += (s, e) =>
            {
                int hour24 = _hour % 12;
                if (_isPm) hour24 += 12;
                if (!_isPm && hour24 == 12) hour24 = 0;
                SelectedDateTime = new DateTime(_baseDate.Year, _baseDate.Month, _baseDate.Day, hour24, _minute, 0);
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            _btnPm.Click += (s, e) => { _isPm = true; UpdateDisplay(); };
            _btnAm.Click += (s, e) => { _isPm = false; UpdateDisplay(); };

            _pnlHours.Resize += (s, e) => BuildHourButtons();
            _pnlMinutes.Resize += (s, e) => BuildMinuteButtons();
        }

        private void BuildHourButtons()
        {
            _pnlHours.Controls.Clear();
            int radius = Math.Min(_pnlHours.Width, _pnlHours.Height) / 2 - 20;
            Point center = new Point(Math.Max(1, _pnlHours.Width / 2), Math.Max(1, _pnlHours.Height / 2));

            for (int i = 1; i <= 12; i++)
            {
                double angle = (i - 3) * 30 * Math.PI / 180;
                int x = center.X + (int)(radius * Math.Cos(angle)) - 18;
                int y = center.Y + (int)(radius * Math.Sin(angle)) - 18;

                var btn = new Button
                {
                    Text = i.ToString(),
                    Width = 36,
                    Height = 36,
                    Location = new Point(x, y),
                    Tag = i
                };
                btn.Click += (s, e) =>
                {
                    _hour = (int)((Button)s).Tag;
                    _pnlHours.Visible = false;
                    _pnlMinutes.Visible = true;
                    UpdateDisplay();
                };
                _pnlHours.Controls.Add(btn);
            }
        }

        private void BuildMinuteButtons()
        {
            _pnlMinutes.Controls.Clear();
            int radius = Math.Min(_pnlMinutes.Width, _pnlMinutes.Height) / 2 - 20;
            Point center = new Point(Math.Max(1, _pnlMinutes.Width / 2), Math.Max(1, _pnlMinutes.Height / 2));

            for (int minute = 0; minute < 60; minute += 5)
            {
                double angle = (minute / 5.0 - 3) * 30 * Math.PI / 180;
                int x = center.X + (int)(radius * Math.Cos(angle)) - 18;
                int y = center.Y + (int)(radius * Math.Sin(angle)) - 18;

                var btn = new Button
                {
                    Text = minute.ToString("00"),
                    Width = 36,
                    Height = 36,
                    Location = new Point(x, y),
                    Tag = minute
                };
                btn.Click += (s, e) =>
                {
                    _minute = (int)((Button)s).Tag;
                    _pnlMinutes.Visible = false;
                    _pnlHours.Visible = true;
                    UpdateDisplay();
                };
                _pnlMinutes.Controls.Add(btn);
            }
        }

        private void UpdateDisplay()
        {
            string ampm = _isPm ? "PM" : "AM";
            _lblTime.Text = string.Format("{0:00}:{1:00} {2}", _hour == 0 ? 12 : _hour, _minute, ampm);

            _btnAm.BackColor = _isPm ? SystemColors.Control : Color.MediumPurple;
            _btnAm.ForeColor = _isPm ? Color.Black : Color.White;
            _btnPm.BackColor = _isPm ? Color.MediumPurple : SystemColors.Control;
            _btnPm.ForeColor = _isPm ? Color.White : Color.Black;
        }
    }
}
