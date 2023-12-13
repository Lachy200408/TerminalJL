# Terminal

## -> Objetivos

### []Crear una terminal grafica con estilos propios

### []Lograr que la terminal manipule el texto insertado

### []Introducir funcionalidades y dar soporte

Primero se crea una instancia de windows form que procese las entradas de teclado.
Se muestran en pantalla los caracteres que se vayan escribiendo.
Cuando se presione <strong>enter</strong>, se recoge todo el script y se almacena.

En la clase Entrada se guarda el string insertado y se separa por argumentos que se guardan en una cola.
Luego un objeto de la clase LLamada obtiene los argumentos de la cola para formular una llamada a la funcion correspondiente.
