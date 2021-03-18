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
            //Console.Write("输入视频地址：");
            //var u = Console.ReadLine();
            //await TestFunction1(u);


            await TestFunction2();
            Console.ReadKey();
        }

        private static async Task TestFunction1(string avideoPageUrl) {
            await VideoHelpers.GetOneVideo(avideoPageUrl);
        }

        private static async Task TestFunction2() {
            var videosData = await VideoHelpers.GetVideoInfo("https://www.91porn.com/index.php");
           videosData.Reverse();
            for (int i = 0; i < videosData.Count; i++) {
                await VideoHelpers.GetOneVideo(videosData[i].PageUrl);
            }
        }
    }


}
