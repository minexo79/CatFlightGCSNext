using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Shapes;

namespace NPilotHUD
{
    public enum Style
    {
        invalid,
        standard,
        standard_reverse,
        nirex         
    }

    public class BorderTextLabel : Label
    {

        private static void Redraw(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BorderTextLabel)d).InvalidateVisual();
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BorderTextLabel), new FrameworkPropertyMetadata(string.Empty, Redraw));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(BorderTextLabel), new FrameworkPropertyMetadata(Brushes.Black, Redraw));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(BorderTextLabel), new FrameworkPropertyMetadata((double)1, Redraw));

        FormattedText ft;
        Typeface tf;
        Point startp;
        Pen pen;
        bool cache = true;

        public BorderTextLabel()
        {
            pen = new Pen(Stroke, StrokeThickness);
            startp = new Point(0, 0);
            tf = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        }
        public void CreateCache()
        {
            pen = new Pen(Stroke, StrokeThickness);
            startp = new Point(0, 0);
            tf = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            ft = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground);

            if (double.IsNaN(Width))
                Width = ft.Width;
            if (double.IsNaN(Height))
                Height = ft.Height;

            if (HorizontalContentAlignment == HorizontalAlignment.Right) startp.X = Width - ft.Width;
            if (HorizontalContentAlignment == HorizontalAlignment.Center) startp.X = (Width - ft.Width) / 2;
            if (VerticalContentAlignment == VerticalAlignment.Bottom) startp.X = Height - ft.Height;
            if (VerticalContentAlignment == VerticalAlignment.Center) startp.X = (Height - ft.Height) / 2;
            var textgeometry = ft.BuildGeometry(startp);
            drawingContext.DrawGeometry(Foreground, pen, textgeometry);

            if (cache)
            {
                cache = !cache;
                CreateCache();
            }
        }

    }
    public partial class NHudControl : UserControl
    {
        private const double FPM_RADIUS = 24;
        private const double FPM_LINE_LEN = 6;

        private TransformGroup transformGroup;
        private RotateTransform rotateTransform;
        private TranslateTransform betaTransform;
        private HudCache cache;
        public NHudControl()
        {
            MaxVerticalSpeedArrowMag = 50;
            InitializeComponent();

            Style s = NPilotHUD.Style.standard_reverse;

            WhiteBrush = new SolidColorBrush(Colors.White);
            BlackBrush = new SolidColorBrush(Colors.Black);
            RedBrush = new SolidColorBrush(Colors.Red);
            InvisiBrush = new SolidColorBrush(Color.FromArgb(0x0, 0x0, 0x0, 0x0));

            if (s == NPilotHUD.Style.standard)
            {
                HudBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xFF, 0x00));
                CommandBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                GroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x99, 0x66, 0x33));
                SkyBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA2, 0xFF));
            }
            else if(s == NPilotHUD.Style.standard_reverse)
            {
                CommandBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xFF, 0x00));
                HudBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
                GroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x99, 0x66, 0x33));
                SkyBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0xA2, 0xFF));
            }
            else if (s == NPilotHUD.Style.nirex)
            {
                HudBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x1C, 0xED, 0x9c));
                SkyBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x8C, 0x72, 0xFF));
                GroundBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x12, 0x15, 0x1E));
                CommandBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));
            }


            cache = new HudCache(HudBrush, CommandBrush, GroundBrush, SkyBrush);

            transformGroup = new TransformGroup();
            rotateTransform = new RotateTransform();
            betaTransform = new TranslateTransform();
        }
        private static void GestureChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NHudControl)d).InvalidateVisual();
        }


        #region Properties

        public SolidColorBrush InvisiBrush { get; set; }
        public SolidColorBrush HudBrush { get; set; }
        public SolidColorBrush CommandBrush { get; set; }
        public SolidColorBrush GroundBrush { get; set; }
        public SolidColorBrush SkyBrush { get; set; }
        public SolidColorBrush WhiteBrush { get; set; }
        public SolidColorBrush BlackBrush { get; set; }
        public SolidColorBrush RedBrush { get; set; }

        public EGPSMode GPSMode
        {
            get { return (EGPSMode)GetValue(GPSModeProperty); }
            set { SetValue(GPSModeProperty, value); }
        }
        public static readonly DependencyProperty GPSModeProperty =
            DependencyProperty.Register("GPSMode", typeof(EGPSMode), typeof(NHudControl), new FrameworkPropertyMetadata(EGPSMode.Invalid, GestureChangedCallback));

        public bool Lag
        {
            get { return (bool)GetValue(LagProperty); }
            set { SetValue(LagProperty, value); }
        }
        public static readonly DependencyProperty LagProperty =
            DependencyProperty.Register("Lag", typeof(bool), typeof(NHudControl), new FrameworkPropertyMetadata(true, GestureChangedCallback));


        public int AltitudeCommandTolerance
        {
            get { return (int)GetValue(AltitudeCommandToleranceProperty); }
            set { SetValue(AltitudeCommandToleranceProperty, value); }
        }
        public static readonly DependencyProperty AltitudeCommandToleranceProperty =
            DependencyProperty.Register("AltitudeCommandTolerance", typeof(int), typeof(NHudControl), new FrameworkPropertyMetadata(10, GestureChangedCallback));

        public int GroundSpeedCommandTolerance
        {
            get { return (int)GetValue(GroundSpeedCommandToleranceProperty); }
            set { SetValue(GroundSpeedCommandToleranceProperty, value); }
        }
        public static readonly DependencyProperty GroundSpeedCommandToleranceProperty =
            DependencyProperty.Register("GroundSpeedCommandTolerance", typeof(int), typeof(NHudControl), new FrameworkPropertyMetadata(10, GestureChangedCallback));

        public int GroundSpeedCommand
        {
            get { return (int)GetValue(GroundSpeedCommandProperty); }
            set { SetValue(GroundSpeedCommandProperty, value); }
        }
        public static readonly DependencyProperty GroundSpeedCommandProperty =
            DependencyProperty.Register("GroundSpeedCommand", typeof(int), typeof(NHudControl), new FrameworkPropertyMetadata(0, GestureChangedCallback));

        public int AltitudeCommand
        {
            get { return (int)GetValue(AltitudeCommandProperty); }
            set { SetValue(AltitudeCommandProperty, value); }
        }
        public static readonly DependencyProperty AltitudeCommandProperty =
            DependencyProperty.Register("AltitudeCommand", typeof(int), typeof(NHudControl), new FrameworkPropertyMetadata(0, GestureChangedCallback));

        public string AltitudeUnit
        {
            get { return (string)GetValue(AltitudeUnitProperty); }
            set { SetValue(AltitudeUnitProperty, value); }
        }
        public static readonly DependencyProperty AltitudeUnitProperty =
            DependencyProperty.Register("AltitudeUnit", typeof(string), typeof(NHudControl), new FrameworkPropertyMetadata("m", GestureChangedCallback));

        public bool StaticRoll
        {
            get { return (bool)GetValue(StaticRollProperty); }
            set { SetValue(StaticRollProperty, value); }
        }
        public static readonly DependencyProperty StaticRollProperty =
            DependencyProperty.Register("StaticRoll", typeof(bool), typeof(NHudControl), new FrameworkPropertyMetadata(true, GestureChangedCallback));

        public double RollCommand
        {
            get { return (double)GetValue(RollCommandProperty); }
            set { SetValue(RollCommandProperty, value); }
        }
        public static readonly DependencyProperty RollCommandProperty =
            DependencyProperty.Register("RollCommand", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double PitchCommand
        {
            get { return (double)GetValue(PitchCommandProperty); }
            set { SetValue(PitchCommandProperty, value); }
        }
        public static readonly DependencyProperty PitchCommandProperty =
            DependencyProperty.Register("PitchCommand", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double YawCommand
        {
            get { return (double)GetValue(YawCommandProperty); }
            set { SetValue(YawCommandProperty, value); }
        }
        public static readonly DependencyProperty YawCommandProperty =
            DependencyProperty.Register("YawCommand", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));
        public double WayPointDistance
        {
            get { return (double)GetValue(WayPointDistanceProperty); }
            set { SetValue(WayPointDistanceProperty, value); }
        }
        public static readonly DependencyProperty WayPointDistanceProperty =
            DependencyProperty.Register("WayPointDistance", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double WayPointNumber
        {
            get { return (double)GetValue(WayPointNumberProperty); }
            set { SetValue(WayPointNumberProperty, value); }
        }
        public static readonly DependencyProperty WayPointNumberProperty =
            DependencyProperty.Register("WayPointNumber", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double GroundSpeed
        {
            get { return (double)GetValue(GroundSpeedProperty); }
            set { SetValue(GroundSpeedProperty, value); }
        }
        public static readonly DependencyProperty GroundSpeedProperty =
            DependencyProperty.Register("GroundSpeed", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double AirSpeed
        {
            get { return (double)GetValue(AirSpeedProperty); }
            set { SetValue(AirSpeedProperty, value); }
        }
        public static readonly DependencyProperty AirSpeedProperty =
            DependencyProperty.Register("AirSpeed", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double RollAngle
        {
            get { return (double)GetValue(RollAngleProperty); }
            set { SetValue(RollAngleProperty, value); }
        }
        public static readonly DependencyProperty RollAngleProperty =
            DependencyProperty.Register("RollAngle", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double PitchAngle
        {
            get { return (double)GetValue(PitchAngleProperty); }
            set { SetValue(PitchAngleProperty, value); }
        }
        public static readonly DependencyProperty PitchAngleProperty =
            DependencyProperty.Register("PitchAngle", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double Altitude
        {
            get { return (double)GetValue(AltitudeProperty); }
            set { SetValue(AltitudeProperty, value); }
        }
        public static readonly DependencyProperty AltitudeProperty =
            DependencyProperty.Register("Altitude", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double VerticalSpeed
        {
            get { return (double)GetValue(VerticalSpeedProperty); }
            set { SetValue(VerticalSpeedProperty, value); }
        }
        public static readonly DependencyProperty VerticalSpeedProperty =
            DependencyProperty.Register("VerticalSpeed", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double MaxVerticalSpeedArrowMag
        {
            get { return (double)GetValue(MaxVerticalSpeedArrowMagProperty); }
            set
            {
                if (value > 0)
                {
                    SetValue(MaxVerticalSpeedArrowMagProperty, value);
                }
            }
        }
        public static readonly DependencyProperty MaxVerticalSpeedArrowMagProperty =
            DependencyProperty.Register("MaxVerticalSpeedArrowMag", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double YawAngle
        {
            get { return (double)GetValue(YawAngleProperty); }
            set { SetValue(YawAngleProperty, value); }
        }
        public static readonly DependencyProperty YawAngleProperty =
            DependencyProperty.Register("YawAngle", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double Mach
        {
            get { return (double)GetValue(MachProperty); }
            set { SetValue(MachProperty, value); }
        }
        public static readonly DependencyProperty MachProperty =
            DependencyProperty.Register("Mach", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double G_Load
        {
            get { return (double)GetValue(G_LoadProperty); }
            set { SetValue(G_LoadProperty, value); }
        }
        public static readonly DependencyProperty G_LoadProperty =
            DependencyProperty.Register("G_Load", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double SpeedMs
        {
            get { return (double)GetValue(SpeedMsProperty); }
            set { SetValue(SpeedMsProperty, value); }
        }
        public static readonly DependencyProperty SpeedMsProperty =
            DependencyProperty.Register("SpeedMs", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double Alpha
        {
            get { return (double)GetValue(AlphaProperty); }
            set { SetValue(AlphaProperty, value); }
        }
        public static readonly DependencyProperty AlphaProperty =
            DependencyProperty.Register("Alpha", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        public double Beta
        {
            get { return (double)GetValue(BetaProperty); }
            set { SetValue(BetaProperty, value); }
        }
        public static readonly DependencyProperty BetaProperty =
            DependencyProperty.Register("Beta", typeof(double), typeof(NHudControl), new FrameworkPropertyMetadata((double)0, GestureChangedCallback));

        #endregion

        #region Display Properties

        private const int VERTICAL_DEG_TO_DISP = 36;
        private const int HORIZONTAL_DEG_TO_DISP = 45;
        private const int YAW_COMPASS_DEG_TO_DISP = 26;

        #endregion

        #region Draw Methods

        private void DrawGroundAndSky(double pitchDeg)
        {
            double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;

            double offset = pitchDeg * vertPixelsPerDeg * 0.85;

            // want to always make sure the canvas is filled by the sky and/or ground rectangles
            // - therefore, create them oversized and limit max offset, to prevent any issues
            // with silly window sizes/aspect ratios
            double maxDim = Grid_Viewport.ActualWidth;
            if (Grid_Viewport.ActualHeight > maxDim)
            {
                maxDim = Grid_Viewport.ActualHeight;
            }
            if (offset > maxDim)
            {
                offset = maxDim;
            }
            else if (offset < -maxDim)
            {
                offset = -maxDim;
            }
            const double OVERSIZE_RATIO = 5;
            double rectDimension = maxDim * OVERSIZE_RATIO;

            cache.GroundAndSky.gndRect.Width = rectDimension;
            cache.GroundAndSky.gndRect.Height = rectDimension;
            Canvas.SetLeft(cache.GroundAndSky.gndRect, -maxDim);
            Canvas.SetTop(cache.GroundAndSky.gndRect, offset);

            cache.GroundAndSky.skyRect.Width = rectDimension;
            cache.GroundAndSky.skyRect.Height = rectDimension;
            Canvas.SetLeft(cache.GroundAndSky.skyRect, -maxDim);
            Canvas.SetBottom(cache.GroundAndSky.skyRect, -offset);

            cache.GroundAndSky.line.X1 = -rectDimension;
            cache.GroundAndSky.line.X2 = rectDimension;
            cache.GroundAndSky.line.Y1 = offset;
            cache.GroundAndSky.line.Y2 = offset;
            cache.GroundAndSky.line.Stroke = HudBrush;
            cache.GroundAndSky.line.StrokeThickness = 2;

            Canvas_Background.Children.Add(cache.GroundAndSky.gndRect);
            Canvas_Background.Children.Add(cache.GroundAndSky.skyRect);
            Canvas_Background.Children.Add(cache.GroundAndSky.line);

        }
        private void DrawCommandPitchTick(double value, double pitchDeg, double rollAngle)
        {
            double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;
            vertPixelsPerDeg -= 1.75;
            double zeroOffset = -(pitchDeg * vertPixelsPerDeg);

            double gridOffset = ((Grid_Viewport.ActualHeight - Grid_PitchIndicator.ActualHeight) / 2.0) * Math.Cos(rollAngle * Math.PI / 180.0);
            zeroOffset += gridOffset;

            double pitchVal = value;
            double offset = (pitchVal * vertPixelsPerDeg) + zeroOffset;
            
            Canvas.SetLeft(cache.PitchCommand.line, -60);
            Canvas.SetTop(cache.PitchCommand.line, -offset);
            Canvas_PitchIndicator.Children.Add(cache.PitchCommand.line);
        }
        private void DrawMajorPitchTick(int index, double offset, double val, bool dispTxt)
        {
            Canvas.SetLeft(cache.MajorPitchTick.lnL[index], -80);
            Canvas.SetTop(cache.MajorPitchTick.lnL[index], -offset);
            Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.lnL[index]);

            Canvas.SetLeft(cache.MajorPitchTick.lnR[index], 40);
            Canvas.SetTop(cache.MajorPitchTick.lnR[index], -offset);
            Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.lnR[index]);

            if (val != 0)
            {
                Canvas.SetLeft(cache.MajorPitchTick.left[index], -80);
                Canvas.SetTop(cache.MajorPitchTick.left[index], -offset);

                Canvas.SetRight(cache.MajorPitchTick.right[index], -80);
                Canvas.SetTop(cache.MajorPitchTick.right[index], -offset);

                Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.left[index]);
                Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.right[index]);
            }

            if (cache.MajorPitchTick.disp[index])
            {
                cache.MajorPitchTick.txtBlkL[index].Text = val.ToString("##0");
                Canvas.SetTop(cache.MajorPitchTick.txtBlkL[index], -offset - 3);
                Canvas.SetLeft(cache.MajorPitchTick.txtBlkL[index], -120);
                Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.txtBlkL[index]);

                cache.MajorPitchTick.txtBlkR[index].Text = val.ToString("##0");
                Canvas.SetTop(cache.MajorPitchTick.txtBlkR[index], -offset - 3);
                Canvas.SetRight(cache.MajorPitchTick.txtBlkR[index], -120);
                Canvas_PitchIndicator.Children.Add(cache.MajorPitchTick.txtBlkR[index]);
            }
        }
        private void DrawMinorPitchTick(int index, double offset, double val)
        {
            Canvas.SetLeft(cache.MinorPitchTick.lnL[index], -60);
            Canvas.SetTop(cache.MinorPitchTick.lnL[index], -offset);
            Canvas_PitchIndicator.Children.Add(cache.MinorPitchTick.lnL[index]);

            Canvas.SetLeft(cache.MinorPitchTick.lnR[index], 35);
            Canvas.SetTop(cache.MinorPitchTick.lnR[index], -offset);
            Canvas_PitchIndicator.Children.Add(cache.MinorPitchTick.lnR[index]);

            if (val != 0)
            {
                Canvas.SetLeft(cache.MinorPitchTick.left[index], -60);
                Canvas.SetTop(cache.MinorPitchTick.left[index], -offset);

                Canvas.SetRight(cache.MinorPitchTick.right[index], -60);
                Canvas.SetTop(cache.MinorPitchTick.right[index], -offset);

                Canvas_PitchIndicator.Children.Add(cache.MinorPitchTick.left[index]);
                Canvas_PitchIndicator.Children.Add(cache.MinorPitchTick.right[index]);
            }
        }
        private void DrawPitchTicks(double pitchDeg, double rollAngle)
        {
            
            double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;
            vertPixelsPerDeg -= 1.75;
            double zeroOffset = -(pitchDeg * vertPixelsPerDeg);

            // the pitch indicator grid is only a percentage of the overall viewport height
            // - need to account for this (sso pitch '0' indicator lines up with ground/sky border)
            double gridOffset = ((Grid_Viewport.ActualHeight - Grid_PitchIndicator.ActualHeight) / 2.0) * Math.Cos(rollAngle * Math.PI / 180.0);

            zeroOffset += gridOffset;

            for (int i = 1; i < 18; i += 2)
            {
                double pitchVal = 5 + i * 5;
                double offset = (pitchVal * vertPixelsPerDeg) + zeroOffset;
                DrawMajorPitchTick(i, offset, pitchVal, true);

                offset -= (5 * vertPixelsPerDeg);
                DrawMinorPitchTick(i, offset, pitchVal - 5);

                offset = -(pitchVal * vertPixelsPerDeg) + zeroOffset;
                DrawMajorPitchTick(i + 1, offset, -pitchVal, true);

                offset += (5 * vertPixelsPerDeg);
                DrawMinorPitchTick(i + 1, offset, -pitchVal + 5);
            }
        }
        private void DrawCompass(double yawDeg, double yawCommand)
        {
            double wl = Grid_Compass.ActualWidth;

            double horzPixelsPerDeg = wl / YAW_COMPASS_DEG_TO_DISP;

            double startYaw = yawDeg - (YAW_COMPASS_DEG_TO_DISP / 2.0);
            int roundedStart = (int)Math.Ceiling(startYaw);

            // this is the x co-ord of the left-most tick to be displayed displayed
            double tickOffset = (roundedStart - startYaw) * horzPixelsPerDeg;

           
            // Yaw Command Drawing
            cache.YawCommand.line.X1 = tickOffset + ((yawCommand + 13 - ((int)Math.Ceiling(yawDeg))) * horzPixelsPerDeg);
            cache.YawCommand.line.X2 = cache.YawCommand.line.X1;
            cache.YawCommand.line.Y1 = 15;
            cache.YawCommand.line.Y2 = 30;
            Canvas_Compass.Children.Add(cache.YawCommand.line);
            
            for (int i = 0; i < YAW_COMPASS_DEG_TO_DISP; i++)
            {
                if (0 == ((i + roundedStart) % 2))
                {
                    cache.Compass.tl[i].X1 = tickOffset + (i * horzPixelsPerDeg);
                    cache.Compass.tl[i].X2 = cache.Compass.tl[i].X1;

                    if (0 == ((i + roundedStart) % 10))
                    {
                        cache.Compass.tl[i].Y1 = 21;
                        int txt = (i + roundedStart);
                        if (txt < 0)
                        {
                            txt += 360;
                        }
                        cache.Compass.ticktext[i].Text = txt.ToString("D2");
                        Canvas.SetTop(cache.Compass.ticktext[i], 2);
                        Canvas.SetLeft(cache.Compass.ticktext[i], cache.Compass.tl[i].X1 - 10);
                        Canvas_Compass.Children.Add(cache.Compass.ticktext[i]);

                    }
                    else
                    {
                        cache.Compass.tl[i].Y1 = 25;
                    }
                    cache.Compass.tl[i].Y2 = 30;
                    cache.Compass.tl[i].Stroke = HudBrush;
                    cache.Compass.tl[i].StrokeThickness = 1;
                    Canvas_Compass.Children.Add(cache.Compass.tl[i]);
                }
            }





        }
        private void DrawHeading(double yawDeg)
        {
            yawDeg = yawDeg % 360;

            if (yawDeg < 0)
            {
                yawDeg += 360;
            }

            int yawInt = (int)yawDeg;
            if (360 == yawInt)
            {
                yawInt = 0;
            }

            double left = (Grid_Compass.ActualWidth / 2) - 30;

            string hdgStr = "HDG ";
            if (yawInt < 100)
            {
                hdgStr += " ";
            }
            if (yawInt < 10)
            {
                hdgStr += " ";
            }

            cache.Heading.heading.Text = hdgStr + yawDeg.ToString("0.0°");
            cache.Heading.heading.Foreground = HudBrush;

            Canvas.SetTop(cache.Heading.border, 44);
            Canvas.SetLeft(cache.Heading.border, left);
            Canvas_Compass.Children.Add(cache.Heading.border);

            cache.Heading.leftLn.X1 = (Grid_Compass.ActualWidth / 2) - 15;
            cache.Heading.leftLn.X2 = (Grid_Compass.ActualWidth / 2);
            Canvas_Compass.Children.Add(cache.Heading.leftLn);

            cache.Heading.rightLn.X1 = (Grid_Compass.ActualWidth / 2) + 15;
            cache.Heading.rightLn.X2 = (Grid_Compass.ActualWidth / 2);
            Canvas_Compass.Children.Add(cache.Heading.rightLn);
            
        }
        private double CalculateRollOuterRadius()
        {
            if (Width >= 400)
                return 1.35;
            return 1.275 - ((Width - 400) / 4000);
        }

        private double CalculateRollOuterAngle()
        {
            if (Width >= 700)
                return 1.5;
            return 3 - ((Width - 400) / 200);
        }


        private void DrawRollTick(int index, double circleRad, double rollAngle, bool isLarge)
        {
            if (true == isLarge)
            {
                cache.RollTick.line[index].Y1 = -24;
            }
            else
            {
                cache.RollTick.line[index].Y1 = -12;
            }

            Canvas.SetTop(cache.RollTick.texts[index], -circleRad - 40);
            Canvas.SetTop(cache.RollTick.line[index], -circleRad);



            double rollOffset = rollAngle;

            cache.RollTick.textRotationTransform[index].Angle = 0 - CalculateRollOuterAngle() + rollOffset;
            cache.RollTick.textRotationTransform[index].CenterX = 0;
            cache.RollTick.textRotationTransform[index].CenterY = circleRad * CalculateRollOuterRadius();


            cache.RollTick.rotationTransform[index].Angle = 0 + rollOffset;
            cache.RollTick.rotationTransform[index].CenterX = 0;
            cache.RollTick.rotationTransform[index].CenterY = circleRad;

            Canvas_HUD.Children.Add(cache.RollTick.texts[index]);
            Canvas_HUD.Children.Add(cache.RollTick.line[index]);
        }
        private void DrawZeroRollTick(double circleRad, double rollCommand, bool staticRollIndicator)
        {
            if (staticRollIndicator)
            {
                cache.ZeroRollTick.rotation.Angle = rollCommand - RollAngle;
            }
            else
            {
                cache.ZeroRollTick.rotation.Angle = rollCommand;
            }
            cache.ZeroRollTick.rotation.CenterX = 0;
            cache.ZeroRollTick.rotation.CenterY = circleRad;

            Canvas.SetTop(cache.ZeroRollTick.triangle, -circleRad);
            Canvas_HUD.Children.Add(cache.ZeroRollTick.triangle);
        }
        private void DrawRollIndicator(double circleRad, double rollAngle, bool staticRollIndicator)
        {
            if (staticRollIndicator)
            {
                cache.RollIndicator.triangleRenderTransform.Angle = 0;
                cache.RollIndicator.triangleRenderTransform.CenterX = 0;
                cache.RollIndicator.triangleRenderTransform.CenterY = circleRad;

                cache.RollIndicator.trapezoidRenderTransform.Angle = 0;
                cache.RollIndicator.trapezoidRenderTransform.CenterX = 0;
                cache.RollIndicator.trapezoidRenderTransform.CenterY = circleRad;
            }
            else
            {
                cache.RollIndicator.triangleRenderTransform.Angle = rollAngle;
                cache.RollIndicator.triangleRenderTransform.CenterX = 0;
                cache.RollIndicator.triangleRenderTransform.CenterY = circleRad;

                cache.RollIndicator.trapezoidRenderTransform.Angle = rollAngle;
                cache.RollIndicator.trapezoidRenderTransform.CenterX = 0;
                cache.RollIndicator.trapezoidRenderTransform.CenterY = circleRad;
            }
            
            cache.RollIndicator.triangle.RenderTransform = cache.RollIndicator.triangleRenderTransform;
            cache.RollIndicator.trapezoid.RenderTransform = cache.RollIndicator.trapezoidRenderTransform;

            Canvas.SetTop(cache.RollIndicator.triangle, -circleRad);
            Canvas.SetTop(cache.RollIndicator.trapezoid, -circleRad);
            Canvas_HUD.Children.Add(cache.RollIndicator.triangle);
            Canvas_HUD.Children.Add(cache.RollIndicator.trapezoid);

        }
        private void DrawRoll(double rollAngle, double rollCommand, bool staticRollIndicator)
        {
            double circleRad = Grid_Viewport.ActualHeight / 3;

            DrawZeroRollTick(circleRad, rollCommand, staticRollIndicator);
            for (int i = 0; i < cache.Roll.tickList.Count; i++)
            {
                if (staticRollIndicator)
                {
                    DrawRollTick(i, circleRad, cache.Roll.tickList[i].Key - rollAngle, cache.Roll.tickList[i].Value);
                }
                else
                {
                    DrawRollTick(i, circleRad, cache.Roll.tickList[i].Key, cache.Roll.tickList[i].Value);
                }
            }
            DrawRollIndicator(circleRad, -rollAngle, staticRollIndicator);
        }
        private void DrawClimbRate(double climbRate)
        {
            cache.ClimbRate.txtBlk.Text = "VERT\n" + climbRate.ToString("+0.0;-0.0;0.0");
            Canvas.SetTop(cache.ClimbRate.txtBlk, 80);
            Canvas.SetLeft(cache.ClimbRate.txtBlk, -15);
            Canvas_RightHUD.Children.Add(cache.ClimbRate.txtBlk);

            Canvas_RightHUD.Children.Add(cache.ClimbRate.zeroLn);

            double maxHUD_Height = Grid_RightHUD.ActualHeight * 0.25;

            double magHeight = Math.Abs(climbRate);
            if (magHeight > MaxVerticalSpeedArrowMag)
            {                  
                magHeight = MaxVerticalSpeedArrowMag;
            }

            cache.ClimbRate.climbMagnitude.Height = maxHUD_Height * (magHeight / (2 * MaxVerticalSpeedArrowMag));
            Canvas.SetLeft(cache.ClimbRate.climbMagnitude, -31);
            Canvas.SetLeft(cache.ClimbRate.triangle, -25);

            if (climbRate > 0)
            {
                cache.ClimbRate.triangleRenderTransform.Angle = 180;
                Canvas.SetTop(cache.ClimbRate.climbMagnitude, -cache.ClimbRate.climbMagnitude.Height);
                Canvas.SetTop(cache.ClimbRate.triangle, -cache.ClimbRate.climbMagnitude.Height);
            }
            else
            {
                cache.ClimbRate.triangleRenderTransform.Angle = 0;
                Canvas.SetTop(cache.ClimbRate.triangle, cache.ClimbRate.climbMagnitude.Height);
            }

            if (Math.Abs(climbRate) > 1.0)
            {
                // don't draw flickering arrow if bouncing around near 0
                Canvas_RightHUD.Children.Add(cache.ClimbRate.triangle);
            }
            Canvas_RightHUD.Children.Add(cache.ClimbRate.climbMagnitude);
        }
        private void DrawMAndG(double mach, double gLoad)
        {
            cache.MAndG.txtBlkGM.Text = "G  " + gLoad.ToString("+0.0;-0.0;0.0") + "\n" + "M  " + mach.ToString("0.00");
            Canvas.SetTop(cache.MAndG.txtBlkGM, -165);
            Canvas.SetLeft(cache.MAndG.txtBlkGM, -25);
            Canvas_LeftHUD.Children.Add(cache.MAndG.txtBlkGM);
        }

        private void DrawGPSLabel(EGPSMode eGPSMode)
        {
            string text = string.Empty;
            switch (eGPSMode)
            {
                case EGPSMode.Invalid:
                    text = "Not Valid";
                    cache.GPSLabel.gpsText.Foreground = RedBrush;
                    break;
                case EGPSMode._1D:
                    text = "Not Valid";
                    cache.GPSLabel.gpsText.Foreground = RedBrush;
                    break;
                case EGPSMode._2D:
                    text = "        2D";
                    cache.GPSLabel.gpsText.Foreground = HudBrush;
                    break;
                case EGPSMode._3D:
                    text = "        3D";
                    cache.GPSLabel.gpsText.Foreground = HudBrush;
                    break;
                default:
                    break;
            }

            cache.GPSLabel.gpsText.Text = "      GPS\n" + text;
            
            cache.GPSLabel.gpsText.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.SetTop(cache.GPSLabel.gpsText, 135);
            Canvas.SetLeft(cache.GPSLabel.gpsText, -40);
            Canvas_RightHUD.Children.Add(cache.GPSLabel.gpsText);
        }

        private void DrawAircraft()
        {
            Canvas_HUD.Children.Add(cache.Aircraft.waterline);
        }
        private void DrawFlightPathMarker(double alpha, double beta)
        {
            double vertPixelsPerDeg = Grid_Viewport.ActualHeight / VERTICAL_DEG_TO_DISP;
            double horzPixelsPerDeg = Grid_Viewport.ActualWidth / HORIZONTAL_DEG_TO_DISP;

            double leftOffset = (-FPM_RADIUS / 2.0) + (beta * horzPixelsPerDeg);
            double rightOffset = (FPM_RADIUS / 2.0) + (beta * horzPixelsPerDeg);
            double topOffset = (-FPM_RADIUS / 2.0) + (alpha * vertPixelsPerDeg);

            Canvas.SetLeft(cache.FlightPathMarker.body, leftOffset);
            Canvas.SetTop(cache.FlightPathMarker.body, topOffset);
            Canvas_HUD.Children.Add(cache.FlightPathMarker.body);

            Canvas.SetLeft(cache.FlightPathMarker.leftLn, leftOffset);
            Canvas.SetTop(cache.FlightPathMarker.leftLn, topOffset);
            Canvas_HUD.Children.Add(cache.FlightPathMarker.leftLn);


            Canvas.SetLeft(cache.FlightPathMarker.rightLn, rightOffset);
            Canvas.SetTop(cache.FlightPathMarker.rightLn, topOffset);
            Canvas_HUD.Children.Add(cache.FlightPathMarker.rightLn);

            Canvas.SetTop(cache.FlightPathMarker.topLn, topOffset);
            Canvas.SetLeft(cache.FlightPathMarker.topLn, beta * horzPixelsPerDeg);
            Canvas_HUD.Children.Add(cache.FlightPathMarker.topLn);
        }
        private void DrawAlphaBeta(double alpha, double beta)
        {
            cache.AlphaBeta.txtBlkSpd.Text = "\u03B1    " + alpha.ToString("+0.0;-0.0;0.0") + "\n\u03B2    " + beta.ToString("+0.0;-0.0;0.0");
            Canvas.SetTop(cache.AlphaBeta.txtBlkSpd, -165);
            Canvas.SetLeft(cache.AlphaBeta.txtBlkSpd, -25);
            Canvas_RightHUD.Children.Add(cache.AlphaBeta.txtBlkSpd);
        }

        private void DrawWayPoint(double WPD, double WPN)
        {
            cache.WayPoint.txtBlk.Text = "Waypoint:\n" + WPD.ToString() + ">" + WPN.ToString();
            Canvas.SetTop(cache.WayPoint.txtBlk, 135);
            Canvas.SetLeft(cache.WayPoint.txtBlk, -30);
            Canvas_LeftHUD.Children.Add(cache.WayPoint.txtBlk);
        }

        private void DrawAltitude(double altitude, string unit, int command)
        {
            cache.Altitude.txtBlk.Text = "ALT ("+ unit + ")";
            Canvas.SetTop(cache.Altitude.txtBlk, -100);
            Canvas.SetLeft(cache.Altitude.txtBlk, -7);
            Canvas_RightHUD.Children.Add(cache.Altitude.txtBlk);

            double y_offset = -((Width / 10) + 10) + 1;
            Canvas.SetTop(cache.Altitude.rect, -cache.Altitude.rect.Height / 2);
            Canvas.SetRight(cache.Altitude.rect, 0 + y_offset);
            Canvas_RightHUD.Children.Add(cache.Altitude.rect);
            double from = Altitude - Altitude % 5 + 15;
            double space = (cache.Altitude.rect.Height - 20) / 5;

            double offset = (from - Altitude) / 25 * (cache.Altitude.rect.Height - 20) - cache.Altitude.rect.Height / 2 + 10;

            for (int i = 1; i < 6; i++)
            {
                cache.Altitude.bit[i].Text = (from - i * 5).ToString("##0");

                if (from - i * 5 == (Round(command)))
                {
                    cache.Altitude.lines[i].Stroke = CommandBrush;
                    cache.Altitude.bit[i].Foreground = CommandBrush;
                }
                else
                {
                    cache.Altitude.lines[i].Stroke = HudBrush;
                    cache.Altitude.bit[i].Foreground = HudBrush;
                }

                Canvas.SetLeft(cache.Altitude.bit[i], cache.Altitude.lines[i].X2 + 4 - y_offset);
                Canvas.SetTop(cache.Altitude.bit[i], cache.Altitude.lines[i].Y1 - 10 - offset + 8);

                Canvas.SetLeft(cache.Altitude.lines[i], -y_offset + 2);
                Canvas.SetTop(cache.Altitude.lines[i], - offset + 8);

                Canvas_RightHUD.Children.Add(cache.Altitude.lines[i]);
                Canvas_RightHUD.Children.Add(cache.Altitude.bit[i]);
            }


            space = (cache.Altitude.rect.Height - 36) / 9;

            cache.Altitude.tb.Text = Altitude.ToString("##0.0");
            if (IsInRange(int.Parse(Altitude.ToString("##0")), Round(command) - AltitudeCommandTolerance, Round(command) + AltitudeCommandTolerance))
            {
                cache.Altitude.tb.Foreground = BlackBrush;
                cache.Altitude.tb.Background = CommandBrush;
            }
            else
            {
                cache.Altitude.tb.Foreground = HudBrush;
                cache.Altitude.tb.Background = BlackBrush;
            }

            Canvas.SetTop(cache.Altitude.tb, - 9);
            Canvas.SetRight(cache.Altitude.tb, 0 + y_offset + 2);
            Canvas_RightHUD.Children.Add(cache.Altitude.tb);
        }

        private void DrawGroundSpeed(double AS, double GS, int command)
        {
            cache.Speed.txtBlkSpdG.Text = "AS\n" + AS.ToString("0");
            Canvas.SetTop(cache.Speed.txtBlkSpdG, +80);
            Canvas.SetLeft(cache.Speed.txtBlkSpdG, -30);
            Canvas_LeftHUD.Children.Add(cache.Speed.txtBlkSpdG);

            cache.GroundSpeed.txtBlk.Text = "GS";
            Canvas.SetTop(cache.GroundSpeed.txtBlk, -100);
            Canvas.SetLeft(cache.GroundSpeed.txtBlk, -30);
            Canvas_LeftHUD.Children.Add(cache.GroundSpeed.txtBlk);

            double y_offset = - 10 - 1;
            Canvas.SetTop(cache.GroundSpeed.rect, -cache.GroundSpeed.rect.Height / 2);
            Canvas.SetRight(cache.GroundSpeed.rect, 0 + y_offset);
            Canvas_LeftHUD.Children.Add(cache.GroundSpeed.rect);
            double from = GS - GS % 5 + 15;
            double space = (cache.GroundSpeed.rect.Height - 20) / 5;

            double offset = (from - GS) / 25 * (cache.GroundSpeed.rect.Height - 20) - cache.GroundSpeed.rect.Height / 2 + 10;

            for (int i = 1; i < 6; i++)
            {
                cache.GroundSpeed.bit[i].Text = (from - i * 5).ToString("##0");

                if (from - i * 5 == (Round(command)))
                {
                    cache.GroundSpeed.lines[i].Stroke = CommandBrush;
                    cache.GroundSpeed.bit[i].Foreground = CommandBrush;
                }
                else
                {
                    cache.GroundSpeed.lines[i].Stroke = HudBrush;
                    cache.GroundSpeed.bit[i].Foreground = HudBrush;
                }


                Canvas.SetLeft(cache.GroundSpeed.bit[i], cache.GroundSpeed.lines[i].X2 + 4 - y_offset);
                Canvas.SetTop(cache.GroundSpeed.bit[i], cache.GroundSpeed.lines[i].Y1 - 10 - offset + 8);

                Canvas.SetLeft(cache.GroundSpeed.lines[i], -y_offset + 2);
                Canvas.SetTop(cache.GroundSpeed.lines[i], -offset + 8);

                Canvas_LeftHUD.Children.Add(cache.GroundSpeed.lines[i]);
                Canvas_LeftHUD.Children.Add(cache.GroundSpeed.bit[i]);
            }



            space = (cache.GroundSpeed.rect.Height - 36) / 9;

            cache.GroundSpeed.tb.Text = GS.ToString("##0.0");
            if (IsInRange(int.Parse(GS.ToString("##0")), Round(command) - GroundSpeedCommandTolerance, Round(command) + GroundSpeedCommandTolerance))
            {
                cache.GroundSpeed.tb.Foreground = BlackBrush;
                cache.GroundSpeed.tb.Background = CommandBrush;
            }
            else
            {
                cache.GroundSpeed.tb.Foreground = HudBrush;
                cache.GroundSpeed.tb.Background = BlackBrush;
            }

            Canvas.SetTop(cache.GroundSpeed.tb, - 9);
            Canvas.SetRight(cache.GroundSpeed.tb, 0 + y_offset + 2);
            Canvas_LeftHUD.Children.Add(cache.GroundSpeed.tb);
        }

        #endregion

        #region Helper Methods

        public void SetGPSMode(int i)
        {
            switch (i)
            {
                case 0:
                    GPSMode = EGPSMode.Invalid;
                    break;
                case 1:
                    GPSMode = EGPSMode._1D;
                    break;
                case 2:
                    GPSMode = EGPSMode._2D;
                    break;
                case 3:
                    GPSMode = EGPSMode._3D;
                    break;
                default:
                    GPSMode = EGPSMode.Invalid;
                    break;
            }
        }

        private bool IsInRange(int a, int min, int max)
        {
            return (a >= min && a < max);
        }

        private int Round(int x)
        {
            int rem = x % 5;
            if (rem >= 3)
            {
                int l = 5 - rem;
                return x + l;
            }
            return x - rem;

        }

        #endregion

        bool lagComponent = true;
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (Lag)
            {
                lagComponent = !lagComponent;
                if (lagComponent)
                {
                    return;
                }
            }

            transformGroup.Children.Clear();
            base.OnRender(drawingContext);

            Canvas_Background.Children.Clear();
            Canvas_PitchIndicator.Children.Clear();
            Canvas_HUD.Children.Clear();
            Canvas_Compass.Children.Clear();
            Canvas_RightHUD.Children.Clear();
            Canvas_LeftHUD.Children.Clear();

            WayPointDistance = Math.Round(WayPointDistance, 2);

            DrawGroundSpeed(AirSpeed, GroundSpeed, GroundSpeedCommand);
            DrawCommandPitchTick(PitchCommand, PitchAngle, RollAngle);
            // DrawWayPoint(WayPointDistance, WayPointNumber);
            // DrawGPSLabel(GPSMode);
            DrawAltitude(Altitude, AltitudeUnit, AltitudeCommand);
            DrawGroundAndSky(PitchAngle);
            DrawPitchTicks(PitchAngle, RollAngle);
            DrawCompass(YawAngle, YawCommand);
            DrawHeading(YawAngle);
            DrawRoll(RollAngle, RollCommand, StaticRoll);
            DrawClimbRate(VerticalSpeed);
            // DrawFlightPathMarker(Alpha, Beta);
            DrawAircraft();
            // DrawMAndG(Mach, G_Load);
            // DrawAlphaBeta(Alpha, Beta);

            rotateTransform.Angle = -RollAngle;
            Canvas_Background.RenderTransform = rotateTransform;

            double horzPixelsPerDeg = Grid_Viewport.ActualWidth / HORIZONTAL_DEG_TO_DISP;

            betaTransform.X = Beta * horzPixelsPerDeg;
            betaTransform.Y = 0;
            transformGroup.Children.Add(betaTransform);
            transformGroup.Children.Add(rotateTransform);


            Canvas_PitchIndicator.RenderTransform = transformGroup;
        }
    }

}
