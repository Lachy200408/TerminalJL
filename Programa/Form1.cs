namespace Terminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text = usuario+ "~";
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;
            CodigosAnteriores = new List<string>(); 
        }
        private List<string> CodigosAnteriores;
        private string usuario = "jeanmartinez24076";
        private string GetLineaActual()
        {
                int currentLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);  
                string currentText = richTextBox1.Lines[currentLine];  
                return currentText;
        }
        private void PutLineaActual(string linea,bool reemplazando, object sender, KeyEventArgs e)
        {
            e.Handled = true;
            int selectionStart = richTextBox1.SelectionStart;  // Obtenemos la posici�n del cursor
            int lineIndex = richTextBox1.GetLineFromCharIndex(selectionStart);  // Obtenemos el �ndice de la l�nea actual
            int firstCharIndex = richTextBox1.GetFirstCharIndexFromLine(lineIndex);  // Obtenemos el �ndice del primer car�cter de la l�nea
            string currentLine = (richTextBox1.Lines.Length >= 1)? richTextBox1.Lines[lineIndex] : "";  // Obtenemos el texto de la l�nea actual

            // Agregamos "linea" al final de la l�nea actual(si reemplazando = false) o reemplazamos la linea actual(si es true)
            if (reemplazando)
                currentLine = linea;
            else
                currentLine += linea;

            // Reemplazamos la l�nea actual con la versi�n modificada
            richTextBox1.Select(firstCharIndex, richTextBox1.Lines[lineIndex].Length);  // Seleccionamos la l�nea actual
            richTextBox1.SelectedText = currentLine;  // Reemplazamos el texto de la l�nea seleccionada

            // Movemos el cursor al final de la l�nea modificada
            richTextBox1.SelectionStart = firstCharIndex + currentLine.Length;
            richTextBox1.SelectionLength = 0;
        }
        //private void PutLineas(string[] lineas, object sender, KeyEventArgs e)
        //{
        //    e.Handled = true;
        //    int selectionStart = richTextBox1.SelectionStart;  // Obtenemos la posici�n del cursor
        //    int lineIndex = richTextBox1.GetLineFromCharIndex(selectionStart);  // Obtenemos el �ndice de la l�nea actual
        //    int firstCharIndex = richTextBox1.GetFirstCharIndexFromLine(lineIndex);  // Obtenemos el �ndice del primer car�cter de la l�nea
        //    string currentLine = richTextBox1.Lines[lineIndex];  // Obtenemos el texto de la l�nea actual

        //    // Agregamos "linea" al final de la l�nea actual

        //    // Reemplazamos la l�nea actual con la versi�n modificada
        //    richTextBox1.Select(firstCharIndex, richTextBox1.Lines[lineIndex].Length);  // Seleccionamos la l�nea actual
        //    richTextBox1.SelectedText = currentLine;  // Reemplazamos el texto de la l�nea seleccionada

        //    // Movemos el cursor al final de la l�nea modificada
        //    richTextBox1.SelectionStart = firstCharIndex + currentLine.Length;
        //    richTextBox1.SelectionLength = 0;
        //}

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch ((byte)e.KeyCode)
            {
                case 8://Backspace
                        e.Handled = Editable();  // Previene el comportamiento predeterminado del backspace
                    break;
                case 13://Enter
                    string s = ObtenerLineaEjecutable(GetLineaActual());

                    if(s == "clear"){
                        richTextBox1.Text = usuario + "~";
                        PutLineaActual("",false, sender, e);
                    }
                    else if (s != "")//Hay algo escrito
                    {
                        if (!(CodigosAnteriores.Contains(s)))
                            CodigosAnteriores.Add(s);

                        Entrada.nueva(s); // A partir de aqui se ejecutan las instrucciones

                        PutLineaActual("\n",false, sender, e);
                        PutLineaActual(Salida.message, false, sender, e);

                        PutLineaActual("\n",false, sender, e);
                        PutLineaActual(usuario + "~",false, sender, e);
                    }
                    else
                    {
                        PutLineaActual("\n",false, sender, e);
                        PutLineaActual(usuario + "~",false, sender, e);
                    }
                    
                    break;
                case 38:
                case 40:
                    if (CodigosAnteriores.Count > 0)    
                    {
                        int indice = CodigosAnteriores.IndexOf(ObtenerLineaEjecutable(GetLineaActual()));
                        if (indice < 0)//Codigo inexistente
                        {
                            PutLineaActual(usuario + "~" + CodigosAnteriores[0], true, sender, e);
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
                            PutLineaActual(usuario + "~" + CodigosAnteriores[indice], true, sender, e);
                        }
                    }
                    break;
                default:
                    break;
            }
            
            
        }

        private bool Editable()
       {// Evitar la escritura en la plantilla de la terminal una vez definida 
            int currentLineIndex = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            int positionInLine = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(currentLineIndex);
            if (positionInLine <= usuario.Length+1|| currentLineIndex!= richTextBox1.Lines.Length-1)
                return true;//No es editable
            return false;// Si lo es
        }
        private string ObtenerLineaEjecutable(string linea)
        {//Metodo que dada la linea de la terminal entrega la parte ejecutable tecleada por el usuario
            int indi = linea.IndexOf("~");
            linea = linea.Substring(indi+1);
            return linea;
        }
    }



}
