using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebKit;

namespace WindowsFormsApp1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            var u1 = "https://www.baidu.com/";
            var u2 = "http://91porn.com/view_video.php?viewkey=9fd05837d19ee2454d7c&page=&viewtype=&category=";
            var u3 = "https://www.google.com/";
            var ua = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36";
            webKitBrowser1.Navigate(u3);
            webKitBrowser1.UserAgent = ua;
            webKitBrowser1.DocumentCompleted += Browser_DocumentCompleted;
            webKitBrowser1.Error += WebKitBrowser1_Error;
        }

        private void WebKitBrowser1_Error(object sender, WebKitBrowserErrorEventArgs e) {

        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            string documentContent = webKitBrowser1.DocumentText;
        }
    }
}
