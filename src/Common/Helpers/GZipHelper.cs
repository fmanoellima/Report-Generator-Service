using System;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace Common.Helpers
{
    /// <summary>
    /// Classe que contém métodos e propriedades relacionadas a String via GZip.
    /// </summary>
    public static class GZipHelper
    {
        /// <summary>
        /// Método que realiza a compactação de string.
        /// </summary>
        /// <param name="_text">Parâmetro que define a string que será compactada.</param>
        /// <returns>Retorna uma string com o conteúdo compactado.</returns>
        public static string Compress(string _text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(_text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);

            return Convert.ToBase64String(gzBuffer);
        }

        /// <summary>
        /// Método que realiza a descompactação de uma string.
        /// </summary>
        /// <param name="_compressedText">Parâmetro que define a string compactada.</param>
        /// <returns>Retorna uma string com o conteúdo descompactado.</returns>
        public static string Decompress(string _compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(_compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}
