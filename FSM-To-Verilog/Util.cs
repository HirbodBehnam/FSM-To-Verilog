using System.Text.Json;

namespace FSM_To_Verilog;

public static class Util
{
	/// <summary>
	/// Parses the given input file into <see cref="Types.InputJson"/>
	/// </summary>
	/// <param name="filename">The filename to read and parse</param>
	/// <returns>The parsed data</returns>
	public static Types.InputJson ParseInputJson(string filename)
	{
		string fileContent = File.ReadAllText(filename);
		return JsonSerializer.Deserialize<Types.InputJson>(fileContent,
			new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
	}
}