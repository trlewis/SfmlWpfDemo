namespace SfmlWpfDemo
{
    using SFML.Graphics;
    using SFML.System;
    using SFML.Window;
    using System;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private CircleShape _circle;
        private byte _color = 0;        
        private RenderWindow _renderWindow;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            this._circle = new CircleShape(20) { FillColor = Color.Magenta };
            this.CreateRenderWindow();

            this._timer = new DispatcherTimer{Interval = new TimeSpan(0,0,0,0,1000/60)};
            this._timer.Tick += Timer_Tick;
            this._timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeCircleColor();
        }

        private void ChangeCircleColor()
        {
            var rand = new Random();
            var color = new Color((byte)rand.Next(), (byte)rand.Next(), (byte)rand.Next());
            this._circle.FillColor = color;
        }

        private void CreateRenderWindow()
        {
            if(this._renderWindow != null)
            {
                this._renderWindow.SetActive(false);
                this._renderWindow.Dispose();
            }

            var context = new ContextSettings { DepthBits = 24 };
            this._renderWindow = new RenderWindow(this.DrawSurface.Handle, context);
            this._renderWindow.MouseButtonPressed += RenderWindow_MouseButtonPressed;
            this._renderWindow.KeyPressed += RenderWindow_KeyPressed;
            this._renderWindow.SetActive(true);
        }

        private void RenderWindow_KeyPressed(object sender, KeyEventArgs e)
        {
            this.ChangeCircleColor();
        }

        private void RenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            this._circle.Position = new Vector2f(e.X, e.Y);
        }

        private void DrawSurface_SizeChanged(object sender, EventArgs e)
        {
            this.CreateRenderWindow();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //System.Windows.Forms.Application.DoEvents(); //doesn't seem to help.
 	        this._renderWindow.DispatchEvents();

            this._renderWindow.Clear(new Color((byte)this._color, (byte)this._color, (byte)this._color));
            this._color = this._color >= 255 ? (byte) 0 : (byte)(this._color + 1);

            this._renderWindow.Draw(this._circle);

            this._renderWindow.Display();
        }
    }
}
