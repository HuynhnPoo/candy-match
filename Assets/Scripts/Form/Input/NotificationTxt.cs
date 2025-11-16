public class NotificationTxt : TextBase
{
    protected override void PrintText()
    {
        this.text.SetText(GameManager.Instance.Notification);
    }
}
