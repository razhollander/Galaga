using Cysharp.Threading.Tasks;
using Zenject;

namespace CoreDomain.Scripts.Utils.Command
{
    public interface ICommand
    {
        public abstract UniTask Execute();
    }

    public interface ISyncCommand
    {
        public abstract void Execute();
    }

    public interface ICommand<T>
    {
        public abstract UniTask<T> Execute();
    }

    public interface ISyncCommand<T>
    {
        public abstract T Execute();
    }

    public abstract class CommandBase : ICommand
    {
        public abstract UniTask Execute();
    }

    public abstract class CommandSyncBase : ISyncCommand
    {
        public abstract void Execute();
    }

    public abstract class CommandBase<T> : ICommand<T>
    {
        public abstract UniTask<T> Execute();
    }

    public abstract class CommandSyncBase<T> : ISyncCommand<T>
    {
        public abstract T Execute();
    }

    public abstract class Command<T> : CommandBase
    {
        public class Factory : PlaceholderFactory<T>
        {
        }
    }

    public abstract class CommandSync<T> : CommandSyncBase
    {
        public class Factory : PlaceholderFactory<T>
        {
        }
    }

    /// <summary>
    /// No Parameter command that return T
    /// </summary>
    /// <typeparam name="TV"></typeparam>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<TV, T> : CommandBase<T>
    {
        public class Factory : PlaceholderFactory<TV>
        {
        }
    }

    /// <summary>
    ///  One Parameter (T) command that return void
    /// </summary>
    /// <typeparam name="TV"></typeparam>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandOneParameter<TV, T> : CommandBase
    {
        public class Factory : PlaceholderFactory<TV, T>
        {
        }
    }

    public abstract class CommandSyncOneParameter<TV, T> : CommandSyncBase
    {
        public class Factory : PlaceholderFactory<TV, T>
        {
        }
    }
}