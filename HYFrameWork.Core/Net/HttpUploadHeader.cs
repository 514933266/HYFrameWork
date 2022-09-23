using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HYFrameWork.Core
{
    /// <summary>
    /// Http上传请求头
    /// </summary>
   public class HttpUploadHeader:HttpRequestHeader
    {
        private string _fileName = string.Empty;
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
        }
        private byte[] _fileData = null;
        /// <summary>
        /// 要上传的文件byte数组
        /// </summary>
        public byte[] FileData
        {
            get
            {
                return _fileData;
            }
        }
        /// <summary>
        /// 上传的内容类型（和ContentType不同）
        /// </summary>
        public string UploadContentType { get; set; } = "application/octet-stream";

        private string _boundary = string.Empty;
        /// <summary>
        /// 协议分隔线
        /// </summary>
        public string Boundary
        {
            get
            {
                if (_boundary.IsNullOrEmpty())
                    _boundary = "----------".Append(StringHelper.GetRandomBlend(30));
                return _boundary;
            }
            set
            {
                _boundary = value;
            }
        }

        /// <summary>
        /// 上传请求头
        /// </summary>
        /// <param name="fileName">要上传的文件路径</param>
        public HttpUploadHeader(string fileName)
        {
            _fileName = fileName;
            Init();
        }

        private void Init()
        {
            if (File.Exists(_fileName))_fileData=File.ReadAllBytes(_fileName);
        }
    }
}
