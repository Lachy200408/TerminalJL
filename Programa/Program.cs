using System;
using System.IO;
using System.Diagnostics;

namespace Terminal{
	public class Program{
		public static void Main(){
			string stringEntrante="";

			//Manipular la excepcion de cuando no se introduce texto
			try{
				if((stringEntrante = Console.ReadLine()!).Length == 0){
					throw new Exception("No se ha escrito nada");
				}
			}
			catch(Exception err){
				Console.WriteLine("Error: " + err + '\n');
				stringEntrante = "";
				Main();
			}

			Entrada entry = new Entrada();

			entry.nueva(stringEntrante);
		}
	}
}