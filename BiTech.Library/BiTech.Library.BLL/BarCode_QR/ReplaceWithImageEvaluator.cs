using Aspose.Words;
using Aspose.Words.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiTech.Library.BLL.BarCode_QR
{
    class ReplaceWithImageEvaluator : IReplacingCallback
    {
        string ImageLink = "";

        public ReplaceWithImageEvaluator(string imageLink) : base()
        {
            this.ImageLink = imageLink;
        }

        ///
        /// NOTE: This is a simplistic method that will only work well when the match
        /// starts at the beginning of a run.
        ///
        ReplaceAction IReplacingCallback.Replacing(ReplacingArgs e)
        {
            DocumentBuilder builder = new DocumentBuilder((Document)e.MatchNode.Document);
            builder.MoveTo(e.MatchNode);

            // Replace 'text to replace' text with an image.
            Shape img = builder.InsertImage(ImageLink);
            img.Height = 70;
            img.Width = 60;
            img.WrapType = WrapType.None;
            e.Replacement = "";
            return ReplaceAction.Replace;
        }
    }
}
