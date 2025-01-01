<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "8input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var lines = File.ReadAllLines(inPath)
	.Select(l => l.ToList()).ToList();
lines.Select(l => new string(l.ToArray())).Dump();
int R = lines.Count;
int C = lines[0].Count;

var a = lines.SelectMany((line, r) => line.Select((x, c) => (r, c, x)))
	.Where(x => x.x != '.')
	.GroupBy(x => x.x)
	.Select(g => new { tip = g.Key, pos = g.Select(x => (x.r, x.c)).ToList() })
	.ToList();

foreach (var g in a)
{
	foreach (var i in g.pos)
		foreach (var j in g.pos)
			if (i != j)
			{
				int dr = j.r - i.r;
				int dc = j.c - i.c;
				
				int gcd = GCD(dr, dc);
				dr /= gcd;
				dc /= gcd;
				
				for (int x = 0;
					Plot(i.r + dr * x, i.c + dc * x);
					x++);

				for (int x = -1;
					Plot(i.r + dr * x, i.c + dc * x);
					x--);
			}
}

lines.Select(l => new string(l.ToArray())).Dump();

lines.Sum(l => l.Count(x => x == '#')).Dump("tot");

bool Plot(int r, int c)
{
	if (r >= 0 && r < R && c >= 0 && c < C)
	{
		lines[r][c] = '#';
		return true;
	}
	return false;
}

int GCD(int a, int b)
{
	if (a == 0) return b;
	return GCD(b % a, a);
}
