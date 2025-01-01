<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

#define TRACE
string inName = "15z1.txt";
checked
{
	char[][] map;
	string commands;
	{
		string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
		var lines = File.ReadAllLines(inPath).ToList().DumpTrace();
		var empty = lines.IndexOf("");
		map = lines[..empty].Select(l => l.ToArray()).ToArray();
		string.Join("\r\n", map.Select(l => new string(l))).Dump();
		commands = string.Concat(lines[(empty + 1)..]).Dump();
	}

	var dir = new Dictionary<char, (int dr, int dc)>
	{
		{ '^', (-1, 0) },
		{ 'v', (1, 0) },
		{ '<', (0, -1) },
		{ '>', (0, 1) },
	};
	
	int R = map.Length;
	int C = map[0].Length;
	int rr = map.ToList().FindIndex(l => l.Contains('@'));
	int rc = map[rr].ToList().IndexOf('@');
	
	foreach (var com in commands)
	{
		var d = dir[com];
		int m = PossibleMoves(rr, rc, d.dr, d.dc);
		$"r {rr}, c {rc}, {com}, {d} PossibleMoves {m}".DumpTrace();
		for (int i = m; i >= 1; i--)
			map[rr + i * d.dr][rc + i * d.dc] = map[rr + (i - 1) * d.dr][rc + (i - 1) * d.dc];
		if (m > 0)
		{
			map[rr][rc] = '.';
			rr = rr + d.dr;
			rc = rc + d.dc;
		}

		int PossibleMoves(int r, int c, int dr, int dc)
		{
			int m = 0;
			while (true)
			{
				m++;
				r += dr;
				c += dc;
				new { m, r, c }.ToString().DumpTrace();
				if (!(r >= 0 && r < R && c >= 0 && c < C))
					return 0;
				if (map[r][c] == '#')
					return 0;
				if (map[r][c] == '.')
					return m;
			}
		}
		
		string.Join("\r\n", map.Select(l => new string(l))).DumpTrace();
	}
	
	long tot = 0;
	for (int r = 0; r < R; r++)
		for (int c = 0; c < C; c++)
			if (map[r][c] == 'O')
				tot += 100L * r + c;
	tot.Dump();
}
