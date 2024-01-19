using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace Terminal{
	public class Executable : Form1{
		//Variable que determina si el proceso termino
		private static bool processIsRunning = false;
		public static bool _processIsRunning(){
			return processIsRunning;
		}

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

				//Habilito la escucha de eventos
				proceso.EnableRaisingEvents = true;

				//Proceso el termino del programa
				proceso.Exited += new EventHandler(async (object? _sender, EventArgs _e) => {
					processIsRunning = false;

					await Task.Delay(100);
                    await Task.Run(form._PutLinea(form.usuarioPC() + ":" + form.pathActual() + ">", false, sender, e));
				});

				//Proceso la salida del programa
				proceso.OutputDataReceived += new DataReceivedEventHandler(async (object sendingProcess, DataReceivedEventArgs outline) => {
						if(!string.IsNullOrEmpty(outline.Data)){
							await Task.Run(form._PutLinea(outline.Data + "\n", false, sender, e));

							//Es hora de comentar el codigo
							//No tengo ni puñetera idea de como lo hice pero lo hice
							//OJO no tocar
						}
					});

				proceso.Start();
			
				//Inicio el proceso
				processIsRunning = true;
				
				proceso.BeginOutputReadLine();

				proceso.WaitForExit();
			}
		}

		//Metodo que ejecuta el programa
		public static async void llamarPrograma(string[] _arrayComando, Form1 form, object sender, KeyEventArgs e){
			try{
				//Aqui es importante saber si un programa externo puede hacer un throw hacia el que lo ejecuta.	

				//Comprobar que el programa existe
				if(!existePrograma(_arrayComando[0].ToLower())){
					throw new Exception(Error.PROGRAMA_NO_EXISTE);
				}

				//Ejecuta y envia la respuesta
				Executable.ejecutarWaitResponse(Executable.executablePath(_arrayComando[0].ToLower()), (_arrayComando.Length>1)? _arrayComando[1] : "", form, sender, e);
			}
			catch(Exception error){
				switch (error.Message)
				{
					case Error.PROGRAMA_NO_EXISTE:{
						await Task.Run(form._PutLinea("El comando '" + _arrayComando[0] + "' no se reconoce como una instruccion valida.",false,sender,e));
	                    await Task.Run(form._PutLinea("\n" + form.usuarioPC() + ":" + form.pathActual() + ">", false, sender, e));
						break;
					}
					default:{
						await Task.Run(form._PutLinea(error.Message,false,sender,e));
	                    await Task.Run(form._PutLinea("\n" + form.usuarioPC() + ":" + form.pathActual() + ">", false, sender, e));
						break;
					}
				}
			}
		}
	}
}