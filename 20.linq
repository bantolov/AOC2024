<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

//#define TRACE
string inName = "20input.txt";
checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace("file", 0)
		.Select(l => l.ToList())
		.ToList();
	int R = lines.Count;
	int C = lines[0].Count;
	int sr = lines.FindIndex(l => l.Contains('S'));
	int sc = lines[sr].IndexOf('S');
	int er = lines.FindIndex(l => l.Contains('E'));
	int ec = lines[er].IndexOf('E');
	new { sr, sc, er, ec }.ToString().Dump();
	var dirs = new (int r, int c)[] { (0, 1), (-1, 0), (0, -1), (1, 0) };
	var dist = new int[R, C];

	int len0 = GetLen();
	var track = GetTrack();
	dist.Dump();
	len0.Dump();
	track.Dump();

	var cheats = new List<(int r1, int c1, int r2, int c2, int saved)>();

	for (int r = 0; r < R; r++)
		for (int c = 0; c < C; c++)
			if (lines[r][c] == '#' && dirs.Any(d => IsPassage(r + d.r, c + d.c)))
				foreach (var d in dirs)
				{
					int r2 = r + d.r;
					int c2 = c + d.c;
					if (track.Contains((r2, c2)))
						TryCheat(r, c, r2, c2);
				}

	cheats.GroupBy(c => c.saved).Select(g => new { saved=g.Key, cheats=g.ToList() })
		.OrderBy(g => g.saved).DumpTrace(1);
	cheats.Where(c => c.saved >= 100).Count().Dump("tot 100");

	void TryCheat(int r1, int c1, int r2, int c2)
	{
		if (!(r2 >= 0 && r2 < R && c2 >= 0 && c2 < C))
			return;
		char old1 = lines[r1][c1];
		char old2 = lines[r2][c2];
		lines[r1][c1] = '.';
		lines[r2][c2] = '.';
		int saved = len0 - GetLen();
		if (saved > 0)
		{
			bool onTrack = IsOnTrack(r1, c1, r2, c2);
			new { r1, c1, r2, c2, saved, d1=dist[r1, c1], d2=dist[r2, c2], onTrack }.ToString().DumpTrace();
			if (onTrack)
				cheats.Add((r1, c1, r2, c2, saved));
		}

		lines[r1][c1] = old1;
		lines[r2][c2] = old2;
	}

	int GetLen()
	{
		for (int r = 0; r < R; r++)
			for (int c = 0; c < C; c++)
				dist[r,c] = int.MaxValue;
		dist[sr, sc] = 0;
		var last = new List<(int r, int c)> { (sr, sc) };
		int steps = 0;
		while (last.Count > 0)
		{
			steps++;
			var next = new List<(int r, int c)>();
			foreach (var l in last)
				foreach (var d in dirs)
				{
					int r = l.r + d.r;
					int c = l.c + d.c;
					if (IsPassage(r, c) && dist[r, c] > steps)
					{
						dist[r, c] = steps;
						next.Add((r, c));
						if (r == er && c == ec)
							return steps;
					}
				}
			last = next;
		}
		return int.MaxValue;
	}

	bool IsOnTrack(int r1, int c1, int r2, int c2)
	{
		if (dist[r1,c1] != dist[r2,c2] - 1)
			return false;
		bool b1 = false, b2 = false;
		var last = new List<(int r, int c)> { (er, ec) };
		if (er == r1 && ec == c1) b1 = true;
		if (er == r2 && ec == c2) b2 = true;
		while (last.Count > 0)
		{
			var next = new List<(int r, int c)>();
			foreach (var l in last)
				foreach (var d in dirs)
				{
					int r = l.r + d.r;
					int c = l.c + d.c;
					if (IsPassage(r, c) && dist[r, c] == dist[l.r, l.c] - 1)
					{
						next.Add((r, c));
						if (r==r1 && c==c1) b1 = true;
						if (r==r2 && c==c2) b2 = true;
					}
				}
			last = next;
		}
		return b1 && b2;
	}

	HashSet<(int r, int c)> GetTrack()
	{
		var track = new HashSet<(int r, int c)> { (er, ec) };
		var last = new List<(int r, int c)> { (er, ec) };
		while (last.Count > 0)
		{
			var next = new List<(int r, int c)>();
			foreach (var l in last)
				foreach (var d in dirs)
				{
					int r = l.r + d.r;
					int c = l.c + d.c;
					if (IsPassage(r, c) && dist[r, c] == dist[l.r, l.c] - 1)
					{
						next.Add((r, c));
						track.Add((r, c));
					}
				}
			last = next;
		}
		return track;
	}

	bool IsPassage(int r, int c)
	{
		return r >= 0 && r < R && c >= 0 && c < C && lines[r][c] != '#';
	}
}
