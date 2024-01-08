using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace Terminal{
	public class Executable : Form1{
		//Metodo que analiza si existe el programa
		private static bool existePrograma(string fileName){
			return (File.Exists(Directorio.actualFunctions() + fileName + SistemaOperativo.barra() + fileName + SistemaOperativo.extension()))? true : false;
		}

		private static string executablePath(string fileName){
			return Directorio.actualFunctions() + fileName + SistemaOperativo.barra() + fileName + SistemaOperativo.extension();
		}

		private static void ejecutarWaitResponse(string _executablePath, string argumentos, Form1 form, object sender, KeyEventArgs e){
			using (Process proceso = new Process()){
				proceso.StartInfo.FileName  = _executablePath;
				proceso.StartInfo.Arguments = argumentos;
				proceso.StartInfo.RedirectStandardOutput = true;
				proceso.StartInfo.UseShellExecute = false;
				proceso.StartInfo.CreateNoWindow = true;
				proceso.OutputDataReceived += new DataReceivedEventHandler(tomarOutput
					/*(object sendingProcess, DataReceivedEventArgs outline) => {
						if(!string.IsNullOrEmpty(outline.Data)){
							form.PutLineaActual(outline.Data, false, sender, e);
						}
					}*/);

				proceso.Start();
				proceso.BeginOutputReadLine();

				proceso.WaitForExit();
			}
		}

		private static void tomarOutput(object sendingProcess, DataReceivedEventArgs outline/*, Form1 form, object sender, KeyEventArgs e*/){
			if(!string.IsNullOrEmpty(outline.Data))
				Salida.message.Enqueue(outline.Data);
		}

		//Metodo que ejecuta el programa
		public static void llamarPrograma(string[] _arrayComando, Form1 form, object sender, KeyEventArgs e){
			try{
				//Aqui es importante saber si un programa externo puede hacer un throw hacia el que lo ejecuta.	

				//Comprobar que el programa existe
				if(!existePrograma(_arrayComando[0])){
					throw new Exception(Error.PROGRAMA_NO_EXISTE);
				}

				//Ejecuta y envia la respuesta
				Executable.ejecutarWaitResponse(Executable.executablePath(_arrayComando[0]), (_arrayComando.Length>1)? _arrayComando[1] : "", form, sender, e);
			}
			catch(Exception error){
				switch (error.Message)
				{
					case Error.PROGRAMA_NO_EXISTE:{
						Salida.message.Enqueue("El comando '" + _arrayComando[0] + "' no se reconoce como una instruccion valida.");
						break;
					}
					default:{
						Salida.message.Enqueue(error.Message);
						break;
					}
				}
			}
		}
	}
}