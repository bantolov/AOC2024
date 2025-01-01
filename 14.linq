<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

//#define TRACE
string inName = "14z.txt"; int nx = 11; int ny = 7;
//string inName = "14input.txt"; int nx = 101; int ny = 103;
checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace();
	var robots = lines.Select(l =>
	{
		var p = l.Split('=', ',', ' ');
		return new { px = long.Parse(p[1]), py = long.Parse(p[2]), vx = long.Parse(p[4]), vy = long.Parse(p[5]) };
	}).ToList();
	robots.DumpTrace();
	
	var r100 = robots.Select(r => new
	{
		 px = ((r.px + 100 * r.vx) % nx + nx) % nx,
		 py = ((r.py + 100 * r.vy) % ny + ny) % ny,
	}).ToList();
	r100.DumpTrace();
	
	var q = new long[4];
	foreach (var r in r100)
	{
		if (r.px < (nx-1)/2 && r.py < (ny-1)/2) q[0]++;
		if (r.px > (nx-1)/2 && r.py < (ny-1)/2) q[1]++;
		if (r.px < (nx-1)/2 && r.py > (ny-1)/2) q[2]++;
		if (r.px > (nx-1)/2 && r.py > (ny-1)/2) q[3]++;
	}
	q.Dump();
	(q[0]*q[1]*q[2]*q[3]).Dump();
	
	int bestScore = 0;
	for (int i = 0; i <= 100_000_000; i++)
	{
		var map = Enumerable.Range(0, ny).Select(y => Enumerable.Repeat('.', nx).ToArray()).ToArray();
		foreach (var r in robots)
		{
			long x = ((r.px + i * r.vx) % nx + nx) % nx;
			long y = ((r.py + i * r.vy) % ny + ny) % ny;
			map[y][x] = 'X';
		}
		int score = 0;
		for (int x = 1; x < nx - 1; x++)
			for (int y = 1; y < ny - 2; y++)
				if (map[y][x] == 'X')
				{
					if ('X' == map[y-1][x-1]) score++;
					if ('X' == map[y-1][x]) score++;
					if ('X' == map[y-1][x+1]) score++;
					if ('X' == map[y][x-1]) score++;
				}
		if (score > bestScore)
		{
			string.Join("\r\n", map.Select(l => new string(l))).Dump($"iteration {i}., score {score}");
			bestScore = score;
		}
	}
}

