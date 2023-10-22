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
		Types.InputJson inputJson = Util.ParseInputJson(args[0]);
		if (!inputJson.Valid())
			Environment.Exit(1);

		// Create the states
		using StreamWriter outputFile = new("output.v");
		outputFile.Write("module GenerateModule(input wire clk, input wire reset, ");
		outputFile.Write(inputJson.GenerateInputs());
		outputFile.WriteLine(", output reg n_state, output reg p_state);");

		// Define the states
		outputFile.WriteLine(inputJson.GenerateStates());

		// Create the next state initializer
		outputFile.WriteLine(inputJson.GenerateNextStateAssigner());

		// Always clock thingy
		outputFile.WriteLine("\talways @(posedge clk) begin");
		outputFile.WriteLine("\t\tif (reset)");
		outputFile.WriteLine($"\t\t\tp_state <= {inputJson.FirstStateName()};");
		outputFile.WriteLine("\t\telse");
		outputFile.WriteLine("\t\t\tp_state <= n_state;");
		outputFile.WriteLine("\tend");

		// Done
		outputFile.WriteLine("endmodule");
	}
}