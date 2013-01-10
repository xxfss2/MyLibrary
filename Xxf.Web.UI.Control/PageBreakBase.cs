using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Jiuzh.CoreBase;
using System.ComponentModel;

namespace Xxf.Web.UI.Control
{
    public class PageBreakBase : System.Web.UI.UserControl, IAsynBindControl,IClientScriptControl 
    {
        private const int _defaultPageSize = 10;
        public PageBreakParam Break_Param = new PageBreakParam();
        public SortParam Sort_Param = new SortParam();
        protected const string PAGESIZE = "xxf_pagesize";
        protected const string PAGEINDEX = "xxf_pageindex";
        protected const string SORTFIELD = "xxf_sortfield";
        protected const string SORTRULE = "xxf_sortrule";
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
          //  _pageName = System.IO.Path.GetFileName(Request.Path);
            if (Request.Form[PAGESIZE] == null)
            {
                Break_Param.PageSize = _defaultPageSize;
                Break_Param.PageIndex = 0;
            }
            else
            {
                Break_Param.PageSize = Convert.ToInt32(Request.Form[PAGESIZE]);
                Break_Param.PageIndex = Convert.ToInt32(Request.Form[PAGEINDEX]);
            }
            if (Request.Form[SORTFIELD] != null && !string .IsNullOrEmpty ( Request .Form [SORTFIELD].ToString ()))
            {
                Sort_Param.SortCode = Request.Form[SORTFIELD];
                if (Request.Form[SORTRULE] != null)
                    Sort_Param.SortRule = (SortRules)Convert.ToInt32(Request.Form[SORTRULE]);
                else
                    Sort_Param.SortRule = SortRules.ESC;
            }
            else
                Sort_Param = null;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public string PageCount
        {
            get
            {
                return Math.Ceiling((double)Break_Param.DataCount / Break_Param.PageSize).ToString();
            }
        }
        /// <summary>
        /// 与该分页控件关联的表格控件
        /// </summary>
        [TypeConverter(typeof(ControlIDConverter))]
        public string TableControl
        {
            get;
            set;
        }
        public string ScriptId;
        public string DivId;
        private string _RepeterID = null;
        public string RepeaterID {
            get
            {
                if (_RepeterID == null)
                    throw new Exception("RepeterID属性未指定值");
                return _RepeterID;
            }
            set
            {
                _RepeterID = value;
            }
        }


        #region IAsynBindControl 成员

        public string GetBindScript()
        {
            return string.Format("{0}.dataBind,{0}", this.ClientID);
        }

        #endregion

        #region IClientScriptControl 成员

        public string GetScript()
        {
            DivId = this.Page.FindControl(TableControl).ClientID;
            ScriptId = this.ClientID;
            return string.Format("var {0} = new Xxf.PageBreak('{0}',{1}, {2}, {3},'{4}');", this.ClientID, Break_Param.PageSize, Break_Param.PageIndex, Break_Param.DataCount, DivId);
        }

        #endregion
    }
}
