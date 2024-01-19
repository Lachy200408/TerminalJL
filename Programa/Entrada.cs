namespace Terminal{
	public static class Entrada{
		private static string stringComando = ""; //Aqui se guardara el string completo
		private static string[] arrayComando = new string[0]; //Aqui se dividira en un array

		public static string[] nueva(string _stringComando){
			stringComando = _stringComando;
			arrayComando = new string[2];

			return conformarArray();
		}

		//Componer el array
		private static string[] conformarArray(){
			if(stringComando.Contains(" ")){
				arrayComando[0] = stringComando.Substring(0, (stringComando.IndexOf(" ")));
				stringComando = stringComando.Substring(stringComando.IndexOf(" ") + 1);
				arrayComando[1] = stringComando;
			}
			else{
				arrayComando[0] = stringComando;
			}

			return arrayComando;
		}
	}
}