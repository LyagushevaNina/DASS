using System;
using System.Collections;
using System.Numerics;
using System.Text;

namespace Client
{
    public class DASSRabinCryptography
    {
        public static int[] GenerateKeys(int s)
        {
            int[] result = new int[4];
            int p = CryptoFunctions.GeneratePrimeNumber(s, 10);
            int q = CryptoFunctions.GeneratePrimeNumber(s, 11);
            int n = p * q;
            Random rnd = new Random();
            int b = rnd.Next(1, n);

            result[0] = n;
            result[1] = b;
            result[2] = p;
            result[3] = q;

            return result;
        }

        public static int Hash(string M)
        {
            byte[] byteM = Encoding.Default.GetBytes(M);
            BitArray bitM = new BitArray(byteM);
            bool[] arr = new bool[bitM.Length + 5];
            bitM.CopyTo(arr, 5);

            for (int i = arr.Length - 1; i >= 5; i--)
            {
                if (arr[i])
                {
                    arr[i] ^= true;
                    arr[i - 2] ^= true;
                    arr[i - 5] ^= true;
                }
            }

            int[] array = new int[1];
            BitArray tmp = new BitArray(5);
            for (int i = 4; i >= 0; i--)
            {
                tmp.Set(i, arr[i]);
            }

            tmp.CopyTo(array, 0);
            return array[0];
        }

        public static string[] Signature(string M, int n, int b)
        {
            string[] result = new string[2];
            int k, u = -1, temp, d = -1, x = -1;
            int Mhash = Hash(M);
            while (d < 0)
            {
                Random Rand = new Random();
                u = Rand.Next(2, b);
                k = Mhash * u % n;
                temp = CryptoFunctions.Reverse_El(k, n);
                d = (b * b) - (4 * -1 * temp);
            }

            x = (int)(((-1 * b) + Math.Sqrt(d)) / 2);

            result[0] = "" + u;
            result[1] = "" + x;
            return result;
        }

        public static bool Verification(string M, int u, int x, int n, int b)
        {
            int Mhash = Hash(M);
            int tmp1 = x * (x + b) % n;
            int tmp2 = Mhash * u % n;
            tmp1 = tmp2;
            if (tmp1 == tmp2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string Encryption(string text, int N)
        {
            char[] char_text = text.ToCharArray();
            string result = "";

            for (int k = 0; k < text.Length; k++)
            {
                string block = "";

                block += (int)char_text[k] + "00";
                // 00 - обозначение верного варианта расшифрования

                long tmp = Int32.Parse(block);
                tmp = tmp * tmp % N; // выполняем преобразование C = M^M mod N.

                if (k != text.Length - 1)
                {
                    result += tmp + "\n"; // разделяем блоки символом переноса строки (его пользователь
                }
                // ввести всё равно в чате не может)
                else
                {
                    result += tmp;
                }
            }

            return result;
        }
        public static string Decryption(string ciphertext, int q, int p, int N)
        {
            char[] char_text = ciphertext.ToCharArray();
            string result = "";
            string[] answer = ciphertext.Split('\n');

            BigInteger[] m = new BigInteger[4];
            BigInteger[] M = new BigInteger[4];

            for (int i = 0; i < answer.Length; i++)
            {
                m[0] = (int)CryptoFunctions.Pow(Int32.Parse(answer[i]), (p + 1) / 4, p);
                m[1] = (p - m[0]) % p;
                m[2] = (int)CryptoFunctions.Pow(Int32.Parse(answer[i]), (q + 1) / 4, q);
                m[3] = (q - m[2]) % q;

                BigInteger a = CryptoFunctions.Reverse_El(q, p) * q;
                BigInteger b = CryptoFunctions.Reverse_El(p, q) * p;

                M[0] = ((a * m[0]) + (b * m[2])) % N;
                M[1] = ((a * m[0]) + (b * m[3])) % N;
                M[2] = ((a * m[1]) + (b * m[2])) % N;
                M[3] = ((a * m[1]) + (b * m[3])) % N;

                for (int k = 0; k < 4; k++)
                {
                    if (M[k] < 0)
                    {
                        M[k] += N;
                    }

                    string tmp = "" + M[k]; // преобразуем возможный ответ в строку
                    char[] char_tmp = tmp.ToCharArray();

                    // проверяем наличине специального обозначения
                    if (char_tmp[tmp.Length - 1] == '0' && char_tmp[tmp.Length - 2] == '0')
                    {
                        result += (char)Int32.Parse(tmp.Substring(0, tmp.Length - 2));
                        break;
                    }
                }
            }

            return result;
        }
    }
}
