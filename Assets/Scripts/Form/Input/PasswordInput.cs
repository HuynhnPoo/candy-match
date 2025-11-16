public class PasswordInput : InputBase
{
    public string password { get; private set; }
    protected override void OnEndEdit(string text)
    {
        this.password = text;
    }
}
