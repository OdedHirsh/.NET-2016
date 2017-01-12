using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.IO;

namespace HTML_Continuens_51
{
    public partial class signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (StreamReader StreamReader = new StreamReader(Request.InputStream))
            {
                int oneByte;
                while ((oneByte = StreamReader.Read()) != -1)
                {
                    char c = (char)oneByte;
                    Debug.Print(c.ToString());
                }


            }
            
        }
    }
}