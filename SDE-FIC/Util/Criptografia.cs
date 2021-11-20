using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SDE_FIC.Util
{
    public class Criptografia
    {
        public static object Rijndael256 { get; private set; }

        /// <summary>
        /// Gerar hash MD5
        /// </summary>
        /// <param name="valor">Valor a ser convertido</param>
        /// <returns>Origem: http://www.devmedia.com.br/criptografia-com-md5-no-dotnet-parte-1/17063 </returns>
        public static string GerarMD5(string valor)
        {
            // Cria uma nova intância do objeto que implementa o algoritmo para
            // criptografia MD5
            MD5 md5Hasher = MD5.Create();

            // Criptografa o valor passado
            byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(valor));

            // Cria um StringBuilder para passarmos os bytes gerados para ele
            StringBuilder strBuilder = new StringBuilder();

            // Converte cada byte em um valor hexadecimal e adiciona ao
            // string builder
            // and format each one as a hexadecimal string.
            for (int i = 0; i < valorCriptografado.Length; i++)
            {
                strBuilder.Append(valorCriptografado[i].ToString("x2"));

            }

            // retorna o valor criptografado como string
            return strBuilder.ToString();
        }

        
    }

    

}