using System.Drawing;

namespace System.Windows.Forms
{
    class MsgBox : Form
    {
        public enum ThemeMode : byte
        {
            Dark,
            Light
        }

        private const int MaxHeight = 60;
        private const int MinWidth = 200;
        private const int MaxWidth = 500;

        private readonly Timer _timer;
        private readonly StringFormat _stringFormat;
        private readonly SolidBrush _fontBrush;
        private ThemeMode _currentTheme;
        private Font _font;
        private string _text;
        private int _duration;

        public ThemeMode Theme
        {
            get
            {
                return _currentTheme;
            }
            set
            {
                _currentTheme = value;
                if (_currentTheme == ThemeMode.Dark)
                {
                    BackColor = Color.FromArgb(30, 30, 30);
                    _fontBrush.Color = Color.White;
                }
                else
                {
                    BackColor = Color.White;
                    _fontBrush.Color = Color.Black;
                }
                Invalidate();
            }
        }

        public new Font Font
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                Invalidate();
            }
        }

        public new string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Invalidate();
            }
        }

        public int Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value > 0 ? value : 0;
            }
        }

        public MsgBox() : this("") { }

        public MsgBox(string text) : this(text, 80) { }

        public MsgBox(string text, int duration, ThemeMode theme) : this(text, duration)
        {
            Theme = theme;
        }

        public MsgBox(string text, int duration)
        {
            _font = new Font
                (
                base.Font.FontFamily,
                12,
                base.Font.Style,
                base.Font.Unit,
                base.Font.GdiCharSet,
                base.Font.GdiVerticalFont
                );
            _stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            _fontBrush = new SolidBrush(Color.White);
            BackColor = Color.FromArgb(30, 30, 30);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Opacity = 0;
            ShowIcon = false;
            ShowInTaskbar = false;
            Duration = duration;
            Height = MaxHeight;
            _text = text;
            _timer = new Timer();
            _timer.Tick += new EventHandler(Tick);
            _timer.Interval = 1;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Tick(object sender, EventArgs e)
        {
            if (_duration > -1 && Opacity < 1)
            {
                Opacity += 0.1;
            }
            else
            {
                if (_duration > -1)
                {
                    _duration--;
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
            if (Owner.Width > MaxWidth) Width = MaxWidth;
            else Width = Owner.Width < MinWidth ? MinWidth : Owner.Width - 12;
            Location = new Point(Owner.Location.X + (Owner.Width - Width) / 2,
                Owner.Location.Y + Owner.Height - Height - 6);
            _timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle rect = new Rectangle(new Point(), Size);
            e.Graphics.DrawString(_text, _font, _fontBrush, rect, _stringFormat);
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
            _fontBrush.Dispose();
            _timer.Dispose();
            _font.Dispose();
            base.Dispose();
        }
    }
}