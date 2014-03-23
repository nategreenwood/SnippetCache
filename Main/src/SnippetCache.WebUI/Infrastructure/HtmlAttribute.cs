using System;
using System.Web;

namespace SnippetCache.WebUI.Infrastructure
{
    public class HtmlAttribute : IHtmlString
    {
        private readonly string _seperator;
        private string _internalValue = String.Empty;

        public HtmlAttribute(string name)
            : this(name, null)
        {
        }

        public HtmlAttribute(string name, string seperator)
        {
            Name = name;
            _seperator = seperator ?? " ";
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public bool Condition { get; set; }

        #region IHtmlString Members

        public string ToHtmlString()
        {
            if (!String.IsNullOrWhiteSpace(_internalValue))
                _internalValue = String.Format("{0}=\"{1}\"", Name,
                                               _internalValue.Substring(0, _internalValue.Length - _seperator.Length));
            return _internalValue;
        }

        #endregion

        public HtmlAttribute Add(string value)
        {
            return Add(value, true);
        }

        public HtmlAttribute Add(string value, bool condition)
        {
            if (!String.IsNullOrWhiteSpace(value) && condition)
                _internalValue += value + _seperator;

            return this;
        }
    }
}