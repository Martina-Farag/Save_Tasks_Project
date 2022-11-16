using ConsoleApp1;
using DevExpress.DataAccess.Json;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TasksApp.Model;
using DialogResult = System.Windows.Forms.DialogResult;

namespace TasksApp
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private readonly ContextDB db;
        private readonly DateTime dateTimeNow = DateTime.Now;
        private readonly string Const_id = "Id";
        private readonly string Const_SettingFile = "appData.json";

        public Form1()
        {
            InitializeComponent();
            db = new ContextDB();


            //Load
            FillProjectList();
            FillYears();
            btn_New_Click(null, null);
            btn_SyncFilter_Click(null, null);
        }

 
        #region UiActions

        private void btn_Add_Click(object sender, EventArgs e)
        {
            Model.Task task = FillTaskFromUi();
            task.creationDateTime = DateTime.Now;
            db.Tasks.Add(task);
            var isDone = db.SaveChanges();
            if (isDone > 0)
                ActionSuccess();
            else
                ActionFailed();
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (la_TaskId.Text.ToLower() != Const_id.ToLower())
            {
                Model.Task taskDb = db.Tasks.FirstOrDefault(x => x.taskId.ToString() == la_TaskId.Text);
                Model.Task task = FillTaskFromUi();
                task.lastUpdateDateTime = DateTime.Now;
                task.taskId = Convert.ToInt32(la_TaskId.Text);
                db.Entry(taskDb).CurrentValues.SetValues(task);
                var isDone = db.SaveChanges();
                if (isDone > 0)
                    ActionSuccess();
                else
                    ActionFailed();
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (la_TaskId.Text.ToLower() != Const_id.ToLower())
            {
                DialogResult dialogResult = MessageBox.Show($"هل انت متأكد من حذف المهمة التي تملتك كود {la_TaskId.Text}", "متأكد", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var task = db.Tasks.FirstOrDefault(x => x.taskId.ToString() == la_TaskId.Text);
                    db.Tasks.Remove(task);
                    var isDone = db.SaveChanges();
                    if (isDone > 0)
                        ActionSuccess();
                    else
                        ActionFailed();
                }
            }
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                Model.Task task = gridView1.GetFocusedRow() as Model.Task;
                FillUi(task);
            }
            catch (Exception e1)
            { }
        }

        private void dt_Start_EditValueChanged(object sender, EventArgs e)
        {
            tx_Duration.Text = CalcolateDuration(dt_Start.EditValue, dt_End.EditValue);
        }

        private void dt_End_EditValueChanged(object sender, EventArgs e)
        {
            tx_Duration.Text = CalcolateDuration(dt_Start.EditValue, dt_End.EditValue);
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            //Butones
            btn_Add.Enabled = true;
            btn_Delete.Enabled = false;
            btn_Update.Enabled = false;
            //Clear
            FillUi(null);
            btn_StartNow_Click(sender, e);
        }

        private void btn_StartNow_Click(object sender, EventArgs e)
        {
            dt_Start.EditValue = DateTimeOffset.Now;
        }

        private void btn_EndNow_Click(object sender, EventArgs e)
        {
            dt_End.EditValue = DateTimeOffset.Now;
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"API-C.Sharp : {cb_Year.Text}-{cb_Month.Text}-{cb_Day.Text}");
            stringBuilder.Append($"\n ---------------------------------------- \n");
            int postion = 0;
            List<Model.Task> tasks = (List<Model.Task>)gridView1.DataSource;
            foreach (var item in tasks)
            {
                postion++;
                stringBuilder.Append($"كود المهمة :{postion}-{item.taskProjectSymbol} \n{item.taskText} \n المدة الزمنية هي ( {item.taskDurationText}) \n ************************ \n");
            }
            stringBuilder.Append($"\n ---------------------------------------- \n");

            var totalDuration = GetTotalDuration(tasks);
            stringBuilder.Append($"مجموع وقت العمل هو :{totalDuration}");
            //Save
            _ = SaveDataInText(stringBuilder).Result;
        }

        private async Task<int> SaveDataInText(StringBuilder stringBuilder)
        {
            string fileName = "data.txt";
            StreamWriter file = new StreamWriter(fileName);
            file.WriteLineAsync(stringBuilder.ToString()).Wait();
            file.Close();
            file.Dispose();
            MessageBox.Show("عملية ناجحة");
            //Open File
            System.Diagnostics.Process.Start(fileName);
            return 0;
        }

        private string GetTotalDuration(List<Model.Task> tasks)
        {
            TimeSpan? totalTimeSpan = new TimeSpan(0);

            foreach (var item in tasks)
            {
                var duration = GetDurations(item.taskStartDateTime, item.taskEndDateTime);
                if (duration.HasValue==true)
                    totalTimeSpan=totalTimeSpan.Value.Add(duration.Value);
            }

            totalTimeSpan = totalTimeSpan.Value.TotalSeconds == 0 ? null : totalTimeSpan;
            return ConvertTimeSpanToString(totalTimeSpan);
        }

        private void cb_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_Month.Properties.Items.Clear();
            //File Month
            int maxMonth = dateTimeNow.Year == Convert.ToInt32(cb_Year.Text) ? dateTimeNow.Month : 12;
            for (int i = 1; i <= maxMonth; i++)
                cb_Month.Properties.Items.Add(i);

            if (cb_Month.Text != string.Empty && Convert.ToInt32(cb_Month.Text) > maxMonth)
                cb_Month.Text = maxMonth.ToString();

        }

        private void cb_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Year.Text == string.Empty || cb_Month.Text == string.Empty)
                return;

            cb_Day.Properties.Items.Clear();
            //File day
            int year = Convert.ToInt32(cb_Year.Text);
            int month = Convert.ToInt32(cb_Month.Text);
            int maxDay = GetStartAndEndMonth.StartAndEndDateOfMonth(year, month).endDayOfMonth;

            if (month == dateTimeNow.Month && year == dateTimeNow.Year)
                maxDay = dateTimeNow.Day;

            for (int i = 1; i <= maxDay; i++)
                cb_Day.Properties.Items.Add(i);

            if (cb_Day.Text != String.Empty && Convert.ToInt32(cb_Day.Text) > maxDay)
                cb_Day.Text = maxDay.ToString();
        }

        private void cb_Day_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btn_ClearDay_Click(object sender, EventArgs e)
        {
            cb_Day.SelectedItem = null;
        }

        private void btn_SyncFilter_Click(object sender, EventArgs e)
        {
            SetDateTimeNow();
        }

        #endregion UiActions

        #region Functions

        private void FillUi(Model.Task task)
        {
            if (task != null)
            {
                la_TaskId.Text = task.taskId.ToString();
                tx_Task.Text = task.taskText;
                cb_ProjectSymbol.Text = task.taskProjectSymbol;
                dt_Start.EditValue = (DateTimeOffset?)task.taskStartDateTime ?? null;
                dt_End.EditValue = (DateTimeOffset?)task.taskEndDateTime ?? null;
                //buttons
                btn_Add.Enabled = false;
                btn_Update.Enabled = true;
                btn_Delete.Enabled = true;
            }
            else
            {
                la_TaskId.Text = Const_id;
                tx_Task.Text = string.Empty;
                cb_ProjectSymbol.Text = "PLS";
                dt_Start.EditValue = null;
                dt_End.EditValue = null;
                tx_Duration.Text = string.Empty;
            }
            tx_Task.Focus();
        }

        private Model.Task FillTaskFromUi() => new Model.Task
        {
            taskText = tx_Task.Text?.Trim(),
            taskProjectSymbol = cb_ProjectSymbol.Text?.Trim(),
            taskStartDateTime = dt_Start.EditValue == null ? null : CastDateTime(dt_Start.EditValue),
            taskEndDateTime = dt_End.EditValue == null ? null : CastDateTime(dt_End.EditValue),
            taskDurationText = tx_Duration.Text?.Trim(),
        };

        private void ActionSuccess()
        {
            MessageBox.Show("عملية ناجحة");
            LoadData();
            btn_New_Click(null, null);
        }

        private void ActionFailed()
        {
            MessageBox.Show("عملية فاشلة");
        }

        private void LoadData()
        {
            IQueryable<Model.Task> query = db.Set<Model.Task>();

            query = query.AsNoTracking();

            List<Expression<Func<Model.Task, bool>>> criteria = GetFilter();

            for (int i = 0; i < criteria.Count; i++)
                query = query.Where(criteria[i]);

            gridControl1.DataSource = query.ToList();
        }

        private string CalcolateDuration(object dt_Start, object dt_End)
        {
            var timeSpan = GetDurations(dt_Start, dt_End);
            return ConvertTimeSpanToString(timeSpan);
        }

        private TimeSpan? GetDurations(object dt_Start, object dt_End)
        {
            if (dt_Start != null && dt_End != null)
            {
                DateTime? start = CastDateTime(dt_Start);
                DateTime? end = CastDateTime(dt_End);
                if ((start.HasValue == false && end.HasValue == false) | start > end)
                    return null;
                else
                    return end.Value.Subtract(start.Value);
            }
            return null;
        }

        private string ConvertTimeSpanToString(TimeSpan? span)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("ـــــ");
            if (span.HasValue == true)
            {
                stringBuilder.Clear();
                if (span.Value.Days > 0)
                    stringBuilder.Append($"{span.Value.Days} يوم ");
                if (span.Value.Hours > 0)
                    stringBuilder.Append($"{span.Value.Hours} ساعة ");
                if (span.Value.Minutes > 0)
                    stringBuilder.Append($"{span.Value.Minutes} دقيقة ");
                if (span.Value.Seconds > 0)
                    stringBuilder.Append($"{span.Value.Seconds} ثانية ");
            }

            return stringBuilder.ToString();
        }

        private void FillYears()
        {
            for (int i = 2021; i <= dateTimeNow.Year; i++)
                cb_Year.Properties.Items.Add(i);
        }

        private void SetDateTimeNow()
        {
            cb_Year.SelectedItem = cb_Year.Properties.Items[cb_Year.Properties.Items.Count - 1];
            cb_Month.SelectedItem = cb_Month.Properties.Items[cb_Month.Properties.Items.Count - 1];
            cb_Day.SelectedItem = cb_Day.Properties.Items[cb_Day.Properties.Items.Count - 1];

            LoadData();
        }

        private DateTime? CastDateTime(object dateTime)
        {
            try
            {
                var data = DateTimeOffset.Parse( dateTime.ToString());
                return data.DateTime;
            }
            catch (Exception e1)
            {
                Nullable<DateTime> date = default;
                return date;
            }
        }
        private void FillProjectList()
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(Const_SettingFile))
                {
                    string json = streamReader.ReadToEnd();
                    Projects projects = JsonConvert.DeserializeObject<Projects>(json);
                    cb_ProjectSymbol.Properties.Items.AddRange(projects.listProjects);
                }
            }
            catch (Exception e1)
            {

            }
        }

        private List<Expression<Func<Model.Task, bool>>> GetFilter()
        {
            List<Expression<Func<Model.Task, bool>>> criteria = new List<Expression<Func<Model.Task, bool>>>();

            if (cb_Year.Text != string.Empty)
                criteria.Add(x => x.taskStartDateTime.Value.Year.ToString() == cb_Year.Text);

            if (cb_Month.Text != string.Empty)
                criteria.Add(x => x.taskStartDateTime.Value.Month.ToString() == cb_Month.Text);

            if (cb_Day.Text != string.Empty)
                criteria.Add(x => x.taskStartDateTime.Value.Day.ToString() == cb_Day.Text);

            return criteria;
        }

        #endregion Functions

      
    }
}