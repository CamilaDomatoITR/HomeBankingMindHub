using System;
using System.Collections.Generic;

namespace HomeBankingMindHub.Models.Utils
{
    public static class RandomGenerator
    {
        private static readonly Random _random = new Random();

        // Método para generar números de tarjeta únicos
        public static string GenerateCardNumber()
        {
            // Puedes definir un formato específico para los números de tarjeta
            // Por ejemplo, un formato de 16 dígitos como "XXXX-XXXX-XXXX-XXXX"
            string format = "XXXX-XXXX-XXXX-XXXX";

            // Genera números aleatorios hasta que se encuentre uno único
            string cardNumber;
            do
            {
                cardNumber = GenerateRandomNumber(format);
            } while (!IsUniqueCardNumber(cardNumber));

            return cardNumber;
        }

        // Método para generar números de cuenta únicos
        public static string GenerateAccountNumber()
        {
            // Puedes definir un formato específico para los números de cuenta
            // Por ejemplo, un formato de 10 dígitos
            string format = "XXXX-XXXX-XX";

            // Genera números aleatorios hasta que se encuentre uno único
            string accountNumber;
            do
            {
                accountNumber = GenerateRandomNumber(format);
            } while (!IsUniqueAccountNumber(accountNumber));

            return accountNumber;
        }

        // Método para generar números aleatorios según un formato dado
        private static string GenerateRandomNumber(string format)
        {
            char[] result = new char[format.Length];
            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == 'X')
                {
                    // Si el formato tiene 'X', se reemplaza con un dígito aleatorio
                    result[i] = (char)(_random.Next(0, 10) + '0');
                }
                else
                {
                    // Si el formato no es 'X', se mantiene el carácter original
                    result[i] = format[i];
                }
            }
            return new string(result);
        }

        // Método para verificar si un número de tarjeta ya existe en la lista de tarjetas
        private static bool IsUniqueCardNumber(string cardNumber)
        {
            // Aquí deberías tener acceso a tu base de datos o a una lista de números de tarjeta existentes
            // En este ejemplo, utilizaremos una lista simulada
            List<string> existingCardNumbers = new List<string> { "1111-1111-1111-1111", "2222-2222-2222-2222", "3333-3333-3333-3333" };

            // Verifica si el número de tarjeta generado ya existe en la lista
            return !existingCardNumbers.Contains(cardNumber);
        }

        // Método para verificar si un número de cuenta ya existe en la lista de cuentas
        private static bool IsUniqueAccountNumber(string accountNumber)
        {
            // Aquí deberías tener acceso a tu base de datos o a una lista de números de cuenta existentes
            // En este ejemplo, utilizaremos una lista simulada
            List<string> existingAccountNumbers = new List<string> { "VIN-123456", "VIN-789012", "VIN-345678" };

            // Verifica si el número de cuenta generado ya existe en la lista
            return !existingAccountNumbers.Contains(accountNumber);
        }

        // Genera un CVV (Card Verification Value) aleatorio de 3 dígitos
        public static int GenerateRandomCvv()
        {
            // Genera un número aleatorio de tres dígitos para el CVV
            int cvv = _random.Next(100, 1000); // Rango de 100 a 999 (tres dígitos)
            return cvv;
        }
    }
}
