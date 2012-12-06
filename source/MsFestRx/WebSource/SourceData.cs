using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WebSource
{
	public class SourceData: ISourceData
	{
		private static List<string> _source;
		static SourceData()
		{
			var rnd = new Random();
			_source = new List<string>(100000);
			for (var i = 0; i < 100000; i++)
			{
				_source.Add(GenerateWord(rnd));
			}
		}

		private static string GenerateWord(Random rnd)
		{
			var sb = new char[6];
			for (var i = 0; i < 6; i++)
			{
				sb[i] = (char)rnd.Next(65, 85);
			}
			return new string(sb);
		}

		public List<string> GetData(string value, bool delay)
		{
			value = value.ToUpper();
			var result = _source.Where(x => x.StartsWith(value)).ToList();
			if (delay)
			{
				Thread.Sleep(TimeSpan.FromMilliseconds(result.Count * 30));
			}
			return result;
		}
	}
}