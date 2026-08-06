using System.IO;

namespace Cuemon.Assets;
public class ManagedDisposable : Disposable
{
    public ManagedDisposable()
    {
        Stream = new MemoryStream();
    }

    public MemoryStream Stream { get; private set; }

    protected override void OnDisposeManagedResources()
    {
        try
        {
            Stream?.Dispose();
        }
        finally
        {
            Stream = null;
        }
    }
}
