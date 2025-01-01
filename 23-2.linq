<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

//#define TRACE
string inName = "23input.txt";
checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace();

	var parsed = lines.Select(l =>
	{
		var p = l.Split('-');
		string an = p[0];
		string bn = p[1];
		if (string.Compare(an, bn) > 0)
			(an, bn) = (bn, an);
		int a = (an[0] - 'a') * 30 + (an[1] - 'a');
		int b = (bn[0] - 'a') * 30 + (bn[1] - 'a');
		return (a, b, an, bn);
	}).ToList();
	parsed.Dump(0);
	
	var connected = new bool[900,900];
	foreach (var p in parsed)
	{
		connected[p.a,p.b] = true;
		connected[p.b,p.a] = true;
	}
	
	var cons = parsed.GroupBy(c => c.a).OrderBy(g => g.Key)
		.Select(g => (a: g.Key, bs: g.Select(c => c.b).OrderBy(x => x).ToList()))
		.ToList();
	cons.Dump(0);

	var names = parsed.SelectMany(p => new[] { new { p.a, p.an }, new { a=p.b, an=p.bn } })
		.Distinct().OrderBy(p => p.a).ToList().Dump("indexes", 0)
		.ToDictionary(p => p.a, p => p.an).Dump("names", 0);
	
	int maxSize = 0;
	foreach (var (a, bs) in cons)
		Find(0, bs, [a]);

	void Find(int start, List<int> bs, List<int> set)
	{
		if (set.Count > maxSize)
		{
			maxSize = set.Count;
			string.Join(",", set.Select(x => names[x])).Dump();
		}
		for (int bx = start; bx < bs.Count; bx++)
		{
			int b = bs[bx];
			if (set.Any(s => !connected[b, s]))
				continue;
			set.Add(b);
			Find(bx + 1, bs, set);
			set.RemoveAt(set.Count - 1);
		}
	}
}
