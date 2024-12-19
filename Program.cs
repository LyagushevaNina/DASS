namespace SecureChat;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // ��������� ��� ���� ��� ������������: ���� ��� �����, ������ ��� ����
        ChatForm aliceForm = new("Alice");
        ChatForm bobForm = new("Bob");

        // ������� ��������� ������ ��� ���� ����
        Thread threadAlice = new(() => Application.Run(aliceForm));
        Thread threadBob = new(() => Application.Run(bobForm));

        threadAlice.Start();
        threadBob.Start();
    }
}
