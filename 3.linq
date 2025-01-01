<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "3input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);

var text = File.ReadAllText(inPath);
//text.Dump();

var r = new Regex(@"mul\((?<a>\d{1,3}),(?<b>\d{1,3})\)|do\(\)|don't\(\)");
var ms = r.Matches(text);
//ms.Dump(2);
long tot = 0;
bool enabled = true;
foreach (Match m in ms)
{
	if (m.Value.StartsWith("mul") && enabled)
	{
		int a = int.Parse(m.Groups["a"].Value);
		int b = int.Parse(m.Groups["b"].Value);
		tot += a * b;
	}
	else if (m.Value == "don't()")
	{
		enabled = false;
	}
	else if (m.Value == "do()")
	{
		enabled = true;
	}

}
tot.Dump();
