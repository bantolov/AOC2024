<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>


void Main()
{
	
	long[] expected = { 2, 4, 1, 2, 7, 5, 4, 5, 0, 3, 1, 7, 5, 5, 3, 0 };
	expected.Length.Dump();
	
	checked
	{
		var ab = new bool[48];
		Find(0);
		"Done.".Dump();

		void Find(int d)
		{
			if (d == 48)
			{
				ADec().Dump("final result");
				return;
			}
			foreach (bool v in new[] { false, true })
			{
				ab[d] = v;
				if (LastDigitsOk(d))
					Find(d + 1);
			}
			ab[d] = false;
		}
		
		long ADec()
		{
			long a = 0;
			for (int i = 48-1; i >= 0; i--)
				a = a * 2 + (ab[i] ? 1 : 0);
			return a;
		}

		bool LastDigitsOk(int d)
		{
			if (d < 10)
				return true;
			long a0 = ADec();
			if (d < 48 - 1)
				return OutputMatches((d - 7)/3, a0);
			else
				return OutputMatches(16, a0);
		}
		
		bool OutputMatches(int outputs, long a0)
		{
			new { outputs, a0, ab = string.Join("", ab.Select(b => b ? 1 : 0)) }.ToString().DumpTrace("");
			
			long a = a0;
			for (int x = 0; x < outputs; x++)
			{
				long b = a % 8;       //2,4   B ← A % 8
				b ^= 2;               //1,2   B ← B ^ 2
				long c = a >> (int)b; //7,5   C ← ⌊ A / (2^B) ⌋ ... A >> B
				b = b ^ c;            //4,5   B ← B ^ C
				b ^= 7;               //1,7   B ← B ^ 7
				$"output {b % 8}, expected {expected[x]}".DumpTrace();
				if (b % 8 != expected[x]) //5,5   Output B % 8
					return false;
				a >>= 3;              //0,3   A ← ⌊ A / (2^3) ⌋ ... A >> 3
			}
			return true;
		}
	
		/*
		0–3 ⇒ values 0–3
		4 ⇒ register A’s value
		5 ⇒ register B’s value
		6 ⇒ register C’s value
		7 ⇒ invalid in valid programs
		*/
	}
	
}
