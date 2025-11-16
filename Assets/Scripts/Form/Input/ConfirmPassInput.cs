public class ConfirmPassInput : InputBase
{
    public string comfirmPassword { get; private set; }

    protected override void OnEndEdit(string text)
    {
        this.comfirmPassword = text;
    }
}
