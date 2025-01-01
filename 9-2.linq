<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "9input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var line = File.ReadAllLines(inPath).Single();
var nums = line.Select(x => (int)(x - '0')).ToList();
nums.Dump(0);

var files = new List<(int pos, int len)>();
var empties = new List<(int pos, int len)>();

int pos = 0;
for (int i = 0; i < nums.Count; i++)
{
	if (i % 2 == 0)
		files.Add((pos, nums[i]));
	else
		empties.Add((pos, nums[i]));
	
	pos += nums[i];
}

files.Dump();
empties.Dump();

for (int f = files.Count - 1; f >= 0; f--)
{
	var ff = files[f];
	int e = empties.FindIndex(ee => ee.len >= ff.len);
	if (e >= 0)
	{
		var ee = empties[e];
		if (ee.pos < ff.pos)
		{
			files[f] = (ee.pos, ff.len);
			empties[e] = (ee.pos + ff.len, ee.len - ff.len);
		}
	}
}

files.Dump();
empties.Dump();

files.Select((f, x) => Enumerable.Range(0, f.len).Select(y => (((long)f.pos) + y) * x).Sum()).Sum().Dump();
