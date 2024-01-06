using System.Collections;
namespace TerminalVisual
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
        private string[] MensajeEntregar= {"Accion ejecutada","Todos los comandos captados"};
        public Dictionary<string, Color> Destacadas = new Dictionary<string, Color>
        {
            {"Hola",Color.Green },
            {"hola",Color.Blue },
            {"ola",Color.Red }
        };


        private string GetLineaActual()
        {
                int currentLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);  
                string currentText = richTextBox1.Lines[currentLine];  
                return currentText;
        }
        private void PutLineaActual(string linea,bool reemplazando, object sender, KeyEventArgs e)
        {
            e.Handled = true;
            int selectionStart = richTextBox1.SelectionStart;  // Obtenemos la posición del cursor
            int lineIndex = richTextBox1.GetLineFromCharIndex(selectionStart);  // Obtenemos el índice de la línea actual
            int firstCharIndex = richTextBox1.GetFirstCharIndexFromLine(lineIndex);  // Obtenemos el índice del primer carácter de la línea
            string currentLine = richTextBox1.Lines[lineIndex];  // Obtenemos el texto de la línea actual

            // Agregamos "linea" al final de la línea actual(si reemplazando = false) o reemplazamos la linea actual(si es true)
            if (reemplazando)
                currentLine = linea;
            else
                currentLine += linea;

            // Reemplazamos la línea actual con la versión modificada
            richTextBox1.Select(firstCharIndex, richTextBox1.Lines[lineIndex].Length);  // Seleccionamos la línea actual
            richTextBox1.SelectedText = currentLine;  // Reemplazamos el texto de la línea seleccionada

            // Movemos el cursor al final de la línea modificada
            richTextBox1.SelectionStart = firstCharIndex + currentLine.Length;
            richTextBox1.SelectionLength = 0;
        }
        private void PutLineas(string[] lineas, object sender, KeyEventArgs e)
        {
            e.Handled = true;
            int selectionStart = richTextBox1.SelectionStart;  // Obtenemos la posición del cursor
            int lineIndex = richTextBox1.GetLineFromCharIndex(selectionStart);  // Obtenemos el índice de la línea actual
            int firstCharIndex = richTextBox1.GetFirstCharIndexFromLine(lineIndex);  // Obtenemos el índice del primer carácter de la línea
            string currentLine = richTextBox1.Lines[lineIndex];  // Obtenemos el texto de la línea actual

            // Agregamos las lineas al final de la línea actual
            for(int i = 0; i < lineas.Length; i++)
                currentLine+= lineas[i]+"\n";
            // Reemplazamos la línea actual con la versión modificada
            richTextBox1.Select(firstCharIndex, richTextBox1.Lines[lineIndex].Length);  // Seleccionamos la línea actual
            richTextBox1.SelectedText = currentLine;  // Reemplazamos el texto de la línea seleccionada

            // Movemos el cursor al final de la línea modificada
            richTextBox1.SelectionStart = firstCharIndex + currentLine.Length;
            richTextBox1.SelectionLength = 0;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch ((byte)e.KeyCode)
            {
                case 8://Backspace
                        e.Handled = Editable(0);  // Previene el comportamiento predeterminado del backspace
                    break;
                case 13://Enter
                    string s = ObtenerLineaEjecutable(GetLineaActual());
                    if (s != "")//Hay algo escrito
                    {
                        if (!(CodigosAnteriores.Contains(s)))
                            CodigosAnteriores.Add(s);
                    }
                    PutLineaActual("\n",false, sender, e);
                    PutLineas(MensajeEntregar,sender, e);
                    PutLineaActual(usuario + "~",false, sender, e);
                    break;
                case 32:
                    //ChangeColorOfLastWord();
                    break;
                case 38://Fecha Arriba y flecha abajo
                case 40:
                    e.Handled = true;
                    if (CodigosAnteriores.Count > 0|| Editable(1))    
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
            }
            
            
        }

        private bool Editable(int X)
       {// Evitar la escritura en la plantilla de la terminal una vez definida, int= 0 se llamara en caso de BackSpace e int=1 en cualquier otro caso 
            int currentLineIndex = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            int positionInLine = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(currentLineIndex);
            if (currentLineIndex != richTextBox1.Lines.Length - 1)
                return true;//No es editable
            else if (X == 0 && positionInLine <= usuario.Length + 1)
                return true;
            return false;// Si lo es
        }
        private string ObtenerLineaEjecutable(string linea)
        {//Metodo que dada la linea de la terminal entrega la parte ejecutable tecleada por el usuario
            int indi = linea.IndexOf("~");
            linea = linea.Substring(indi+1);
            return linea;
        }
        private void PalabrasDestacadas()
        {//Trabaja la linea actual y da color a las palabras que se considero
            string s = ObtenerLineaEjecutable(GetLineaActual());
            s= s.Trim();
            if(s != null&& s!="")
            {
                int indi = -1;
                int p = s.IndexOf(" ");
                if (p != -1)
                {
                    indi = p;
                    while (p != -1)
                    {
                        indi = p;
                        p = s.Substring(indi + 1).IndexOf(" ");
                    }
                }
                s = s.Substring(indi+1);
                if (Destacadas.ContainsKey(s))
                {
                    richTextBox1.Select(indi + usuario.Length - 1, s.Length+ usuario.Length-1 );
                    // Aplicar un color al texto seleccionado
                    richTextBox1.SelectionColor = Destacadas[s];
                }
            }

        }
        private void ChangeColorOfLastWord()
        {
            string text = richTextBox1.Text;
            string[] words = text.Split(' ');
            if (words.Length >= 1)
            {
                string lastWord = words[words.Length - 1]; // -2 because splitting by space adds an empty string at the end
                int startIndex = text.LastIndexOf(lastWord);
                if (startIndex != -1)
                {
                    richTextBox1.Select(startIndex, lastWord.Length);
                    richTextBox1.SelectionColor = Color.Blue;
                    // Reset the selection color to the default after a brief delay
                    Task.Delay(1000).ContinueWith(t =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            richTextBox1.SelectionColor = richTextBox1.ForeColor;
                        });
                    });
                }
            }
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((!((byte)e.KeyChar == 13 || (byte)e.KeyChar == 8 || (byte)e.KeyChar == 40)) && Editable(1))
                e.Handled = true;
        }
    }



}
