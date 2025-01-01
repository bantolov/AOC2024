<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "4input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);

var lines = File.ReadAllLines(inPath);
//lines.Dump();

int R = lines.Length;
int C = lines[0].Length;
new { R, C }.Dump();

Queue<char> q = new();

int tot = 0;

for (int r = 0; r < R; r++)
{
	q.Clear();
	for (int c = 0; c < C; c++)
		Add(lines[r][c]);
}

for (int r = 0; r < R; r++)
{
	q.Clear();
	for (int c = C - 1; c >= 0; c--)
		Add(lines[r][c]);
}

for (int c = 0; c < C; c++)
{
	q.Clear();
	for (int r = 0; r < R; r++)
		Add(lines[r][c]);
}

for (int c = 0; c < C; c++)
{
	q.Clear();
	for (int r = R - 1; r >= 0; r--)
		Add(lines[r][c]);
}

for (int d = -R + 1; d <= C - 1; d++)
{
	q.Clear();
	for (int c = 0; c < C; c++)
	{
		int r = 0 - d + c;
		if (r >= 0 && r < R && c >= 0 && c < C)
		{
			Add(lines[r][c]);
			//new { r, c, q }.Dump(d.ToString());
		}
	}
}

for (int d = -R + 1; d <= C - 1; d++)
{
	q.Clear();
	for (int c = C - 1; c >= 0; c--)
	{
		int r = 0 - d + c;
		if (r >= 0 && r < R && c >= 0 && c < C)
		{
			Add(lines[r][c]);
			//new { r, c, q }.Dump(d.ToString());
		}
	}
}

for (int d = 0; d <= R + C - 2; d++)
{
	q.Clear();
	for (int c = 0; c < C; c++)
	{
		int r = d - c;
		if (r >= 0 && r < R && c >= 0 && c < C)
		{
			Add(lines[r][c]);
			//new { r, c, q }.Dump(d.ToString());
		}
	}
}

for (int d = 0; d <= R + C - 2; d++)
{
	q.Clear();
	for (int c = C - 1; c >= 0; c--)
	{
		int r = d - c;
		if (r >= 0 && r < R && c >= 0 && c < C)
		{
			Add(lines[r][c]);
			//new { r, c, q }.Dump(d.ToString());
		}
	}
}

tot.Dump();

const string find = "XMAS";

void Add(char v)
{
	q.Enqueue(v);
	if (q.Count > 4)
		q.Dequeue();
	if (Found())
		tot++;
}

bool Found()
{
	if (q.Count == 4)
	{
		int x = 0;
		foreach (var e in q)
			if (e != find[x++])
				return false;
		return true;
	}
	return false;
}
