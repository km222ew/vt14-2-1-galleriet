using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gallery.Model;

namespace Gallery
{
    public partial class Default : System.Web.UI.Page
    { 
        private Galleri _gallery;

        private Galleri Gallery
        {
            get
            {
                return _gallery ?? (_gallery = new Galleri());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var fileName = Request.QueryString["img"];

            //Om det finns en bild i querystring så sätts den som stor bild
            if (fileName != null)
            {
                Gallery.ImageExists(fileName);
                FullImage.Visible = true;
                FullImage.ImageUrl = "~/Content/Images/" + fileName;
            }

            //Om "success" finns i querystring så visas rätt-meddelande
            if (Request.QueryString["upload"] == "success")
            {
                successPanel.Visible = true;
            }

            //foreach (Control Control in this.Form.Controls)
            //{
            //    if ((Control) is HyperLink)
            //    {
            //        var hyperlink = Control as HyperLink;
            //        if (hyperlink.NavigateUrl == fileName)
            //        {
            //            hyperlink.CssClass = "active";
            //        }
            //    }
            //}
        }

        public IEnumerable<Thumbnail> Repeater_GetData()
        {
            return Gallery.GetImagesNames();
        }

        //Sätter en border runt en tumnagel när den binds
        protected void Repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var fileName = Request.QueryString["img"];
            var item = (Thumbnail)e.Item.DataItem;

            if (fileName == item.Name)
            {
                var hyperLink = (HyperLink)e.Item.FindControl("HyperLink");
                hyperLink.CssClass = "active";
            }
        }

        protected void Button_Click(object sender, EventArgs e)
        {

            if (IsValid)
            {
                if (FileUpload.HasFile)
                {
                    try
                    {
                        var fileName = Gallery.SaveImage(FileUpload.FileContent, FileUpload.FileName);
                        Response.Redirect(String.Format("?img={0}{1}", fileName, "&upload=success"), false);
                    }
                    catch (Exception)
                    {
                        CustomValidator cv = new CustomValidator();

                        cv.IsValid = false;

                        cv.ErrorMessage = "Ett fel inträffade då bilden skulle överföras.";

                        this.Page.Validators.Add(cv);
                    }
                }
            }   
        }

        protected void closeMessage_Click(object sender, ImageClickEventArgs e)
        {
            var fileName = Request.QueryString["img"];
            Response.Redirect(String.Format("?img={0}", fileName));
        }               
    }
}