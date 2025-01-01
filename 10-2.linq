<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "10input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var lines = File.ReadAllLines(inPath).Dump();
int R = lines.Length;
int C = lines[0].Length;

var scores = new List<long>();
for (int r = 0; r < R; r++)
	for (int c = 0; c < C; c++)
		if (lines[r][c] == '0')
			scores.Add(GetScore(r, c));

scores.Dump().Sum().Dump();

long GetScore(int r0, int c0)
{
	var last = new HashSet<(int r, int c, long n)>();
	last.Add((r0, c0, 1));
	
	for (char x = '1'; x <= '9'; x++)
	{
		var next = new List<(int r, int c, long n)>();
		foreach (var l in last)
		{
			if (Find(l.r + 1, l.c, x))
				next.Add((l.r + 1, l.c, l.n));
			if (Find(l.r - 1, l.c, x))
				next.Add((l.r - 1, l.c, l.n));
			if (Find(l.r, l.c - 1, x))
				next.Add((l.r, l.c - 1, l.n));
			if (Find(l.r, l.c + 1, x))
				next.Add((l.r, l.c + 1, l.n));
		}

		//next.Dump($"{r0}, {c0}");
		last = next.GroupBy(q => (q.r, q.c))
			.Select(g => (g.Key.r, g.Key.c, g.Sum(q => q.n)))
			.ToHashSet();
	}
	return last.Select(q => q.n).Sum();
}

bool Find(int r, int c, char x)
{
	return r >= 0 && r < R && c >= 0 && c < C
		&& lines[r][c] == x;
}
