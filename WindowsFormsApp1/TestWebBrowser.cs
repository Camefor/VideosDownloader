using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1 {
    public partial class TestWebBrowser : Form {
        public TestWebBrowser() {
            InitializeComponent();
        }

        private void TestWebBrowser_Load(object sender, EventArgs e) {
            var u1 = "https://www.baidu.com/";
            var u2 = "http://91porn.com/view_video.php?viewkey=9fd05837d19ee2454d7c&page=&viewtype=&category=";
            var u3 = "https://www.google.com/";
            var ua = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.82 Safari/537.36";
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(u2);
            webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted; ;
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            string txt = webBrowser1.DocumentText;
        }
    }
}
