using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace StikyNotes
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// ��������
        /// </summary>
        public WindowsData Datas { get; set; }

        /// <summary>
        /// ��ʱ������Ƿ�λ�ڴ����Ե
        /// </summary>
        DispatcherTimer timer;

        public ProgramData ProgramData { get; set; }
        #region ����
        public RelayCommand NewWindowCommand { get; private set; }
        public RelayCommand OpenSettingCommand { get; private set; }
        public RelayCommand OpenAboutCommand { get; private set; }
        public RelayCommand AddFontSizeCommand { get; private set; }
        public RelayCommand ReduceFontSizeCommand { get; private set; }
        public RelayCommand<object> MoveWindowCommand { get; private set; }

        public RelayCommand<MainWindow> DeletePaWindowCommand { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += HideWindowDetect;
            timer.Start();

            NewWindowCommand = new RelayCommand(NewWindowMethod);
            OpenSettingCommand = new RelayCommand(OpenSettingMethod);
            OpenAboutCommand = new RelayCommand(OpenAboutMethod);
            DeletePaWindowCommand = new RelayCommand<MainWindow>(DeleteWindowMethod);
            MoveWindowCommand = new RelayCommand<object>(MoveWindowMethod);
            AddFontSizeCommand = new RelayCommand(AddFontSizeMethod);
            ReduceFontSizeCommand = new RelayCommand(ReduceFontSizeMethod);
            ProgramData=ProgramData.Instance;
        }

        private void HideWindowDetect(object sender, EventArgs e)
        {
            
        }



       
        /// <summary>
        /// ����ش���
        /// </summary>
        private void OpenAboutMethod()
        {
            var win=new AboutWindow();
            win.Show();
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void ReduceFontSizeMethod()
        {
            if (Datas.FontSize > 8)
            {
                Datas.FontSize -= 2;
            }
        }
        /// <summary>
        /// �Ŵ�����
        /// </summary>
        private void AddFontSizeMethod()
        {
            if (Datas.FontSize < 32)
            {
                Datas.FontSize += 2;
            }
        }

        /// <summary>
        /// �����ô���
        /// </summary>
        private void OpenSettingMethod()
        {
            var win = new SettingWindow();
            var vm=new SettingViewModel(win);
            win.DataContext = vm;
            win.Show();
        }

        /// <summary>
        /// �ƶ�����
        /// </summary>
        /// <param name="e">��ǰ��Window</param>
        private void MoveWindowMethod(object e)
        {
            var win = e as MainWindow;
            win.ResizeMode = ResizeMode.NoResize;
            win.DragMove();
            win.ResizeMode = ResizeMode.CanResize;

            //            var newPos=win.PointFromScreen(new Point(0, 0));
            //            Datas.StartUpPosition = newPos;
        }

        /// <summary>
        /// �½�����
        /// </summary>
        void NewWindowMethod()
        {
            MainWindow win = new MainWindow();
            var vm = new MainViewModel();
            vm.Datas = new WindowsData();
            win.DataContext = vm;
            win.Show();
            ProgramData.Instance.Datas.Add(vm.Datas);
            WindowsManager.Instance.Windows.Add(win);
            
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        void DeleteWindowMethod(MainWindow obj)
        {
            var win = obj as MainWindow;
            if (WindowsManager.Instance.Windows.Contains(win))
            {
                WindowsManager.Instance.Windows.Remove(win);
                ProgramData.Instance.Datas.Remove(Datas);
            }
            win.Close();
        }

    }
}