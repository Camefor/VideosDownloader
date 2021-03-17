using CameforTools;
using Microsoft.ClearScript.V8;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Videos {
    class Program {

        static async Task Main(string[] args) {
            Console.Write("输入视频地址：");
            var u = Console.ReadLine();
            await TestFunction1(u);
            Console.ReadKey();
        }

        private static async Task RunMain() {
            VideoHelpers video = new VideoHelpers();
            var videosData = await video.GetVideoInfo();
            for (int i = 0; i < videosData.Count; i++) {
                videosData[i].DownloadUrl = (await VideoHelpers.GetVideoUrlFormVideoPage(videosData[i].PageUrl)).Item2;
                if (!videosData[i].DownloadUrl.StartsWith("https://v2")) {
                    var fileName = Path.Combine(@"D:\projects\1212\Videos\Videos\src", videosData[i].Title + ".mp4");
                    await AnalyticalContent.SaveResourcesAsync(videosData[i].DownloadUrl, fileName);
                }
            }
        }


        public static async Task TestFunction1(string avideoPageUrl) {
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
                        if (notfoundcount >= 5) break;
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
            Console.WriteLine("下载结束!");
            CombinationVideo(subdir, videoTitle);
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
        public static string Execute(string command, int seconds = 0) {
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
