using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.LogAgent.AccessLayer;
using FISCA.LogAgent;
using System.Xml;
using FISCA.Authentication;

namespace K12.LogView.Modules
{
    public partial class ViewForm : BaseForm
    {

        private const string TimeDisplayFormat = "yyyy/MM/dd HH:mm:ss";

        private BackgroundWorker BGW = new BackgroundWorker();

        ActionRecordCollection ActionColl = new ActionRecordCollection();

        bool ISSpecial = false;


        FiscaAccessLayer fl = new FiscaAccessLayer();

        //模式
        ModeType _ModeType = ModeType.依日期;

        //開始時間
        DateTime time1 = new DateTime();
        //結束時間
        DateTime time2 = new DateTime();
        //關鍵字
        string StringTextBoxX1;

        //先進先出
        Queue<string> timerList2 = new Queue<string>();

        string _GTarget;
        List<string> _TargetIDList = new List<string>();

        FISCA.Data.QueryHelper _queryHelper = new FISCA.Data.QueryHelper();
        UpdateHelper _Update = new UpdateHelper();
        DataTable dt { get; set; }

        TimeSpan ts1 = new TimeSpan();

        //使用時間
        double ffl;

        /// <summary>
        /// 總體瀏覽模式
        /// </summary>
        public ViewForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 依照學生/班級/教師/課程,的瀏覽模式
        /// </summary>
        /// <param name="GTarget">傳入模式</param>
        /// <param name="TargetID">傳入ID</param>
        public ViewForm(string GTarget, List<string> TargetIDList)
        {
            InitializeComponent();

            _GTarget = GTarget;
            _TargetIDList = TargetIDList;
            guoupPanel2();
        }

        /// <summary>
        /// 使用者瀏覽模式
        /// </summary>
        /// <param name="GTarget">傳入使用者帳號</param>
        public ViewForm(string UserName)
        {
            InitializeComponent();
            textBoxX1.Text = FISCA.Authentication.DSAServices.UserAccount;
            guoupPanel2();
        }

        private void guoupPanel2()
        {
            labelX3.Enabled = false;
            textBoxX1.Enabled = false;
            groupPanel2.Text = "步驟2選擇篩選條件(本模式,無法使用篩選條件)";
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            timer2.Tick += new EventHandler(timer2_Tick);

            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            DateTime dt1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime dt2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            dateTimeInput1.Value = dt1.AddDays(-7);
            dateTimeInput2.Value = dt2;

            timerString();
            Random r = new Random();
            for (int i = 0; i < r.Next(timerList2.Count) + 1; i++)
                UpdateTip();

            btnReF_Click(null, null);
        }

        void timer2_Tick(object sender, EventArgs e)
        {
            TimeSpan ts2 = new TimeSpan(0, 0, 0, 0, timer2.Interval);
            ts1 = ts1 + ts2;
            ffl = ts1.TotalMilliseconds * 0.001;
            //this.Text = "查詢資料中..." + ts1.ToString(@"mm\:ss\.ffff");
            this.Text = "查詢資料中..." + ffl + "秒";
        }

        private void btnReF_Click(object sender, EventArgs e)
        {
            RunBGW();

            if (!string.IsNullOrEmpty(_GTarget) && _TargetIDList.Count != 0)
                _ModeType = ModeType.依類別與ID;
            else if (!string.IsNullOrEmpty(textBoxX1.Text))
            {
                _ModeType = ModeType.依關鍵字;
            }
            else
                _ModeType = ModeType.依日期;

            BGW.RunWorkerAsync();
        }

        private void RunBGW()
        {
            ts1 = new TimeSpan();
            timer2.Start();
            onLock(false);
            time1 = dateTimeInput1.Value;
            time2 = dateTimeInput2.Value.AddDays(1).AddSeconds(-1);
            StringTextBoxX1 = textBoxX1.Text.Trim();
            dataGridViewX1.Rows.Clear();
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!ISSpecial)
            {
                if (_ModeType == ModeType.依類別與ID)
                {
                    #region 依類別與ID

                    string st = string.Join("','", _TargetIDList.ToArray());
                    StringBuilder st_3 = new StringBuilder();
                    st_3.Append(string.Format("where server_time>'{0}' ", time1.ToString(TimeDisplayFormat)));
                    st_3.Append(string.Format("and server_time<'{0}' ", time2.ToString(TimeDisplayFormat)));
                    st_3.Append("and target_category='" + _GTarget + "' ");
                    st_3.Append(string.Format("and target_id in ('{0}') ", st));
                    st_3.Append("and not (action_by LIKE '%[特殊歷程]%')");

                    if (CheckDataRow(st_3.ToString()))
                        e.Cancel = true;

                    dt = GetQuery(st_3.ToString());

                    #endregion
                }
                else if (_ModeType == ModeType.依關鍵字)
                {
                    #region 依關鍵字

                    StringBuilder st_3 = new StringBuilder();
                    st_3.Append(string.Format("where server_time>'{0}' ", time1.ToString(TimeDisplayFormat)));
                    st_3.Append(string.Format("and server_time<'{0}' ", time2.ToString(TimeDisplayFormat)));
                    st_3.Append(string.Format("and (actor LIKE '%{0}%' ", StringTextBoxX1));
                    st_3.Append(string.Format("or action LIKE '%{0}%' ", StringTextBoxX1));
                    st_3.Append(string.Format("or action_by LIKE '%{0}%' ", StringTextBoxX1));
                    st_3.Append(string.Format("or description LIKE '%{0}%'", StringTextBoxX1));
                    st_3.Append(string.Format("or client_info LIKE '%{0}%') ", StringTextBoxX1));
                    st_3.Append(" and not (action_by LIKE '%[特殊歷程]%')");

                    if (CheckDataRow(st_3.ToString()))
                        e.Cancel = true;

                    dt = GetQuery(st_3.ToString());

                    #endregion
                }
                else
                {
                    #region 預設為取得全部資料再進行篩選

                    StringBuilder st_3 = new StringBuilder();
                    st_3.Append(string.Format("where server_time>'{0}' ", time1.ToString(TimeDisplayFormat)));
                    st_3.Append(string.Format("and server_time<'{0}' ", time2.ToString(TimeDisplayFormat)));
                    st_3.Append("and not (action_by LIKE '%[特殊歷程]%')");

                    string a = "111";

                    if (CheckDataRow(st_3.ToString()))
                    {
                        e.Cancel = true;
                        return;
                    }

                    dt = GetQuery(st_3.ToString());

                    #endregion
                }
            }
            else
            {
                if (_ModeType == ModeType.依類別與ID)
                {
                    #region 依類別與ID

                    string st = string.Join("','", _TargetIDList.ToArray());
                    StringBuilder st_3 = new StringBuilder();
                    st_3.Append(string.Format("where server_time>'{0}' ", time1.ToString(TimeDisplayFormat)));
                    st_3.Append(string.Format("and server_time<'{0}' ", time2.ToString(TimeDisplayFormat)));
                    st_3.Append("and target_category='" + _GTarget + "' ");
                    st_3.Append(string.Format("and target_id in ('{0}')", st));

                    if (CheckDataRow(st_3.ToString()))
                        e.Cancel = true;

                    dt = GetQuery(st_3.ToString());

                    #endregion
                }
                else if (_ModeType == ModeType.依關鍵字)
                {
                    #region 使用登入帳號條件

                    StringBuilder st_3 = new StringBuilder();
                    st_3.Append(string.Format("where server_time>'{0}' ", time1.ToString(TimeDisplayFormat)));
                    st_3.Append(string.Format("and server_time<'{0}' ", time2.ToString(TimeDisplayFormat)));
                    st_3.Append(string.Format("and (actor LIKE '%{0}%' ", StringTextBoxX1));
                    st_3.Append(string.Format("or action LIKE '%{0}%' ", StringTextBoxX1));
                    st_3.Append(string.Format("or action_by LIKE '%{0}%' ", StringTextBoxX1));
                    st_3.Append(string.Format("or description LIKE '%{0}%' ", StringTextBoxX1));
                    st_3.Append(string.Format("or client_info LIKE '%{0}%') ", StringTextBoxX1));

                    if (CheckDataRow(st_3.ToString()))
                        e.Cancel = true;

                    dt = GetQuery(st_3.ToString());

                    #endregion
                }
                else
                {
                    #region 預設為取得全部資料再進行篩選

                    StringBuilder st_3 = new StringBuilder();
                    st_3.Append(string.Format("where server_time>'{0}' ", time1.ToString(TimeDisplayFormat)));
                    st_3.Append(string.Format("and server_time<'{0}' ", time2.ToString(TimeDisplayFormat)));

                    if (CheckDataRow(st_3.ToString()))
                    {
                        e.Cancel = true;
                        return;
                    }

                    dt = GetQuery(st_3.ToString());

                    #endregion
                }
            }
        }

        private DataTable GetQuery(string st_3)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select * from log ");
            sb.Append(st_3.ToString());
            DataTable dt = _queryHelper.Select(sb.ToString());
            return dt;
        }

        private bool CheckDataRow(string st)
        {
            StringBuilder sb_new = new StringBuilder();
            sb_new.Append("select id from log ");
            sb_new.Append(st);

            DataTable dt_new = _queryHelper.Select(sb_new.ToString());
            if (dt_new.Rows.Count > 5000)
            {
                DialogResult dr = MsgBox.Show(string.Format("符合條件之系統歷程記錄共「{0}」筆!\n取得超過「5000」筆以上資料需要較久時間!\n且可能發生例外狀況\n您確認要繼續取得資料?", dt_new.Rows.Count.ToString()), MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dr == System.Windows.Forms.DialogResult.No)
                    return true;
            }
            return false;
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            onLock(true);

            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    #region 畫面建置
                    List<Obj> list = new List<Obj>();
                    foreach (DataRow row in dt.Rows)
                    {
                        list.Add(new Obj(row));
                    }

                    if (_ModeType == ModeType.依類別與ID)
                    {
                        textBoxX1.Enabled = false;
                    }

                    foreach (Obj ar in list)
                    {
                        dataGridViewX1.Rows.Add(ValueInser(ar));
                    }


                    dataGridViewX1.Sort(Column1, ListSortDirection.Descending);

                    #endregion
                }
                else
                {
                    MsgBox.Show("資料取得發生錯誤:\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("資料取得作業,已取消");
            }

            timer2.Stop();
            this.Text = "系統歷程，取得資料筆數「" + dataGridViewX1.Rows.Count + "筆」花費時間「" + ffl + "秒」";
        }

        //傳入Recrod,將該記錄建立為DataGridViewRow
        private DataGridViewRow ValueInser(Obj ar)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridViewX1);
            row.Tag = ar;
            //時間
            row.Cells[Column1.Index].Value = DateTime.Parse(ar.server_time).ToString(TimeDisplayFormat);
            row.Cells[Column3.Index].Value = ar.client_info.IPAddress1;
            //電腦名稱
            row.Cells[Column6.Index].Value = ar.client_info.HostName;
            //登入帳號
            row.Cells[Column2.Index].Value = ar.actor;
            //功能
            row.Cells[Column7.Index].Value = ar.action_by;
            //動作
            row.Cells[Column4.Index].Value = ar.action;
            //描述
            row.Cells[Column5.Index].Value = ar.description;

            return row;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            #region 匯出
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            DataGridViewExport export = new DataGridViewExport(dataGridViewX1);
            export.Save(saveFileDialog1.FileName);

            if (new CompleteForm().ShowDialog() == DialogResult.Yes)
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
            #endregion
        }

        private void onLock(bool ol)
        {
            dateTimeInput1.Enabled = ol;
            dateTimeInput2.Enabled = ol;
            groupPanel2.Enabled = ol;
            buttonX2.Enabled = ol;
            btnReF.Enabled = ol;
        }

        private void dataGridViewX1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                TextBoxForm vf = new TextBoxForm(dataGridViewX1.Rows[e.RowIndex].Tag as Obj);
                vf.ShowDialog();
            }
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            btnReF.Pulse(5);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timerString()
        {
            timerList2.Enqueue("提示：使用滑鼠左鍵,連點一行,可開啟該資料的詳細說明");
            timerList2.Enqueue("提示：開始日期/結束日期 輸入8/15將會自動轉換為日期格式");
            timerList2.Enqueue("提示：[登入帳號/電腦名稱]篩選條件 英文字母 大/小寫有所差異");
            timerList2.Enqueue("提示：[動作]篩選條件 請務必輸入完整動作名稱");
            timerList2.Enqueue("提示：[描述]篩選條件 只需要輸入包含的文字即可,如姓名/學號");
            timerList2.Enqueue("提示：如果日期區間定義過大,資料量過多,需要較多處理時間!");
            timerList2.Enqueue("提示：外掛撰寫者,如未使用Log機制寫入Log,將不會有相關記錄");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTip();
        }

        private void UpdateTip()
        {
            string msg = timerList2.Dequeue();
            labelX4.Text = msg;
            timerList2.Enqueue(msg);
        }

        private void 檢視詳細資料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count == 1)
            {
                DataGridViewRow row = dataGridViewX1.SelectedRows[0];
                TextBoxForm vf = new TextBoxForm(row.Tag as Obj);
                vf.ShowDialog();
            }
        }

        private void dataGridViewX1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count == 1)
                檢視詳細資料ToolStripMenuItem.Enabled = true;
            else
                檢視詳細資料ToolStripMenuItem.Enabled = false;
        }

        private void 刪除系統歷程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //認證使用者是否為admin超級使用者
            if (DSAServices.IsSysAdmin)
            {
                DialogResult dr = MsgBox.Show("如果刪除系統歷程\n未來將無法比對使用者對資料的修改?\n您確認要繼續刪除作業?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    PasswordForm pf = new PasswordForm();

                    DialogResult dr2 = pf.ShowDialog();
                    if (dr2 == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (pf.CheckNow())
                        {
                            StringBuilder sb = new StringBuilder();
                            StringBuilder sb2 = new StringBuilder();
                            List<string> list = new List<string>();

                            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
                            {
                                Obj obj = (Obj)row.Tag;
                                if (!list.Contains(obj.id))
                                {
                                    list.Add(obj.id);
                                    sb2.Append("時間「" + obj.server_time + "」");
                                    sb2.Append("電腦「" + obj.client_info.HostName + "」");
                                    sb2.Append("帳號「" + obj.actor + "」");
                                    sb2.Append("功能「" + obj.action_by + "」");
                                    sb2.Append("動作「" + obj.action + "」\n");
                                }
                            }

                            sb.AppendLine(string.Format("已刪除「{0}」筆系統歷程資料", list.Count.ToString()));
                            sb.AppendLine("詳細內容：");
                            sb.AppendLine(sb2.ToString());

                            if (list.Count > 0)
                            {
                                string qu = string.Format("delete from log where id in({0})", string.Join(",", list));
                                _Update.Execute(qu);

                                //FISCA.LogAgent.ApplicationLog.Log("[特殊歷程]", "刪除系統歷程", sb.ToString());
                                MsgBox.Show("已刪除系統歷程!!");

                                btnReF_Click(null, null);
                            }
                            else
                            {
                                MsgBox.Show("未選擇歷程資料\n已取消操作!!");
                            }
                        }
                        else
                        {
                            MsgBox.Show("已取消操作!!");
                        }
                    }
                    else
                    {
                        MsgBox.Show("授權碼錯誤!!");
                    }
                }
                else
                {
                    MsgBox.Show("已取消操作!!");
                }
            }
        }

        private void ViewForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //超級使用者
            if (DSAServices.IsSysAdmin)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    ISSpecial = true;

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("已開啟功能:");
                    sb.AppendLine("[特殊歷程檢視]");
                    MsgBox.Show(sb.ToString());
                }
            }
        }

        private void labelX1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //超級使用者
            //才有權限開啟Log刪除功能
            if (DSAServices.IsSysAdmin)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    刪除系統歷程ToolStripMenuItem.Visible = true;

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("已開啟功能:");
                    sb.AppendLine("[系統歷程刪除功能]");
                    MsgBox.Show(sb.ToString());
                }
            }
        }
    }

    enum ModeType
    {
        依日期, 依關鍵字, 依類別與ID
    }
}
