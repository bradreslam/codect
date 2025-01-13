using Svg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Classes
{
	public class SvgResourceManager
	{
		private static readonly ResourceManager resourceManager = new ("BLL.Resource1", typeof(SvgResourceManager).Assembly);

		private static readonly ConcurrentDictionary<string, string> svgCache = new();

		private static readonly ConcurrentDictionary<string, SvgDocument> documentCache = new();

		private static readonly Dictionary<string, SvgDocument> preloadedDocuments = new();

		static SvgResourceManager()
		{
			PreloadSvgDocuments(new[] { "Component_Background", "E_Wire_Off", "E_Wire_On", "N_Wire_Off",
				"N_Wire_On", "W_Wire_Off", "W_Wire_On", "S_Wire_Off", "S_Wire_On" });
		}

		private static void PreloadSvgDocuments(IEnumerable<string> resourceNames)
		{
			foreach (string resourceName in resourceNames)
			{
				if (!preloadedDocuments.ContainsKey(resourceName))
				{
					string svgContent = resourceManager.GetString(resourceName);
					if (svgContent != null)
					{
						using MemoryStream ms = new(System.Text.Encoding.UTF8.GetBytes(svgContent));
						SvgDocument svgDocument = SvgDocument.Open<SvgDocument>(ms);
						preloadedDocuments[resourceName] = svgDocument;
					}
				}
			}
		}

		public static SvgDocument GetPreloadedSvgDocument(string resourceName)
		{
			if (preloadedDocuments.TryGetValue(resourceName, out SvgDocument svgDocument))
			{
				return svgDocument; // Return a deep copy to avoid modifying the original.
			}

			throw new KeyNotFoundException($"Resource '{resourceName}' is not preloaded.");
		}

		private static string GetSvgString(string resourceName)
		{
			return svgCache.GetOrAdd(resourceName, name =>
			{
				var svgContent = resourceManager.GetString(name);
				if (string.IsNullOrEmpty(svgContent))
				{
					throw new KeyNotFoundException($"Resource '{name}' not found in .resx file.");
				}
				return svgContent;
			});
		}
		public static SvgDocument GetSvgDocument(string resourceName)
		{
			return documentCache.GetOrAdd(resourceName, name =>
			{
				string svgContent = GetSvgString(name);
				using MemoryStream ms = new(System.Text.Encoding.UTF8.GetBytes(svgContent));
				return SvgDocument.Open<SvgDocument>(ms);
			});
		}
	}
}
