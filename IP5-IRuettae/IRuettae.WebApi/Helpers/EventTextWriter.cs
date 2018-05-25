using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace IRuettae.WebApi.Helpers
{
    public class EventTextWriter : TextWriter
    {

        public override Encoding Encoding => Encoding.Default;

        public override void Write(char value)
        {
            CharWritten?.Invoke(this, value);
        }

        public event EventHandler<char> CharWritten;
    }
}