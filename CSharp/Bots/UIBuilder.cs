/*****************************************************************************\
*
* Copyright (c) Pragmatismo.io. All rights reserved.
* Licensed under the MIT license.
*
* Pragmatismo.io: http://pragmatismo.io
*
* MIT License:
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files (the
* "Software"), to deal in the Software without restriction, including
* without limitation the rights to use, copy, modify, merge, publish,
* distribute, sublicense, and/or sell copies of the Software, and to
* permit persons to whom the Software is furnished to do so, subject to
* the following conditions:
*
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
* NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
* LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
* OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
* WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*
* 
\*****************************************************************************/

#region

#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

#endregion

#endregion

namespace Pragmatismo.Io.Framework.Bots
{
    public class UiBuilder
    {
        public enum Animation
        {
            enter,
            shrug,
            thinking,
            thinking_end,
            head_bump,
            got_it,
            got_it_end,
            point_up,
            cheer,
            smile,
            smile_end,
            frown,
            frown_end,
            spin_around,
            fist_pump,
            dance,
            dance_end,
            antenna_glow,
            wave_flag,
            wave_flag_end
        }

        public static XDocument CreateDialog()
        {
            var doc = new XDocument(
                new XElement("dialog")
            );
            doc.Root?.SetAttributeValue("id", Guid.NewGuid().ToString());
            return doc;
        }

        public static void AppendBulletList(XDocument document, IEnumerable<string> items)
        {
            var list = new XElement("list");
            document.Root?.Add(list);

            foreach (var item in items)
            {
                var listItem = new XElement("listItem");
                list.Add(listItem);
                listItem.Value = item;
            }
        }

        public static void AppendAnimation(XDocument document, Animation animation)
        {
            var list = new XElement("animation");
            document.Root?.Add(list);
            list.SetAttributeValue("kind", animation.ToString());
        }

        public static void AppendLabel(XDocument document, string label)
        {
            var labelElement = new XElement("label") {Value = label};
            document.Root?.Add(labelElement);
        }

        public static void AppendLink(XDocument document, string link)
        {
            var linkElement = new XElement("link") {Value = link};
            document.Root?.Add(linkElement);
        }

        public static void AppendCalendar(XDocument document, DateTime date)
        {
            var element = new XElement("calendar") {Value = date.ToString(CultureInfo.InvariantCulture)};
            document.Root?.Add(element);
        }

        public static void AppendImage(XDocument document, string url)
        {
            var imageElement = new XElement("image") {Value = url};
            document.Root?.Add(imageElement);
        }

        public static void AppendIframe(XDocument document, string url)
        {
            var imageElement = new XElement("iframe") {Value = url};
            document.Root?.Add(imageElement);
        }

        public static void AppendMusic(XDocument document, string url)
        {
            var element = new XElement("music") {Value = url};
            document.Root?.Add(element);
        }

        public static void SetClearScreen(XDocument document, bool clear)
        {
            document.Root?.SetAttributeValue("clearScreen", clear);
        }

        public static Guid GetId(XDocument dialog)
        {
            return Guid.Parse(dialog.Root?.Attribute("dialog")?.Value);
        }
    }
}