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

            if (fileName != null)
            {
                Gallery.ImageExists(fileName);
                FullImage.Visible = true;
                FullImage.ImageUrl = "~/Content/Images/" + fileName;
            }

            if (Request.QueryString["upload"] == "success")
            {
                successPanel.Visible = true;
            }
        }

        public IEnumerable<Thumbnail> Repeater_GetData()
        {
            return Gallery.GetImagesNames();
        }

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
                        Gallery.SaveImage(FileUpload.FileContent, FileUpload.FileName);
                        Response.Redirect(String.Format("?img={0}{1}", FileUpload.FileName, "&upload=success"), false);
                    }
                    catch (Exception)
                    {
                        //Response.Redirect(String.Format("?img={0}{1}", FileUpload.FileName, "&upload=failed"), false);

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