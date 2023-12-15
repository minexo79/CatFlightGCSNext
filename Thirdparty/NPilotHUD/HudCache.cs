using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NPilotHUD
{
    public class HudCache
    {
        public const int RollCacheCount = 10;
        public YawCommandCache YawCommand;
        public WayPointCache WayPoint;
        public GPSLabelCache GPSLabel;
        public GroundAndSkyCache GroundAndSky;
        public PitchCommandCache PitchCommand;
        public MajorPitchTickCache MajorPitchTick;
        public MinorPitchTickCache MinorPitchTick;
        public CompassCache Compass;
        public HeadingCache Heading;
        public RollTickCache RollTick;
        public ZeroRollTickCache ZeroRollTick;
        public RollIndicatorCache RollIndicator;
        public RollCache Roll;
        public GroundSpeedCache GroundSpeed;
        public AltitudeCache Altitude;
        public ClimbRateCache ClimbRate;
        public SpeedCache Speed;
        public MAndGCache MAndG;
        public AircraftCache Aircraft;
        public FlightPathMarkerCache FlightPathMarker;
        public AlphaBetaCache AlphaBeta;

        public HudCache(SolidColorBrush HudBrush, SolidColorBrush CommandBrush, SolidColorBrush GroundBrush, SolidColorBrush SkyBrush)
        {
            GroundSpeed = new GroundSpeedCache(HudBrush);
            Altitude = new AltitudeCache(HudBrush);
            YawCommand = new YawCommandCache(CommandBrush);
            WayPoint = new WayPointCache(HudBrush);
            GPSLabel = new GPSLabelCache(HudBrush);
            GroundAndSky = new GroundAndSkyCache(GroundBrush, SkyBrush);
            PitchCommand = new PitchCommandCache(CommandBrush);
            MajorPitchTick = new MajorPitchTickCache(HudBrush);
            MinorPitchTick = new MinorPitchTickCache(HudBrush);
            Compass = new CompassCache(HudBrush);
            Heading = new HeadingCache(HudBrush);
            RollTick = new RollTickCache(HudBrush);
            ZeroRollTick = new ZeroRollTickCache(CommandBrush);
            RollIndicator = new RollIndicatorCache(HudBrush);
            Roll = new RollCache(HudBrush);
            ClimbRate = new ClimbRateCache(HudBrush);
            Speed = new SpeedCache(HudBrush);
            MAndG = new MAndGCache(HudBrush);
            Aircraft = new AircraftCache(HudBrush);
            FlightPathMarker = new FlightPathMarkerCache(HudBrush);
            AlphaBeta = new AlphaBetaCache(HudBrush);
        }

        
        public class GroundSpeedCache
        {
            public BorderTextLabel txtBlk;
            public Rectangle rect;

            public Line[] lines;
            public BorderTextLabel[] bit;
            public TextBlock tb;

            public GroundSpeedCache(SolidColorBrush HudBrush)
            {
                lines = new Line[6];
                bit = new BorderTextLabel[6];

                txtBlk = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 16
                };

                rect = new Rectangle
                {
                    Width = 60,
                    Height = 150,
                    StrokeThickness = 2,
                    Stroke = HudBrush
                };

                double space = (rect.Height - 20) / 5;
                for (int i = 1; i < 6; i++)
                {

                    Line li = new Line
                    {
                        X1 = -rect.Width,
                        Y1 = (-rect.Height / 2 + space * i) - 2.5
                    };

                    li.X2 = li.X1 + 10;
                    li.Y2 = li.Y1;
                    li.StrokeThickness = 2;
                    li.Stroke = HudBrush;

                    BorderTextLabel texti = new BorderTextLabel
                    {
                        Width = 22,
                        StrokeThickness = 0,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Foreground = HudBrush,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold
                    };

                    lines[i] = li;
                    bit[i] = texti;
                }

                tb = new TextBlock
                {
                    Width = rect.Width - 4,
                    TextAlignment = TextAlignment.Center,
                    Foreground = HudBrush,
                    Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x0, 0x0, 0x0)),
                    FontSize = 17,
                    FontWeight = FontWeights.Bold
                };
            }
        }

        public class AltitudeCache
        {
            public BorderTextLabel txtBlk;
            public Rectangle rect;

            public Line[] lines;
            public BorderTextLabel[] bit;
            public TextBlock tb;

            public AltitudeCache(SolidColorBrush HudBrush)
            {
                lines = new Line[6];
                bit = new BorderTextLabel[6];

                txtBlk = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 16
                };

                rect = new Rectangle
                {
                    Width = 60,
                    Height = 150,
                    StrokeThickness = 2,
                    Stroke = HudBrush
                };

                double space = (rect.Height - 20) / 5;
                for (int i = 1; i < 6; i++)
                {

                    Line li = new Line
                    {
                        X1 = -rect.Width,
                        Y1 = (-rect.Height / 2 + space * i) - 2.5
                    };

                    li.X2 = li.X1 + 10;
                    li.Y2 = li.Y1;
                    li.StrokeThickness = 2;
                    li.Stroke = HudBrush;

                    BorderTextLabel texti = new BorderTextLabel
                    {
                        Width = 22,
                        StrokeThickness = 0,
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Foreground = HudBrush,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold
                    };

                    lines[i] = li;
                    bit[i] = texti;
                }

                tb = new TextBlock
                {
                    Width = rect.Width - 4,
                    TextAlignment = TextAlignment.Center,
                    Foreground = HudBrush,
                    Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x0, 0x0, 0x0)),
                    FontSize = 17,
                    FontWeight = FontWeights.Bold
                };
            }
        }

        public class GPSLabelCache
        {
            public BorderTextLabel gpsText;

            public GPSLabelCache(SolidColorBrush HudBrush)
            {
                gpsText = new BorderTextLabel
                {
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Foreground = HudBrush,
                    StrokeThickness = 0,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold
                };
            }
        }
        
        public class WayPointCache
        {
            public BorderTextLabel txtBlk;

            public WayPointCache(SolidColorBrush HudBrush)
            {
                txtBlk = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 16
                };
            }
        }


        public class GroundAndSkyCache
        {
            public LinearGradientBrush gndGradBrush;
            public LinearGradientBrush skyGradBrush;

            public Rectangle gndRect;
            public Rectangle skyRect;
            public Line line;

            public GroundAndSkyCache(SolidColorBrush GroundBrush, SolidColorBrush SkyBrush)
            {
                // // Ground
                // gndGradBrush = new LinearGradientBrush
                // {
                //     StartPoint = new Point(0, 0),
                //     EndPoint = new Point(0, 1)
                // };
                // 
                // gndGradBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0x91, 0x5E, 0x04), 0.0));
                // gndGradBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0x91, 0x5E, 0x04), 0.0));
                // 
                // 
                // // Sky
                // skyGradBrush = new LinearGradientBrush
                // {
                //     StartPoint = new Point(0, 0),
                //     EndPoint = new Point(0, 1)
                // };
                // 
                // skyGradBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0x00, 0xA2, 0xFF), 0.75));
                // skyGradBrush.GradientStops.Add(new GradientStop(Color.FromRgb(0x00, 0xA2, 0xFF), 1));

                gndRect = new Rectangle
                {
                    Fill = GroundBrush
                };

                skyRect = new Rectangle
                {
                    Fill = SkyBrush
                };

                line = new Line();
            }
        }

        public class YawCommandCache
        {
            public Line line;
            public YawCommandCache(SolidColorBrush CommandBrush)
            {
                line = new Line
                {
                    X1 = 0,
                    X2 = 0,
                    Y1 = 0,
                    Y2 = 20,
                    Stroke = CommandBrush,
                    StrokeThickness = 5
                };
            }
        }

        public class PitchCommandCache
        {
            public Line line;
            public PitchCommandCache(SolidColorBrush CommandBrush)
            {
                line = new Line
                {
                    X1 = 0,
                    X2 = 120,
                    Y1 = 0,
                    Y2 = 0,
                    Stroke = CommandBrush,
                    StrokeThickness = 5
                };
            }
        }
        

        public class MajorPitchTickCache
        {
            public const int TickCount = 36;
            public Line[] lnL;
            public Line[] lnR;
            public Line[] left;
            public Line[] right;
            public bool[] disp;

            public BorderTextLabel[] txtBlkL;
            public BorderTextLabel[] txtBlkR;

            public MajorPitchTickCache(SolidColorBrush HudBrush)
            {
                lnL = new Line[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    lnL[i] = new Line
                    {
                        X1 = 0,
                        X2 = 40,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 2
                    };
                }

                lnR = new Line[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    lnR[i] = new Line
                    {
                        X1 = 0,
                        X2 = 40,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 2
                    };
                }

                left = new Line[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    left[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = 0,
                        Y2 = 7,
                        Stroke = HudBrush,
                        StrokeThickness = 2
                    };
                }

                for (int i = 0; i < 9; i++)
                {
                    left[i * 2].Y2 = -5;
                }

                right = new Line[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    right[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = 0,
                        Y2 = 7,
                        Stroke = HudBrush,
                        StrokeThickness = 2
                    };
                }

                for (int i = 0; i < 9; i++)
                {
                    right[i * 2].Y2 = -5;
                }

                disp = new bool[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    disp[i] = true;
                }

                txtBlkL = new BorderTextLabel[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    txtBlkL[i] = new BorderTextLabel
                    {
                        Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Foreground = HudBrush,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold
                    };
                }

                txtBlkR = new BorderTextLabel[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    txtBlkR[i] = new BorderTextLabel
                    {
                        Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        Foreground = HudBrush,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold
                    };
                }

            }

        }

        public class MinorPitchTickCache
        {
            public const int TickCount = 36;
            public Line[] lnL;
            public Line[] lnR;
            public Line[] left;
            public Line[] right;
            public MinorPitchTickCache(SolidColorBrush HudBrush)
            {
                lnL = new Line[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    lnL[i] = new Line
                    {
                        X1 = 0,
                        X2 = 25,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 1
                    };
                }

                lnR = new Line[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    lnR[i] = new Line
                    {
                        X1 = 0,
                        X2 = 25,
                        Y1 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 1
                    };
                }

                left = new Line[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    left[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = 0,
                        Y2 = 5,
                        Stroke = HudBrush,
                        StrokeThickness = 1
                    };
                }

                for (int i = 0; i < 9; i++)
                {
                    left[i * 2].Y2 = -5;
                }

                right = new Line[TickCount];
                for (int i = 0; i < TickCount; i++)
                {
                    right[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y1 = 0,
                        Y2 = 5,
                        Stroke = HudBrush,
                        StrokeThickness = 1
                    };
                }

                for (int i = 0; i < 18; i++)
                {
                    right[i * 2].Y2 = -5;
                }
            }
        }

        public class CompassCache
        {
            public Line[] tl;
            public BorderTextLabel[] ticktext;
            public CompassCache(SolidColorBrush HudBrush)
            {
                tl = new Line[26];
                for (int i = 0; i < tl.Length; i++)
                {
                    tl[i] = new Line();
                }

                ticktext = new BorderTextLabel[26];
                for (int i = 0; i < ticktext.Length; i++)
                {
                    ticktext[i] = new BorderTextLabel
                    {
                        FontSize = 16,
                        Stroke = HudBrush,
                        Foreground = HudBrush,
                        FontFamily = new FontFamily("Courier New")
                    };
                }
            }
        }

        public class HeadingCache
        {
            public BorderTextLabel heading;
            public Border border;
            public Thickness thickness;
            public Line leftLn;
            public Line rightLn;

            public HeadingCache(SolidColorBrush HudBrush)
            {
                heading = new BorderTextLabel
                {
                    FontFamily = new FontFamily("Courier New"),
                    Stroke = HudBrush,
                    FontSize = 16,
                    Foreground = HudBrush
                };

                thickness = new Thickness(1);
                border = new Border
                {
                    BorderThickness = thickness,
                    Child = heading,
                    BorderBrush = HudBrush
                };

                leftLn = new Line
                {
                    Y1 = 44,
                    Y2 = 30,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };

                rightLn = new Line
                {
                    Y1 = 44,
                    Y2 = 30,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };
            }

        }

        public class RollTickCache
        {
            public Line[] line;
            public BorderTextLabel[] texts;
            public RotateTransform[] rotationTransform;
            public RotateTransform[] textRotationTransform;

            public RollTickCache(SolidColorBrush HudBrush)
            {
                rotationTransform = new RotateTransform[RollCacheCount];
                textRotationTransform = new RotateTransform[RollCacheCount];
                texts = new BorderTextLabel[RollCacheCount];

                for (int i = 0; i < RollCacheCount; i++)
                {
                    rotationTransform[i] = new RotateTransform();
                    textRotationTransform[i] = new RotateTransform();

                    texts[i] = new BorderTextLabel
                    {
                        Foreground = HudBrush,
                        Stroke = HudBrush,
                        StrokeThickness = 1,
                        FontSize = 12
                    };
                    texts[i].RenderTransform = textRotationTransform[i];
                }

                texts[9].Text = "-60";
                texts[8].Text = "-45";
                texts[7].Text = "-30";
                texts[6].Text = "-20";
                texts[5].Text = "-10";

                texts[4].Text = "60";
                texts[3].Text = "45";
                texts[2].Text = "30";
                texts[1].Text = "20";
                texts[0].Text = "10";

                line = new Line[RollCacheCount];
                for (int i = 0; i < RollCacheCount; i++)
                {
                    line[i] = new Line
                    {
                        X1 = 0,
                        X2 = 0,
                        Y2 = 0,
                        Stroke = HudBrush,
                        StrokeThickness = 2,
                        RenderTransform = rotationTransform[i]
                    };
                };
            }
        }

        public class ZeroRollTickCache
        {
            private Polygon CreateTriangle(SolidColorBrush HudBrush, double x1, double y1, double x2, double y2, double x3, double y3)
            {
                Polygon triangle = new Polygon
                {
                    Stroke = HudBrush,
                    Fill = HudBrush,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Point triP1 = new Point(x1, y1);
                Point triP2 = new Point(x2, y2);
                Point triP3 = new Point(x3, y3);
                PointCollection pc = new PointCollection
                {
                    triP1,
                    triP2,
                    triP3
                };
                triangle.Points = pc;

                return triangle;
            }

            public Polygon triangle;
            public RotateTransform rotation;
            public ZeroRollTickCache(SolidColorBrush CommandBrush)
            {
                rotation = new RotateTransform();
                triangle = CreateTriangle(CommandBrush, 0, 0, 12, -16, -12, -16);
                triangle.RenderTransform = rotation;
            }
        }

        public class RollIndicatorCache
        {
            public Polygon triangle;
            public Polygon trapezoid;
            public Point trapP1;
            public Point trapP2;
            public Point trapP3;
            public Point trapP4;
            public PointCollection pcTrap;
            public RotateTransform triangleRenderTransform;// = new RotateTransform(rollAngle, 0, circleRad);
            public RotateTransform trapezoidRenderTransform;// = new RotateTransform(rollAngle, 0, circleRad);

            private Polygon CreateTriangle(SolidColorBrush HudBrush, double x1, double y1, double x2, double y2, double x3, double y3)
            {
                Polygon triangle = new Polygon
                {
                    Stroke = HudBrush,
                    Fill = HudBrush,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Point triP1 = new Point(x1, y1);
                Point triP2 = new Point(x2, y2);
                Point triP3 = new Point(x3, y3);
                PointCollection pc = new PointCollection
                {
                    triP1,
                    triP2,
                    triP3
                };
                triangle.Points = pc;

                return triangle;
            }

            public RollIndicatorCache(SolidColorBrush HudBrush)
            {
                triangleRenderTransform = new RotateTransform();
                trapezoidRenderTransform = new RotateTransform();

                triangle = CreateTriangle(HudBrush, 0, 0, 9, 12, -9, 12);

                trapezoid = new Polygon
                {
                    Stroke = HudBrush,
                    Fill = HudBrush,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                trapP1 = new Point(-12, 16);
                trapP2 = new Point(12, 16);
                trapP3 = new Point(15, 20);
                trapP4 = new Point(-15, 20);

                pcTrap = new PointCollection
                {
                    trapP1,
                    trapP2,
                    trapP3,
                    trapP4
                };

                trapezoid.Points = pcTrap;

            }

        }

        public class RollCache
        {
            public List<KeyValuePair<double, bool>> tickList;
            public RollCache(SolidColorBrush HudBrush)
            {
                tickList = new List<KeyValuePair<double, bool>>
                {
                    new KeyValuePair<double, bool>(10, false),
                    new KeyValuePair<double, bool>(20, false),
                    new KeyValuePair<double, bool>(30, true),
                    new KeyValuePair<double, bool>(45, false),
                    new KeyValuePair<double, bool>(60, true),

                    new KeyValuePair<double, bool>(-10, false),
                    new KeyValuePair<double, bool>(-20, false),
                    new KeyValuePair<double, bool>(-30, true),
                    new KeyValuePair<double, bool>(-45, false),
                    new KeyValuePair<double, bool>(-60, true),
                };
            }
        }

        public class ClimbRateCache
        {
            public BorderTextLabel txtBlk;
            public Line zeroLn;
            public Rectangle climbMagnitude;
            public RotateTransform triangleRenderTransform;
            public Polygon triangle;

            public ClimbRateCache(SolidColorBrush HudBrush)
            {

                triangleRenderTransform = new RotateTransform(180);
                triangle = CreateTriangle(HudBrush, -9, 0, 9, 0, 0, 12);
                triangle.RenderTransform = triangleRenderTransform;

                txtBlk = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 16
                };

                zeroLn = new Line
                {
                    X1 = -10,
                    X2 = -40,
                    Y1 = 0,
                    Y2 = 00,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };

                climbMagnitude = new Rectangle
                {
                    Fill = HudBrush,
                    Width = 12
                };
            }

            private Polygon CreateTriangle(SolidColorBrush HudBrush, double x1, double y1, double x2, double y2, double x3, double y3)
            {
                Polygon triangle = new Polygon
                {
                    Stroke = HudBrush,
                    Fill = HudBrush,
                    StrokeThickness = 1,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                };

                Point triP1 = new Point(x1, y1);
                Point triP2 = new Point(x2, y2);
                Point triP3 = new Point(x3, y3);
                PointCollection pc = new PointCollection
                {
                    triP1,
                    triP2,
                    triP3
                };
                triangle.Points = pc;

                return triangle;
            }


        }

        public class SpeedCache
        {

            public BorderTextLabel txtBlkSpdG;
            public BorderTextLabel txtBlkSpdA; 
            public SpeedCache(SolidColorBrush HudBrush)
            {
                txtBlkSpdG = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 16
                };

                txtBlkSpdA = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 16
                };
            }
        }

        public class MAndGCache
        {
            public BorderTextLabel txtBlkGM;

            public MAndGCache(SolidColorBrush HudBrush)
            {
                txtBlkGM = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 16
                };
            }

        }

        public class AircraftCache
        {
            public Polyline waterline;

            public AircraftCache(SolidColorBrush HudBrush)
            {
                double segmentLength = 6;
                waterline = new Polyline
                {
                    Stroke = HudBrush,
                    StrokeThickness = 2
                };

                Point p1 = new Point(-4 * segmentLength, 0);
                Point p2 = new Point(-2 * segmentLength, 0);
                Point p3 = new Point(-segmentLength, segmentLength);
                Point p4 = new Point(0, 0);
                Point p5 = new Point(segmentLength, segmentLength);
                Point p6 = new Point(2 * segmentLength, 0);
                Point p7 = new Point(4 * segmentLength, 0);

                PointCollection pc = new PointCollection
                {
                    p1,
                    p2,
                    p3,
                    p4,
                    p5,
                    p6,
                    p7
                };

                waterline.Points = pc;
            }
        }

        public class FlightPathMarkerCache
        {
            public Ellipse body;
            public Line leftLn;
            public Line rightLn;
            public Line topLn;

            private const double FPM_RADIUS = 24;
            private const double FPM_LINE_LEN = 6;

            public FlightPathMarkerCache(SolidColorBrush HudBrush)
            {
                body = new Ellipse
                {
                    Stroke = HudBrush,
                    StrokeThickness = 2,
                    Width = FPM_RADIUS,
                    Height = FPM_RADIUS
                };

                leftLn = new Line
                {
                    X1 = 0,
                    X2 = -FPM_LINE_LEN,
                    Y1 = FPM_RADIUS / 2.0,
                    Y2 = FPM_RADIUS / 2.0,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };

                rightLn = new Line
                {
                    X1 = 0,
                    X2 = FPM_LINE_LEN,
                    Y1 = FPM_RADIUS / 2.0,
                    Y2 = FPM_RADIUS / 2.0,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };

                topLn = new Line
                {
                    X1 = 0,
                    X2 = 0,
                    Y1 = 0,
                    Y2 = -FPM_LINE_LEN,
                    Stroke = HudBrush,
                    StrokeThickness = 1
                };
            }
        }

        public class AlphaBetaCache
        {
            public BorderTextLabel txtBlkSpd;

            public AlphaBetaCache(SolidColorBrush HudBrush)
            {
                txtBlkSpd = new BorderTextLabel
                {
                    Stroke = HudBrush,
                    Foreground = HudBrush,
                    FontSize = 16
                };
            }
        }
    }
}
