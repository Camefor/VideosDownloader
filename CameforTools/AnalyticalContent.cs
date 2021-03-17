using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CameforTools {
    public class AnalyticalContent {

        //public static async Task<string> GetHtml(string url) {
        //    try {
        //        Thread.Sleep(500);
        //        HttpWebRequest myReq =
        //        (HttpWebRequest)WebRequest.Create(url);
        //        using (HttpWebResponse response = (HttpWebResponse)await myReq.GetResponseAsync()) {
        //            // Get the stream associated with the response.
        //            using (Stream receiveStream = response.GetResponseStream()) {
        //                // Pipes the stream to a higher level stream reader with the required encoding format. 
        //                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8)) {
        //                    return await readStream.ReadToEndAsync();
        //                }
        //            }
        //        }
        //    } catch (Exception) {
        //        return null;
        //    }
        //}

        public static async Task<string> GetHtml(string aurl) {
            HttpHeader header = new HttpHeader();
            header.accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "POST";
            header.userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            header.maxTry = 300;
            //在这里自己拼接一下Cookie，不用复制过来的那个GetCookie方法了，原来的那个写法还是比较严谨的
            CookieContainer cc = new CookieContainer();
            /**
             * __cfduid=daefcc18d6371543709b9a8d40c1c27621615815548; 
             * CLIPSHARE=m41c785g13llq3u2iabj5t23eo; 
             * SL_GWPT_Show_Hide_tmp=1;
             * SL_wptGlobTipTmp=1;
             * language=cn_CN
             * **/
            Cookie language = new Cookie("language", "cn_CN");
            language.Domain = ".91porn.com";
            cc.Add(language);
            return await HTMLHelper.GetHtml(aurl, cc, header);
        }

        public static async Task<string> GetHtml2(string url = "https://www.91porn.com/index.php") {
            try {
                Thread.Sleep(500);
                var client = new RestClient(url);
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
                request.AddParameter(
                    "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW",
                    "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"session_language\"\r\n\r\ncn_CN\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--",
                    ParameterType.RequestBody);
                IRestResponse response = await client.ExecuteAsync(request);
                return response.Content;
            } catch (Exception) {
                return null;
            }
        }


        public static string HtmlToPlainText(string html) {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);
            return text;
        }

        /// <summary>
        /// 保存资源到本地
        /// </summary>
        /// <param name="imgUrl">url</param>
        /// <param name="fullPath">要保存的路径</param>
        public static async Task SaveResourcesAsync(string imgUrl, string fullPath) {
            if (System.IO.File.Exists(fullPath)) {
                return;
            }
            Thread.Sleep(100);
            Console.WriteLine(@$"正在下载: {Path.GetFileName(fullPath)}……");
            try {
                using (HttpClient client = new HttpClient()) {
                    client.Timeout = new TimeSpan(1, 1, 1);
                    using (HttpResponseMessage response = client.GetAsync(imgUrl).Result) {
                        response.EnsureSuccessStatusCode();
                        var respnseBody = await response.Content.ReadAsByteArrayAsync();
                        using (Stream resStream = (response.Content.ReadAsStreamAsync().Result)) {
                            //https://blog.lindexi.com/post/C-dotnet-%E5%B0%86-Stream-%E4%BF%9D%E5%AD%98%E5%88%B0%E6%96%87%E4%BB%B6%E7%9A%84%E6%96%B9%E6%B3%95.html
                            using (var fileStream = File.Create(fullPath)) {
                                resStream.Seek(0, SeekOrigin.Begin);
                                await resStream.CopyToAsync(fileStream);
                            }
                        }
                    }

                }
            } catch (Exception ex) {
                Console.WriteLine($"出现异常错误:{ex.Message}");
                return;
            }
        }
    }
}
