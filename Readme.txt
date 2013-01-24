Programa para solucionar problemas ocasionados por restituir un modelo fotogram�trico con un archivo de aerotriangulaci�n o un archivo de calibraci�n de c�mara incorrectos.

Requiere que pasemos los siguientes par�metros:

1. Archivo de aerotriangulaci�n de entrada.
2. Archivo de aerotriangulaci�n de salida.
3. Nombre del modelo fotogram�trico (formado por nombre de foto izquierda, gui�n y nombre de foto derecha)
4. Nombre del archivo bind mal restituido.
5. Nombre del archivo bind a generar.
6. Valor umbral por encima del cual el programa muestra un mensaje de error indicando el n�mero de entidad cuyo valor de paralaje supera dicho umbral.

Un ejemplo de l�nea de comandos ser�a:

CambiaCamaras "Calarca Malo.prj" "Calarca Bueno.prj" "18047-18048" "COLOMBIA_1a.bind" "nuevo.bind" 2

