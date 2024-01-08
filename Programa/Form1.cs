namespace Terminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text = usuarioPC() + ":" + pathActual() + ">";
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            CodigosAnteriores = new List<string>();
        }
        private List<string> CodigosAnteriores;
        private string GetLineaActual()
        {
                string currentText = richTextBox1.Lines[richTextBox1.Lines.Length-1];  
                return currentText;
        }
        private void PutLineaActual(string linea,bool reemplazando, object sender, KeyEventArgs e)
        {
            e.Handled = true;
            int firstCharIndex = richTextBox1.GetFirstCharIndexFromLine(richTextBox1.Lines.Length-1);  // Obtenemos el �ndice del primer car�cter de la l�nea
            string currentLine = (richTextBox1.Lines.Length >= 1)? richTextBox1.Lines[richTextBox1.Lines.Length-1] : "";  // Obtenemos el texto de la l�nea actual

            // Agregamos "linea" al final de la l�nea actual(si reemplazando = false) o reemplazamos la linea actual(si es true)
            if (reemplazando)
                currentLine = linea;
            else
                currentLine += linea;

            // Reemplazamos la l�nea actual con la versi�n modificada
            richTextBox1.Select(firstCharIndex, richTextBox1.Lines[richTextBox1.Lines.Length-1].Length);  // Seleccionamos la l�nea actual
            richTextBox1.SelectedText = currentLine;  // Reemplazamos el texto de la l�nea seleccionada

            // Movemos el cursor al final de la l�nea modificada
            richTextBox1.SelectionStart = firstCharIndex + currentLine.Length;
            richTextBox1.SelectionLength = 0;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch ((byte)e.KeyCode)
            {
                case 8://Backspace
                    e.Handled = !Editable(true);  // Previene el comportamiento predeterminado del backspace
                    break;
                case 13://Enter
                    string s = ObtenerLineaEjecutable(GetLineaActual());

                    if (s!="" && s!="clear" && CadenaEspacios(s.Length)!=s)//Hay algo escrito
                    {
                        string[] arrayComando = Entrada.nueva(s); // Se obtiene el array
                        
                        Executable.llamarPrograma(arrayComando, this, sender, e); // Se llama al programa

                        PutLineaActual("\n",false, sender, e);
                        while(Salida.message.Count >= 1) PutLineaActual(Salida.message.Dequeue()+"\n", false, sender, e);
                    }
                    
                    if(s == "clear"){
                        richTextBox1.Text = usuarioPC() + ":" + pathActual() + ">";
                        PutLineaActual("",false, sender, e);
                    }
                    else
                    {
                        PutLineaActual("\n",false, sender, e);
                        PutLineaActual(usuarioPC() + ":" + pathActual() + ">",false, sender, e);
                    }

                    if (!(CodigosAnteriores.Contains(s)) && s!="" && CadenaEspacios(s.Length)!=s)
                        CodigosAnteriores.Add(s);
                    
                    break;
                case 38:
                case 40:
                    e.Handled = true;
                    if (CodigosAnteriores.Count>0 && Editable())    
                    {
                        int indice = CodigosAnteriores.IndexOf(ObtenerLineaEjecutable(GetLineaActual()));
                        if (indice < 0)//Codigo inexistente
                        {
                            PutLineaActual(usuarioPC() + ":" + pathActual() + ">" + CodigosAnteriores[0], true, sender, e);
                        }
                        else
                        if (CodigosAnteriores.Count > 1)
                        {
                            if (e.KeyCode == Keys.Up)
                                indice--;
                            else
                                indice++;
                            if (indice < 0)
                                indice = CodigosAnteriores.Count - 1;
                            else
                                if (indice > CodigosAnteriores.Count - 1)
                                indice = 0;
                            PutLineaActual(usuarioPC() + ":" + pathActual() + ">" + CodigosAnteriores[indice], true, sender, e);
                        }
                    }
                    break;
                default:
                    if(!Editable()){
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    break;
            }
        }

        private bool Editable(bool backspace=false)
       {// Evitar la escritura en la plantilla de la terminal una vez definida
            int extra = (backspace)? 2 : 1; 
            int currentLineIndex = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            int positionInLine = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(currentLineIndex);
            if (richTextBox1.Lines.Length>1 && (positionInLine <= usuarioPC().Length + pathActual().Length + extra) || currentLineIndex != richTextBox1.Lines.Length-1){
                return false;//No es editable
            }
            return true;// Si lo es
        }
        private string ObtenerLineaEjecutable(string linea)
        {//Metodo que dada la linea de la terminal entrega la parte ejecutable tecleada por el usuarioPC()
            int indi = linea.IndexOf(">");
            linea = linea.Substring(indi+1);
            return linea;
        }
        private string CadenaEspacios(int length){
            return (length == 1)? " " : " " + CadenaEspacios(length-1);
        }
        private string usuarioPC(){
            return Environment.UserName + "@" + Environment.MachineName;
        }
        private string pathActual(){
            return (SistemaOperativo.home() == Directorio.actual())? "~" : Directorio.actual();
        }
    }
}
