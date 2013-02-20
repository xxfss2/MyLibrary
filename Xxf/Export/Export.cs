using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace Xxf.Export
{
    public class Export
    {
        public void CreateExcel<T>(ICollection<T> entitys, string fileName, string titleName, string headerText, bool isIE)
        {
            System.Text.StringBuilder strb = new System.Text.StringBuilder();

            strb.Append("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\"");

            strb.Append("xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");

            strb.Append("xmlns=\"http://www.w3.org/TR/REC-html40\">");

            strb.Append("<head><meta http-equiv='Content-Type' content='text ml; charset=utf-8'>");

            strb.Append("<style>");

            strb.Append(".xl26");

            strb.Append("{mso-style-parent:style0;");

            strb.Append("font-family:\"Times New Roman\", \"serif\";font-size: 11px;");

            strb.Append("mso-font-charset:0;");
            //数字到字符串
            strb.Append("mso-number-format:\"@\";}");

            strb.Append("</style>");

            strb.Append("<xml>");

            strb.Append("<x:ExcelWorkbook>");

            strb.Append("<x:ExcelWorksheets>");

            strb.Append("<x:ExcelWorksheet>");

            strb.Append("<x:Name>Sheet1</x:Name>");

            strb.Append("<x:WorksheetOptions>");

            strb.Append("<x:DefaultRowHeight>285</x:DefaultRowHeight>");

            strb.Append("<x:Selected/>");

            strb.Append("<x:Panes>");

            strb.Append("<x:Pane>");

            strb.Append("<x:Number>3</x:Number>");

            //strb.Append("<x:ActiveCol>1 </x:ActiveCol>");

            strb.Append("</x:Pane>");

            strb.Append("</x:Panes>");

            strb.Append("<x:ProtectContents>False</x:ProtectContents>");

            strb.Append("<x:ProtectObjects>False</x:ProtectObjects>");

            strb.Append("<x:ProtectScenarios>False</x:ProtectScenarios>");

            strb.Append("</x:WorksheetOptions>");

            strb.Append("</x:ExcelWorksheet>");

            strb.Append("<x:WindowHeight>6750</x:WindowHeight>");

            strb.Append("<x:WindowWidth>10620</x:WindowWidth>");

            strb.Append("<x:WindowTopX>480</x:WindowTopX>");

            strb.Append("<x:WindowTopY>75</x:WindowTopY>");

            strb.Append("<x:ProtectStructure>False</x:ProtectStructure>");

            strb.Append("<x:ProtectWindows>False</x:ProtectWindows>");

            strb.Append("</x:ExcelWorkbook>");

            strb.Append("</xml>");

            strb.Append("");

            strb.Append("</head><body style=\"font-family: 宋体;\"><table align=\"center\" style='border-collapse:collapse;table-layout:fixed'><tr>");

            if (entitys.Count == 0)
                goto noData;

            PropertyInfo[] propertyInfos = entitys.FirstOrDefault().GetType().GetProperties();
            int filesCount = propertyInfos.Count();
            strb.Append("<td colspan='" + filesCount + "' align='center' style='font-size:28px'> <b>" + titleName + "</b> </td></tr>");
            //写列标题    
            if (string.IsNullOrEmpty(headerText))
            {
                strb.Append("<tr>");
                for (int i = 0; i < filesCount; i++)
                {
                    strb.Append("<td><b>" + propertyInfos[i].Name + "</b></td>");
                }
                strb.Append("</tr>");
            }
            else
            {
                strb.Append(headerText);
            }
            //写数据    

            foreach (T entity in entitys)
            {
                strb.Append("<tr>");
                for (int j = 0; j < filesCount; j++)
                {
                    strb.Append("<td class='xl26' style='font-size:12px'>" + propertyInfos[j].GetValue(entity, null) + "</td>");
                }
                strb.Append("</tr>");
            }
        noData:
            strb.Append("</body></html>");

            HttpContext.Current.Response.Clear();

            HttpContext.Current.Response.Buffer = true;

            HttpContext.Current.Response.Charset = "UTF-8";
            //HttpUtility.UrlEncode(fileName, Encoding.UTF8)

            if (isIE)
            {
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8) + ".xls");
            }
            else
            {
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName + ".xls");
            }

            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");//设置输出流为简体中文    

            //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;

            HttpContext.Current.Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。    

            HttpContext.Current.Response.Write(strb);

            HttpContext.Current.Response.End();
        }
    }
}
