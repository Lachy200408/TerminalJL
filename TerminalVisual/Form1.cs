using System.Collections;
using System.Windows.Forms;
using System.Drawing.Text;

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
            // Asociar el menú contextual con el ícono de la bandeja del sistema
            notifyIcon.ContextMenuStrip = trayMenu;
            // Agregar un elemento al menú contextual
            trayMenu.Items.Add("Opción 1", null, opcion1_Click);

            // Crear un separador
            trayMenu.Items.Add(new ToolStripSeparator());

            // Agregar otro elemento
            trayMenu.Items.Add("Opción 2", null, opcion2_Click);
        }
        private List<string> CodigosAnteriores;
        private string usuario = "jeanmartinez24076";
        private string[] MensajeEntregar= {"Accion ejecutada hola","Todos los comandos captados"};
        public Dictionary<string, Color> Destacadas = new Dictionary<string, Color>
        {
            {"Hola",Color.Green },
            {"hola",Color.Blue },
            {"ola",Color.Red }
        };
        NotifyIcon notifyIcon = new NotifyIcon();

        private string GetLineaActual()
        {
                int currentLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);  
                string currentText = richTextBox1.Lines[currentLine];  
                return currentText;
        }
        private void PutLineaActual(string linea)
        {
            richTextBox1.AppendText(linea);
            Colors();
        }
        private void PutLineas(string[] lineas)
        {
            for (int i = 0; i < lineas.Length; i++)
            {
                richTextBox1.AppendText(lineas[i]);
                Colors();
                richTextBox1.AppendText("\n");
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch ((byte)e.KeyCode)
            {
                case 8://Backspace
                        e.Handled = Editable(0);  // Previene el comportamiento predeterminado del backspace
                    break;
                case 13://Enter
                    if (Editable(0)==false)
                    {
                        string s = ObtenerLineaEjecutable(GetLineaActual());
                        if (s != "")//Hay algo escrito
                        {
                            if (!(CodigosAnteriores.Contains(s)))
                                CodigosAnteriores.Add(s);
                        }
                        PutLineaActual("\n");
                        PutLineas(MensajeEntregar);
                        PutLineaActual("\n");
                        PutLineaActual(usuario + "~");
                    }
                    e.Handled = true;
                    break;
                case 32:
                    break;
                case 38://Fecha Arriba y flecha abajo
                case 40:
                    ArrowsCenter(e);
                    break;
            }
        }
        private void opcion1_Click(object sender, EventArgs e)
        {
            // Acciones al hacer clic en la opción 1
            MessageBox.Show("Opcion 1 ejecutando");
        }

        private void opcion2_Click(object sender, EventArgs e)
        {
            // Acciones al hacer clic en la opción 2
            MessageBox.Show("Opcion 2 ejecutando");
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!(/*(byte)e.KeyChar == 13 ||*/ (byte)e.KeyChar == 8 || (byte)e.KeyChar == 40)) && Editable(1))
            {
                e.Handled = true;
            }
        }
        private void ArrowsCenter(KeyEventArgs e)
        {
            if (CodigosAnteriores.Count > 0 && Editable(1)==false)
            {
                e.Handled=true;
                int lineaIndex = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);//Indice de la linea actual
                int PrimerIndice = richTextBox1.GetFirstCharIndexFromLine(lineaIndex);//Indice global con que inicia la linea
                int InicioEjecutable = PrimerIndice + usuario.Length + 1;// Posicion del inicio de la parte ejecutable
                string LineaActual = ObtenerLineaEjecutable(GetLineaActual());//Obtenemos la linea actual
                int indice = CodigosAnteriores.IndexOf(LineaActual);
                richTextBox1.Select(InicioEjecutable, LineaActual.Length);
                richTextBox1.SelectedText = "";
                if (indice < 0)//Codigo inexistente
                {
                    indice = 0;
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
                }
                PutLineaActual(CodigosAnteriores[indice]);
            }
        }
        private bool Editable(int X)
       {// Evitar la escritura en la plantilla de la terminal una vez definida, int= 0 se llamara en caso de BackSpace e int=1 en cualquier otro caso
            int currentLineIndex = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            int positionInLine = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(currentLineIndex);
            if (currentLineIndex != richTextBox1.Lines.Length - 1)
                return true;//No es editable
            else if (X == 0 && positionInLine <= usuario.Length + 1 && richTextBox1.Lines[currentLineIndex].ToString().IndexOf("~")!=-1)
                return true;
            return false;// Si lo es
        }
        
        
        private string ObtenerLineaEjecutable(string linea)
        {//Metodo que dada la linea de la terminal entrega la parte ejecutable tecleada por el usuario
            int indi = linea.IndexOf("~");
            if(indi!= -1)
            linea = linea.Substring(indi+1);
            return linea;
        }
        private void Colors()
        {//Primero obtenemos el indice inicial de la linea(1), luego deberiamos obtener la string ejecutable de la linea(2)
         //para evaluarla y verificar si existen Palabras destacadas dentro de ella e inmediatamente se detecten las mismas asignarle a cada cual
         // su color correspondiente(3)
            //(1)
            int lineaIndex = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            int PrimerIndice = richTextBox1.GetFirstCharIndexFromLine(lineaIndex);
            int InicioEjecutable = PrimerIndice;
            if (richTextBox1.Lines[lineaIndex].IndexOf("~")!=-1)
            InicioEjecutable += usuario.Length + 1;
            //(2)
            string LineaActual = ObtenerLineaEjecutable(GetLineaActual());
            //(3)
            if (LineaActual != "" && Editable(0)==false )
            {
                int indexLastSpace = LineaActual.LastIndexOf(" "); 
                while (indexLastSpace!=-1)
                {
                    if(LineaActual.Substring(indexLastSpace)!="")
                    {
                        string palabra = LineaActual.Substring(indexLastSpace+1);
                        if (Destacadas.ContainsKey(palabra))
                        {
                            richTextBox1.Select(indexLastSpace+1 + InicioEjecutable, palabra.Length);
                            richTextBox1.SelectionColor = Destacadas[palabra];
                            richTextBox1.Select(PrimerIndice + richTextBox1.Lines[lineaIndex].ToString().Length, 0);
                            richTextBox1.SelectionColor = richTextBox1.ForeColor;
                        }
                        else
                        {
                            richTextBox1.Select(indexLastSpace + 1 + InicioEjecutable, palabra.Length);
                            richTextBox1.SelectionColor = richTextBox1.ForeColor;
                            richTextBox1.Select(PrimerIndice + richTextBox1.Lines[lineaIndex].ToString().Length, 0);
                            richTextBox1.SelectionColor = richTextBox1.ForeColor;
                        }
                        LineaActual = LineaActual.Substring(0, indexLastSpace);
                        indexLastSpace = LineaActual.LastIndexOf(" ");
                    }
                }
                if(Destacadas.ContainsKey(LineaActual))
                {//En este punto linea actual solo contiene la primera palabra
                    richTextBox1.Select(InicioEjecutable, LineaActual.Length);
                    richTextBox1.SelectionColor = Destacadas[LineaActual];
                    richTextBox1.Select(PrimerIndice + richTextBox1.Lines[lineaIndex].ToString().Length, 0);
                    richTextBox1.SelectionColor = richTextBox1.ForeColor;
                }
                else
                {
                    richTextBox1.Select(InicioEjecutable, LineaActual.Length);
                    richTextBox1.SelectionColor = richTextBox1.ForeColor;
                    richTextBox1.Select(PrimerIndice + richTextBox1.Lines[lineaIndex].ToString().Length, 0);
                    richTextBox1.SelectionColor = richTextBox1.ForeColor;
                }
            }
        }
        

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            Colors();
        }
        private void TrayMenuFill()
        {
            InstalledFontCollection installedFontCollection;
            FontFamily[] fontFamilies;
            installedFontCollection = new InstalledFontCollection();
            fontFamilies = installedFontCollection.Families;
            List<string> nombresFuentes = new List<string>();
            foreach (FontFamily fontFamily in fontFamilies)
            {
                nombresFuentes.Add(fontFamily.Name);
            }

        }


}



}
