<Query Kind="Program">
  <AutoDumpHeading>true</AutoDumpHeading>
  <RuntimeVersion>8.0</RuntimeVersion>
</Query>

void Main()
{
	string inName = "2input.txt";
	
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	
	var lines = File.ReadAllLines(inPath).Dump(0);
	List<int> l1 = new();
	List<int> l2 = new();
	int tot = 0;
	foreach (var line in lines)
	{
		var levels = GetNumbers(line.Dump());
		var levelsDown = levels.Select(n => -n).ToList();

		bool safe = Ok(levels).Dump(line) || Ok(levelsDown).Dump(line);
			
		tot += safe ? 1 : 0;
	}
	tot.Dump();
	
}

bool Ok(List<int> levels)
{
	for (int skip = 0; skip < levels.Count; skip++)
		if (Ok(levels, skip))
			return true;
	return false;
}

bool Ok(List<int> levels, int skip)
{
	int last = skip == 0 ? levels[1] : levels[0];
	for (int i = skip == 0 ? 2 : 1; i < levels.Count; i++)
	{
		if (i == skip)
			continue;
		else
		{
			var d = levels[i] - last;
			if (d < 1 || d > 3)
				return false;
			last = levels[i];
		}
	}
	return true;
}

//==========================================================================

List<int> GetNumbers(string line)
	=> line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToList();
