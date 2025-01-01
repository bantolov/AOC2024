<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

#define TRACE

string inName = "17input.txt";

checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace();
	long a = long.Parse(lines[0].Split(' ')[2]);
	long b = long.Parse(lines[1].Split(' ')[2]);
	long c = long.Parse(lines[2].Split(' ')[2]);
	var p = lines[4].Split(' ')[1].Split(',').Select(x => long.Parse(x)).ToList();
	
	new { a, b, c }.DumpTrace();
	p.DumpTrace("p");
	
	int ip = 0;
	List<long> output = [];

	while (ip >= 0 && ip < p.Count)
	{
		long op = p[ip + 1];
		long combo = op switch
		{
			>= 0 and <= 3 => op,
			4 => a,
			5 => b,
			6 => c,
			_ => -1111111111,
			//7 => throw new Exception("7"),
			//_ => throw new Exception("_")
		};
		
		switch (p[ip])
		{
			case 0:
				a = a / Pow2(combo);
				break;
			case 1:
				b = b ^ op;
				break;
			case 2:
				b = combo % 8;
				break;
			case 3:
				if (a != 0)
					ip = (int)(op - 2);
				break;
			case 4:
				b = b ^ c;
				break;
			case 5:
				output.Add(combo % 8);
				break;
			case 6:
				b = a / Pow2(combo);
				break;
			case 7:
				c = a / Pow2(combo);
				break;
			default:
				throw new Exception();
		}
		
		ip += 2; // TODO if not jump.
	}
	
	string.Join(",", output).Dump("output");


	long Pow2(long x)
	{
		return long.RotateLeft(1, (int)x);
	}
}
