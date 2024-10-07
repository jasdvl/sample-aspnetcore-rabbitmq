using System.Diagnostics;

namespace PubLib.Messaging.RabbitMQ.Infrastructure;

/// <summary>
/// Provides a base implementation for disposable objects.
/// </summary>
public abstract class DisposableObject : IDisposable
{
    private int _disposeCount;

    /// <summary>
    /// Gets a value indicating whether the object has been disposed.
    /// </summary>
    public bool IsDisposed => _disposeCount > 0;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public virtual void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Finalizer that performs cleanup when the object is garbage collected.
    /// </summary>
    ~DisposableObject()
    {
        Dispose(false);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposeCount, 1) == 0)
        {
            if (disposing)
            {
                DisposeManagedResources();
            }

            DisposeUnmanagedResources();
        }
    }

    /// <summary>
    /// Releases the managed resources used by the object. This method is called by the Dispose method.
    /// Derived classes should override this method to release managed resources.
    /// </summary>
    protected virtual void DisposeManagedResources() { }

    /// <summary>
    /// Releases the unmanaged resources used by the object. This method is called by the Dispose method.
    /// Derived classes should override this method to release unmanaged resources.
    /// </summary>
    protected virtual void DisposeUnmanagedResources() { }

    /// <summary>
    /// Throws an ObjectDisposedException if the object has been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the object has been disposed.</exception>
    protected void ThrowIfDisposed()
    {
        if (IsDisposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    /// <summary>
    /// Asserts that the object has not been disposed. This method is only compiled in DEBUG builds.
    /// </summary>
    [Conditional("DEBUG")]
    protected void AssertNotDisposed()
    {
        Debug.Assert(!IsDisposed, "Object accessed after being disposed.");
    }
}

