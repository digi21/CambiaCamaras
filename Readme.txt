Programa para solucionar problemas ocasionados por restituir un modelo fotogramétrico con un archivo de aerotriangulación o un archivo de calibración de cámara incorrectos.

Requiere que pasemos los siguientes parámetros:

1. Archivo de aerotriangulación de entrada.
2. Archivo de aerotriangulación de salida.
3. Nombre del modelo fotogramétrico (formado por nombre de foto izquierda, guión y nombre de foto derecha)
4. Nombre del archivo bind mal restituido.
5. Nombre del archivo bind a generar.
6. Valor umbral por encima del cual el programa muestra un mensaje de error indicando el número de entidad cuyo valor de paralaje supera dicho umbral.

Un ejemplo de línea de comandos sería:

CambiaCamaras "Calarca Malo.prj" "Calarca Bueno.prj" "18047-18048" "COLOMBIA_1a.bind" "nuevo.bind" 2

