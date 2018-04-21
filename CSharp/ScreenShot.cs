using System.Drawing;
using System.IO;
using System.Net;

namespace Pragmatismo.Io.Framework
{
    public class ScreenShot
    {
        public Bitmap GetBitmapFromWebPage(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request != null)
                using (var stream = request.GetResponse().GetResponseStream())
                {
                    if (stream != null) return new Bitmap(stream);
                }
            return null;
        }
    }
}