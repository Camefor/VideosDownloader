using System;
using System.Collections.Generic;
using System.Text;

namespace Videos.Models {
    public class VideosInfo {


        /// <summary>
        /// 视频页面地址
        /// </summary>
        public string PageUrl { get; set; }


        /// <summary>
        /// 视频标题
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumb { get; set; }


        /// <summary>
        /// 视频时长
        /// </summary>
        public string Duration { get; set; }


        /// <summary>
        /// 视频最终下载地址
        /// </summary>
        public string DownloadUrl { get; set; }

    }
}
