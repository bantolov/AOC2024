<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

//#define TRACE

string inName = "12input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var lines = File.ReadAllLines(inPath).Dump();
int R = lines.Length;
int C = lines[0].Length;
(int r, int c)[] moves = [(0, 1), (1, 0), (0, -1), (-1, 0)];

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
	
	fields.DumpTrace(lines[r0][c0].ToString());

	long a = fields.Count;
	long p = Sides(fields);
	new { a, p }.ToString().Dump(lines[r0][c0].ToString());
	tot += a * p;
}

tot.Dump();

bool IsSameRegion((int r, int c) current, (int r, int c)  next)
{
	return next.r >= 0 && next.r < R && next.c >= 0 && next.c < C
		&& lines[next.r][next.c] == lines[current.r][current.c];
}

int Sides(HashSet<(int r, int c)> fields)
{
	List<(int r, int c, bool inside)> hSides = new(); // Iznad polja (r, c).
	List<(int r, int c, bool inside)> vSides = new(); // Lijevo od polja (r, c).
	
	foreach (var f in fields)
	{
		if (!IsSameRegion(f, (f.r - 1, f.c)))
			hSides.Add((f.r, f.c, true));
			
		if (!IsSameRegion(f, (f.r + 1, f.c)))
			hSides.Add((f.r + 1, f.c, false));
			
		if (!IsSameRegion(f, (f.r, f.c - 1)))
			vSides.Add((f.r, f.c, true));
			
		if (!IsSameRegion(f, (f.r, f.c + 1)))
			vSides.Add((f.r, f.c + 1, false));
	}
	
	hSides.DumpTrace();
	vSides.DumpTrace();

	hSides = hSides.OrderBy(s => s.r).ThenBy(s => s.c).ToList();
	var hSides2 = new List<(int r, int c, bool inside)>() { hSides[0] };
	for (int i = 1; i < hSides.Count; i++)
		if (!(hSides[i-1].r == hSides[i].r && hSides[i-1].c == hSides[i].c - 1 && hSides[i-1].inside == hSides[i].inside))
			hSides2.Add(hSides[i]);

	vSides = vSides.OrderBy(s => s.c).ThenBy(s => s.r).ToList();
	var vSides2 = new List<(int r, int c, bool inside)>() { vSides[0] };
	for (int i = 1; i < vSides.Count; i++)
		if (!(vSides[i - 1].c == vSides[i].c && vSides[i - 1].r == vSides[i].r - 1 && vSides[i-1].inside == vSides[i].inside))
			vSides2.Add(vSides[i]);

	hSides2.DumpTrace();
	vSides2.DumpTrace();

	return hSides2.Count + vSides2.Count;
}
