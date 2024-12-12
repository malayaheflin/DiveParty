using System;

public static class Eventhandler
{
    public static event Action<bool> PlayerPressBtn;
    public static void CallPlayerPressBtn(bool isPress)
    {
        PlayerPressBtn?.Invoke(isPress);
    }
}
