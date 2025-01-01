<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

//#define TRACE
using RC = (int r, int c);
using Keypad = System.Collections.Generic.Dictionary<char, (int r, int c)>;

//int directionalKeypadCount = 1 + 2;
int directionalKeypadCount = 1 + 25;
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

	Dictionary<(char k0, char k1), string[]> possibleMovesCache = new();
	Dictionary<(string s, int level), long[]> bestMovesCache = new();

	long tot = 0;
	foreach (var l in lines)
	{
		var moves = GetBestMovesS(l, numericKeypad, 1);
		moves.DumpTrace(l);
		long count = moves.Last();
		long num = int.Parse(l[..^1]);
		$"{count} x {num} = {count * num}".Dump(l);
		tot += count * num;
	}
	tot.Dump();
	
	long[] GetBestMovesS(string s, Keypad keypad, int level)
	{
		if (keypad == numericKeypad)
			return GetBestMovesSCompute(s, keypad, level);

		var key = (s, level);
		long[] moves;
		if (bestMovesCache.TryGetValue(key, out moves))
			return moves;
		moves = GetBestMovesSCompute(s, keypad, level);
		bestMovesCache.Add(key, moves);
		return moves;
	}

	long[] GetBestMovesSCompute(string s, Keypad keypad, int level)
	{
		int depth = directionalKeypadCount + 1 - level;
		var moves = new long[depth];
		char last = 'A';
		foreach (char key in s)
		{
			var keyMoves = GetBestMovesK(last, key, keypad, level);
			if (keyMoves.Length != depth) throw new Exception();
			for (int d = 0; d < depth; d++)
				moves[d] += keyMoves[d];
			last = key;
		}
		return moves;
	}

	long[] GetBestMovesK(char k0, char k1, Keypad keypad, int level)
	{
		var possibleMoves = GetPossibleMovesCached(k0, k1, keypad);
		return GetMovesToNextKey(possibleMoves, level, k0, keypad);
	}
	
	string[] GetPossibleMovesCached(char k0, char k1, Keypad keypad)
	{
		if (keypad == numericKeypad)
			return GetPossibleMoves(k0, k1, keypad);
		
		var key = (k0, k1);
		string[] moves;
		if (possibleMovesCache.TryGetValue(key, out moves))
			return moves;
		moves = GetPossibleMoves(k0, k1, keypad);
		possibleMovesCache.Add(key, moves);
		return moves;
	}
	
	string[] GetPossibleMoves(char k0, char k1, Keypad keypad)
	{
		var p0 = keypad[k0];
		var p1 = keypad[k1];
		var dr = p1.r - p0.r;
		var dc = p1.c - p0.c;
		
		string mr = dr > 0 ? new string('v', dr) : new string('^', -dr);
		string mc = dc > 0 ? new string('>', dc) : new string('<', -dc);

		if (mr.Length == 0 || mc.Length == 0)
		{
			string m = mc + mr + "A";
			return [mc + mr + "A"];
		}
		else
		{
			string m1 = mc + mr + "A";
			string m2 = mr + mc + "A";
			bool valid1 = IsValid(m1, k0, keypad);
			bool valid2 = IsValid(m2, k0, keypad);
			if (!valid2)
				return [m1];
			if (!valid1)
				return [m2];
			return [m1, m2];
		}
	}
	
	long[] GetMovesToNextKey(string[] possibleMoves, int level, char start, Keypad keypad)
	{
		if (level == directionalKeypadCount)
		{
			return [possibleMoves[0].Length];
		}
		else if (possibleMoves.Length == 1)
		{
			var mDeep = GetBestMovesS(possibleMoves[0], controlKeypad, level + 1);
			return [possibleMoves[0].Length, .. mDeep];
		}
		else
		{
			var mDeep1 = GetBestMovesS(possibleMoves[0], controlKeypad, level + 1);
			var mDeep2 = GetBestMovesS(possibleMoves[1], controlKeypad, level + 1);
			return mDeep1.Last() <= mDeep2.Last()
				? [possibleMoves[0].Length, .. mDeep1]
				: [possibleMoves[1].Length, .. mDeep2];
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
