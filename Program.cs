using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using SystemTrayCursor.Properties;

namespace SystemTrayCursor
{
    static class Program
    {

        public static Boolean blackCursor = false;
        public static Boolean active = true;

        private static System.Timers.Timer aTimer;
        static int _x, _y;

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        public struct POINT
        {
            public int X;
            public int Y;
        }


        [STAThread]
        static void Main()
        {

            try {
                SetTimer();

                AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MyCustomApplicationContext());

            }
            catch {

            }
            finally {
                Console.WriteLine("Shut down Programm");
                resetCursor();
            }

        }


        static Boolean changeColor(Color c1, Color c2)
        {
            float minContrast = 0.275f;

            float brightness_1 = c1.GetBrightness();
            float brightness_2 = c2.GetBrightness();

            Console.WriteLine(Math.Abs(brightness_1 - brightness_2));
            return (Math.Abs(brightness_1 - brightness_2) <= minContrast);
        }

        static void updateCursor()
        {
            POINT point;
            if (active && (GetCursorPos(out point) && Math.Abs(point.X - _x) > 10 || Math.Abs(point.Y - _y) > 10))
            {
                int deltaX = _x - point.X;
                int deltaY = _y - point.Y;

                int angle = (int)(Math.Atan2(deltaX, deltaY) * 180 / Math.PI);


                if (blackCursor) {
                    if(changeColor(GetPixelColor(new Point(point.X,point.Y)), Color.FromArgb(0, 0, 0)))
                    {
                        setCursorAngle(angle.ToString());
                    }
                    else
                    {
                        setCursorAngleBlack(angle.ToString());
                    }
                }
                else {
                    setCursorAngle(angle.ToString());
                }

                _x = point.X;
                _y = point.Y;
            }
        }


        static Color GetPixelColor(Point position)
        {
            using (var bitmap = new Bitmap(1, 1))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(position, new Point(0, 0), new Size(1, 1));
                }
                return bitmap.GetPixel(0, 0);
            }
        }


        private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(25);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            updateCursor();
        }

        static void OnProcessExit(object sender, EventArgs e) {
            resetCursor();
        }

        private static void resetCursor()
        {
            ChangeCursor("C:\\Windows\\Cursors\\aero_arrow.cur");
        }

        private static void setCursorAngle(string angle)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            ChangeCursor(Environment.CurrentDirectory + "\\lib\\cursors\\rotated_1_wb_cur\\default_cursor_" + angle + ".cur");
        }

        private static void setCursorAngleBlack(string angle)
        {
            ChangeCursor(Environment.CurrentDirectory+"\\lib\\cursors\\rotated_cur\\default_cursor_" + angle + ".cur");
        }


 

        private static void ChangeCursor(string curFile)
        {
            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Arrow", curFile);
            SystemParametersInfo(SPI_SETCURSORS, 0, 0, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        const int SPI_SETCURSORS = 0x0057;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDCHANGE = 0x02;

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);






        public class MyCustomApplicationContext : ApplicationContext
        {
            public static NotifyIcon trayIcon;

            public MyCustomApplicationContext()
            {
                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                    Icon = new System.Drawing.Icon(@"C:\Users\Kirstein\Pictures\cursors\cursoricon.ico"),
                    ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Change Color", change),
                new MenuItem("Stop", toggle),
                new MenuItem("Exit", Exit)
            }),
                    Visible = true
                };
            }

            void Exit(object sender, EventArgs e)
            {
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                trayIcon.Visible = false;

                Application.Exit();
            }

            static void toggle(object sender, EventArgs e)
            {
                active = active ? false : true;
                trayIcon.ContextMenu.MenuItems[1].Text = active ? "Stop" : "Start";

                if (!active) {
                    resetCursor();
                }
            }

            static void change(object sender, EventArgs e)
            {
                blackCursor = blackCursor ? false : true;
            }
        }

    }

    



}
