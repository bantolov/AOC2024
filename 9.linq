<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "9input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var line = File.ReadAllLines(inPath).Single();
var nums = line.Select(x => (int)(x - '0')).ToList();
nums.Dump(0);
var tape = new int[nums.Sum()];
var empty = new List<int>();

int pos = 0;
for (int i = 0; i < nums.Count; i++)
	for (int j = 0; j < nums[i]; j++)
	{
		if (i % 2 == 0)
			tape[pos++] = i / 2;
		else
		{
			empty.Add(pos);
			tape[pos++] = -1;
		}
	}
		
tape.Dump(0);
empty.Dump(0);
int e = 0;
for (int x = tape.Length - 1; x >= 0 && e < empty.Count && x > empty[e]; x--)
{
	if (tape[x] != -1)
	{
		//new { ee=empty[e], t=tape[x] }.ToString().Dump();
		tape[empty[e++]] = tape[x];
		tape[x] = -1;
	}
}

tape.Dump(0);

tape.Where(x => x != -1).Select((x, y) => ((long)x)*y).Dump(0).Sum().Dump();
