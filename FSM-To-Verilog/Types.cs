namespace FSM_To_Verilog;

public static class Types
{
	public struct InputJson
	{
		public Node[] Nodes { get; set; }
		public Link[] Links { get; set; }

		/// <summary>
		/// Checks if given input is valid or not.
		/// If not, prints the reason and returns false
		/// </summary>
		/// <returns>True if valid</returns>
		public bool Valid()
		{
			// Only and only one state should be marked as the starting state
			if (Nodes.Count(x => x.IsAcceptState) != 1)
			{
				Console.WriteLine("Only and only one state should be marked as the starting state");
				return false;
			}
			// Names must be distinct
			HashSet<string> nodeNames = new(Nodes.Select(x => x.Name));
			if (nodeNames.Count != Nodes.Length)
			{
				Console.WriteLine("Node names must be distinct");
				return false;
			}
			// Check if all link source and dests belong to nodes
			if (!Links.All(x => nodeNames.Contains(x.Dest) && nodeNames.Contains(x.Source)))
			{
				Console.WriteLine("Links source and destinations must be in nodes");
				return false;
			}
			// GtG
			return true;
		}
	}
	
	public struct Node
	{
		public string Name { get; set; }
		public bool IsAcceptState { get; set; }
	}
	
	public struct Link
	{
		public string Name { get; set; }
		public string Source { get; set; }
		public string Dest { get; set; }
	}
}