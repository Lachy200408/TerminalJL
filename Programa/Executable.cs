using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Terminal{
	public class Executable : Directorio{
		//Metodo que analiza si existe el programa
		private static bool existePrograma(string fileName){
			return (File.Exists(Directorio.actualFunctions() + fileName + SistemaOperativo.barra() + fileName + SistemaOperativo.extension()))? true : false;
		}

		private static string executablePath(string fileName){
			return Directorio.actualFunctions() + fileName + SistemaOperativo.barra() + fileName + SistemaOperativo.extension();
		}

		private static string ejecutarWaitResponse(string _executablePath, string argumentos){
			string salida="";

			using (Process proceso = new Process()){
				proceso.StartInfo.FileName  = _executablePath;
				proceso.StartInfo.Arguments = argumentos;
				proceso.StartInfo.RedirectStandardOutput = true;
				proceso.StartInfo.UseShellExecute = false;
				proceso.StartInfo.CreateNoWindow = true;

				proceso.Start();

				//Devolver la salida
				salida = proceso.StandardOutput.ReadToEnd()!;

				proceso.WaitForExit();
			}

			return salida.Substring(0, salida.Length-2);
		}

		//Metodo que ejecuta el programa
		public static void llamarPrograma(string[] _arrayComando){
			try{
				//Aqui es importante saber si un programa externo puede hacer un throw hacia el que lo ejecuta.	

				//Comprobar que el programa existe
				if(!existePrograma(_arrayComando[0])){
					throw new Exception(Error.PROGRAMA_NO_EXISTE);
				}

				//Ejecuta y espera la respuesta
				Salida.message = Executable.ejecutarWaitResponse(Executable.executablePath(_arrayComando[0]), _arrayComando[1]);
			}
			catch(Exception error){
				switch (error.Message)
				{
					case Error.PROGRAMA_NO_EXISTE:{
						Salida.message = "El comando '" + _arrayComando[0] + "' no se reconoce como una instruccion valida.";
						break;
					}
					default:{
						Salida.message = error.Message;
						break;
					}
				}
			}
		}
	}
}