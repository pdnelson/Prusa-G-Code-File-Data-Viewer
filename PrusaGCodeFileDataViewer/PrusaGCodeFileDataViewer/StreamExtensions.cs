using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrusaGCodeFileDataViewer
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Checks if there is another byte at the end of the file.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>False if no byte, true if byte.</returns>
        public static bool Peek(this Stream stream)
        {
            if (stream.ReadByte() < 0) return false;
            stream.Seek(stream.Position - 2, SeekOrigin.Current);
            return true;
        }

        public static string ReadLine(this Stream stream)
        {
            bool hasHitNewLine = false;
            StringBuilder sb = new StringBuilder();
            byte[] b = new byte[1];

            while(!hasHitNewLine)
            {
                if (stream.Peek())
                {
                    stream.Read(b, 0, 1);
                    if ((char)b[0] == '\n')
                    {
                        hasHitNewLine = true;
                    }
                    else if ((char)b[0] == '\r')
                    {
                        stream.Read(b, 0, 1);
                        hasHitNewLine = true;
                    }
                    else if (stream.Peek())
                    {
                        sb.Append(Encoding.ASCII.GetChars(b));
                    }
                    else hasHitNewLine = true;
                }

                // We have reached the end of the file
                else hasHitNewLine = true;
            }

            return sb.ToString();
        }
    }
}
