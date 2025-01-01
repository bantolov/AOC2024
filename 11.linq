<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "11input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var line = File.ReadAllLines(inPath).Single();
var a = line.Split(' ').Select(p => long.Parse(p)).ToList();
var b = new List<long>();

checked
{
	for (int i = 0; i < 75; i++)
	{
		foreach (var x in a)
		{
			if (x == 0)
			{
				b.Add(1);
				continue;
			}

			string d = x.ToString();
			if (d.Length % 2 == 0)
			{
				long x1 = long.Parse(d[..(d.Length / 2)]);
				long x2 = long.Parse(d[(d.Length / 2)..]);
				b.Add(x1);
				b.Add(x2);
				continue;
			}

			b.Add(x * 2024);
		}
		$"{i+1}: {b.Count()}".Dump();
		var c = a;
		a = b;
		b = c;
		b.Clear();
	}
}
