<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "1input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);

var lines = File.ReadAllLines(inPath).Dump(0);
List<int> l1 = new();
List<int> l2 = new();
foreach (var line in lines)
{
	var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
	l1.Add(int.Parse(parts[0]));
	l2.Add(int.Parse(parts[1]));
}
l1.Sort();
l2.Sort();
int tot = 0;
for (int i = 0; i < l1.Count; i++)
	tot += l1[i] * l2.Count(n => n == l1[i]);
tot.Dump();
