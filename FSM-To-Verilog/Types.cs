using System.Text;

namespace FSM_To_Verilog;

public static class Types
{
	public struct InputJson
	{
		/// <summary>
		/// List of inputs of the module
		/// </summary>
		public string[] Inputs { get; set; }

		/// <summary>
		/// List of states of the FSM
		/// </summary>
		public Node[] Nodes { get; set; }

		/// <summary>
		/// List of the transitions
		/// </summary>
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

			// Check inputs
			if (Inputs.Length == 0)
			{
				Console.WriteLine("Your FSM must have at least one input");
				return false;
			}

			// Inputs must be distinct
			if (Inputs.Distinct().Count() != Inputs.Length)
			{
				Console.WriteLine("Inputs must be distinct");
				return false;
			}

			// GtG
			return true;
		}

		/// <summary>
		/// Gets the first state's name
		/// </summary>
		/// <returns>The name of the first state</returns>
		public string FirstStateName()
		{
			return Nodes.First(x => x.IsAcceptState).Name;
		}

		/// <summary>
		/// Generates the input wires of the module
		/// </summary>
		/// <returns>The list of inputs</returns>
		public string GenerateInputs()
		{
			return string.Join(',', Inputs.Select(x => "input wire " + x));
		}

		/// <summary>
		/// Generates the local param of all states.
		/// I used one hot method to create them
		/// </summary>
		/// <returns>The verilog line which defines all possible states</returns>
		public string GenerateStates()
		{
			StringBuilder result = new("\tlocalparam ");
			result.Append($"[{Inputs.Length}:0] ");
			for (var i = 0; i < Inputs.Length; i++)
			{
				var allZero = new StringBuilder(new string('0', Inputs.Length))
				{
					[i] = '1'
				};
				result.Append($"{Inputs[i]}={Inputs.Length}'b{allZero}, ");
			}

			return result.AppendLine(";").ToString();
		}

		public string GenerateNextStateAssigner()
		{
			StringBuilder result = new("\talways @(*) begin\n");
			result.AppendLine("\t\tcase (p_state)");
			foreach (string state in Nodes.Select(x => x.Name))
			{
				result.AppendLine($"\t\t\t{state}: begin");
				foreach (Link transition in Links.Where(x => x.Source == state))
				{
					result.AppendLine($"\t\t\t\tif ({transition.Name}) begin");
					result.AppendLine($"\t\t\t\t\tn_state = {transition.Dest};");
					result.AppendLine("\t\t\t\tend");
				}

				result.AppendLine($"");
				result.AppendLine("\t\t\tend");
			}

			result.AppendLine("\t\tendcase");
			result.AppendLine("\tend");
			return result.ToString();
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