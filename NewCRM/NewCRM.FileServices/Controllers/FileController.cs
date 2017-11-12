using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace NewCRM.FileServices.Controllers
{
    [RoutePrefix("api/filestorage")]
    public class FileController : ApiController
    {
        private static readonly String _fileStoragePath = ConfigurationManager.AppSettings["FileStorage"];
        private static readonly String[] _denyUploadTypes = new[] { ".exe", ".bat", ".bat" };

        [Route("upload"), HttpPost]
        public IHttpActionResult Upload()
        {
            var responses = new List<FileStorageResponse>();

            try
            {
                var request = HttpContext.Current.Request;
                var accountId = request["accountId"];
                if (String.IsNullOrEmpty(accountId))
                {
                    responses.Add(new FileStorageResponse
                    {
                        IsSuccess = false,
                        Message = $@"{nameof(accountId)}参数验证失败"
                    });

                    return Json(responses);
                }

                var files = request.Files;
                if (files == null || files.Count == 0)
                {
                    responses.Add(new FileStorageResponse
                    {
                        IsSuccess = false,
                        Message = "未能获取到上传的文件"
                    });

                    return Json(responses);
                }
                var type = request["type"];
                var middlePath = "";
                if (type == "1")
                {
                    middlePath = "wallpaper";
                }
                else if (type == "2")
                {
                    middlePath = "face";
                }


                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];

                    var fileExtension = file.FileName.Substring(file.FileName.LastIndexOf("."));
                    if (_denyUploadTypes.Any(d => d.ToLower() == fileExtension))
                    {
                        responses.Add(new FileStorageResponse
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
                        var fileName = $@"{Guid.NewGuid().ToString().Replace("-", "")}{fileExtension}";
                        if (!Directory.Exists(fileFullPath))
                        {
                            Directory.CreateDirectory(fileFullPath);
                        }
                        using (var fileStream = new FileStream($@"{fileFullPath}/{Guid.NewGuid().ToString().Replace("-", "") }.jpg", FileMode.CreateNew, FileAccess.Write))
                        {
                            file.InputStream.Read(bytes, 0, bytes.Length);
                            fileStream.Write(bytes, 0, bytes.Length);
                        }

                        responses.Add(new FileStorageResponse
                        {
                            IsSuccess = true,
                            Model = fileFullPath.Substring(fileFullPath.IndexOf(@"/")) + fileName
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                responses.Add(new FileStorageResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }

            return Json(responses);
        }
    }

    internal class FileStorageResponse
    {
        public Boolean IsSuccess { get; set; }

        public String Message { get; set; }

        public String Model { get; set; }
    }
}