// namespace Terminal{
// 	public static class EventoSalida{
// 		public delegate void ProgramaOutputEventHandler(object sender, EventArgs e);

// 		public static event ProgramaOutputEventHandler programOutput;

//         private static void _startOutput(){
// 			programOutput?.Invoke(null, EventArgs.Empty);
// 		}

// 		public static void startOutput(string message){
// 			Salida.message = message;
// 			_startOutput();
// 		}
// 	}
// }