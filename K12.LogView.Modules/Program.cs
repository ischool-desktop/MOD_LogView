
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA;
using FISCA.Presentation;
using FISCA.Permission;

namespace K12.LogView.Modules
{
    public static class Program
    {
        [MainMethod()]
        static public void Main()
        {
            //string URL查詢系統歷程 = "ischool/高中系統/共用/系統/查詢系統歷程";
            //string URL查詢個人歷程 = "ischool/高中系統/共用/系統/查詢使用者登入帳號歷程";
            //string URL學生歷程 = "ischool/高中系統/共用/系統/學生/學生歷程";
            //string URL班級歷程 = "ischool/高中系統/共用/系統/班級/班級歷程";
            //string URL教師歷程 = "ischool/高中系統/共用/系統/教師/教師歷程";
            //string URL課程歷程 = "ischool/高中系統/共用/系統/課程/課程歷程";

            string SystemLog = "查詢系統歷程";
            string UserLog = "查詢個人歷程";
            //string StudentLog = "學生歷程";
            //string ClassLog = "班級歷程";
            //string TeacherLog = "教師歷程";
            //string CourseLog = "課程歷程";

            //FISCA.Features.Register(URL查詢系統歷程, arg =>
            //{
            //    new ViewForm().ShowDialog();
            //});

            //FISCA.Features.Register(URL查詢個人歷程, arg =>
            //{
            //    new ViewForm("查詢個人歷程").ShowDialog();
            //});

            //FISCA.Features.Register(URL學生歷程, arg =>
            //{
            //    new ViewForm("student", K12.Presentation.NLDPanels.Student.SelectedSource).ShowDialog();
            //});

            //FISCA.Features.Register(URL班級歷程, arg =>
            //{
            //    new ViewForm("class", K12.Presentation.NLDPanels.Class.SelectedSource).ShowDialog();
            //});

            //FISCA.Features.Register(URL教師歷程, arg =>
            //{
            //    new ViewForm("teacher", K12.Presentation.NLDPanels.Teacher.SelectedSource).ShowDialog();
            //});

            //FISCA.Features.Register(URL課程歷程, arg =>
            //{
            //    new ViewForm("course", K12.Presentation.NLDPanels.Course.SelectedSource).ShowDialog();
            //});

            #region 日誌相關功能
            MenuButton menuButton = FISCA.Presentation.MotherForm.StartMenu;
            menuButton["系統歷程"].Image = Properties.Resources.folder_zoom_64;
            //menuButton["系統歷程"][SystemLog].Image = Properties.Resources.boolean_field_fav_64;
            menuButton["系統歷程"][SystemLog].Enable = Permissions.系統歷程總覽權限;
            menuButton["系統歷程"][SystemLog].Click += delegate
            {
                new ViewForm().ShowDialog();
            };

            //menuButton["系統歷程"][UserLog].Image = Properties.Resources.boolean_field_fav_64;
            menuButton["系統歷程"][UserLog].Enable = Permissions.使用者歷程權限;
            menuButton["系統歷程"][UserLog].Click += delegate
            {
                new ViewForm("查詢個人歷程").ShowDialog();
            };

            //RibbonBarItem rbItem = FISCA.Presentation.MotherForm.RibbonBarItems["學生", "其它"];
            //rbItem[StudentLog].Size = RibbonBarButton.MenuButtonSize.Medium;
            //rbItem[StudentLog].Image = Properties.Resources.folder_zoom_64;
            //rbItem[StudentLog].Enable = (Permissions.學生歷程權限 && K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0);
            //rbItem[StudentLog].Click += delegate
            //{
            //    new ViewForm("student", K12.Presentation.NLDPanels.Student.SelectedSource).ShowDialog();
            //};

            //rbItem = FISCA.Presentation.MotherForm.RibbonBarItems["班級", "其它"];
            //rbItem[ClassLog].Size = RibbonBarButton.MenuButtonSize.Medium;
            //rbItem[ClassLog].Image = Properties.Resources.folder_zoom_64;
            //rbItem[ClassLog].Enable = (Permissions.班級歷程權限 && K12.Presentation.NLDPanels.Class.SelectedSource.Count > 0);
            //rbItem[ClassLog].Click += delegate
            //{
            //    new ViewForm("class", K12.Presentation.NLDPanels.Class.SelectedSource).ShowDialog();
            //};

            //rbItem = FISCA.Presentation.MotherForm.RibbonBarItems["教師", "其它"];
            //rbItem[TeacherLog].Size = RibbonBarButton.MenuButtonSize.Medium;
            //rbItem[TeacherLog].Image = Properties.Resources.folder_zoom_64;
            //rbItem[TeacherLog].Enable = (Permissions.教師歷程權限 && K12.Presentation.NLDPanels.Teacher.SelectedSource.Count > 0);
            //rbItem[TeacherLog].Click += delegate
            //{
            //    new ViewForm("teacher", K12.Presentation.NLDPanels.Teacher.SelectedSource).ShowDialog();
            //};

            //rbItem = FISCA.Presentation.MotherForm.RibbonBarItems["課程", "其它"];
            //rbItem[CourseLog].Size = RibbonBarButton.MenuButtonSize.Medium;
            //rbItem[CourseLog].Image = Properties.Resources.folder_zoom_64;
            //rbItem[CourseLog].Enable = (Permissions.課程歷程權限 && K12.Presentation.NLDPanels.Course.SelectedSource.Count > 0);
            //rbItem[CourseLog].Click += delegate
            //{
            //    new ViewForm("course", K12.Presentation.NLDPanels.Course.SelectedSource).ShowDialog();
            //};
            #endregion

            #region SelectedSourceChanged
            //K12.Presentation.NLDPanels.Student.SelectedSourceChanged += delegate
            //{
            //    if (K12.Presentation.NLDPanels.Student.SelectedSource.Count > 0)
            //    {
            //        FISCA.Presentation.MotherForm.RibbonBarItems["學生", "其它"][StudentLog].Enable = Permissions.學生歷程權限;
            //    }
            //    else
            //    {
            //        FISCA.Presentation.MotherForm.RibbonBarItems["學生", "其它"][StudentLog].Enable = false;
            //    }
            //};

            //K12.Presentation.NLDPanels.Class.SelectedSourceChanged += delegate
            //{
            //    if (K12.Presentation.NLDPanels.Class.SelectedSource.Count > 0)
            //    {
            //        FISCA.Presentation.MotherForm.RibbonBarItems["班級", "其它"][ClassLog].Enable = Permissions.班級歷程權限;
            //    }
            //    else
            //    {
            //        FISCA.Presentation.MotherForm.RibbonBarItems["班級", "其它"][ClassLog].Enable = false;
            //    }
            //};

            //K12.Presentation.NLDPanels.Teacher.SelectedSourceChanged += delegate
            //{
            //    if (K12.Presentation.NLDPanels.Teacher.SelectedSource.Count > 0)
            //    {
            //        FISCA.Presentation.MotherForm.RibbonBarItems["教師", "其它"][TeacherLog].Enable = Permissions.教師歷程權限;
            //    }
            //    else
            //    {
            //        FISCA.Presentation.MotherForm.RibbonBarItems["教師", "其它"][TeacherLog].Enable = false;
            //    }
            //};

            //K12.Presentation.NLDPanels.Course.SelectedSourceChanged += delegate
            //{
            //    if (K12.Presentation.NLDPanels.Course.SelectedSource.Count > 0)
            //    {
            //        FISCA.Presentation.MotherForm.RibbonBarItems["課程", "其它"][CourseLog].Enable = Permissions.課程歷程權限;
            //    }
            //    else
            //    {
            //        FISCA.Presentation.MotherForm.RibbonBarItems["課程", "其它"][CourseLog].Enable = false;
            //    }
            //};
            #endregion

            #region 權限登錄
            Catalog ribbon = RoleAclSource.Instance["系統"]["系統歷程"];
            ribbon.Add(new RibbonFeature(Permissions.系統歷程總覽國中, SystemLog));

            ribbon = RoleAclSource.Instance["系統"]["系統歷程"];
            ribbon.Add(new RibbonFeature(Permissions.使用者歷程國中, "查詢個人歷程"));

            //ribbon = RoleAclSource.Instance["學生"]["功能按鈕"];
            //ribbon.Add(new RibbonFeature(Permissions.學生歷程國中, StudentLog));

            //ribbon = RoleAclSource.Instance["班級"]["功能按鈕"];
            //ribbon.Add(new RibbonFeature(Permissions.班級歷程國中, ClassLog));

            //ribbon = RoleAclSource.Instance["教師"]["功能按鈕"];
            //ribbon.Add(new RibbonFeature(Permissions.教師歷程國中, TeacherLog));

            //ribbon = RoleAclSource.Instance["課程"]["功能按鈕"];
            //ribbon.Add(new RibbonFeature(Permissions.課程歷程國中, CourseLog));
            #endregion

            //Log修改記錄
            FISCA.Permission.RoleAclSource.Instance.PermissionChanged += new EventHandler<PermissionChangedEventArgs>(Instance_PermissionChanged);
        }

        static void Instance_PermissionChanged(object sender, PermissionChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("已修改角色「" + e.RoleName + "」的權限狀態"); //角色名稱

            foreach (PermissionChangeInfo info in e.ChangeSet)
            {
                //如果有舊權限的處理行為
                if (info.OriginPermission.HasValue)
                {
                    //是否已指定新權限
                    if (info.NewPermission.HasValue)
                    {
                        string PermissionName1 = GetPermissionName(info.OriginPermission.Value);
                        string PermissionName2 = GetPermissionName(info.NewPermission.Value);
                        sb.AppendLine("權限已由「" + PermissionName1 + "」" + "修改為「" + PermissionName2 + "」功能「" + info.Title + "」");
                    }
                    else //沒有新權限
                    {
                        string PermissionName1 = GetPermissionName(info.OriginPermission.Value);
                        sb.AppendLine("權限已由「" + PermissionName1 + "」" + "修改為「未指定」功能「" + info.Title + "」");
                    }
                }
                else //沒權限
                {
                    //是否已指定新權限
                    if (info.NewPermission.HasValue)
                    {
                        string PermissionName2 = GetPermissionName(info.NewPermission.Value);
                        sb.AppendLine("權限新增指定為「" + PermissionName2 + "」功能「" + info.Title + "」");
                    }
                    else //沒有新權限
                    {
                        //沒舊權限,也沒新權限
                        //略過
                    }
                }

            }

            FISCA.LogAgent.ApplicationLog.Log("系統歷程", "權限修改", sb.ToString());
        }

        private static string GetPermissionName(AccessOptions accessOptions)
        {
            string name = "";
            switch (accessOptions)
            {
                case AccessOptions.Edit:
                    name = "可編輯";
                    break;
                case AccessOptions.Execute:
                    name = "可執行";
                    break;
                case AccessOptions.Custom:
                    name = "特殊";
                    break;
                case AccessOptions.View:
                    name = "可檢視";
                    break;
                case AccessOptions.None:
                    name = "無權限";
                    break;
                default:
                    name = "無權限";
                    break;
            }

            return name;
        }
    }
}
