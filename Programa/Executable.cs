namespace program{
	public class Executable : Directorio{
		//Metodo que analiza si existe el programa
		private static bool existePrograma(string fileName){
			if(File.Exists(Directorio.actualFunctions() + fileName + ".exe")){
				return true;
			}
			else{
				return false;
			}
		}

		/*

		//Metodo que ejecuta el programa
		public static string ejecutar(string fileName, string[] argsv){
			try{
				//Aqui es importante saber si un programa externo puede hacer un throw hacia el que lo ejecuta.	
			}
			catch(Exception error){
				Console.WriteLine(error + "El comando '" + fileName + "' no se reconoce como una instruccion valida.");
			}
		}

		*/
	}
}