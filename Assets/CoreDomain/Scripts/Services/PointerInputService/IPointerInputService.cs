namespace CoreDomain.Scripts.Services.PointerInputService
{
    public interface IPointerInputService
    {
        bool IsPointerOverGUI();
        bool IsPointerDown();
    }
}