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

for (int r = 0; r + 2 < R; r++)
	for (int c = 0; c + 2 < C; c++)
	{
		if (lines[r+1][c+1] == 'A')
		{
			char tl = lines[r][c];
			char tr = lines[r][c+2];
			char bl = lines[r+2][c];
			char br = lines[r+2][c+2];
			if ((tl == 'M' && br == 'S' || tl == 'S' && br == 'M')
				&& (bl == 'M' && tr == 'S' || bl == 'S' && tr == 'M'))
			{
				tot++;
			}
		}
	}

tot.Dump();
