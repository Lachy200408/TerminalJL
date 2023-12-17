namespace Terminal{
	public class Entrada{
		private string stringComando; //Aqui se guardara el string completo
		private string[] arrayComando; //Aqui se dividira en un array

		//Constructor
		public Entrada(){
			this.stringComando = "";
			this.arrayComando = new string[0];
		}

		public void nueva(string _stringComando){
			this.stringComando = _stringComando;
			this.arrayComando = new string[WhiteSpaces.contar(this.stringComando)+1];

			this.conformarArray();
			this.enviarLlamada();
		}

		//Componer el array
		protected void conformarArray(){
			for(int cont=0; this.stringComando != ""; cont++){
				int indice = this.stringComando.IndexOf(" ");
				if(indice != -1){
					this.arrayComando[cont] = this.stringComando.Substring(0,indice);
					this.stringComando = this.stringComando.Substring(indice+1);
				}
				else{
					this.arrayComando[cont] = this.stringComando;
					break;
				}
			}
		}

		//Enviarle el array a Llamada
		private void enviarLlamada(){
			Executable.llamarPrograma(this.arrayComando);
		}
	}
}