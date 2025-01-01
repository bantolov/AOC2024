<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

//#define TRACE
string inName = "15input.txt";
checked
{
	char[][] map;
	string commands;
	{
		string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
		var lines = File.ReadAllLines(inPath).ToList().DumpTrace();
		var empty = lines.IndexOf("");
		map = lines[..empty].Select(l => l
			.SelectMany(c => c switch
			{
				'O' => new[] { '[', ']' },
				'@' => new[] { '@', '.' },
				_ => new[] { c, c }
			}).ToArray()).ToArray();
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
		HashSet<(int r, int c)> fields = new();
		bool m = GetFieldsToMove(rr, rc, d.dr, d.dc, fields);
		$"r {rr}, c {rc}, {com}, {d} GetFieldsToMove {m} {fields.Count}".DumpTrace();
		if (m)
		{
			foreach (var f in fields.Reverse())
			{
				map[f.r + d.dr][f.c + d.dc] = map[f.r][f.c];
				map[f.r][f.c] = '.';
			}
			rr = rr + d.dr;
			rc = rc + d.dc;
		}
		
		bool GetFieldsToMove(int r, int c, int dr, int dc, HashSet<(int r, int c)> analyzed)
		{
			if (analyzed.Contains((r, c)))
				return true;
			
			int m = PossibleMoves(r, c, dr, dc);
			for (int i = 0; i < m; i++)
				analyzed.Add((r + i * d.dr, c + i * d.dc));
				
			if (dr != 0)
			for (int i = 1; i < m; i++)
			{
				int r1 = r + i * d.dr;
				int c1 = c + i * d.dc;
				if (map[r1][c1] == '[')
				{
					if (!GetFieldsToMove(r1, c1 + 1, dr, dc, analyzed))
						return false;
				}
				else if (map[r1][c1] == ']')
				{
					if (!GetFieldsToMove(r1, c1 - 1, dr, dc, analyzed))
						return false;
				}
			}
			return m > 0;
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
			if (map[r][c] is 'O' or '[')
				tot += 100L * r + c;
	tot.Dump();
}
