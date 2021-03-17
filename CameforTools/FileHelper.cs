using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CameforTools {
    public class FileHelper {
        public static void SaveTsFile(byte[] aResult, string aFilePath) {
            try {
                if (aResult == null) {
                    return;
                }
                //https://stackoverflow.com/questions/18021662/system-io-ioexception-the-process-cannot-access-the-file-because-it-is-being-us?rq=1
                //if (!System.IO.File.Exists(aFilePath)) {
                //    System.IO.File.Create(aFilePath);
                //}
                //https://www.cnblogs.com/niuniu0108/p/7306350.html
                //把byte[] 写入文件
                var _buff = aResult;
                Thread.Sleep(100);
                using (FileStream SourceStream = new FileStream(aFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)) {
                    using (BinaryWriter binaryWriter = new BinaryWriter(SourceStream)) {
                        binaryWriter.Write(_buff);
                    }
                }
                Console.WriteLine("------------------.");
                Console.WriteLine("保存文件成功.");
                Console.WriteLine("------------------.");
            } catch (Exception ex) {
                Console.WriteLine("Message: ", ex.Message);
            }
        }
    }

    public static class DownloadFileHelper {

        static readonly HttpClient client = new HttpClient();

        public static async Task<byte[]> Download(string aurl) {
            HttpResponseMessage response = await client.GetAsync(aurl);
            //Console.WriteLine("正在下载第 : {0}个视频片段...", i + 1);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                response.EnsureSuccessStatusCode();
                Console.WriteLine("------------------.");
                Console.WriteLine("下载文件成功.");
                Console.WriteLine("------------------.");
                return await response.Content.ReadAsByteArrayAsync();
            }
            return null;
        }
    }
}
