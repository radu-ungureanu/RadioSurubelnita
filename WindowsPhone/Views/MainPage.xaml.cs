using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WindowsPhone.Common;
using System.Windows.Navigation;

namespace WindowsPhone.Views
{
    public partial class MainPage : PageBase
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public override void LoadState(IDictionary<string, string> navigationInformation, IDictionary<string, object> state)
        {
            SystemTray.IsVisible = true;
        }

        public override void SaveState(IDictionary<string, object> state)
        {
            var url = "http://live.radiosurubelnita.ro:8000";
            var uri = new Uri(url);

            var _downloader = WebRequest.CreateHttp(uri);
            _downloader.AllowReadStreamBuffering = false;
            _downloader.BeginGetResponse(new AsyncCallback(GetAsyncData), _downloader);
        }

        private void GetAsyncData(IAsyncResult ar)
        {
            var request = ar.AsyncState as HttpWebRequest;
            var response = request.EndGetResponse(ar) as HttpWebResponse;
            var stream = response.GetResponseStream();
            player.SetSource(

            //byte[] buffer = new byte[128 * 1024];
            //while (true)
            //{
            //    //read = (byte)r.ReadByte();
            //    //_radioStream.WriteByte(read);

            //    int read = stream.Read(buffer, 0, buffer.Length);
            //    _radioStream.Write(buffer, 0, read);
            //}
        }
    }
}