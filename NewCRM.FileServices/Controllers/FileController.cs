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

                var uploadType = request["uploadtype"];
                if(String.IsNullOrEmpty(uploadType))
                {
                    responses.Add(new
                    {
                        IsSuccess = false,
                        Message = $@"{nameof(uploadType)}参数验证失败"
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


                for(var i = 0 ; i < files.Count ; i++)
                {
                    var file = files[i];
                    var fileExtension = RequestFile.GetFileExtension(file);

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
                        var requestFile = CreateRequestFile(accountId, uploadType, fileExtension);

                        using(var fileStream = new FileStream(requestFile.FullPath, FileMode.Create, FileAccess.Write))
                        {
                            file.InputStream.Read(bytes, 0, bytes.Length);
                            fileStream.Write(bytes, 0, bytes.Length);
                        }

                        if(!File.Exists(requestFile.FullPath))
                        {
                            responses.Add(new
                            {
                                IsSuccess = false,
                                Message = $@"文件上传失败"
                            });
                        }

                        using(var originalImage = Image.FromFile(requestFile.FullPath))
                        {
                            requestFile.Image = originalImage;

                            if(requestFile.FileType == FileType.Icon)
                            {
                                requestFile.GetReducedImage(49, 49);
                                responses.Add(new { IsSuccess = true, requestFile.Url });
                            }
                            else if(requestFile.FileType == FileType.Face)
                            {
                                requestFile.GetReducedImage(20, 20);
                                return Json(new { avatarUrls = requestFile.Url, msg = "", success = true });
                            }
                            else
                            {
                                responses.Add(new
                                {
                                    IsSuccess = true,
                                    originalImage.Width,
                                    originalImage.Height,
                                    Title = "",
                                    requestFile.Url,
                                    Md5 = requestFile.Calculate(file.InputStream),
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
                    Message = ex.ToString()
                });
            }

            return Json(responses);
        }

        private static RequestFile CreateRequestFile(String accountId, String fileType, String fileExtension)
        {
            var requestFile = new RequestFile();

            var middlePath = requestFile.GetFileType(fileType);
            var fileFullPath = $@"{_fileStoragePath}/{accountId}/{middlePath}/";
            var fileName = $@"{Guid.NewGuid().ToString().Replace("-", "")}.{fileExtension}";
            if(!Directory.Exists(fileFullPath))
            {
                Directory.CreateDirectory(fileFullPath);
            }

            requestFile.Path = fileFullPath;
            requestFile.Name = fileName;
            requestFile.FullPath = $@"{fileFullPath}{fileName}";
            requestFile.FileType = middlePath;
            requestFile.ResetUrl();
            return requestFile;
        }

    }
}

public enum FileType
{
    Wallpaper = 1,
    Face = 2,
    Icon = 3
}

public class RequestFile
{
    public String Path { get; set; }

    public String Name { get; set; }

    public String FullPath { get; set; }

    public String Url { get; set; }

    public Image Image { get; set; }

    public FileType FileType { get; set; }

    public void ResetUrl()
    {
        if(String.IsNullOrEmpty(FullPath))
        {
            return;
        }

        Url = FullPath.Substring(FullPath.IndexOf("/", StringComparison.Ordinal));
    }

    public FileType GetFileType(string fileType)
    {
        if(fileType.ToLower() == FileType.Wallpaper.ToString().ToLower())
        {
            return FileType.Wallpaper;
        }
        else if(fileType.ToLower() == FileType.Face.ToString().ToLower())
        {
            return FileType.Face;
        }
        else if(fileType.ToLower() == FileType.Icon.ToString().ToLower())
        {
            return FileType.Icon;
        }

        throw new Exception($@"{fileType}:未被识别为有效的上传类型");
    }

    public static string GetFileExtension(HttpPostedFile file)
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

    public void GetReducedImage(int width, int height)
    {
        // 源图宽度及高度 
        var imageFromWidth = Image.Width;
        var imageFromHeight = Image.Height;

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
                var reducedImage = Image.GetThumbnailImage(width, height, callb, IntPtr.Zero);

                //将图片以指定的格式保存到到指定的位置
                var newName = $@"small_{Guid.NewGuid().ToString().Replace("-", "")}.png";
                var newFileFullPath = $@"{Path}{newName}";
                FullPath = newFileFullPath;
                ResetUrl();
                reducedImage.Save(newFileFullPath, ImageFormat.Png);
            }
        }
        catch(Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }

    public String Calculate(Stream stream)
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