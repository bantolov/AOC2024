<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "12input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var lines = File.ReadAllLines(inPath).Dump();
int R = lines.Length;
int C = lines[0].Length;

long tot = 0;
var done = new bool[R, C];

for (int r = 0; r < R; r++)
	for (int c = 0; c < C; c++)
		if (lines[r][c] != '.')
			if (!done[r, c])
				ProcessRegion(r, c);

void ProcessRegion(int r0, int c0)
{
	HashSet<(int r, int c)> fields = new() { (r0, c0) };
	HashSet<(int r, int c)> last = new() { (r0, c0) };
	while (true)
	{
		HashSet<(int r, int c)> next = new();
		
		foreach (var f in last)
		{
			TryAdd(f.r-1, f.c);
			TryAdd(f.r+1, f.c);
			TryAdd(f.r, f.c-1);
			TryAdd(f.r, f.c+1);
		}

		void TryAdd(int r, int c)
		{
			if (r >= 0 && r < R && c >= 0 && c < C
				&& !done[r, c]
				&& lines[r][c] == lines[r0][c0])
			{
				done[r,c] = true;
				next.Add((r, c));
				fields.Add((r, c));
			}
		}
		
		if (next.Count == 0)
			break;
		
		last = next;
	}

	//fields.Dump(lines[r0][c0].ToString());

	int p = 0;
	foreach (var f in fields)
	{
		PAdd(f.r - 1, f.c);
		PAdd(f.r + 1, f.c);
		PAdd(f.r, f.c - 1);
		PAdd(f.r, f.c + 1);

		void PAdd(int r, int c)
		{
			if (!(r >= 0 && r < R && c >= 0 && c < C
				&& lines[r][c] == lines[f.r][f.c]))
				p++;
		}
	}
	//new { area=fields.Count, p }.Dump(lines[r0][c0].ToString());
	
	tot += ((long)fields.Count) * p;

}

tot.Dump();