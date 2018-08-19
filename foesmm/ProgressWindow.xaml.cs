using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using foesmm.common;

namespace foesmm
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window, IProgress
    {
        public string TaskTitle
        {
            get => TaskTitleLabel.Content.ToString();
            set => TaskTitleLabel.Content = value;
        }

        protected string ShortTitle { get; set; }

        public string Overall
        {
            get => OverallLabel.Content.ToString();
            set => OverallLabel.Content = value;
        }

        public double OverallDone
        {
            get => OverallProgress.Value;
            set => OverallProgress.Value = value;
        }

        public double OverallTotal
        {
            get => OverallProgress.Maximum;
            set
            {
                OverallProgress.IsIndeterminate = value <= 0;
                OverallProgress.Maximum = value;
            }
        }

        public string Step
        {
            get => StepLabel.Content.ToString();
            set
            {
                StepLabel.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
                StepProgress.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
                StepLabel.Content = value;
            }
        }

        public double StepDone
        {
            get => StepProgress.Value;
            set => StepProgress.Value = value;
        }

        public double StepTotal
        {
            get => StepProgress.Maximum;
            set
            {
                StepProgress.IsIndeterminate = value <= 0;
                StepProgress.Maximum = value;
            }
        }

        private bool _windowShown;
        private bool _rollback;
        private bool _completed;

        public Task[] Tasks { get; set; }
        private Task CurrentTask { get; set; }
        private List<Task> CompletedTasks => new List<Task>();

        public ProgressWindow(string taskTitle, string shortTitle = null)
        {
            InitializeComponent();

            TaskTitle = taskTitle;
            ShortTitle = shortTitle ?? taskTitle;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_windowShown) return;
            _windowShown = true;

            foreach (var task in Tasks)
            {
                if (_rollback) break;
                CurrentTask = task;
                task.ApplyAction(this);
                CompletedTasks.Insert(0, CurrentTask);
                CurrentTask = null;
            }

            if (_rollback)
            {
                foreach (var completedTask in CompletedTasks)
                {
                    completedTask.RollbackAction?.Invoke(this);
                }
            }

            _completed = true;
            DialogResult = _rollback;
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_completed) return;

            if (MessageBox.Show($"Are you sure, you want to cancel '{ShortTitle}' task?", "Cancel?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            _rollback = true;
        }
    }
}
