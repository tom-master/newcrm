using System;

namespace NewCRM.WebApi.Areas.HelpPage
{
    /// <summary>
    /// This represents a preformatted text sample on the help page. There's a display template named TextSample associated with this class.
    /// </summary>
    public class TextSample
    {
        public TextSample(string text) => Text = text ?? throw new ArgumentNullException("text");

        public string Text { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as TextSample;
            return other != null && Text == other.Text;
        }

        public override int GetHashCode() => Text.GetHashCode();

        public override string ToString() => Text;
    }
}