using System;
using System.Drawing;
using System.Windows.Forms;

namespace VirtualBoxDiskCreator
{
    class MsgBox : Form
    {
        private readonly Timer _timer;
        private readonly Font _font;
        private readonly Brush _brush;
        private readonly StringFormat _stringFormat;
        public int Duration { get; set; }
        public int CharSize { get; set; } = 12;
        public string Message { get; set; }

        public MsgBox() : this("") { }

        public MsgBox(string text) : this(text, 80) { }

        public MsgBox(string text, int duration)
        {
            _font = new Font
                (
                Font.FontFamily,
                CharSize,
                Font.Style,
                Font.Unit,
                Font.GdiCharSet,
                Font.GdiVerticalFont
                );
            _stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            _brush = new SolidBrush(Color.White);
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.FromArgb(30, 30, 30);
            FormBorderStyle = FormBorderStyle.None;
            Opacity = 0;
            ShowIcon = false;
            ShowInTaskbar = false;
            Message = text;
            Duration = duration;
            _timer = new Timer();
            _timer.Tick += new EventHandler(Tick);
            _timer.Interval = 1;
        }

        private void Tick(object sender, EventArgs e)
        {
            if (Duration > 0 && Opacity < 1)
            {
                Opacity += 0.1;
            }
            else
            {
                if (Duration > 0)
                {
                    Duration--;
                }
                else if (Opacity > 0)
                {
                    Opacity -= 0.01;
                }
                else
                {
                    Dispose();
                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Dispose();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Location = new Point(Owner.Location.X + 6, Owner.Location.Y + Owner.Height - 66);
            Size = new Size(Owner.Width - 12, 60);
            _timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rect = new Rectangle(new Point(), Size);
            e.Graphics.DrawString(Message, _font, _brush, rect, _stringFormat);
            if (!Owner.Focused) Owner.Focus();
        }

        public new void Show()
        {
            if (Owner == null)
            {
                throw new ArgumentNullException("Owner is Null.\nUse Show(this)");
            }
        }

        public new void ShowDialog()
        {
            if (Owner == null)
            {
                throw new ArgumentNullException("Owner is Null.\nUse Show(this)");
            }
        }

        public new void Dispose()
        {
            _stringFormat.Dispose();
            _font.Dispose();
            _brush.Dispose();
            _timer.Dispose();
            base.Dispose();
        }
    }
}