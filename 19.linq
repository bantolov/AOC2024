<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

#define TRACE
string inName = "19input.txt";
checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace("file", 0);
	
	var towels = lines[0].Split(", ").DumpTrace("towels");

	var tot = lines[2..].Select(p =>
	{
		var ends = new long[p.Length + 1];
		ends[0] = 1;
		for (int e = 0; e < p.Length; e++)
		{
			$"{e}: {ends[e]}".Dump(p);
			if (ends[e] > 0)
				foreach (var t in towels)
					if (Fits(e, t))
					{
						$"{e} + {t}".Dump(p);
						ends[e + t.Length] += ends[e];
					}
		}
		string.Join(" ", ends).Dump();

		bool Fits(int e, string t)
		{
			if (!(e + t.Length < ends.Length))
				return false;
			for (int i = 0; i < t.Length; i++)
				if (p[e + i] != t[i])
					return false;
			return true;
		}

		return ends[p.Length];
	}).ToList();
	tot.Dump();
	tot.Sum().Dump();
}
