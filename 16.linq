<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

#define TRACE

using State = (int r, int c, int d, int p);

string inName = "16input.txt";

var dirs = new (int dr, int dc)[] { (0, 1), (-1, 0), (0, -1), (1, 0) };

checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).ToList().DumpTrace();
	int R = lines.Count;
	int C = lines[0].Length;
	int sr = lines.FindIndex(l => l.Contains('S'));
	int sc = lines[sr].IndexOf('S');
	int er = lines.FindIndex(l => l.Contains('E'));
	int ec = lines[er].IndexOf('E');
	new { sr, sc, er, ec }.ToString().DumpTrace();
	var minPoints = new int[R, C, 4];
	
	for (int r = 0; r < R; r++)
		for (int c = 0; c < C; c++)
			for (int d = 0; d < 4; d++)
				minPoints[r, c, d] = int.MaxValue;
				
	minPoints[sr,sc,0] = 0;

	HashSet<State> last = [ (sr, sc, 0, 0) ];

	while (last.Count > 0)
	{
		//last.DumpTrace();
		HashSet<State> next = new();
		
		foreach (var s in last)
		{
			var dir = dirs[s.d];
			Try(s.r, s.c, (s.d + 3) % 4, s.p + 1000);
			Try(s.r, s.c, (s.d + 1) % 4, s.p + 1000);
			Try(s.r + dir.dr, s.c + dir.dc, s.d, s.p + 1);
		}

		void Try(int r, int c, int d, int p)
		{
			if (r >= 0 && r < R && c >= 0 && c < C
				&& lines[r][c] != '#'
				&& minPoints[r, c, d] > p)
			{
				minPoints[r, c, d] = p;
				next.Add((r, c, d, p));
			}
		}
		
		last = next;
	}
	
	//Enumerable.Range(0, 4).Select(d=>minPoints[139, 3, d]).Dump();
	
	int best = Enumerable.Range(0, 4)
		.Select(d => minPoints[er, ec, d])
		.Min()
		.Dump("tot");

	last = Enumerable.Range(0, 4)
		.Where(d => minPoints[er, ec, d] == best)
		.Select(d => (er, ec, d, best))
		.ToHashSet();
		
	HashSet<(int r, int c)> path = [(er, ec)];

	while (last.Count > 0)
	{
		last.DumpTrace("last");
		//path.DumpTrace("path");
		HashSet<State> next = new();

		foreach (var s in last)
		{
			var dir = dirs[s.d];
			Try(s.r, s.c, (s.d + 1) % 4, s.p - 1000);
			Try(s.r, s.c, (s.d + 3) % 4, s.p - 1000);
			Try(s.r - dir.dr, s.c - dir.dc, s.d, s.p - 1);
		}

		void Try(int r, int c, int d, int p)
		{
			if (r >= 0 && r < R && c >= 0 && c < C
				&& lines[r][c] != '#'
				&& minPoints[r, c, d] == p)
			{
				next.Add((r, c, d, p));
				path.Add((r, c));
			}
		}

		last = next;
	}
	
	path.Count.Dump("tot2");
}
