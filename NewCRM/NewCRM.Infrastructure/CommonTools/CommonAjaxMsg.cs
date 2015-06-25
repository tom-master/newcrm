using System;

namespace NewCRM.Infrastructure.CommonTools
{
    [Serializable]
    public sealed class CommonAjaxMsg
    {
        private AjaxFlg _ajaxFlg;
        private String _message;
        private String _url;
        private Object _data;

        public AjaxFlg AjaxF
        {
            get { return _ajaxFlg; }
            set { _ajaxFlg = value; }
        }

        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public String Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public Object Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
