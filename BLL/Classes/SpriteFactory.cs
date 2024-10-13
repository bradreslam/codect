using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using BLL.Models;
using Codect.Classes;

namespace BLL.Classes
{
	public class SpriteFactory
	{
		private string baseSprite = "../Codect_Sprites/Component_Background.png";

		public Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
		{
			Bitmap result = new Bitmap(width, height);
			using (Graphics g = Graphics.FromImage(result))
			{
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;
				g.DrawImage(sourceBMP, 0, 0, width, height);
			}
			return result;
		}

		public Bitmap CreateSprite(List<ContactPoint> endpoints, string featureSprite, bool spriteType)
		{
			Bitmap result = new Bitmap(baseSprite);
			List<Bitmap> endpointSprites = new();
			WireDictionary wd = new();
			foreach (ContactPoint endPoint in endpoints)
			{
				endpointSprites.Add(new Bitmap(wd.GetPath(endPoint, spriteType)));
			}

			using (Graphics g = Graphics.FromImage(result))
			{
				g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

				foreach (Bitmap sprite in endpointSprites)
				{
					g.DrawImage(sprite, new Point(0, 0));
				}

				if (featureSprite != null)
				{
					g.DrawImage(new Bitmap(featureSprite), new Point(0, 0));
				}
			}

			Bitmap endSprite = ResizeBitmap(result, 25, 25);

			return endSprite;
		}
	}
}
