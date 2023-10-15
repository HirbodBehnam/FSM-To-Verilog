using System.Text.Json;
using FSM_To_Verilog;

class Program
{
	public static void Main(string[] args)
	{
		// Check JSON arguments
		if (args.Length < 1)
		{
			Console.WriteLine("Pass the input json as the first argument.");
			Console.WriteLine("Generate the json from this site: https://arash1381-y.github.io/fsmNinja/");
			Environment.Exit(1);
		}

		// Parse the json
		Types.InputJson inputJson = Util.ParseInputJson(args[1]);
		if (!inputJson.Valid())
			Environment.Exit(1);
	}
}