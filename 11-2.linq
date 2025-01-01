<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "11input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var line = File.ReadAllLines(inPath).Single();
var vs = line.Split(' ').Select(p => long.Parse(p)).ToList().Dump();
int n = 75;

Dictionary<(long v, int r), long> mem = new();

checked
{
	vs.Sum(v => Len(v, n)).Dump("tot");
}

mem.Count.Dump();

long LenCached(long v, int r)
{
	if (r == 0)
		return 1;
		
	var key = (v, r);
	if (mem.TryGetValue(key, out long result))
		return result;

	result = Len(v, r);
	mem[key] = result;
	return result;
}

long Len(long v, int r)
{
	if (r == 0)
		return 1;
		
	if (v == 0)
		return LenCached(1, r - 1);

	string d = v.ToString();
	if (d.Length % 2 == 0)
	{
		long v1 = long.Parse(d[..(d.Length / 2)]);
		long v2 = long.Parse(d[(d.Length / 2)..]);
		return LenCached(v1, r - 1) + LenCached(v2, r - 1);
	}
	
	return LenCached(v * 2024, r - 1);
}
