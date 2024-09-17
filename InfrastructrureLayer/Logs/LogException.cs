using ApplicationLayer.Logging;
using Serilog;

namespace InfrastructrureLayer.Logs {
	public class LogException : ILogException {
        public void LogExceptions(Exception ex) {
			LogToFile(ex.Message);
			LogToConsole(ex.Message);
			LogToDebugger(ex.Message);
		}

		public void LogToFile(string message) => Log.Information(message);

		public void LogToConsole(string message) => Log.Warning(message);

		public void LogToDebugger(string message) => Log.Debug(message);
	}
}
