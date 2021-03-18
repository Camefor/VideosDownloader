
using AngleSharp.Html.Parser;
using CameforTools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Videos.Models;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.IO;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.ClearScript;
using System.Diagnostics;

namespace Videos {
    public class VideoHelpers {
        static HtmlParser htmlParser = new HtmlParser();

        /// <summary>
        /// 获取视频对象
        /// </summary>
        /// <returns></returns>
        public static async Task<List<VideosInfo>> GetVideoInfo(string url) {
            List<VideosInfo> videosInfos = new List<VideosInfo>();
            var sourceHtmlDom = await AnalyticalContent.GetHtml(url);
            Console.WriteLine("正在获取视频数据……");
            var dom = htmlParser.ParseDocument(sourceHtmlDom);
            var rows = dom.QuerySelectorAll("div.videos-text-align a");//元素选择器//行
            foreach (var item in rows) {
                var videoHref = item.GetAttribute("href");
                int _finded = videoHref.LastIndexOf("&page");//移除后面的
                if (_finded != -1) videoHref = videoHref.Substring(0, _finded);
                videosInfos.Add(new VideosInfo {
                    PageUrl = videoHref,
                    Duration = item.GetElementsByTagName("span")[0].InnerHtml,
                    Thumb = item.GetElementsByClassName("img-responsive")[0].GetAttribute("src"),
                    Title = AnalyticalContent.HtmlToPlainText(item.GetElementsByClassName("video-title title-truncate m-t-5")[0].OuterHtml)
                });
            }
            Console.WriteLine($"获取视频数据完成,共:{videosInfos.Count} 条数据.");
            return videosInfos;
        }


        /// <summary>
        /// 获取这个字符串:
        /// %3c%73%6f%75%72%63%65%20%73%72%63%3d%27%68%74%74%70%73%3a%2f%2f%66%64%63%2e%39%31%70%34%39%2e%63%6f%6d%2f%6d%33%75%38%2f%34%34%34%31%33%39%2f%34%34%34%31%33%39%2e%6d%33%75%38%27%20%74%79%70%65%3d%27%61%70%70%6c%69%63%61%74%69%6f%6e%2f%78%2d%6d%70%65%67%55%52%4c%27%3e
        /// </summary>
        /// <returns></returns>
        private static string GetEncryptionString(string targetStr) {
            var t1 = targetStr.Split("//-->");
            var t2 = t1[0].Split(@"   <script>
             < !--");
            var t3 = t2[0].Split("document.write(strencode2(");
            var t4 = AnalyticalContent.HtmlToPlainText(t3[1]);
            t4 = t4.Replace(@"\", "").Replace(@"n", "").Replace("t", "");
            return t4;
        }


        /// <summary>
        /// 使用指定的解密js脚本解密字符串
        /// </summary>
        /// <param name="aEncryptionStr"></param>
        /// <returns></returns>
        private static string DecryptString(string aEncryptionStr) {
            string res;
            using (ScriptEngine engine = new ScriptEngine("jscript")) {
                ParsedScript parsed = engine.Parse(@";var encode_version = 'jsjiami.com.v5', eexda = '__0x9ff10',  __0x9ff10=['w7FkXcKcwqs=','VMKAw7Fhw6Q=','w5nDlTY7w4A=','wqQ5w4pKwok=','dcKnwrTCtBg=','w45yHsO3woU=','54u75py15Y6177y0PcKk5L665a2j5pyo5b2156i677yg6L+S6K2D5pW65o6D5oqo5Lmn55i/5bSn5L21','RsOzwq5fGQ==','woHDiMK0w7HDiA==','54uS5pyR5Y6r7764wr3DleS+ouWtgeaesOW/sOeooe+/nei/ruitteaWsuaOmeaKiuS4o+eateW2i+S8ng==','bMOKwqA=','V8Knwpo=','csOIwoVsG1rCiUFU','5YmL6ZiV54qm5pyC5Y2i776Lw4LCrOS+muWssOacteW8lOeqtg==','w75fMA==','YsOUwpU=','wqzDtsKcw5fDvQ==','wqNMOGfCn13DmjTClg==','wozDisOlHHI=','GiPConNN','XcKzwrDCvSg=','U8K+wofCmcO6'];(function(_0x1f2e93,_0x60307d){var _0x1f9a0b=function(_0x35f19b){while(--_0x35f19b){_0x1f2e93['push'](_0x1f2e93['shift']());}};_0x1f9a0b(++_0x60307d);}(__0x9ff10,0x152));var _0x43d9=function(_0x13228a,_0x2ce452){_0x13228a=_0x13228a-0x0;var _0x424175=__0x9ff10[_0x13228a];if(_0x43d9['initialized']===undefined){(function(){var _0x270d2c=typeof window!=='undefined'?window:typeof process==='object'&&typeof require==='function'&&typeof global==='object'?global:this;var _0x58680b='ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';_0x270d2c['atob']||(_0x270d2c['atob']=function(_0x5536e1){var _0x15e9d3=String(_0x5536e1)['replace'](/=+$/,'');for(var _0x4e6299=0x0,_0x3590d2,_0x48c90b,_0x557f6a=0x0,_0x2b086d='';_0x48c90b=_0x15e9d3['charAt'](_0x557f6a++);~_0x48c90b&&(_0x3590d2=_0x4e6299%0x4?_0x3590d2*0x40+_0x48c90b:_0x48c90b,_0x4e6299++%0x4)?_0x2b086d+=String['fromCharCode'](0xff&_0x3590d2>>(-0x2*_0x4e6299&0x6)):0x0){_0x48c90b=_0x58680b['indexOf'](_0x48c90b);}return _0x2b086d;});}());var _0x4a2d38=function(_0x1f120d,_0x1d6e11){var _0x4c36f9=[],_0x1c4b64=0x0,_0x18ce5c,_0x39c9fa='',_0x6d02b2='';_0x1f120d=atob(_0x1f120d);for(var _0x13b203=0x0,_0x24d88b=_0x1f120d['length'];_0x13b203<_0x24d88b;_0x13b203++){_0x6d02b2+='%'+('00'+_0x1f120d['charCodeAt'](_0x13b203)['toString'](0x10))['slice'](-0x2);}_0x1f120d=decodeURIComponent(_0x6d02b2);for(var _0x1f76f3=0x0;_0x1f76f3<0x100;_0x1f76f3++){_0x4c36f9[_0x1f76f3]=_0x1f76f3;}for(_0x1f76f3=0x0;_0x1f76f3<0x100;_0x1f76f3++){_0x1c4b64=(_0x1c4b64+_0x4c36f9[_0x1f76f3]+_0x1d6e11['charCodeAt'](_0x1f76f3%_0x1d6e11['length']))%0x100;_0x18ce5c=_0x4c36f9[_0x1f76f3];_0x4c36f9[_0x1f76f3]=_0x4c36f9[_0x1c4b64];_0x4c36f9[_0x1c4b64]=_0x18ce5c;}_0x1f76f3=0x0;_0x1c4b64=0x0;for(var _0x2b6a92=0x0;_0x2b6a92<_0x1f120d['length'];_0x2b6a92++){_0x1f76f3=(_0x1f76f3+0x1)%0x100;_0x1c4b64=(_0x1c4b64+_0x4c36f9[_0x1f76f3])%0x100;_0x18ce5c=_0x4c36f9[_0x1f76f3];_0x4c36f9[_0x1f76f3]=_0x4c36f9[_0x1c4b64];_0x4c36f9[_0x1c4b64]=_0x18ce5c;_0x39c9fa+=String['fromCharCode'](_0x1f120d['charCodeAt'](_0x2b6a92)^_0x4c36f9[(_0x4c36f9[_0x1f76f3]+_0x4c36f9[_0x1c4b64])%0x100]);}return _0x39c9fa;};_0x43d9['rc4']=_0x4a2d38;_0x43d9['data']={};_0x43d9['initialized']=!![];}var _0x302f80=_0x43d9['data'][_0x13228a];if(_0x302f80===undefined){if(_0x43d9['once']===undefined){_0x43d9['once']=!![];}_0x424175=_0x43d9['rc4'](_0x424175,_0x2ce452);_0x43d9['data'][_0x13228a]=_0x424175;}else{_0x424175=_0x302f80;}return _0x424175;};function strencode2(_0x4f0d7a){var _0x4c6de5={'Anfny':function _0x4f6a21(_0x51d0ce,_0x5a5f36){return _0x51d0ce(_0x5a5f36);}};return _0x4c6de5[_0x43d9('0x0','fo#E')](unescape,_0x4f0d7a);};(function(_0x17883e,_0x4a42d3,_0xe4080c){var _0x301ffc={'lPNHL':function _0x1c947e(_0x4d57b6,_0x51f6a5){return _0x4d57b6!==_0x51f6a5;},'EPdUx':function _0x55f4cc(_0x34b7bc,_0x9f930c){return _0x34b7bc===_0x9f930c;},'kjFfJ':'jsjiami.com.v5','DFsBH':function _0x5f08ac(_0x1e6fa1,_0x4c0aef){return _0x1e6fa1+_0x4c0aef;},'akiuH':_0x43d9('0x1','KYjt'),'VtfeI':function _0x4f3b7b(_0x572344,_0x5f0cde){return _0x572344(_0x5f0cde);},'Deqmq':_0x43d9('0x2','oYRG'),'oKQDc':_0x43d9('0x3','i^vo'),'UMyIE':_0x43d9('0x4','oYRG'),'lRwKx':function _0x5b71b4(_0x163a75,_0x4d3998){return _0x163a75===_0x4d3998;},'TOBCR':function _0x314af8(_0x3e6efe,_0x275766){return _0x3e6efe+_0x275766;},'AUOVd':_0x43d9('0x5','lALy')};_0xe4080c='al';try{if('EqF'!==_0x43d9('0x6','xSW]')){_0xe4080c+=_0x43d9('0x7','oYRG');_0x4a42d3=encode_version;if(!(_0x301ffc[_0x43d9('0x8','fo#E')](typeof _0x4a42d3,_0x43d9('0x9','*oMH'))&&_0x301ffc[_0x43d9('0xa','ov6D')](_0x4a42d3,_0x301ffc[_0x43d9('0xb','3k]D')]))){_0x17883e[_0xe4080c](_0x301ffc[_0x43d9('0xc','@&#[')]('ɾ��',_0x301ffc[_0x43d9('0xd','i^vo')]));}}else{return _0x301ffc[_0x43d9('0xe','rvlM')](unescape,input);}}catch(_0x23e6c5){if('svo'!==_0x301ffc[_0x43d9('0xf','TpCD')]){_0x17883e[_0xe4080c]('ɾ���汾�ţ�js�ᶨ�ڵ���');}else{_0xe4080c='al';try{_0xe4080c+=_0x301ffc[_0x43d9('0x10','doK*')];_0x4a42d3=encode_version;if(!(_0x301ffc[_0x43d9('0x11','ZRZ4')](typeof _0x4a42d3,_0x301ffc['UMyIE'])&&_0x301ffc[_0x43d9('0x12','@&#[')](_0x4a42d3,_0x301ffc['kjFfJ']))){_0x17883e[_0xe4080c](_0x301ffc[_0x43d9('0x13','KYjt')]('ɾ��',_0x43d9('0x14','xSW]')));}}catch(_0x4202f6){_0x17883e[_0xe4080c](_0x301ffc[_0x43d9('0x15','oYRG')]);}}}}(window));;encode_version = 'jsjiami.com.v5';
                        ");
                res = (string)parsed.CallMethod("strencode2", aEncryptionStr);
            }
            return res;
        }

        private static async Task<Tuple<string, string>> GetVideoUrlFormVideoPage(string url) {
            try {
                Thread.Sleep(500);
                var sourceHtmlDom = await AnalyticalContent.GetHtml(url);
                var dom = htmlParser.ParseDocument(sourceHtmlDom);
                var videoHeader = dom.QuerySelectorAll("h4.login_register_header");//元素选择器//行
                var elements = dom.QuerySelectorAll("div.video-container video");//元素选择器//行
                var videoTitle = AnalyticalContent.HtmlToPlainText(videoHeader[0].InnerHtml).TrimEnd();
                videoTitle = Regex.Replace(videoTitle, @"\s", "");
                var content = elements[0].OuterHtml;
                var res = DecryptString(GetEncryptionString(content));
                res = res.Remove(0, 14);//移除前面定长字符
                int _finded = res.LastIndexOf(@"type=");//移除后面的
                if (_finded != -1) res = res.Substring(0, _finded);
                Console.WriteLine(@$"获取视频成功：{videoTitle} : {res}");
                return Tuple.Create<string, string>(videoTitle, res); ;
            } catch (Exception ex) {
                Console.WriteLine($"获取视频地址失败,错误:{ex.Message}");
                return null;
            }
        }


        public static async Task GetOneVideo(string avideoPageUrl) {
            var data = await VideoHelpers.GetVideoUrlFormVideoPage(avideoPageUrl);
            var videoTitle = data.Item1;
            var videoUrl = data.Item2;
            var subdir = Path.Combine(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Video"), videoTitle);
            if (!Directory.Exists(subdir)) Directory.CreateDirectory(subdir);
            var arr = videoUrl?.Split('/');
            if (arr?.Length > 2) {
                int notfoundcount = 0;
                if (int.TryParse(arr[5], out int code)) {//4352680
                    for (int i = 0; i < 300; i++) {
                        if (notfoundcount >= 5) {
                            //break; //nothing to do
                        } else {
                            string Afragment = code.ToString() + i.ToString();// A fragment 一个片段
                            var url = @$"https://cdn.91p07.com//m3u8/{code}/{Afragment}.ts";
                            var savepath = @$"{subdir}\{Afragment}.ts";//保存目录
                            var file = await DownloadFileHelper.Download(url);
                            if (file != null) {
                                FileHelper.SaveTsFile(file, savepath);
                            } else {
                                notfoundcount++;
                            }
                        }
                        
                    }
                }
            }
            CombinationVideo(subdir, videoTitle);
            Console.WriteLine("下载结束!");
        }


        /// <summary>
        /// 合成完整视频
        /// </summary>
        /// <param name="apath">D:\TS</param>
        /// <param name="avideoName"></param>
        private static void CombinationVideo(string apath, string avideoName, string avideoextension = "mp4") {
            string cmd = @$"copy/b  {apath}\*.ts  {apath}\{avideoName}.{avideoextension}";
            Execute(cmd);
        }

        /// <summary>  
        /// 执行DOS命令，返回DOS命令的输出  
        /// </summary>  
        /// <param name="dosCommand">dos命令</param>  
        /// <param name="milliseconds">等待命令执行的时间（单位：毫秒），  
        /// 如果设定为0，则无限等待</param>  
        /// <returns>返回DOS命令的输出</returns>  
        private static string Execute(string command, int seconds = 300000) {
            string output = ""; //输出字符串  
            if (command != null && !command.Equals("")) {
                Process process = new Process();//创建进程对象  
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令  
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动 
                startInfo.RedirectStandardInput = false;//不重定向输入  
                startInfo.RedirectStandardOutput = true; //重定向输出  
                startInfo.CreateNoWindow = true;//不创建窗口  
                process.StartInfo = startInfo;
                try {
                    if (process.Start())//开始进程  
                    {
                        if (seconds == 0) {
                            process.WaitForExit();//这里无限等待进程结束  
                        } else {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒  
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);//捕获异常，输出异常信息
                } finally {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }
    }
}
