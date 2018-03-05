using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;

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

                for(var i = 0 ; i < files.Count ; i++)
                {
                    var file = files[i];
                    var fileExtension = GetFileExtension(file);

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

                        var tempFile = CreateFilePath(accountId, middlePath, fileExtension);
                        var md5 = Calculate(file.InputStream);
                        file.InputStream.Position = 0;
                        using(var fileStream = new FileStream(tempFile.FullPath, FileMode.Create, FileAccess.Write))
                        {
                            file.InputStream.Read(bytes, 0, bytes.Length);
                            fileStream.Write(bytes, 0, bytes.Length);
                        }
                        using(var originalImage = Image.FromFile(tempFile.FullPath))
                        {
                            if(middlePath == UploadType.Icon)
                            {
                                GetReducedImage(49, 49, originalImage, tempFile);
                                responses.Add(new { IsSuccess = true, tempFile.Url });
                            }
                            else if(middlePath == UploadType.Face)
                            {
                                GetReducedImage(20, 20, originalImage, tempFile);
                                return Json(new { avatarUrls = tempFile.Url, msg = "", success = true });
                            }
                            else
                            {
                                responses.Add(new
                                {
                                    IsSuccess = true,
                                    originalImage.Width,
                                    originalImage.Height,
                                    Title = "",
                                    tempFile.Url,
                                    Md5 = md5,
                                });
                            }
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

        private static UploadType GetUploadType(string uploadtype)
        {
            if(uploadtype.ToLower() == UploadType.Wallpaper.ToString().ToLower())
            {
                return UploadType.Wallpaper;
            }
            else if(uploadtype.ToLower() == UploadType.Face.ToString().ToLower())
            {
                return UploadType.Face;
            }
            else if(uploadtype.ToLower() == UploadType.Icon.ToString().ToLower())
            {
                return UploadType.Icon;
            }

            throw new Exception($@"{uploadtype}:未被识别为有效的上传类型");
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

        private static TempFile CreateFilePath(String accountId, UploadType middlePath, String fileExtension)
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
                FullPath = $@"{fileFullPath}{fileName}"
            };
            tempFile.ResetUrl();
            return tempFile;
        }

        public void GetReducedImage(int width, int height, Image imageFrom, TempFile tempFile)
        {
            // 源图宽度及高度 
            var imageFromWidth = imageFrom.Width;
            var imageFromHeight = imageFrom.Height;
            try
            {
                // 生成的缩略图实际宽度及高度.如果指定的高和宽比原图大，则返回原图；否则按照指定高宽生成图片
                if(width >= imageFromWidth && height >= imageFromHeight)
                {
                    return;
                }
                else
                {
                    Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(() => { return false; });
                    //调用Image对象自带的GetThumbnailImage()进行图片缩略
                    var reducedImage = imageFrom.GetThumbnailImage(width, height, callb, IntPtr.Zero);
                    //将图片以指定的格式保存到到指定的位置
                    var newName = $@"small_{Guid.NewGuid().ToString().Replace("-", "")}.png";
                    var newFileFullPath = $@"{tempFile.Path}{newName}";
                    tempFile.FullPath = newFileFullPath;
                    tempFile.ResetUrl();
                    reducedImage.Save(newFileFullPath, ImageFormat.Png);
                }
            }
            catch(Exception ex)
            {
                //抛出异常
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// 计算指定流的MD5值
        /// </summary>
        /// <param name="stream">指定需要计算的流</param>
        /// <returns></returns>
        public static String Calculate(Stream stream)
        {
            if(stream == null)
            {
                return "";
            }
            var md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(stream);
            var b = md5.Hash;
            md5.Clear();
            var sb = new StringBuilder(32);
            foreach(var t in b)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
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

    public void ResetUrl()
    {
        if(String.IsNullOrEmpty(FullPath))
        {
            return;
        }

        Url = FullPath.Substring(FullPath.IndexOf("/", StringComparison.Ordinal));
    }
}