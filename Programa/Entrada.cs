namespace Terminal{
	public class Entrada{
		private string stringComando; //Aqui se guardara el string completo
		private string[] arrayComando; //Aqui se dividira en un array

		//Constructor
		public Entrada(string _stringComando){
			this.stringComando = _stringComando;
			this.arrayComando = new string[WhiteSpaces.contar(this.stringComando)+1];

			this.conformarArray();
			this.enviarLlamada();
		}

		//Componer el array
		protected void conformarArray(){
			string stringAux = "";
			short contador = 0;

			for(short i=0; i<this.stringComando.Length; i++){
				stringAux += this.stringComando[i];

				if(i == stringComando.Length-1 || this.stringComando[i+1] == ' '){
					this.arrayComando[contador] = stringAux;
					stringAux = "";
					contador++;
				}
			}
		}

		//Enviarle el array a Llamada
		private void enviarLlamada(){
			Executable.llamarPrograma(this.arrayComando);
		}
	}
}