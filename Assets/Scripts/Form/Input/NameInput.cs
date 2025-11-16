public class NameInput : InputBase
{
    public string nameID { get; private set; }
    protected override void OnEndEdit(string text)
    {
        this.nameID = text;
    }


}
