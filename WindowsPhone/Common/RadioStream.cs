using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WindowsPhone.Common
{
    public class RadioStream : MediaStreamSource
    {
        protected override void CloseMedia()
        {
            throw new NotImplementedException();
        }

        protected override void GetDiagnosticAsync(MediaStreamSourceDiagnosticKind diagnosticKind)
        {
            throw new NotImplementedException();
        }

        protected override void GetSampleAsync(MediaStreamType mediaStreamType)
        {
            throw new NotImplementedException();
        }

        protected override void OpenMediaAsync()
        {
            throw new NotImplementedException();
        }

        protected override void SeekAsync(long seekToTime)
        {
            throw new NotImplementedException();
        }

        protected override void SwitchMediaStreamAsync(MediaStreamDescription mediaStreamDescription)
        {
            throw new NotImplementedException();
        }
    }
}
