using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ELearningWeb.Models;
using E_LearningWeb.Bussiness;
using System.Text;
using System.Data;
using E_LearningWeb.student;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ELearningWeb.Document;
using Localization;
using E_LearningWeb.Models;
using ELearningWeb;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace E_LearningWeb_Mobile
{
    public partial class salesAgentStudentCourseTable : System.Web.UI.UserControl
    {
        protected PagedDataSource pds = new PagedDataSource();
        List<studentScheduleInfo> studentScheduleList = new List<studentScheduleInfo>();
        string[] weekNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        global::System.Resources.ResourceManager rm;
        protected void Page_Load(object sender, EventArgs e)
        {
            // in mobile.aspx, we set to load this page only when pageId = AGENT_4ND_PAGE_ID
            // but it will still run into here even pageId != AGENT_4ND_PAGE_ID
            if (Request.QueryString["pageId"] != Constant.AGENT_4ND_PAGE_ID)
            {
                return;
            }
            if (!windowsJs.CheckAuthority(Constant.AGENT_PAGE_ID))
            {
                windowsJs.SessionIsNull(this.Page);
                return;
            }            
            
            if (!IsPostBack)
            {
                rm = new global::System.Resources.ResourceManager("Resources.global",
                    global::System.Reflection.Assembly.Load("App_GlobalResources"));

                List<salesAgentStudentInfo> studentListInfo = new List<salesAgentStudentInfo>();
                studentListInfo = salesAgentManager.GetstudentInfo(Session["userID"].ToString(), "U.fName, U.lName", "");

				ddlStudentName.Items.Add(new ListItem(GetResourceString("select"), ""));
				string userID = "";
                foreach (salesAgentStudentInfo studentTemp in studentListInfo)
                {
                    if (String.Compare(userID, studentTemp.studentUserID, true) != 0)
                    {
                        ListItem myListItem = new ListItem(studentTemp.student_Name, studentTemp.studentUserID);
                        ddlStudentName.Items.Add(myListItem);
                        userID = studentTemp.studentUserID;
                    }
                }
                // Init blank list
				pds.DataSource = new List<studentScheduleInfo>();
                dtlStudentScheduleInfo.DataSource = pds;
                dtlStudentScheduleInfo.DataBind();
            }

            if (string.IsNullOrEmpty(this.currentWeek.Value))
            {
                this.currentWeek.Value = FormatDate(DateTime.Now);
            }
        }

        private void GetScheduleWeekData(DateTime baseDate)
        {
            this.currentWeek.Value = FormatDate(baseDate);
            DateTime firstDateOfWeek = CalculateFirstDateOfWeek(baseDate);
            DateTime lastDateOfWeek = firstDateOfWeek.AddDays(6);
            
            List<studentScheduleInfo> studentScheduleFilterList = new List<studentScheduleInfo>();
            foreach (studentScheduleInfo studentSchedule in studentScheduleList)
            {
                // filter out records not in this week
                DateTime startimeDT = new DateTime();
                DateTime.TryParse(studentSchedule.startDT, out startimeDT);
                if (FormatDate(startimeDT).CompareTo(FormatDate(firstDateOfWeek)) < 0
                    || FormatDate(startimeDT).CompareTo(FormatDate(lastDateOfWeek)) > 0)
                {
                    continue;
                }

                DateTime endtimeDT = new DateTime();
                DateTime.TryParse(studentSchedule.endDT, out endtimeDT);

                string txt = studentSchedule.courseName;
                if (!string.IsNullOrEmpty(txt))//判断当前是否有课
                {
                    txt = txt + "<br/>(" + GetTime(startimeDT) + "~" + GetTime(endtimeDT) + ")";
                    string status = studentSchedule.courseStatus;
                    //新排
                    if (status == "2200")
                    {
                        txt = string.Format("<font color=blue>{0}</font>", txt);//新排的课蓝色显示
                    }
                    //旷课
                    else if (status == Constant.SCHEDULE_STATUSID_ABSENT_S
                        || status == Constant.SCHEDULE_STATUSID_ABSENT_T
                        || status == Constant.SCHEDULE_STATUSID_NO_SHOW_S
                        || status == Constant.SCHEDULE_STATUSID_NO_SHOW_T)
                    {
                        txt = string.Format("<font color=red>{0}</font>", txt);//旷课红色显示
                    }
                    //已上
                    //else if (studentScheduleInfoTemp.courseStatus == "2210")
                    else if (status == Constant.SCHEDULE_STATUSID_LATE_S
                        || status == Constant.SCHEDULE_STATUSID_LATE_T
                        || status == Constant.SCHEDULE_STATUSID_COMPLETE)
                    {
                        txt = string.Format("<font color=black>{0}</font>", txt);//已上课程黑色显示
                    }
                    //取消
                    else if (status == Constant.SCHEDULE_STATUSID_CANCEL_S
                        || status == Constant.SCHEDULE_STATUSID_CANCEL_T)
                    {
                        txt = string.Format("<font color=gray>{0}</font>", txt);//取消课程灰色显示
                    }
                    else
                    {
                        txt = string.Format("<font color=green>{0}</font>", txt);//其它情况绿色显示
                    }
                    studentSchedule.courseName = txt;
                    studentScheduleFilterList.Add(studentSchedule);
                }
            }
            // now we get courses for this week, construct show list
            List<studentScheduleInfo> studentScheduleShowList = new List<studentScheduleInfo>();
            DateTime dt = firstDateOfWeek;
            while (dt <= lastDateOfWeek)
            {
                Boolean hasCourse = false;
                studentScheduleInfo info = new studentScheduleInfo();
                info.startDT = FormatDate(dt) + "<br>" + GetResourceString(weekNames[GetWeekByDateTime(dt)]);
                foreach (studentScheduleInfo schedule in studentScheduleFilterList)
                {
                    DateTime currentDT = new DateTime();
                    DateTime.TryParse(schedule.startDT, out currentDT);
                    if (FormatDate(currentDT).CompareTo(FormatDate(dt)) == 0)
                    {
                        if (hasCourse)
                        {
                            info.courseName = info.courseName + "<br><br>" + schedule.courseName;
                        }
                        else
                        {
                            info.courseName = schedule.courseName;
                            hasCourse = true;
                        }
                    }
                }
                if (!hasCourse)
                {
                    info.courseName = "-";
                }
                studentScheduleShowList.Add(info);
                dt = dt.AddDays(1);
            }

            pds.DataSource = studentScheduleShowList;
            dtlStudentScheduleInfo.DataSource = pds;
            dtlStudentScheduleInfo.DataBind();

        }

        private DateTime CalculateFirstDateOfWeek(DateTime baseDate)
        {
            // week = 0: Sunday, 1: Monday, ... , 6: Saturday
            int week = GetWeekByDateTime(baseDate);
            DateTime firstDateOfWeek = new DateTime();
            if (IsLang(Constant.ENG_LANG))
            {
                // Foe English language, first day of week is Sunday
                // e.g. for 2017.10.08, should show: 2017.10.08 - 2017.10.14
                firstDateOfWeek = baseDate.AddDays(-week);
            }
            else if (IsLang(Constant.CHI_LANG))
            {
                // Foe Chinese language, first day of week is Monday
                // e.g. for 2017.10.08, should show: 2017.10.02 - 2017.10.08
                if (week == 0)
                {
                    firstDateOfWeek = baseDate.AddDays(-6);
                }
                else
                {
                    firstDateOfWeek = baseDate.AddDays(1 - week);
                }
            }
            return firstDateOfWeek;
        }

        private Boolean IsLang(string lang)
        {
            return HttpContext.Current.Session["PreferredCulture"].ToString().ToUpper() == lang;
        }

        private int GetWeekByDateTime(DateTime dt)
        {
            return Convert.ToInt32(dt.DayOfWeek.ToString("d"));
        }

        private String FormatDate(DateTime dt)
        {
            return dt.Date.ToString("yyyy/MM/dd");
        }

        private String GetTime(DateTime dt)
        {
            return dt.ToString("HH:mm");
        }

        protected void Item_Bound(Object sender, DataListItemEventArgs e)
        {
            // set background to light gray for odd row
            if (e.Item.ItemType == ListItemType.Item)
            {
                e.Item.BackColor = System.Drawing.Color.LightGray;
            }
			// set no record display info
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (pds.Count <= 0)
                {
                    ((Label)e.Item.FindControl("lblNoRecord")).Visible = true;
                }
            }
        }

        protected void NextWeek_Click(object sender, EventArgs e)
        {
            DateTime dt = Convert.ToDateTime(this.currentWeek.Value).AddDays(7);
            GetScheduleWeekData(dt);
        }
        protected void PrevWeek_Click(object sender, EventArgs e)
        {
            DateTime dt = Convert.ToDateTime(this.currentWeek.Value).AddDays(-7);
            GetScheduleWeekData(dt);
        }

        private string GetResourceString(String key)
        {
            string languageCode = Session["PreferredCulture"].ToString().ToUpper();
            return rm.GetString(key, new global::System.Globalization.CultureInfo(languageCode));
        }

        protected void ddlStudentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStudentName.SelectedItem != null)
            {
                if (!string.IsNullOrEmpty(ddlStudentName.SelectedItem.Value))
                {
                    try
                    {
                        studentScheduleList = studentManager.GetstudentScheduleInfo(ddlStudentName.SelectedItem.Value, 0, 0);
                        GetScheduleWeekData(DateTime.Now);
                    }
                    catch
                    {
                        windowsJs.Alert(this.Page, "周课表数据获取失败，请与系统管理员联系。");
                        return;
                    }
                }
            }
			else
			{
				pds.DataSource = new List<studentScheduleInfo>();
                dtlStudentScheduleInfo.DataSource = pds;
                dtlStudentScheduleInfo.DataBind();
			}
        }
    }
}
