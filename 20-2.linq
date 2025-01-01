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
	//dist.Dump(0);
	len0.Dump();
	track.Dump(0);

	var cheats = new List<(int r1, int c1, int r2, int c2, int saved)>();

	int minSaved = 100;

	for (int i = 0; i < track.Count; i++)
	{
		int r1 = track[i].r;
		int c1 = track[i].c;
		for (int j = i + minSaved; j < track.Count; j++)
		{
			int r2 = track[j].r;
			int c2 = track[j].c;
			int d = Math.Abs(r2 - r1) + Math.Abs(c2 - c1);
			if (d <= 20)
			{
				int saved = (j - i) - d;
				if (saved >= minSaved)
					cheats.Add((r1, c1, r2, c2, saved));
			}
		}
	}
	
	cheats.Count().Dump("tot");
	
	cheats.GroupBy(c => c.saved)
		.Select(g => new { cheats=g.Count(), saved=g.Key })
		.ToList().Dump(0);
	
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

	List<(int r, int c)> GetTrack()
	{
		var track = new List<(int r, int c)> { (er, ec) };
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
