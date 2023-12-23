namespace Terminal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int caracteres=0;        

        private string GetLineaActual()
        {
                int currentLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);  
                string currentText = richTextBox1.Lines[currentLine];  
                return currentText;
        }
        private void PutLineaActual(string linea)
        {
            int currentLine = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            richTextBox1.Text = linea;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch ((byte)e.KeyCode)
            {
                case 13:
                    caracteres = 0;
                    
                    Entrada.nueva(GetLineaActual());
                    PutLineaActual(Salida.message);

                    break;
                case 8:
                    if ( caracteres == 0)
                    {
                        e.Handled = true;  // Previene el comportamiento predeterminado del backspace
                    }
                    else
                        caracteres--;
                    break;
                default:
                    if(!((e.KeyCode == Keys.Shift)|| (e.KeyCode == Keys.CapsLock)||(e.KeyCode == Keys.Escape) || (e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up) || (e.KeyCode == Keys.Left) || (e.KeyCode == Keys.Right) || (e.KeyCode == Keys.Control) || (e.KeyCode == Keys.Alt)))
                    caracteres++;
                    break;
            }
            
            
        }
    }



}
