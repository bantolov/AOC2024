<Query Kind="Statements">
  <AutoDumpHeading>true</AutoDumpHeading>
</Query>

string inName = "5input.txt";

string inPath = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), inName);
var lines = File.ReadAllLines(inPath);
//lines.Dump();

List<Tuple<int, int>> rules = new();
long tot = 0;

foreach (var line in lines)
{
	if (line.Contains('|'))
	{
		var parts = line.Split('|').Select(p => int.Parse(p)).ToList();
		rules.Add(Tuple.Create(parts[0], parts[1]));
	}
	else if (!string.IsNullOrWhiteSpace(line))
	{
		var pages = line.Split(',').Select(p => int.Parse(p)).ToList();
		var index = pages
			.Select((p, x) => (p, x))
			.ToDictionary(px => px.p, px => px.x);
		//index.Dump();
		
		//if (Check(index))
			//tot += pages[pages.Count / 2];
			
		if (!Check(index))
		{
			TopologicalSort(pages, rules);
			
			tot += pages[pages.Count / 2];
		}
	}
}
tot.Dump();

//rules.Dump();

bool Check(Dictionary<int, int> pages)
{
	foreach (var rule in rules)
		if (pages.TryGetValue(rule.Item1, out int before)
			&& pages.TryGetValue(rule.Item2, out int after))
			if (before > after)
				return false;
	return true;
}

static void TopologicalSort<T>(List<T> list, IEnumerable<Tuple<T, T>> dependencies)
{
	var dependenciesByDependent = new Dictionary<T, List<T>>();
	foreach (var relation in dependencies)
	{
		List<T> group;
		if (!dependenciesByDependent.TryGetValue(relation.Item2, out group))
		{
			group = new List<T>();
			dependenciesByDependent.Add(relation.Item2, group);
		}
		group.Add(relation.Item1);
	}

	var result = new List<T>();
	var givenList = new HashSet<T>(list);
	var processed = new HashSet<T>();
	var analysisStarted = new List<T>();
	foreach (var element in list)
		AddDependenciesBeforeElement(element, result, givenList, dependenciesByDependent, processed, analysisStarted);
	list.Clear();
	list.AddRange(result);
}

static void AddDependenciesBeforeElement<T>(T element, List<T> result, HashSet<T> givenList, Dictionary<T, List<T>> dependencies, HashSet<T> processed, List<T> analysisStarted)
{
	if (!processed.Contains(element) && givenList.Contains(element))
	{
		if (analysisStarted.Contains(element))
		{
			int circularReferenceIndex = analysisStarted.IndexOf(element);
			throw new Exception(String.Format(
				"Circular dependency detected on elements:\r\n{0}.",
				String.Join(",\r\n", analysisStarted.GetRange(circularReferenceIndex, analysisStarted.Count - circularReferenceIndex))));
		}
		analysisStarted.Add(element);

		if (dependencies.ContainsKey(element))
			foreach (T dependency in dependencies[element])
				AddDependenciesBeforeElement(dependency, result, givenList, dependencies, processed, analysisStarted);

		analysisStarted.RemoveAt(analysisStarted.Count - 1);
		processed.Add(element);
		result.Add(element);
	}
}