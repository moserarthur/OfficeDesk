using DAL;
using MetaDados;
using System;
using System.Collections.Generic;


namespace BAL
{
    public class Checker
    {
        /// <summary>
        /// Faz todos os tratamentos do campo preenchido
        /// </summary>
        /// <param name="info"></param>
        /// <returns>String</returns>
        public static string StringCleaner(string info)
        {
            info = info.Replace("R", "").Replace("$", "").Replace(",", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace("-", "");

            return info;
        }

        /// <summary>
        /// Verifica se o campo está preenchido no tamanho adequado
        /// </summary>
        /// <param name="info"></param>
        /// <param name="tamanho"></param>
        /// <returns>Bool</returns>
        public static bool StringChecker(string info, int tamanho)
        {
            return (!string.IsNullOrEmpty(info) && info.Length <= tamanho); // (true)
        }

        /// <summary>
        /// Verifica se o campo preenchido é vazio ou não
        /// </summary>
        /// <param name="campos"></param>
        /// <returns>Bool</returns>
        public static bool CheckerNullOrEmpty(List<string> campos)
        {
            bool vazio = false;

            foreach (var item in campos)
            {
                if (String.IsNullOrEmpty(item))
                {
                    vazio = true;
                }
            }

            return vazio;

        }
    }
}
