using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Digi21.Digi3D.Sensors.ConicSensor;
using Digi21.Math;
using System.IO;
using Digi21.DigiNG.DigiTab;
using Digi21.DigiNG.IO;
using Digi21.DigiNG.Entities;

namespace CambiaCamaras
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("CambiaCámaras v1.0\n");

            if (CompruebaParámetros(args))
            {
                TransformaArchivo(args);
            }

            Console.WriteLine("Archivo transformado satisfactoriamente.");
        }

        private static bool CompruebaParámetros(string[] args)
        {
            if (args.Length < 6)
            {
                Console.Error.WriteLine("Error: No se ha especificado correctamente el número de parámetros.");
                Console.Error.WriteLine();
                Console.Error.WriteLine("CambiaCamaras [archivo de orientaciones original] [archivo de orientaciones nuevo] [nombre de modelo] [archivo .bind a traducir] [archivo .bind a crear] [valor del paralaje para mostrar alarmas]");
                return false;
            }

            if (File.Exists(args[4]))
            {
                Console.Error.WriteLine("Error: El archivo {0} ya existe.",
                    args[4]);
                return false;
            }

            return true;
        }
        private static void TransformaArchivo(string[] args)
        {
            try
            {
                ConicSensor sensorOriginal = new ConicSensor();
                sensorOriginal.Load(args[0], args[2], false);

                ConicSensor sensorNuevo = new ConicSensor();
                sensorNuevo.Load(args[1], args[2], false);

                double paralajeAlarma = double.Parse(args[5]);

                using (BinDouble archivoOriginal = new BinDouble(args[3]))
                {
                    using (BinDouble archivoNuevo = new BinDouble(args[4], 0, true, true))
                    {
                        int índiceEntidad = 0;
                        foreach (var entidad in archivoOriginal)
                        {
                            double paralajeEntidad;
                            var entidadTransformada = Transforma(sensorOriginal, sensorNuevo, entidad, out paralajeEntidad);
                            archivoNuevo.Add(entidadTransformada);

                            if (Math.Abs(paralajeEntidad) >= paralajeAlarma)
                            {
                                Console.WriteLine(string.Format("{0}\t{1}"), 
                                    índiceEntidad,
                                    paralajeEntidad);
                            }
                        }
                    }
                }

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static Entity Transforma(ConicSensor sensorOriginal, ConicSensor sensorNuevo, Entity entidad, out double paralajeMáximo)
        {
            if (entidad is ReadOnlyLine)
            {
                ReadOnlyLine línea = entidad as ReadOnlyLine;
                Line nueva = línea.Clone();

                paralajeMáximo = double.MinValue;
                for (int i = 0; i < nueva.Points.Count; i++)
                {
                    Point2D[] coordenadas = sensorOriginal.ModelToPixel(nueva.Points[i]);
                    ModelParalax modelo = sensorNuevo.PixelToModel(coordenadas);

                    paralajeMáximo = Math.Max(paralajeMáximo, modelo.Paralax);
                    nueva.Points[i] = modelo.Model;
                }

                return nueva;
            }

            if (entidad is ReadOnlyPoint)
            {
                ReadOnlyPoint punto = entidad as ReadOnlyPoint;
                Point nuevo = punto.Clone();

                Point2D[] coordenadas = sensorOriginal.ModelToPixel(punto.Coordinate);
                ModelParalax modelo = sensorOriginal.PixelToModel(coordenadas);
                nuevo.Coordinate = modelo.Model;
                paralajeMáximo = modelo.Paralax;
                return nuevo;
            }

            if (entidad is ReadOnlyText)
            {
                ReadOnlyText texto = entidad as ReadOnlyText;
                Text nuevo = texto.Clone();

                Point2D[] coordenadas = sensorOriginal.ModelToPixel(texto.Coordinate);
                ModelParalax modelo = sensorOriginal.PixelToModel(coordenadas);
                nuevo.Coordinate = modelo.Model;
                paralajeMáximo = modelo.Paralax;
                return nuevo;
            }

            throw new ArgumentException(string.Format("No se pueden transformar entidades de tipo {0}", entidad.GetType().ToString()));
        }
    }
}
