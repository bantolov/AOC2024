<Query Kind="Statements" />

//#define TRACE
const int n = 2000;
const long m = 16777216;

string inName = "22input.txt";

checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath);
	//string[] lines = ["1", "2", "3", "2024"];
	lines.DumpTrace("lines");
	
	long tot = 0;
	Dictionary<(int d4, int d3, int d2, int d1), long> totPrices = new();
	foreach (string l in lines)
	{
		Dictionary<(int d4, int d3, int d2, int d1), long> buyerPrice = new();
		long s = long.Parse(l).DumpTrace("s0");
		int d1 = 0, d2 = 0, d3 = 0, d4 = 0;
		int lastP = (int)(s % 10);
		for (int i = 0; i < n; i++)
		{
			s = s ^ (s << 6) % m;
			s = s ^ (s >> 5) % m;
			s = s ^ (s << 11) % m;

			int p = (int)(s % 10);
		
			d4 = d3;
			d3 = d2;
			d2 = d1;
			d1 = p - lastP;

			"".DumpTrace();
			$"{(i >= 3 ? "valid:" : "wait...")}".DumpTrace();
			$"{d4} {d3} {d2} {d1} => {p} from {s}".DumpTrace();

			if (i >= 3)
				buyerPrice.TryAdd((d4, d3, d2, d1), p);
			
			lastP = p;
		}
		s.DumpTrace($"s{n}");
		buyerPrice.Select(kv => new { key = kv.Key.ToString(), kv.Value }).DumpTrace();
		
		foreach (var kv in buyerPrice)
		{
			var old = totPrices.GetValueOrDefault(kv.Key, 0);
			totPrices[kv.Key] = old + kv.Value;
		}
		tot += s;
	}
	tot.Dump();
	totPrices.OrderByDescending(kv => kv.Value).Take(10)
		.Select(kv => new { key = kv.Key.ToString(), kv.Value })
		.Dump();
}
