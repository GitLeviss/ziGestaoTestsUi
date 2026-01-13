using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zCustodiaUi.utils
{
    using System;
    using System.Linq;

    public enum DocumentType
    {
        Cpf,
        Cnpj
    }

    public static class DataGenerator
    {
        /// <summary>
        /// Generates a valid and formatted Brazilian CPF or CNPJ.
        /// </summary>
        /// <param name="type">The type of document to generate (CPF or CNPJ).</param>
        /// <returns>A formatted string representing the CPF or CNPJ.</returns>
        public static string Generate(DocumentType type)
        {
            return type switch
            {
                DocumentType.Cpf => GenerateCpf(),
                DocumentType.Cnpj => GenerateCnpj(),
                _ => throw new ArgumentException("Invalid document type specified."),
            };
        }

        private static string GenerateCpf()
        {
            Random random = new Random();
            int[] cpfBase = new int[9];

            for (int i = 0; i < 9; i++)
            {
                cpfBase[i] = random.Next(0, 10);
            }

            int firstDigit = CalculateCheckDigit(cpfBase, new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 });
            int secondDigit = CalculateCheckDigit(cpfBase.Append(firstDigit).ToArray(), new int[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });

            string cpf = string.Concat(cpfBase) + firstDigit + secondDigit;
            return FormatCpf(cpf);
        }

        private static string GenerateCnpj()
        {
            Random random = new Random();
            int[] cnpjBase = new int[12];

            for (int i = 0; i < 8; i++)
            {
                cnpjBase[i] = random.Next(0, 10);
            }
            // Filial number (0001 is common)
            cnpjBase[8] = 0;
            cnpjBase[9] = 0;
            cnpjBase[10] = 0;
            cnpjBase[11] = 1;

            int firstDigit = CalculateCheckDigit(cnpjBase, new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });
            int secondDigit = CalculateCheckDigit(cnpjBase.Append(firstDigit).ToArray(), new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });

            string cnpj = string.Concat(cnpjBase) + firstDigit + secondDigit;
            return FormatCnpj(cnpj);
        }

        private static int CalculateCheckDigit(int[] digits, int[] weights)
        {
            int sum = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                sum += digits[i] * weights[i];
            }
            int remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        private static string FormatCpf(string cpf) =>
            $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";

        private static string FormatCnpj(string cnpj) =>
            $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
    }
}
