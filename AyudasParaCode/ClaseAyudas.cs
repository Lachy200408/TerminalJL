using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyudaParaCode
{
    public class ClaseAyudas
    {
        public Queue<string> GetQueue(string s)
        {
            if (s != ""/* y otras posibles condicionales*/)
            {
                Queue<string> queue = new Queue<string>();
                while(true)
                {
                    int index = s.IndexOf(" ");
                    if (index!=-1)
                    {
                        queue.Enqueue(s.Substring(0,index));
                        s = s.Substring(index+1);
                    }
                    else
                    { 
                        if(s!="")
                            queue.Enqueue(s);
                        break; 
                    }
                }
                return queue;
            }
            else
                throw new Exception("Comando no valido");
        }
    }
}
