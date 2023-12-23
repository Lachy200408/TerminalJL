namespace Terminal{
	public static class Entrada{
		private static string stringComando = ""; //Aqui se guardara el string completo
		private static string[] arrayComando = new string[1]; //Aqui se dividira en un array

		public static void nueva(string _stringComando){
			stringComando = _stringComando;
			arrayComando = new string[WhiteSpaces.contar(stringComando)+1];

			conformarArray();
			enviarLlamada();
		}

		//Componer el array
		private static void conformarArray(){
			for(int cont=0; stringComando != ""; cont++){
				int indice = stringComando.IndexOf(" ");
				if(indice != -1){
					arrayComando[cont] = stringComando.Substring(0,indice);
					stringComando = stringComando.Substring(indice+1);
				}
				else{
					arrayComando[cont] = stringComando;
					break;
				}
			}
		}

		//Enviarle el array a Llamada
		private static void enviarLlamada(){
			Executable.llamarPrograma(arrayComando);
		}
	}
}