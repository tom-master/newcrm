using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace NewCRM.Infrastructure.CommonTools
{
    public class FileUpLoadHelper
    {
        #region 字段
        /// <summary>
        /// 文件路径
        /// </summary>
        private string _filePath;
        /// <summary>
        /// 源文件名称
        /// </summary>
        private string _oldFileName;
        /// <summary>
        /// 新文件名称
        /// </summary>
        private string _newFileName;
        /// <summary>
        /// 缩图文件名称
        /// </summary>
        private string _thumbnailFileName;
        /// <summary>
        /// 服务器文件路径
        /// </summary>
        private string _webFilePath;
        /// <summary>
        /// 服务器缩图文件路径
        /// </summary>
        private string _webThumbnailFilePath;
        /// <summary>
        /// 提示信息
        /// </summary>
        private string _tipMsg;
        /// <summary>
        /// 是否按日期创建目录
        /// </summary>
        private readonly bool _isDate;
        /// <summary>
        /// 是否生成缩图
        /// </summary>
        private readonly bool _isThumbnail;
        /// <summary>
        /// 缩图宽度
        /// </summary>
        private readonly int _thumbnailWidth;
        /// <summary>
        /// 缩图高度
        /// </summary>
        private readonly int _thumbnailHeight;
        /// <summary>
        /// 缩图模式
        /// </summary>
        private readonly ThumbnailMode _thumbnailMode = ThumbnailMode.AUTO;
        /// <summary>
        /// 文件大小
        /// </summary>
        private int _fileSize;
        /// <summary>
        /// 图片宽度
        /// </summary>
        private int _fileWidth;
        /// <summary>
        /// 图片高度
        /// </summary>
        private int _fileHeight;
        /// <summary>
        /// 文件类型
        /// </summary>
        private string _fileContentType;
        /// <summary>
        /// 文件扩展名
        /// </summary>
        private string _fileExtension;
        /// <summary>
        /// 是否是图片
        /// </summary>
        private bool _isImage;
        /// <summary>
        /// 是否创建新文件名
        /// </summary>
        private bool _isCreateNewFileName;
        /// <summary>
        /// 是否加水印文字
        /// </summary>
        private bool _isWatermarkText = false;
        /// <summary>
        /// 水印文字
        /// </summary>
        private string _watermarkText;
        /// <summary>
        /// 水印图文件名称（文字）
        /// </summary>
        private string _watermarkFileName;
        /// <summary>
        /// 服务器水印图文件路径（文字）
        /// </summary>
        private string _webWatermarkFilePath;
        /// <summary>
        /// 允许上传文件最大 100M
        /// </summary>
        private int _maxFileSize = 104857600;
        #endregion

        #region 属性
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }
        /// <summary>
        /// 源文件名称
        /// </summary>
        public string OldFileName
        {
            get { return _oldFileName; }
        }
        /// <summary>
        /// 新文件名称
        /// </summary>
        public string NewFileName
        {
            get { return _newFileName; }
        }
        /// <summary>
        /// 缩图文件名称
        /// </summary>
        public string ThumbnailFileName
        {
            get { return _thumbnailFileName; }
        }
        /// <summary>
        /// 服务器文件路径
        /// </summary>
        public string WebFilePath
        {
            get { return _webFilePath; }
        }
        /// <summary>
        /// 服务器缩图文件路径
        /// </summary>
        public string WebThumbnailFilePath
        {
            get { return _webThumbnailFilePath; }
        }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string TipMsg
        {
            get { return _tipMsg; }
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize
        {
            get { return _fileSize; }
        }
        /// <summary>
        /// 图片宽度
        /// </summary>
        public int FileWidth
        {
            get { return _fileWidth; }
        }
        /// <summary>
        /// 图片高度
        /// </summary>
        public int FileHeight
        {
            get { return _fileHeight; }
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileContentType
        {
            get { return _fileContentType; }
        }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension
        {
            get { return _fileExtension; }
        }
        /// <summary>
        /// 水印图文件名称（文字）
        /// </summary>
        public string WatermarkFileName
        {
            get { return _watermarkFileName; }
        }
        /// <summary>
        /// 服务器水印图文件路径（文字）
        /// </summary>
        public string WebWatermarkFilePath
        {
            get { return _webWatermarkFilePath; }
        }
        #endregion

        /// <summary>
        /// 构造函数 (上传图片，自动产生upload目录,并且按日期生成子目录，不生成缩图，生成新文件名，不加水印文字)
        /// </summary>
        public FileUpLoadHelper()
            : this("/upload/", true)
        {
        }

        /// <summary>
        /// 构造函数(上传图片，不生成缩图，生成新文件名，不加水印文字)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="isDate">是否按日期创建目录</param>
        public FileUpLoadHelper(string filePath, bool isDate) :
            this(filePath, isDate, true, true, false, 0, 0, ThumbnailMode.AUTO, false, "")
        {
        }

        /// <summary>
        /// 构造函数(上传文件)
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="isDate">是否按日期创建目录</param>
        /// <param name="isCreateNewFileName">是否创建新文件名</param>
        public FileUpLoadHelper(string filePath, bool isDate, bool isCreateNewFileName) :
            this(filePath, isDate, isCreateNewFileName, false, false, 0, 0, ThumbnailMode.AUTO, false, "")
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="isDate">是否按日期创建目录</param>
        /// <param name="isCreateNewFileName">是否创建新文件名</param>
        /// <param name="isImage">是否是图片</param>
        /// <param name="isThumbnail">是否生成缩图</param>
        /// <param name="thumbnailWidth">缩图宽度</param>
        /// <param name="thumbnailHeight">缩图高度</param>
        /// <param name="thumbnailMode">缩图模式</param>
        /// <param name="isWatermarkText">是否加水印文字</param>
        /// <param name="watermarkText">水印文字</param>
        public FileUpLoadHelper(string filePath, bool isDate, bool isCreateNewFileName,
            bool isImage,bool isThumbnail, int thumbnailWidth, int thumbnailHeight, ThumbnailMode thumbnailMode, 
            bool isWatermarkText, string watermarkText)
        {
            _isThumbnail = isThumbnail;
            _filePath = filePath;
            _isDate = isDate;
            _isImage = isImage;
            _isCreateNewFileName = isCreateNewFileName;
            _thumbnailWidth = thumbnailWidth;
            _thumbnailHeight = thumbnailHeight;
            _thumbnailMode = thumbnailMode;
            _isWatermarkText = isWatermarkText;
            _watermarkText = watermarkText;
            if (_isDate)
                _filePath += DateTime.Now.ToString("yyyyMMdd") + "/";
            string savePath = HttpContext.Current.Server.MapPath(_filePath);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        private void MakeThumbnail()
        {
            Image originalImage = Image.FromFile(_webFilePath);
            int towidth = _thumbnailWidth;
            int toheight = _thumbnailHeight;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            if (ow < towidth && oh < toheight)
                originalImage.Save(_webThumbnailFilePath);
            else
            {
                switch (_thumbnailMode)
                {
                    case ThumbnailMode.HW:
                        break;
                    case ThumbnailMode.W:
                        toheight = originalImage.Height * _thumbnailWidth / originalImage.Width;
                        break;
                    case ThumbnailMode.H:
                        towidth = originalImage.Width * _thumbnailHeight / originalImage.Height;
                        break;
                    case ThumbnailMode.CUT:
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * _thumbnailHeight / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    case ThumbnailMode.AUTO:
                        if (ow > oh)
                            toheight = (int)((double)oh / (double)ow * (double)towidth);
                        else
                            towidth = (int)((double)ow / (double)oh * (double)toheight);
                        break;
                    default:
                        if (ow > oh)
                            toheight = (int)((double)oh / (double)ow * (double)towidth);
                        else
                            towidth = (int)((double)ow / (double)oh * (double)toheight);
                        break;
                }
                //进行缩图
                Bitmap img = new Bitmap(towidth, toheight);
                img.SetResolution(72f, 72f);
                Graphics gdiobj = Graphics.FromImage(img);
                gdiobj.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gdiobj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gdiobj.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gdiobj.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                gdiobj.FillRectangle(new SolidBrush(Color.White), 0, 0,
                                towidth, toheight);
                Rectangle destrect = new Rectangle(0, 0,
                                towidth, toheight);
                gdiobj.DrawImage(originalImage, destrect, 0, 0, ow,
                                oh, GraphicsUnit.Pixel);
                System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters(1);
                ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
                try
                {
                    if (ici != null)
                        img.Save(_webThumbnailFilePath, ici, ep);
                    else
                        img.Save(_webThumbnailFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (System.Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    gdiobj.Dispose();
                    img.Dispose();
                }
            }
            originalImage.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            System.Drawing.Imaging.ImageCodecInfo[] encoders;
            encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (int index = encoders.Length - 1; index >= 0; index--)
            {
                if (encoders[index].MimeType == mimeType)
                    return encoders[index];
            }
            return null;
        }

        /// <summary>
        /// 创建新的文件名
        /// </summary>
        /// <returns></returns>
        private string CreateFileName()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmss") +
                System.Guid.NewGuid().ToString().ToLower().Replace("-", "").Substring(0, 4);
        }

        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        private void AddWatermarkText()
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(this._webFilePath);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            System.Drawing.Font f = new System.Drawing.Font("Verdana", 16);
            System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            g.DrawString(this._watermarkText, f, b, 15, 15);
            g.Dispose();
            image.Save(this._webWatermarkFilePath);
            image.Dispose();
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public bool SaveFile(HttpPostedFileBase postedFile)
        {
            this._fileSize = postedFile.ContentLength;
            if (this._fileSize > 0)
            {
                _oldFileName = postedFile.FileName;
                _fileContentType = postedFile.ContentType;
                _fileExtension = new FileInfo(_oldFileName).Extension;
                bool isfileTypeImages = false;
                if (_fileContentType == "image/png" || _fileContentType == "image/bmp" || _fileContentType == "image/gif" || _fileContentType == "image/pjpeg" || _fileContentType == "image/jpeg")
                    isfileTypeImages = true;
                if (_isImage && !isfileTypeImages)
                {
                    _tipMsg = "图片格式不对！";
                    return false;
                }
                if (this._fileSize > this._maxFileSize)
                {
                    _tipMsg = string.Format("上传文件超过系统允许大小:{0}M",
                        this._maxFileSize);
                    return false;
                }
                if (_isCreateNewFileName)
                    this._newFileName = CreateFileName() + _fileExtension;
                else
                    this._newFileName = this._oldFileName;
                _webFilePath = HttpContext.Current.Server.MapPath(_filePath + _newFileName);
                if (isfileTypeImages)
                {
                    if (!File.Exists(_webFilePath))
                    {
                        try
                        {
                            postedFile.SaveAs(_webFilePath);
                            if (this._isThumbnail)
                            {
                                _thumbnailFileName = "s_" + this._newFileName;
                                _webThumbnailFilePath = HttpContext.Current.Server.MapPath(_filePath + _thumbnailFileName);
                                MakeThumbnail();
                            }
                            if (this._isWatermarkText)
                            {
                                this._watermarkFileName = "sy_" + this._newFileName;
                                _webWatermarkFilePath = HttpContext.Current.Server.MapPath(this._filePath + this._watermarkFileName);
                                this.AddWatermarkText();
                            }
                            if (File.Exists(_webFilePath))
                            {
                                System.Drawing.Image originalImage = System.Drawing.Image.FromFile(_webFilePath);
                                try
                                {
                                    _fileHeight = originalImage.Height;
                                    _fileWidth = originalImage.Width;
                                }
                                finally
                                {
                                    originalImage.Dispose();
                                }
                            }
                            _tipMsg = string.Format("提示：文件“{0}”成功上传，文件类型为：{1}，文件大小为：{2}B",
                                _newFileName, _fileContentType, _fileSize);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            _tipMsg = "提示：文件上传失败，失败原因：" + ex.Message;
                        }
                    }
                    else
                        _tipMsg = "提示：文件已经存在，请重命名后上传";
                }
                else
                {
                    if (!File.Exists(_webFilePath))
                    {
                        try
                        {
                            postedFile.SaveAs(_webFilePath);
                            _tipMsg = string.Format("提示：文件“{0}”成功上传，文件类型为：{1}，文件大小为：{2}B",
                                _newFileName, _fileContentType, _fileSize);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            _tipMsg = "提示：文件上传失败，失败原因：" + ex.Message;
                        }
                    }
                    else
                        _tipMsg = "提示：文件已经存在，请重命名后上传";
                }
            }
            else
                _tipMsg = "提示:文件不能为空,请选择要上传文件!";
            return false;
        }
    }

    /// <summary>
    /// 缩图模式 
    /// HW:   指定高宽缩放（可能变形）  
    /// W     指定宽，高按比例 
    /// H     指定高，宽按比例  
    /// Cut:  指定高宽裁减（不变形） 
    /// Auto: 自动 
    /// </summary>
    public enum ThumbnailMode
    {
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        HW,
        /// <summary>
        /// 指定宽，高按比例 
        /// </summary>
        W, 
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H, 
        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        CUT,
        /// <summary>
        /// 自动
        /// </summary>
        AUTO
    }
}
