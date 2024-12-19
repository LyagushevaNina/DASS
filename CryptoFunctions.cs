using System;

namespace Client
{
    public class CryptoFunctions
    {
        public static long Pow(long a, int b, int mod)
        {
            long result = 1;
            for (int i = 1; i <= b; i++)
            {
                result *= a;
                result %= mod;
            }

            return result;
        }
        public static int Gcd(int a, int b)
        {
            int tmp;
            while (b != 0)
            {
                tmp = b;
                b = a % b;
                a = tmp;
            }

            return a;
        }
        public static int Reverse_El(int a, int b)
        {
            int b0 = 1, b1 = 0, a0 = 0, a1 = 1, mod = b;
            int r, q, t;
            while (true)
            {
                q = b / a;
                r = b - (a * q);
                if (r == 0)
                {
                    if (a1 < 0)
                    {
                        a1 += mod;
                    }

                    return a1; // d = a * a1 + b * b1
                }

                b = a;
                a = r;
                t = b0;
                b0 = b1;
                b1 = t - (b1 * q);
                t = a0;
                a0 = a1;
                a1 = t - (a1 * q);
            }
        }
        public static int GeneratePrimeNumber(int n, int k)
        {
            Random rnd = new Random();
            while (true)
            {
                int number = 0;
                while (number % 2 == 0 || (number + 1) % 4 != 0)
                {
                    number = rnd.Next((int)Math.Pow(2, n - 1), (int)Math.Pow(2, n));
                }

                int tmp = number - 1;
                int d = 1;
                while (tmp % 2 == 0)
                {
                    tmp /= 2;
                    d *= 2;
                }

                d = (number - 1) / d;

                bool prime = true;
                for (int i = 0; i < k; i++)
                {
                    if (!MillerTest(d, number))
                    {
                        prime = false;
                        break;
                    }
                }

                if (prime)
                {
                    return number;
                }
            }
        }

        private static bool MillerTest(int d, int number)
        {
            Random rnd = new Random();
            int a = rnd.Next(2, number - 2);
            int x = (int)Pow(a, d, number);
            if (x == 1 || x == number - 1)
            {
                return true;
            }

            while (d != number - 1)
            {
                x = x * x % number;
                d *= 2;

                if (x == 1)
                {
                    return false;
                }

                if (x == number - 1)
                {
                    return true;
                }
            }

            return false;
        }

        public static string XORCipher(string message, int key)
        {
            char[] char_msg = message.ToCharArray();
            char[] char_key = ("" + key).ToCharArray();
            string result = "";

            for (int i = 0; i < message.Length; i++)
            {
                char_msg[i] ^= char_key[i % char_key.Length];
                result += char_msg[i];
            }

            return result;
        }

        public static string[] GetArrAnswer(string msg)
        {
            string[] result = msg.Split('$');
            return result;
        }
    }
}
