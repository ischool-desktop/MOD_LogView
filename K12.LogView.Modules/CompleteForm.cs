using System;
using System.Windows.Forms;

namespace K12.LogView.Modules
{
    //匯出完成的確認畫面
    public partial class CompleteForm : FISCA.Presentation.Controls.BaseForm
    {
        public CompleteForm()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}