using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K12.LogView.Modules
{
    /// <summary>
    /// 代表目前使用者的相關權限資訊。
    /// </summary>
    public static class Permissions
    {
        //權限代碼來自ischool_Senior with FISCA\SmartSchool.Core\Resources\FeatureDefinition.xml
        //進行控管
        public static string 系統歷程總覽 { get { return "System0020"; } }
        public static string 系統歷程總覽國中 { get { return "StartButton0006"; } }

        public static string 使用者歷程 { get { return "System0010"; } }
        public static string 使用者歷程國中 { get { return "StartButton0005"; } }

        public static string 學生歷程 { get { return "Button0320"; } }
        public static string 學生歷程國中 { get { return "JHSchool.Student.Ribbon0200"; } }

        public static string 班級歷程 { get { return "Button0440"; } }
        public static string 班級歷程國中 { get { return "JHSchool.Class.Ribbon0200"; } }

        public static string 教師歷程 { get { return "Button0510"; } }
        public static string 教師歷程國中 { get { return "JHSchool.Teacher.Ribbon0200"; } }

        public static string 課程歷程 { get { return "Button0620"; } }
        public static string 課程歷程國中 { get { return "JHSchool.Course.Ribbon0200"; } }


        /// <summary>
        /// 進階日誌檢視
        /// </summary>
        public static bool 系統歷程總覽權限 { get { return FISCA.Permission.UserAcl.Current[系統歷程總覽].Executable || FISCA.Permission.UserAcl.Current[系統歷程總覽國中].Executable; } }

        /// <summary>
        /// 學生日誌檢視
        /// </summary>
        public static bool 學生歷程權限 { get { return FISCA.Permission.UserAcl.Current[學生歷程].Executable || FISCA.Permission.UserAcl.Current[學生歷程國中].Executable; } }


        /// <summary>
        /// 班級日誌檢視
        /// </summary>
        public static bool 班級歷程權限 { get { return FISCA.Permission.UserAcl.Current[班級歷程].Executable || FISCA.Permission.UserAcl.Current[班級歷程國中].Executable; } }

        /// <summary>
        /// 教師日誌檢視
        /// </summary>
        public static bool 教師歷程權限 { get { return FISCA.Permission.UserAcl.Current[教師歷程].Executable || FISCA.Permission.UserAcl.Current[教師歷程國中].Executable; } }

        /// <summary>
        /// 課程日誌檢視
        /// </summary>
        public static bool 課程歷程權限 { get { return FISCA.Permission.UserAcl.Current[課程歷程].Executable || FISCA.Permission.UserAcl.Current[課程歷程國中].Executable; } }

        /// <summary>
        /// 使用者歷程檢視
        /// </summary>
        public static bool 使用者歷程權限 { get { return FISCA.Permission.UserAcl.Current[使用者歷程].Executable || FISCA.Permission.UserAcl.Current[使用者歷程國中].Executable; } }

    }
}
