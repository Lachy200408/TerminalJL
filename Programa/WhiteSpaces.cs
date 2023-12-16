namespace Terminal{
	public class WhiteSpaces{
		public static int contar(string cadena){
			int cont=0;
			for(short i=0; i<cadena.Length-1; i++){
				if(cadena[i]==' '){
					cont++;
				}
			}
			return cont;
		}
	}
}