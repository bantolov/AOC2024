<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "7input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var lines = File.ReadAllLines(inPath).Dump();

long tot = 0;
foreach (var l in lines)
{
	var parts = l.Split(':');
	var goal = long.Parse(parts[0]);
	var nums = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
	
	long current = nums[0];

	if (Try(current, 1, nums, goal))
		tot += goal;
}

tot.Dump();

static bool Try(long current, int pos, List<int> nums, long goal)
{
	if (pos == nums.Count)
		if (current == goal)
		{
			current.Dump();
			return true;
		}
		else
			return false;
		
	if (current > goal)
		return false;
	
	return
		Try(current + nums[pos], pos + 1, nums, goal)
		|| Try(current * nums[pos], pos + 1, nums, goal)
		|| Try(Concat(current, nums[pos]), pos + 1, nums, goal);
}

static long Concat(long a, int b)
{
	return long.Parse(a.ToString() + b.ToString());
}
