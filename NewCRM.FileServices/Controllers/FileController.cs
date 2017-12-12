using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace NewCRM.FileServices.Controllers
{
    [RoutePrefix("api/filestorage")]
    public class FileController : ApiController
    {
        private static readonly String _fileStoragePath = ConfigurationManager.AppSettings["FileStorage"];
        private static readonly String[] _denyUploadTypes = { ".exe", ".bat", ".bat" };

        [Route("upload"), HttpPost,HttpOptions]
        public IHttpActionResult Upload()
        {
            var responses = new List<dynamic>();

            try
            {
                var request = HttpContext.Current.Request;
                var accountId = request["accountId"];
                if (String.IsNullOrEmpty(accountId))
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = $@"{nameof(accountId)}参数验证失败"
                    });

                    return Json(responses);
                }

                var files = request.Files;
                if (files.Count == 0)
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = "未能获取到上传的文件"
                    });

                    return Json(responses);
                }
                var uploadtype = request["uploadtype"];
                if (String.IsNullOrEmpty(uploadtype))
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = $@"{nameof(uploadtype)}参数验证失败"
                    });

                    return Json(responses);
                }
                var middlePath = "";
                if (uploadtype.ToLower() == UploadType.Wallpaper.ToString().ToLower())
                {
                    middlePath = UploadType.Wallpaper.ToString();
                }
                else if (uploadtype.ToLower() == UploadType.Face.ToString().ToLower())
                {
                    middlePath = UploadType.Face.ToString();
                }
                else if (uploadtype.ToLower() == UploadType.Icon.ToString().ToLower())
                {
                    middlePath = UploadType.Icon.ToString();
                }


                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    string fileExtension;
                    if (file.FileName.StartsWith("__avatar"))
                    {
                        fileExtension = file.ContentType.Substring(file.ContentType.LastIndexOf("/", StringComparison.Ordinal) + 1);
                        if (fileExtension == "jpeg")
                        {
                            fileExtension = "jpg";
                        }
                    }
                    else
                    {
                        fileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal)+1);
                    }

                    if (_denyUploadTypes.Any(d => d.ToLower() == fileExtension))
                    {
                        responses.Add(new
                        {
                            IsSuccess = false,
                            Message = $@"后缀名为：{fileExtension}的文件禁止上传"
                        });
                    }
                    else
                    {
                        var bytes = new byte[file.InputStream.Length];
                        file.InputStream.Position = 0;
                        var fileFullPath = $@"{_fileStoragePath}/{accountId}/{middlePath}/";
                        var fileName = $@"{Guid.NewGuid().ToString().Replace("-", "")}.{fileExtension}";
                        if (!Directory.Exists(fileFullPath))
                        {
                            Directory.CreateDirectory(fileFullPath);
                        }

                        var md5 = CalculateFile.Calculate(file.InputStream);
                        file.InputStream.Position = 0;
                        using (var fileStream = new FileStream(fileFullPath + fileName, FileMode.Create, FileAccess.Write))
                        {
                            file.InputStream.Read(bytes, 0, bytes.Length);
                            fileStream.Write(bytes, 0, bytes.Length);

                            if (uploadtype.ToLower() == UploadType.Face.ToString().ToLower())
                            {
                                return Json(new { avatarUrls = fileFullPath.Substring(fileFullPath.IndexOf("/", StringComparison.Ordinal)) + fileName, msg = "", success = true });
                            }
                        }
                        using (Image originalImage = Image.FromFile(fileFullPath + fileName))
                        {
                            responses.Add(new
                            {
                                IsSuccess = true,
                                originalImage.Width,
                                originalImage.Height,
                                Title = fileName,
                                Url = fileFullPath.Substring(fileFullPath.IndexOf("/", StringComparison.Ordinal)) + fileName,
                                Md5 = md5,
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responses.Add(new
                {
                    IsSuccess = false,
                    Message = ex.GetType().Name
                });
            }

            return Json(responses);
        }
    }
}

public enum UploadType
{
    Wallpaper = 1,
    Face = 2,
    Icon = 3
}