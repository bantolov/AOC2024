<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

#define TRACE
string inName = "13input.txt";
// 92881336231543

checked
{

	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace();
	int testN = (lines.Length + 1) / 4;
	var tests = Enumerable.Range(0, testN)
		.Select(i =>
		{
			string lineA = lines[4 * i];
			string lineB = lines[4 * i + 1];
			string lineP = lines[4 * i + 2];

			var partsA = lineA.Split('+', ',');
			long xa = long.Parse(partsA[1]);
			long ya = long.Parse(partsA[3]);
			var partsB = lineB.Split('+', ',');
			long xb = long.Parse(partsB[1]);
			long yb = long.Parse(partsB[3]);
			var partsP = lineP.Split('=', ',');
			long xp = long.Parse(partsP[1]) + 10000000000000;
			long yp = long.Parse(partsP[3]) + 10000000000000;

			new { xa, ya, xb, yb, xp, yp }.ToString().DumpTrace();
			return (xa, ya, xb, yb, xp, yp);
		})
		.ToList();

	long tot = 0;
	foreach (var (xa, ya, xb, yb, xp, yp) in tests)
	{
		string title = new { xa, ya, xb, yb, xp, yp }.ToString();
		long up = yp * xa - xp * ya;
		long down = xa * yb - xb * ya;
		if (down == 0)
		{
			throw new Exception("RijesiULiniji(xa, ya, xb, yb, xp, yp);");
		}
		else
		{
			long b = Math.DivRem(up, down, out long reminder);
			if (reminder == 0)
			{
				long a = Math.DivRem(xp - b * xb, xa, out long reminderA);
				long a2 = Math.DivRem(yp - b * yb, ya, out long reminderA2);
				if (reminderA == 0)
				{
					if (a != a2)
						throw new Exception();
					long result = a * 3 + b * 1;
					tot += result;
					result.DumpTrace(title);
				}
				else
					"Nema rješenja.".DumpTrace(title);
			}
			else
				"Nema rješenja.".DumpTrace(title);
		}
	}

	tot.Dump();
}