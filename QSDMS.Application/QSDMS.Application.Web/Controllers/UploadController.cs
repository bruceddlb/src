using iFramework.Framework;
using QSDMS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QSDMS.Application.Web.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /Upload/
        public JsonResult Product(HttpPostedFileBase file)
        {
            var result = new ReturnMessage(false) { Message = "上传图片失败!" };
            result = Upload(ImageType.Product, new ImageWidthHeight[] {
                new ImageWidthHeight(){ Height=640,Width=640},
                new ImageWidthHeight(){ Height=300,Width=300},
                new ImageWidthHeight(){ Height=100,Width=100},
            }, false);
            return Json(result);
        }

        public JsonResult MemberRemit(HttpPostedFileBase file)
        {
            var result = new ReturnMessage(false) { Message = "上传图片失败!" };
            result = Upload(ImageType.MemberRemit, null, false);
            return Json(result);
        }

        public JsonResult ShopRemit(HttpPostedFileBase file)
        {
            var result = new ReturnMessage(false) { Message = "上传图片失败!" };
            result = Upload(ImageType.ShopRemit, null, false);
            return Json(result);
        }

        public JsonResult Certificate(HttpPostedFileBase file)
        {
            var result = new ReturnMessage(false) { Message = "上传图片失败!" };
            result = Upload(ImageType.Certificate, null, false);
            return Json(result);
        }

        public JsonResult Advertising(HttpPostedFileBase file)
        {
            var result = new ReturnMessage(false) { Message = "上传图片失败!" };
            result = Upload(ImageType.Advertising, new ImageWidthHeight[] {
                new ImageWidthHeight(){ Height=320,Width=544}
            }, false);
            return Json(result);
        }

        public JsonResult HeadPortrait(HttpPostedFileBase file)
        {
            var result = new ReturnMessage(false) { Message = "上传图片失败!" };
            result = Upload(ImageType.HeadPortrait, null, false);
            return Json(result);
        }

        [HttpGet]
        public JsonResult AccountCode(string url)
        {
            var result = new ReturnMessage(false) { Message = "获取用户二维码失败!" };
            var filePath = string.Format("/File/{0}/{1}/{2}.png", ImageType.AccountCode, DateTime.Now.ToString("yyyyMMdd"), Guid.NewGuid().ToString("N"));
            var flag = QRCodeHelper.GeneralQRCode(url, Server.MapPath(filePath), "M", 6, "Byte", "12");
            if (flag)
            {
                result.IsSuccess = true;
                result.Message = "获取用户二维码成功!";
                result.ResultData["file"] = filePath;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private ReturnMessage Upload(ImageType type, IEnumerable<ImageWidthHeight> imageSizes, bool isAddHost = false)
        {
            var result = new ReturnMessage(false) { Message = "上传文件失败!" };
            var fileList = new List<string>();
            try
            {
                var flag = true;
                var uploadHelper = new FileUpload(type.ToString());
                for (var i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var filePath = string.Format("/File/{0}/{1}/", type.ToString(), DateTime.Now.ToString("yyyyMMdd"));
                    var fileModel = uploadHelper.Save(file, Server.MapPath(filePath));
                    if (!fileModel.Status)
                    {
                        flag = false;
                        break;
                    }

                    var imagePath = filePath + fileModel.FileNewName;
                    if (isAddHost)
                    {
                        imagePath = string.Format("http://{0}{1}{2}", Request.Url.Host, Request.Url.Port == 80 ? "" : ":" + Request.Url.Port, imagePath);
                    }

                    fileList.Add(imagePath);

                    //生成图片则生成缩略图
                    if (imageSizes != null)
                    {
                        foreach (var size in imageSizes)
                        {
                            SaveFile(fileModel, size.Width, size.Height);
                        }
                    }
                }

                if (flag)
                {
                    result.IsSuccess = true;
                    result.Message = "上传文件成功!";
                    result.ResultData["files"] = fileList;
                }
                else
                {
                    result.Message = "上传文件失败,请重新上传!";
                }
            }
            catch (Exception ex)
            {
                ex.Data["Method"] = "UploadController => Upload";
                new ExceptionHelper().LogException(ex);
            }
            return result;
        }

        private void SaveFile(FileModel model, int width, int height)
        {
            var newName = string.Format("{0}_{1}_{2}", model.FileNewNoExtensionName, width, height);
            Thumbnail.MakeThumbnail(model.PhysicFullPath + model.FileNewName, model.PhysicFullPath + newName + model.FileExtension, width, height, ThumbnailMode.H, 200);
            model.FileNewName = newName + model.FileExtension;
        }
    }

    public class ImageWidthHeight
    {
        public int Width { get; set; }

        public int Height { get; set; }
    }
}