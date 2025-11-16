public class MoveStepTxt : TextBase
{
    protected override void PrintText()
    {
        text.SetText(GameManager.Instance.MoveStep.ToString());
    }


}
