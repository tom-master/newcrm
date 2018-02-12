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

        [Route("upload"), HttpPost, HttpOptions]
        public IHttpActionResult Upload()
        {
            var responses = new List<dynamic>();

            try
            {
                var request = HttpContext.Current.Request;
                var accountId = request["accountId"];

                if(String.IsNullOrEmpty(accountId))
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = $@"{nameof(accountId)}参数验证失败"
                    });

                    return Json(responses);
                }

                var files = request.Files;
                if(files.Count == 0)
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = "未能获取到上传的文件"
                    });

                    return Json(responses);
                }

                var uploadtype = request["uploadtype"];
                if(String.IsNullOrEmpty(uploadtype))
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = $@"{nameof(uploadtype)}参数验证失败"
                    });

                    return Json(responses);
                }

                if(String.IsNullOrEmpty(_fileStoragePath))
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = $@"未能找到服务器配置的存储路径"
                    });
                    return Json(responses);
                }

                var middlePath = GetUploadType(uploadtype);
                if(String.IsNullOrEmpty(middlePath))
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = $@"{uploadtype}:未被识别为有效的上传类型"
                    });
                    return Json(responses);
                }

                for(int i = 0 ; i < files.Count ; i++)
                {
                    var file = files[i];
                    string fileExtension = GetFileExtension(file);

                    if(_denyUploadTypes.Any(d => d.ToLower() == fileExtension))
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


                        TempFile tempFile = CreateFilePath(accountId, middlePath, fileExtension);

                        var md5 = CalculateFile.Calculate(file.InputStream);
                        file.InputStream.Position = 0;
                        using(var fileStream = new FileStream(tempFile.FullPath, FileMode.Create, FileAccess.Write))
                        {
                            file.InputStream.Read(bytes, 0, bytes.Length);
                            fileStream.Write(bytes, 0, bytes.Length);

                            if(uploadtype.ToLower() == UploadType.Face.ToString().ToLower())
                            {
                                return Json(new { avatarUrls = tempFile.Url, msg = "", success = true });
                            }
                        }
                        using(Image originalImage = Image.FromFile(tempFile.FullPath))
                        {
                            responses.Add(new
                            {
                                IsSuccess = true,
                                originalImage.Width,
                                originalImage.Height,
                                Title = "",
                                Url = tempFile.Url,
                                Md5 = md5,
                            });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                responses.Add(new
                {
                    IsSuccess = false,
                    Message = ex.GetType().Name
                });
            }

            return Json(responses);
        }

        private static string GetUploadType(string uploadtype)
        {
            if(uploadtype.ToLower() == UploadType.Wallpaper.ToString().ToLower())
            {
                return UploadType.Wallpaper.ToString().ToLower();
            }
            else if(uploadtype.ToLower() == UploadType.Face.ToString().ToLower())
            {
                return UploadType.Face.ToString().ToLower();
            }
            else if(uploadtype.ToLower() == UploadType.Icon.ToString().ToLower())
            {
                return UploadType.Icon.ToString().ToLower();
            }
            return "";
        }

        private static string GetFileExtension(HttpPostedFile file)
        {
            string fileExtension;
            if(file.FileName.StartsWith("__avatar"))
            {
                fileExtension = file.ContentType.Substring(file.ContentType.LastIndexOf("/", StringComparison.Ordinal) + 1);
                if(fileExtension == "jpeg")
                {
                    fileExtension = "jpg";
                }
            }
            else
            {
                fileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1);
            }

            return fileExtension.ToLower();
        }

        private static TempFile CreateFilePath(String accountId, String middlePath, String fileExtension)
        {
            var fileFullPath = $@"{_fileStoragePath}/{accountId}/{middlePath}/";
            var fileName = $@"{Guid.NewGuid().ToString().Replace("-", "")}.{fileExtension}";
            if(!Directory.Exists(fileFullPath))
            {
                Directory.CreateDirectory(fileFullPath);
            }

            var tempFile = new TempFile
            {
                Path = fileFullPath,
                Name = fileName,
                FullPath = $@"{fileFullPath}{fileName}",
                Url = ""
            };
            tempFile.Url = tempFile.FullPath.Substring(tempFile.FullPath.IndexOf("/", StringComparison.Ordinal));
            return tempFile;
        }
    }
}

public enum UploadType
{
    Wallpaper = 1,
    Face = 2,
    Icon = 3
}

public class TempFile
{
    public String Path { get; set; }

    public String Name { get; set; }

    public String FullPath { get; set; }

    public String Url { get; set; }
}