using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace WindowsPhone.Common
{
    public class PageBase : PhoneApplicationPage
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.LoadState(NavigationContext.QueryString, this.State);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.SaveState(this.State);
            base.OnNavigatedFrom(e);
        }

        public virtual void LoadState(IDictionary<string, string> navigationInformation, IDictionary<string, object> state)
        {
        }

        public virtual void SaveState(IDictionary<string, object> state)
        {

        }
    }
}
