using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLChuoiNhaHangKhachSan.GUI
{
    public class TimePickerForm : Form
    {
        private readonly DateTime _baseDate;
        private int _hour;
        private int _minute;
        private bool _isPm;

        private Label _lblTime;
        private Panel _pnlHours;
        private Panel _pnlMinutes;
        private Button _btnOk;
        private Button _btnAm;
        private Button _btnPm;

        public DateTime SelectedDateTime { get; private set; }

        public TimePickerForm(DateTime baseDate)
        {
            _baseDate = baseDate;
            _hour = baseDate.Hour;
            _minute = baseDate.Minute - (baseDate.Minute % 5);
            _isPm = _hour >= 12;
            if (_hour == 0) _hour = 12; // midnight shows as 12 AM
            else if (_hour > 12) _hour -= 12;

            InitializeUi();
            BuildHourButtons();
            BuildMinuteButtons();
            UpdateDisplay();
        }

        private void InitializeUi()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(320, 420);
            this.Text = "Ch\u1ECDn gi\u1EDD"; // Ch?n gi?
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            _lblTime = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 80,
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _pnlHours = new Panel
            {
                Dock = DockStyle.Top,
                Height = 220,
                Padding = new Padding(10)
            };

            _pnlMinutes = new Panel
            {
                Dock = DockStyle.Top,
                Height = 220,
                Padding = new Padding(10),
                Visible = false
            };

            var bottomPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };

            _btnOk = new Button
            {
                Text = "OK",
                AutoSize = true,
                Padding = new Padding(12, 6, 12, 6)
            };
            _btnOk.Click += (s, e) =>
            {
                int hour24 = _hour % 12;
                if (_isPm) hour24 += 12;
                if (!_isPm && hour24 == 12) hour24 = 0; // 12 AM -> 0
                SelectedDateTime = new DateTime(_baseDate.Year, _baseDate.Month, _baseDate.Day, hour24, _minute, 0);
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            _btnPm = new Button { Text = "PM", AutoSize = true, Padding = new Padding(10, 6, 10, 6) };
            _btnPm.Click += (s, e) => { _isPm = true; UpdateDisplay(); };

            _btnAm = new Button { Text = "AM", AutoSize = true, Padding = new Padding(10, 6, 10, 6) };
            _btnAm.Click += (s, e) => { _isPm = false; UpdateDisplay(); };

            bottomPanel.Controls.Add(_btnOk);
            bottomPanel.Controls.Add(_btnPm);
            bottomPanel.Controls.Add(_btnAm);

            this.Controls.Add(bottomPanel);
            this.Controls.Add(_pnlMinutes);
            this.Controls.Add(_pnlHours);
            this.Controls.Add(_lblTime);
        }

        private void BuildHourButtons()
        {
            _pnlHours.Controls.Clear();
            int radius = 90;
            Point center = new Point(_pnlHours.Width / 2, _pnlHours.Height / 2);
            _pnlHours.Resize += (s, e) => BuildHourButtons();

            for (int i = 1; i <= 12; i++)
            {
                double angle = (i - 3) * 30 * Math.PI / 180; // start at 12 o'clock
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
            int radius = 90;
            Point center = new Point(_pnlMinutes.Width / 2, _pnlMinutes.Height / 2);
            _pnlMinutes.Resize += (s, e) => BuildMinuteButtons();

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
