using System;

namespace NewCRM.WebApi.Areas.HelpPage
{
    /// <summary>
    /// This represents an invalid sample on the help page. There's a display template named InvalidSample associated with this class.
    /// </summary>
    public class InvalidSample
    {
        public InvalidSample(string errorMessage) => SetErrorMessage(errorMessage ?? throw new ArgumentNullException("errorMessage"));

        private String errorMessage;

        public String GetErrorMessage() => errorMessage;
        private void SetErrorMessage(String value) => errorMessage = value;

        public override bool Equals(object obj)
        {
            var other = obj as InvalidSample;
            return other != null && GetErrorMessage() == other.GetErrorMessage();
        }

        public override int GetHashCode() => GetErrorMessage().GetHashCode();

        public override string ToString() => GetErrorMessage();
    }
}