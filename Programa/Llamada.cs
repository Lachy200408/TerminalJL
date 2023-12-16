namespace Terminal{
	public class Llamada{
		//Metodo que va a extraer la informacion util del array
		public static void build(string[] arrayComando){
			string nombreFuncion = arrayComando[0];

			Console.WriteLine(nombreFuncion);
		}
	}
}