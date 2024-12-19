namespace SecureChat;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Запускаем два окна для демонстрации: одно для Алисы, другое для Боба
        ChatForm aliceForm = new("Alice");
        ChatForm bobForm = new("Bob");

        // Создаем отдельные потоки для двух форм
        Thread threadAlice = new(() => Application.Run(aliceForm));
        Thread threadBob = new(() => Application.Run(bobForm));

        threadAlice.Start();
        threadBob.Start();
    }
}
