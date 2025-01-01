<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "6input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var lines0 = File.ReadAllLines(inPath).Dump()
	.Select(l => l.ToList())
	.ToList();
int R = lines0.Count;
int C = lines0[0].Count;

var r0 = lines0.FindIndex(l => l.Contains('^'));
var c0 = lines0[r0].IndexOf('^');
var pos0 = (r: r0, c: c0).Dump();

var moves = new (int dx, int dy)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };

var lines = lines0.Select(l => l.ToList()).ToList();
bool Search()
{
	int move = 0;
	var pos = pos0;
	lines[pos.r][pos.c] = 'X';
	
	var states = new HashSet<(int r, int c, int m)>();
	
	while (true)
	{
		var state = (pos.r, pos.c, move);
		if (states.Contains(state))
			return true;
		states.Add(state);
		
		var pos2 = Add(pos, moves[move]);

		if (!(pos2.r >= 0 && pos2.r < R && pos2.c >= 0 && pos2.c < C))
			break;

		if (lines[pos2.r][pos2.c] == '#')
		{
			move = (move + 1) % moves.Length;
		}
		else
		{
			lines[pos2.r][pos2.c] = 'X';
			pos = pos2;
		}
	}

	//lines.Select(l => new string(l.ToArray())).Dump();
	return false;
}

Search();

List<(int r, int c)> search = new();

int tot = 0;
for (int r = 0; r < R; r++)
	for (int c = 0; c < C; c++)
		if (lines[r][c] == 'X')
		{
			tot++;
			if (r != r0 || c != c0)
				search.Add((r, c));
		}

tot.Dump();
search.Dump("search", 0);

tot = 0;
foreach (var s in search)
{
	lines = lines0.Select(l => l.ToList()).ToList();
	lines[s.r][s.c] = '#';
	if (Search())
		tot++;
		//s.Dump("obstacle for cycle");
}
tot.Dump();

static (int r, int c) Add((int, int) pair1, (int, int) pair2)
{
	return (pair1.Item1 + pair2.Item1, pair1.Item2 + pair2.Item2);
}
