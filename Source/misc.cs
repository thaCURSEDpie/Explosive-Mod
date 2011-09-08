using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTA;
using System.IO;

namespace terrorNiko
{
    /// <summary>
    /// Class with some handy functions
    /// </summary>
    class handyFunctions
    {
        public static byte[] loadFile(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();
            return ImageData; //return the byte data
        }

        /// <summary>
        /// Gets the angle between two 3D vectors
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second vector</param>
        /// <returns>Angle in radians</returns>
        public static double getAngleBetweenVectors(Vector3 vector1, Vector3 vector2)
        {
            return Math.Acos((double)(GTA.Vector3.Dot(vector1, vector2) / (vector1.Length() * vector2.Length())));
        }

        /// <summary>
        /// Gets the angle between two 2D vectors
        /// </summary>
        /// <param name="vector1">First vector</param>
        /// <param name="vector2">Second Vector</param>
        /// <returns>Angle in radians</returns>
        public static double getAngleBetweenVectors(Vector2 vector1, Vector2 vector2)
        {
            return Math.Acos((double)(GTA.Vector2.Dot(vector1, vector2) / (vector1.Length() * vector2.Length())));
        }

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="degree">Angle in degrees</param>
        /// <returns>Angle in radians</returns>
        public static float degToRad(float degree)
        {
            return (float)(degree / 180 * Math.PI);
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="rad">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static float radToDeg(float rad)
        {
            return (float)(rad / Math.PI * 180);
        }
    }
}
