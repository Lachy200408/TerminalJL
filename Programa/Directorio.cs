namespace Terminal{
	public class Directorio{
		//Metodo que devuelve el directorio actual
		public static string actual(){
			return Path.GetFullPath(".");
		}

		//Metodo que devuelve el path de las funciones en el directorio actual
		protected static string actualFunctions(){
			return Directorio.actual() + SistemaOperativo.barra() + "Functions" + SistemaOperativo.barra();
		}
	}
}