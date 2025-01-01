<Query Kind="Statements" />

#define TRACE
string inName = "23input.txt";
checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace();
	
	var cons = new Dictionary<string, HashSet<string>>();
	void Add(string a, string b)
	{
		HashSet<string> h;
		if (!(cons.TryGetValue(a, out h)))
		{
			h = new();
			cons.Add(a, h);
		}
		h.Add(b);
	}
	
	foreach (string l in lines)
	{
		var p = l.Split('-');
		Add(p[0], p[1]);
		Add(p[1], p[0]);
	}
	
	int tot = 0;
	foreach (string a in cons.Keys.Where(x => x.StartsWith("t")))
		foreach (string b in cons[a])
			if (!(b.StartsWith("t") && string.Compare(a, b) > 0))
				foreach (string c in cons[a])
					if (!(c.StartsWith("t") && string.Compare(a, c) > 0))
						if (string.Compare(b, c) > 0)
							if (cons[b].Contains(c))
							{
								tot++;
								$"{a},{b},{c}".Dump();
							}
	tot.Dump();
}