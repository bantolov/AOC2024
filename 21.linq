<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

#define TRACE
using RC = (int r, int c);
using Keypad = System.Collections.Generic.Dictionary<char, (int r, int c)>;

//int directionalKeypadCount = 1 + 25;
int directionalKeypadCount = 1 + 2;
string inName = "21input.txt";
var numericKeypad = new Keypad
{
	{ '7', (1, 1) }, { '8', (1, 2) }, { '9', (1, 3) },
	{ '4', (2, 1) }, { '5', (2, 2) }, { '6', (2, 3) },
	{ '1', (3, 1) }, { '2', (3, 2) }, { '3', (3, 3) },
	                 { '0', (4, 2) }, { 'A', (4, 3) },
};
var controlKeypad = new Keypad
{
	                 { '^', (1, 2) }, { 'A', (1, 3) },
	{ '<', (2, 1) }, { 'v', (2, 2) }, { '>', (2, 3) },
};
checked
{
	string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
	var lines = File.ReadAllLines(inPath).DumpTrace("file");

	long tot = 0;
	foreach (var l in lines)
	{
		var moves = GetBestMovesS(l, numericKeypad, 1);
		moves.Select(m => new { m.Length, m }).Dump(l);
		long count = moves.Last().Length;
		long num = int.Parse(l[..^1]);
		$"{count} x {num} = {count * num}".Dump(l);
		tot += count * num;
	}
	tot.Dump();

	string[] GetBestMovesS(string s, Keypad keypad, int level)
	{
		List<string[]> moves = [];
		char last = 'A';
		foreach (char key in s)
		{
			moves.Add(GetBestMovesK(last, key, keypad, level));
			last = key;
		}
		
		int depth = moves[0].Length;
		var joinedMoves = new string[depth];
		for (int d = 0; d < depth; d++)
			joinedMoves[d] = string.Concat(moves.Select(m => m[d]));
		return joinedMoves;
	}

	string[] GetBestMovesK(char k0, char k1, Keypad keypad, int level)
	{
		var p0 = keypad[k0];
		var p1 = keypad[k1];
		var dr = p1.r - p0.r;
		var dc = p1.c - p0.c;
		return GetMovesToNextKey(dr, dc, level, k0, keypad);
	}

	string[] GetMovesToNextKey(int dr, int dc, int level, char start, Keypad keypad)
	{
		string mr = dr > 0 ? new string('v', dr) : new string('^', -dr);
		string mc = dc > 0 ? new string('>', dc) : new string('<', -dc);

		if (level == directionalKeypadCount)
		{
			string m = mc + mr + "A";
			return [m];
		}
		else if (mr.Length == 0 || mc.Length == 0)
		{
			string m = mc + mr + "A";
			var mDeep = GetBestMovesS(m, controlKeypad, level + 1);
			return [m, .. mDeep];
		}
		else
		{
			string m1 = mc + mr + "A";
			string m2 = mr + mc + "A";
			bool valid1 = IsValid(m1, start, keypad);
			bool valid2 = IsValid(m2, start, keypad);
			var mDeep1 = GetBestMovesS(m1, controlKeypad, level + 1);
			var mDeep2 = GetBestMovesS(m2, controlKeypad, level + 1);
			if (!valid2)
				return [m1, .. mDeep1];
			if (!valid1)
				return [m2, .. mDeep2];
			return mDeep1.Last().Length <= mDeep2.Last().Length
				? [m1, .. mDeep1]
				: [m2, .. mDeep2];
		}
	}
	
	bool IsValid(string moves, char start, Keypad keypad)
	{
		if (keypad == numericKeypad)
		{
			if (start == '0') return !moves.StartsWith("<");
			if (start == 'A') return !moves.StartsWith("<<");
			if (start == '1') return !moves.StartsWith("v");
			if (start == '4') return !moves.StartsWith("vv");
			if (start == '7') return !moves.StartsWith("vvv");
		}
		else
		{
			if (start == '^') return !moves.StartsWith("<");
			if (start == 'A') return !moves.StartsWith("<<");
			if (start == '<') return !moves.StartsWith("^");
		}
		return true;
	}
}
