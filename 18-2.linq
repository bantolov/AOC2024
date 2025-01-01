<Query Kind="Statements" />

#define TRACE
//string inName = "18z.txt"; int nx = 7; int ny = 7;
string inName = "18input.txt"; int nx = 71; int ny = 71;
checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace(0);
	var a = new bool[nx, ny];
	for (int l = 0; l < lines.Length; l++)
	{
		$"Drop {l+1}".Dump();
		string line = lines[l];
		var parts = line.Split(',').Select(i => int.Parse(i)).ToList();
		a[parts[0], parts[1]] = true;

		//ReportA();
		var d = new int[nx, ny];
		for (int x = 0; x < nx; x++)
			for (int y = 0; y < ny; y++)
				d[x, y] = int.MaxValue;
		d[0, 0] = 0;
		int dist = 0;
		var last = new List<(int x, int y)> { (0, 0) };
		var dirs = new (int x, int y)[] { (0, 1), (-1, 0), (0, -1), (1, 0) };
		while (last.Count > 0)
		{
			dist++;
			var next = new List<(int x, int y)> { };
			foreach (var pos in last)
				foreach (var dir in dirs)
				{
					var x = pos.x + dir.x;
					var y = pos.y + dir.y;

					if (x >= 0 && x < nx && y >= 0 && y < ny
						&& d[x, y] > dist
						&& a[x, y] == false)
					{
						d[x, y] = dist;
						next.Add((x, y));
					}
				}
			last = next;
			if (d[nx-1,ny-1] != int.MaxValue)
				continue;
		}
		if (d[nx-1,ny-1] == int.MaxValue)
		{
			line.Dump("LINE");
			return;
		}
	}
	
	void ReportA()
	{
		string.Join("\r\n", Enumerable.Range(0, ny)
			.Select(y => new string(Enumerable.Range(0, nx)
				.Select(x => a[x,y] ? '#' : '.').ToArray()))).DumpTrace();
	}
}
