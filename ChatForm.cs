using Client;

namespace SecureChat;

public partial class ChatForm : Form
{
    private bool IsConnected = false;
    private bool SignatureCheck = false;
    private Trent client;
    private int ID;
    private string msgArg = "1$";
    private string temp;
    private int n, b, p, q, U, X;
    private int TrentN, TrentB;
    private int AliceN, AliceB, AliceP, AliceQ;
    private int BobN, BobB, BobP, BobQ;
    private int session_key;
    private string TimeS, TimeS1, TimeS2;
    private string TimeBS, TimeBS1, TimeBS2;

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (IsConnected)
        {
            DisconnectUser();
        }
    }

    private void Button_Click(object sender)
    {
        if (IsConnected)
        {
            DisconnectUser();
        }
    }

    private void DisconnectUser()
    {
        client.Disconnect(ID);
        client = null;
        IsConnected = false;

    }

    private void SessionKeyBtn_Click(object sender)
    {
        //Генерация ключей
        if (ID == 2)
        {
            int[] result = new int[4];
            result = DASSRabinCryptography.GenerateKeys(10);
            n = result[0]; // n - открытый ключ
            b = result[1]; // b - открытый ключ
            p = result[2]; // p - закрытый ключ
            q = result[3]; // q - закрытый ключ

            client.SendMessage("$00$" + n + "$" + b, ID);
            client.SendMessage("$00$" + n + "$" + b, ID);
        }
        else
        {
            client.SendMessage("2" + "$1$" + "Alice$Bob", ID);
        }
    }

    public void MessageCallBack(string msg)
    {
        string[] answer = msg.Split('$');

        switch (answer[0])
        {
            case "0":
                //connected users
                for (int i = 1; i < answer.Length; i++)
                {
                    UserList.Items.Add(answer[i]);
                }

                break;

            case "1":

                //personal comms
                if (ID.ToString() == answer[0])
                {
                    switch (answer[1])
                    {
                        case "00":
                            if (ID == 3)
                            {
                                TrentN = int.Parse(answer[2]);
                                TrentB = int.Parse(answer[3]);

                                int[] result_a = new int[4];
                                result_a = DASSRabinCryptography.GenerateKeys(13);
                                AliceN = result_a[0];
                                AliceB = result_a[1];
                                AliceP = result_a[2];
                                AliceQ = result_a[3];

                                client.SendMessage("4" + "$01$" + AliceN + "$" + AliceB, ID);
                                client.SendMessage("2" + "$01$" + AliceN + "$" + AliceB, ID);
                            }

                            if (ID == 4)
                            {
                                ;

                                TrentN = int.Parse(answer[2]);
                                TrentB = int.Parse(answer[3]);

                                int[] result_a = new int[4];
                                result_a = DASSRabinCryptography.GenerateKeys(12);
                                BobN = result_a[0];
                                BobB = result_a[1];
                                BobP = result_a[2];
                                BobQ = result_a[3];

                                client.SendMessage("3" + "$02$" + BobN + "$" + BobB, ID);
                                client.SendMessage("2" + "$02$" + BobN + "$" + BobB, ID);
                            }

                            break;
                        case "01":
                            ;

                            AliceN = int.Parse(answer[2]);
                            AliceB = int.Parse(answer[3]);

                            break;

                        case "02":

                            BobN = int.Parse(answer[2]);
                            BobB = int.Parse(answer[3]);
                            break;

                        case "1":
                            string[] sign_result = new string[2];

                            sign_result = DASSRabinCryptography.Signature(answer[2] + AliceN + AliceB, n, b);

                            // Подписанный ключ Алисы
                            ConsoleOutput.Items.Add(answer[2] + " " + AliceN + ", " + sign_result[0] + ", " + sign_result[1] + ")");

                            temp = sign_result[0] + "$" + sign_result[1] + "$";

                            sign_result = DASSRabinCryptography.Signature(answer[2] + BobN + BobB, n, b);

                            // Подписанный ключ Боба
                            ConsoleOutput.Items.Add(answer[2] + " " + BobN + ", " + sign_result[0] + ", " + sign_result[1] + ")");

                            temp += sign_result[0] + "$" + sign_result[1];

                            client.SendMessage("3" + "$2$" + temp, ID);
                            break;

                        case "2":

                            SignatureCheck = true;

                            SignatureCheck &= DASSRabinCryptography.Verification("Алиса" + AliceN + AliceB,
                                int.Parse(answer[2]), int.Parse(answer[3]), TrentN, TrentB);

                            SignatureCheck &= DASSRabinCryptography.Verification("Боб" + BobN + BobB,
                                int.Parse(answer[4]), int.Parse(answer[5]), TrentN, TrentB);

                            if (SignatureCheck)
                            {
                                // Генерация сеансового ключа 
                                session_key = CryptoFunctions.GeneratePrimeNumber(12, 12);

                                // Метка времени
                                TimeS = "" + DateTimeOffset.Now.ToUnixTimeSeconds();
                                TimeS1 = TimeS.Substring(2, 4);
                                TimeS2 = TimeS.Substring(6, TimeS.Length - 6);

                                sign_result = DASSRabinCryptography.Signature("" + session_key + " " + TimeS1 + TimeS2, AliceN, AliceB);

                                //Подпись
                                ConsoleOutput.Items.Add(session_key + TimeS1 + TimeS2 + ", " + sign_result[0] + ", " + sign_result[1]);

                                temp = "" + DASSRabinCryptography.Encryption(session_key.ToString(), BobN);

                                temp += "$" + DASSRabinCryptography.Encryption(TimeS1, BobN);

                                temp += "$" + DASSRabinCryptography.Encryption(TimeS2, BobN);

                                temp += "$" + DASSRabinCryptography.Encryption(sign_result[0], BobN);

                                temp += "$" + DASSRabinCryptography.Encryption(sign_result[1], BobN);

                                client.SendMessage("$3$" + temp + "$" + answer[2] + "$" + answer[3] +
                                    "$" + answer[4] + "$" + answer[5], ID);
                            }

                            break;

                        case "3":

                            session_key = int.Parse(DASSRabinCryptography.Decryption(answer[2], BobQ, BobP, BobN));

                            TimeS1 = "" + int.Parse(DASSRabinCryptography.Decryption(answer[3], BobQ, BobP, BobN));
                            TimeS2 = "" + int.Parse(DASSRabinCryptography.Decryption(answer[4], BobQ, BobP, BobN));

                            U = int.Parse(DASSRabinCryptography.Decryption(answer[5], BobQ, BobP, BobN));

                            X = int.Parse(DASSRabinCryptography.Decryption(answer[6], BobQ, BobP, BobN));

                            SignatureCheck = true;
                            SignatureCheck &= DASSRabinCryptography.Verification("" + session_key + " " + TimeS1 + TimeS2, U, X, AliceN, AliceB);
                            SignatureCheck &= DASSRabinCryptography.Verification("Алиса" + AliceN + AliceB, int.Parse(answer[7]),
                                int.Parse(answer[8]), TrentN, TrentB);
                            SignatureCheck &= DASSRabinCryptography.Verification("Боб" + BobN + BobB, int.Parse(answer[8]),
                                int.Parse(answer[9]), TrentN, TrentB);
                            if (SignatureCheck)
                            {
                                TimeBS = "" + DateTimeOffset.Now.ToUnixTimeSeconds();
                                TimeBS1 = TimeBS.Substring(2, 4);
                                TimeBS2 = TimeBS.Substring(6, TimeBS.Length - 6);

                                SignatureCheck = true;
                                SignatureCheck &= string.Compare(TimeS1, TimeBS1) == 0;
                                SignatureCheck &= int.Parse(TimeBS2) - int.Parse(TimeS2) + 3 <= 1;
                            }
                    }
                }
        }
    }
}

