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
			img.Height = 85;
			img.Width = 60;
			img.WrapType = WrapType.None;
			e.Replacement = "";
			return ReplaceAction.Replace;
		}
	}
	class ReplaceWithImageQR : IReplacingCallback
	{
		string ImageLink = "";

		public ReplaceWithImageQR(string imageLink) : base()
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

			img.Height = 38;
			img.Width = 38;
			img.WrapType = WrapType.None;

			e.Replacement = "";
			return ReplaceAction.Replace;
		}
	}

	class ReplaceWithImageQR_Large : IReplacingCallback
	{
		string ImageLink = "";

		public ReplaceWithImageQR_Large(string imageLink) : base()
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

			img.Height = 50;
			img.Width = 50;
			img.WrapType = WrapType.None;

			e.Replacement = "";
			return ReplaceAction.Replace;
		}
	}

	//VINH
	class ReplaceWithImageQRBook_Export : IReplacingCallback
	{
		string ImageLink = "";
		private int WHeight_QR = 80;

		public ReplaceWithImageQRBook_Export(string imageLink) : base()
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
			//if(ImageLink != null)
			Shape img = builder.InsertImage(ImageLink);

			img.Height = Math.Max(img.Height * 0.4, WHeight_QR);
			img.Width = Math.Max(img.Width * 0.4, WHeight_QR);
			img.WrapType = WrapType.None;

			e.Replacement = "";
			return ReplaceAction.Replace;
		}
	}
}
