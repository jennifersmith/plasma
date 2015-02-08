namespace Plasma.Core
{
    public interface IRequestProcessor
    {
        AspNetResponse ProcessRequest(AspNetRequest request);
        AspNetResponse ProcessRequest(string requestPath);
    }
}